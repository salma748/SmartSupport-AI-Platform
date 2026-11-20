using System.Text.Json;

namespace SmartSupport_AI.BLL;

public record KnowledgeSource(int Id, string Title, string Content);
public record GroundedAnswer(KnowledgeSource Source, string Text, string ProviderStatus);

public sealed class AIOrchestrator(IAIProvider provider)
{
    private const string KnowledgeInstructions = """
        Use SelectArticle to request one of the supplied approved articles only if relevant.
        When its result arrives, return one exact excerpt from that article as plain text.
        Do not add commentary, instructions, invented facts or formatting. The backend verifies the excerpt.
        """;

    private static readonly object Tools = new[] { new
    {
        functionDeclarations = new[] { new
        {
            name = "SelectArticle",
            description = "Read one approved article that may address the customer's question",
            parameters = new { type = "OBJECT", properties = new { articleId = new { type = "INTEGER" } },
                required = new[] { "articleId" } }
        } }
    } };

    public async Task<GroundedAnswer?> AnswerAsync(string question, IReadOnlyList<KnowledgeSource> sources, CancellationToken ct)
    {
        if (SupportRules.Classify(question) != SupportIntent.Knowledge) return null;
        sources = sources.Where(source => !SupportRules.ContainsRoleOverride(source.Title) &&
            !SupportRules.ContainsRoleOverride(source.Content)).ToArray();
        if (sources.Count == 0) return null;
        var fallback = sources[0];
        using var deadline = CancellationTokenSource.CreateLinkedTokenSource(ct);
        deadline.CancelAfter(TimeSpan.FromSeconds(75));
        var prompt = SmartSupportPrompt.Instructions + "\n" + KnowledgeInstructions;
        var turns = new List<object>
        {
            new { role = "user", parts = new[] { new { text = JsonSerializer.Serialize(new
                { question, approvedArticles = sources.Select(a => new { a.Id, a.Title }) }) } } }
        };
        try
        {
            var model = await provider.GenerateAsync(prompt, turns, Tools, deadline.Token);
            if (model is null) return new(fallback, fallback.Content, "Unavailable");
            if (!TrySelect(model.Value, sources, out var selected))
                return new(fallback, fallback.Content, "InvalidSelection");
            turns.Add(model.Value);
            turns.Add(new
            {
                role = "user",
                parts = new[] { new { functionResponse = new { name = "SelectArticle",
                    response = new { result = new { selected!.Id, selected.Title, Content = selected.Content[..Math.Min(selected.Content.Length, 4000)] } } } } }
            });
            var answer = await provider.GenerateAsync(prompt, turns, Tools, deadline.Token);
            var excerpt = ReadText(answer);
            return excerpt is { Length: >= 20 and <= 1500 } && selected.Content.Split('\n').Any(line => line.Trim().Equals(excerpt, StringComparison.Ordinal))
                ? new(selected, excerpt, "VerifiedExcerpt")
                : new(selected, selected.Content, "ArticleFallback");
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            return new(fallback, fallback.Content, "TimedOut");
        }
    }

    private static bool TrySelect(JsonElement model, IReadOnlyList<KnowledgeSource> sources, out KnowledgeSource? selected)
    {
        selected = null;
        if (!Property(model, "parts", JsonValueKind.Array, out var parts)) return false;
        var calls = parts.EnumerateArray().Where(part => Property(part, "functionCall", JsonValueKind.Object, out _)).Take(2).ToArray();
        if (calls.Length != 1 || !Property(calls[0], "functionCall", JsonValueKind.Object, out var call) ||
            !Property(call, "name", JsonValueKind.String, out var name) || name.GetString() != "SelectArticle" ||
            !Property(call, "args", JsonValueKind.Object, out var args) || args.EnumerateObject().Count() != 1 ||
            !Property(args, "articleId", JsonValueKind.Number, out var arg) || !arg.TryGetInt32(out var id))
            return false;
        selected = sources.FirstOrDefault(source => source.Id == id);
        return selected is not null;
    }

    private static string? ReadText(JsonElement? content)
    {
        if (content is null || !Property(content.Value, "parts", JsonValueKind.Array, out var parts)) return null;
        var text = parts.EnumerateArray()
            .Where(part => !Property(part, "thought", JsonValueKind.True, out _))
            .Select(part => Property(part, "text", JsonValueKind.String, out var value) ? value.GetString() : null);
        return string.Concat(text).Trim();
    }

    private static bool Property(JsonElement item, string key, JsonValueKind kind, out JsonElement value)
    {
        value = default;
        return item.ValueKind == JsonValueKind.Object && item.TryGetProperty(key, out value) && value.ValueKind == kind;
    }
}
