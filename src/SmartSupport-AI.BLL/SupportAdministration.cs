using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories;

namespace SmartSupport_AI.BLL;

public record SupportOverview(int Conversations, int Tickets, int Responses, int EscalatedResponses, int Rated, double? AverageRating);
public record ArticleListing(int Id, string Title, string Status, string Category);
public record AdminDesk(SupportOverview Overview, IReadOnlyList<ArticleListing> Articles, IReadOnlyList<string> Policies);

public sealed class SupportAdministration(SupportUnitOfWork work)
{
    public async Task<AdminDesk> ReadAsync(CancellationToken ct)
    {
        var overview = new SupportOverview(
            await work.Query<Conversation>().CountAsync(ct), await work.Query<SupportTicket>().CountAsync(ct),
            await work.Query<AIResponse>().CountAsync(ct), await work.Query<AIResponse>().CountAsync(r => r.Escalated, ct),
            await work.Query<CustomerFeedback>().CountAsync(ct), await work.Query<CustomerFeedback>().AverageAsync(f => (double?)f.Rating, ct));
        return new(overview, await work.Query<KnowledgeArticle>().OrderByDescending(a => a.UpdatedAt).Take(100)
            .Select(a => new ArticleListing(a.ArticleId, a.Title, a.Status, a.Category.Name)).ToListAsync(ct),
            await work.Query<SystemPolicy>().OrderBy(p => p.PolicyName).Select(p => p.PolicyName).ToListAsync(ct));
    }

    public Task<int> ArticleAsync(DeskIdentity actor, int id, string title, string category, string content, bool publish, CancellationToken ct) =>
        work.AtomicAsync("knowledge-administration", async () =>
        {
            if (actor.Role != "Admin") throw new UnauthorizedAccessException();
            title = title.Trim();
            category = category.Trim();
            if (title.Length is < 1 or > 200 || category.Length is < 1 or > 100 || content.Trim().Length is < 1 or > 4000)
                throw new ArgumentException("Enter a title, category, and article body of up to 4,000 characters.");
            var group = await work.Query<ArticleCategory>().SingleOrDefaultAsync(c => c.Name == category, ct);
            if (group is null)
            {
                group = new ArticleCategory { Name = category };
                work.Add(group);
            }
            var article = id == 0 ? new KnowledgeArticle { CreatedBy = actor.ProfileId } :
                await work.Query<KnowledgeArticle>().SingleOrDefaultAsync(a => a.ArticleId == id, ct) ?? throw new ArgumentException("Unknown article.");
            article.Title = title;
            article.Category = group;
            article.Content = content.Trim();
            article.Status = publish ? "Published" : "Draft";
            article.UpdatedAt = DateTime.UtcNow;
            if (id == 0) work.Add(article);
            work.Add(new ActionLog { UserId = actor.UserId, ActionType = "KnowledgeArticleUpdated", CreatedAt = DateTime.UtcNow });
            await work.SaveAsync(ct);
            return article.ArticleId;
        }, ct);

    public Task<bool> PolicyAsync(DeskIdentity actor, string name, string value, CancellationToken ct) =>
        work.AtomicAsync("policy-administration", async () =>
        {
            if (actor.Role != "Admin") throw new UnauthorizedAccessException();
            if (value.Length > 4000 || (name == "RefundEligibility" && !ValidRefundPolicy(value)) ||
                (name == "DefaultTicketPriority" && value is not ("Low" or "Normal" or "High" or "Urgent")) ||
                name is not ("RefundEligibility" or "DefaultTicketPriority"))
                throw new ArgumentException("Enter valid settings for a supported policy.");
            var policy = await work.Query<SystemPolicy>().SingleOrDefaultAsync(p => p.PolicyName == name, ct);
            if (policy is null)
            {
                policy = new SystemPolicy { PolicyName = name };
                work.Add(policy);
            }
            policy.PolicyValue = value;
            policy.UpdatedBy = actor.ProfileId;
            policy.UpdatedAt = DateTime.UtcNow;
            work.Add(new ActionLog { UserId = actor.UserId, ActionType = "PolicyUpdated", Details = name, CreatedAt = DateTime.UtcNow });
            return true;
        }, ct);

    private static bool ValidRefundPolicy(string value)
    {
        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(value);
            var root = doc.RootElement;
            if (root.ValueKind != System.Text.Json.JsonValueKind.Object ||
                !root.TryGetProperty("eligibleStatuses", out var statuses) ||
                statuses.ValueKind != System.Text.Json.JsonValueKind.Array) return false;
            var first = statuses.EnumerateArray().FirstOrDefault();
            if (first.ValueKind != System.Text.Json.JsonValueKind.String || string.IsNullOrWhiteSpace(first.GetString())) return false;
            var now = DateTime.UtcNow;
            return SupportRules.Eligible(value, first.GetString()!, now, now, 1m);
        }
        catch (System.Text.Json.JsonException) { return false; }
    }
}
