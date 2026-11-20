using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories;

namespace SmartSupport_AI.BLL;

public record DeskIdentity(int UserId, int ProfileId, string Role, string Name);
public record DeskMessage(int Sequence, string Sender, string Text);
public record DeskTicket(int Id, int ConversationId, string Status, string? Summary);
public record AgentTicketDetails(IReadOnlyList<string> History, IReadOnlyList<string> Notes, IReadOnlyList<string> ToolOutcomes);
public record DeskThread(int Id, string Status, IReadOnlyList<DeskMessage> Messages, IReadOnlyList<DeskTicket> Tickets,
    bool AgentOnline, AgentTicketDetails? AgentDetails);
public record RefundOffer(int OrderId, string OrderNumber, decimal Amount);
public record DeskReply(string State, RefundOffer? Offer = null);

public interface ISupportPresence
{
    bool IsAvailable(int agentId);
}

public sealed class SupportDesk(SupportUnitOfWork work, IAIProvider provider, ISupportPresence presence)
{
    public Task<User?> FindUserAsync(string email, CancellationToken ct) =>
        work.Query<User>().AsNoTracking().SingleOrDefaultAsync(u => u.Email == email && u.Status == "Active", ct);

    public async Task<DeskIdentity?> IdentityAsync(int userId, CancellationToken ct)
    {
        var user = await work.Query<User>().AsNoTracking().Include(u => u.Customer).Include(u => u.SupportAgent)
            .Include(u => u.Administrator).SingleOrDefaultAsync(u => u.UserId == userId && u.Status == "Active", ct);
        return user?.Role switch
        {
            "Customer" when user.Customer is not null => new(userId, user.Customer.CustomerId, "Customer", user.FullName),
            "Agent" or "SupportAgent" when user.SupportAgent is not null => new(userId, user.SupportAgent.AgentId, "Agent", user.FullName),
            "Admin" or "Administrator" when user.Administrator is not null => new(userId, user.Administrator.AdminId, "Admin", user.FullName),
            _ => null
        };
    }

    public Task<int> StartAsync(DeskIdentity actor, CancellationToken ct) =>
        work.AtomicAsync($"customer:{actor.ProfileId}", async () =>
        {
            if (actor.Role != "Customer") throw new UnauthorizedAccessException();
            var config = await work.Query<AIConfiguration>().Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync(ct);
            if (config is null) throw new InvalidOperationException("An active support configuration is required.");
            var conversation = new Conversation
            {
                CustomerId = actor.ProfileId, ConfigId = config.ConfigId, Status = "AI",
                Channel = "Web", StartedAt = DateTime.UtcNow
            };
            work.Add(conversation);
            await work.SaveAsync(ct);
            return conversation.ConversationId;
        }, ct);

    public async Task<List<DeskTicket>> TicketsAsync(DeskIdentity actor, CancellationToken ct)
    {
        var results = await work.Query<SupportTicket>().AsNoTracking()
            .Where(t => actor.Role == "Customer" && t.Conversation.CustomerId == actor.ProfileId ||
                actor.Role == "Agent" && (t.Status == "Queued" || t.Status == "Assigned" ||
                    t.Assignments.Any(a => a.IsCurrent && a.AgentId == actor.ProfileId)))
            .OrderByDescending(t => t.CreatedAt).Take(100)
            .Select(t => new { t.TicketId, t.ConversationId, t.Status, t.ConvSummary,
                Assigned = t.Assignments.Where(a => a.IsCurrent).Select(a => (int?)a.AgentId).FirstOrDefault() })
            .ToListAsync(ct);
        return results.Where(t => actor.Role == "Customer" || t.Status == "Queued" ||
                t.Assigned == actor.ProfileId || t.Status == "Assigned" && (t.Assigned is null || !presence.IsAvailable(t.Assigned.Value)))
            .Select(t => new DeskTicket(t.TicketId, t.ConversationId, t.Status,
                actor.Role == "Agent" && t.Assigned == actor.ProfileId ? t.ConvSummary : null)).ToList();
    }

    public Task<List<int>> ConversationsAsync(DeskIdentity actor, CancellationToken ct) =>
        work.Query<Conversation>().AsNoTracking().Where(c => actor.Role == "Customer" && c.CustomerId == actor.ProfileId)
            .OrderByDescending(c => c.StartedAt).Take(100).Select(c => c.ConversationId).ToListAsync(ct);

    public Task<int> CustomerUserIdAsync(int id, CancellationToken ct) =>
        work.Query<Conversation>().Where(c => c.ConversationId == id).Select(c => c.Customer.UserId).SingleAsync(ct);

    public Task<bool> CanReadAsync(DeskIdentity actor, int id, CancellationToken ct) =>
        work.Query<Conversation>().AnyAsync(c => c.ConversationId == id &&
            (actor.Role == "Customer" && c.CustomerId == actor.ProfileId ||
             actor.Role == "Agent" && c.Tickets.Any(t => t.Assignments.Any(a => a.IsCurrent && a.AgentId == actor.ProfileId))), ct);

    public async Task<DeskThread> ReadAsync(DeskIdentity actor, int id, CancellationToken ct)
    {
        if (!await CanReadAsync(actor, id, ct)) throw new UnauthorizedAccessException();
        var status = await work.Query<Conversation>().Where(c => c.ConversationId == id).Select(c => c.Status).SingleAsync(ct);
        var messages = await work.Query<Message>().Where(m => m.ConversationId == id).OrderByDescending(m => m.SequenceNo)
            .Take(100).OrderBy(m => m.SequenceNo).Select(m => new DeskMessage(m.SequenceNo, m.SenderType, m.Content)).ToListAsync(ct);
        var tickets = await work.Query<SupportTicket>().Where(t => t.ConversationId == id)
            .Select(t => new DeskTicket(t.TicketId, id, t.Status, actor.Role == "Agent" ? t.ConvSummary : null)).ToListAsync(ct);
        var assignedAgent = await work.Query<TicketAssignment>().Where(a => a.Ticket.ConversationId == id &&
                a.IsCurrent && a.Ticket.Status == "InProgress").Select(a => a.AgentId).FirstOrDefaultAsync(ct);
        AgentTicketDetails? agentDetails = null;
        if (actor.Role == "Agent")
        {
            var history = await work.Query<TicketStatusHistory>().Where(h => h.Ticket.ConversationId == id)
                .OrderByDescending(h => h.ChangedAt).Take(30).Select(h => h.NewStatus).ToListAsync(ct);
            var notes = await work.Query<InternalNote>().Where(n => n.Ticket.ConversationId == id)
                .OrderByDescending(n => n.CreatedAt).Take(30).Select(n => n.Content).ToListAsync(ct);
            var tools = await work.Query<ToolExecution>().Where(t => t.Response.Message.ConversationId == id)
                .OrderByDescending(t => t.ExecutedAt).Take(30).Select(t => t.ToolName + ": " + t.Status).ToListAsync(ct);
            agentDetails = new(history, notes, tools);
        }
        return new(id, status, messages, tickets, assignedAgent != 0 && presence.IsAvailable(assignedAgent), agentDetails);
    }

    public async Task<DeskReply> SendAsync(DeskIdentity actor, int id, string message, CancellationToken ct, Guid? requestId = null)
    {
        message = ValidateText(message);
        var trigger = await work.AtomicAsync($"conversation:{id}", async () =>
        {
            if (!await CanReadAsync(actor, id, ct)) throw new UnauthorizedAccessException();
            var state = await work.Query<Conversation>().Where(c => c.ConversationId == id).Select(c => c.Status).SingleAsync(ct);
            var requestKey = requestId?.ToString("N");
            if (requestKey is not null)
            {
                var prefix = requestKey + ":";
                var prior = await work.Query<ActionLog>().Where(a => a.ConversationId == id && a.UserId == actor.UserId &&
                        a.ActionType == "MessageRequest" && a.Details != null && a.Details.StartsWith(prefix))
                    .Select(a => a.Details).SingleOrDefaultAsync(ct);
                if (prior is not null && int.TryParse(prior[prefix.Length..], out var messageId))
                {
                    var original = await work.Query<Message>().SingleAsync(m => m.MessageId == messageId && m.ConversationId == id, ct);
                    if (original.Content != message) throw new ArgumentException("The retry does not match the original message.");
                    return (incoming: original, state);
                }
            }
            if (state == "Closed" || actor.Role == "Agent" && (state != "Human" || !presence.IsAvailable(actor.ProfileId))) throw new UnauthorizedAccessException();
            var incoming = await AppendAsync(id, actor.Role, message, actor.ProfileId, ct);
            if (requestKey is not null) Audit(actor, id, "MessageRequest", $"{requestKey}:{incoming.MessageId}");
            return (incoming, state);
        }, ct);
        if (actor.Role != "Customer" || trigger.state != "AI") return new(trigger.state);
        if (await work.Query<AIResponse>().AnyAsync(r => r.TriggerMsgId == trigger.incoming.MessageId, ct))
            return new("Recorded");
        var started = System.Diagnostics.Stopwatch.GetTimestamp();

        var previous = await work.Query<AIResponse>().Where(r => r.TriggerMessage.ConversationId == id &&
            r.TriggerMessage.SequenceNo < trigger.incoming.SequenceNo).OrderByDescending(r => r.TriggerMessage.SequenceNo)
            .Select(r => r.Intent.Name).FirstOrDefaultAsync(ct);
        var intent = SupportRules.Classify(message, Enum.TryParse<SupportIntent>(previous, out var parsed) ? parsed : null);
        var knowledge = intent == SupportIntent.Knowledge ? await RetrieveAsync(message, ct) : [];
        var grounded = knowledge.Count == 0 ? null : await new AIOrchestrator(provider).AnswerAsync(message,
            knowledge.Select(a => new KnowledgeSource(a.ArticleId, a.Title, a.Content)).ToArray(), ct);

        return await work.AtomicAsync($"conversation:{id}", async () =>
        {
            var conversation = await work.Query<Conversation>().SingleAsync(c => c.ConversationId == id && c.CustomerId == actor.ProfileId, ct);
            if (conversation.Status != "AI") return new DeskReply(conversation.Status);
            if (await work.Query<AIResponse>().AnyAsync(r => r.TriggerMsgId == trigger.incoming.MessageId, ct))
                return new DeskReply("Recorded");
            if (grounded is not null)
            {
                await work.LockAsync("knowledge-administration", ct);
                var source = grounded.Source;
                var current = await work.Query<KnowledgeArticle>().AsNoTracking().AnyAsync(a =>
                    a.ArticleId == source.Id && a.Status == "Published" && a.Title == source.Title && a.Content == source.Content, ct);
                if (!current) grounded = null;
            }
            var answer = SupportRules.Refusal;
            var state = intent == SupportIntent.OutOfScope ? "Outside scope" : "Resolved";
            var tool = "";
            var outcome = "Success";
            OrderStatusCheck? check = null;
            RefundOffer? offer = null;
            SupportTicket? ticket = null;
            if (intent is SupportIntent.OrderStatus or SupportIntent.RefundRequest or SupportIntent.RefundStatus)
            {
                var order = await FindOrderAsync(actor.ProfileId, id, message, ct);
                tool = intent == SupportIntent.OrderStatus ? "GetOrderStatus" : intent == SupportIntent.RefundStatus ? "GetRefundRequestStatus" : "CheckRefundEligibility";
                if (order is null)
                {
                    state = "Awaiting information";
                    outcome = "Unverified";
                    answer = "Which order number should I check? You can find it in your order confirmation.";
                    if (await work.Query<ActionLog>().CountAsync(log => log.ConversationId == id && log.ActionType == "OrderUnverified", ct) >= 2)
                    {
                        ticket = await EscalateAsync(conversation, actor, "Repeated order identification failure", "No order ownership or reference could be verified", ct);
                        answer = "I couldn't identify the order from the information available.";
                    }
                    Audit(actor, id, "OrderUnverified", intent.ToString());
                }
                else if (intent == SupportIntent.OrderStatus)
                {
                    check = new OrderStatusCheck { ConversationId = id, OrderId = order.OrderId, StatusReturned = order.OrderStatus, CheckedAt = DateTime.UtcNow };
                    answer = $"Order {order.OrderNumber} is {order.OrderStatus}. Tracking and delivery estimates are unavailable here.";
                }
                else if (intent == SupportIntent.RefundStatus)
                {
                    var refund = await work.Query<RefundRequest>().Where(r => r.OrderId == order.OrderId)
                        .OrderByDescending(r => r.InitiatedAt).FirstOrDefaultAsync(ct);
                    answer = refund is null ? $"There is no recorded refund request for order {order.OrderNumber}." :
                        $"Refund request #{refund.RefundId} for order {order.OrderNumber} has status {refund.Status}.";
                }
                else
                {
                    var policy = await PolicyAsync("RefundEligibility", ct);
                    if (SupportRules.Eligible(policy, order.OrderStatus, order.PlacedAt, DateTime.UtcNow, order.TotalAmount))
                    {
                        offer = new(order.OrderId, order.OrderNumber, order.TotalAmount);
                        Audit(actor, id, "RefundOffered", order.OrderId.ToString(System.Globalization.CultureInfo.InvariantCulture));
                        state = "Awaiting confirmation";
                        answer = $"Order {order.OrderNumber} qualifies for a refund request. Enter your reason and confirm to submit it for review.";
                    }
                    else
                    {
                        outcome = "Review required";
                        ticket = await EscalateAsync(conversation, actor, "Refund eligibility could not be confirmed", $"{order.OrderNumber}: {outcome}", ct);
                        answer = "I couldn't verify refund eligibility automatically.";
                    }
                }
            }
            else if (intent == SupportIntent.Clarification)
            {
                state = "Awaiting information";
                answer = "How can I help with your order, refund, product, account, or company policy?";
            }
            else if (intent == SupportIntent.Knowledge && grounded is not null)
            {
                tool = "SearchKnowledgeBase";
                answer = $"{grounded.Source.Title}\n{grounded.Text}";
                Audit(actor, id, "KnowledgeProvider", grounded.ProviderStatus);
            }
            else if (intent is SupportIntent.Human or SupportIntent.Complaint or SupportIntent.Knowledge)
            {
                ticket = await EscalateAsync(conversation, actor,
                    intent == SupportIntent.Human ? "Customer requested an agent" : "Unresolved support request", intent.ToString(), ct);
                answer = intent == SupportIntent.Human ? "I’ve requested human support." : "I couldn't resolve this with the available information.";
            }

            if (ticket is not null)
            {
                state = ticket.Status;
                answer += $" Ticket #{ticket.TicketId} is {ticket.Status.ToLowerInvariant()}.";
            }
            var output = await AppendAsync(id, "AI", answer, null, ct);
            var intentRow = await work.Query<Intent>().SingleOrDefaultAsync(i => i.Name == intent.ToString(), ct);
            if (intentRow is null) throw new InvalidOperationException("Support intents must be provisioned before chat.");
            var response = new AIResponse
            {
                Message = output, TriggerMessage = trigger.incoming, ConfigId = conversation.ConfigId,
                IntentId = intentRow.IntentId, CreatedAt = DateTime.UtcNow, Escalated = ticket is not null
            };
            work.Add(response);
            if (grounded is not null) work.Add(new ResponseArticle { Response = response, ArticleId = grounded.Source.Id, WasUsed = true, RankPosition = 1 });
            if (tool.Length > 0)
            {
                var execution = new ToolExecution { Response = response, ToolName = tool, Status = outcome, OutputResult = check?.StatusReturned ?? outcome, ExecutedAt = DateTime.UtcNow };
                work.Add(execution);
                if (check is not null)
                {
                    check.ToolExecution = execution;
                    work.Add(check);
                }
            }
            Audit(actor, id, "AIResponse", state);
            conversation.ResolutionType = state;
            work.Add(new PerformanceMetric { ConfigId = conversation.ConfigId, MetricType = "ResponseLatencyMs",
                MetricValue = (decimal)System.Diagnostics.Stopwatch.GetElapsedTime(started).TotalMilliseconds,
                PeriodStart = trigger.incoming.SentAt, PeriodEnd = DateTime.UtcNow });
            return new DeskReply(state, offer);
        }, ct);
    }

    public Task<int> ConfirmRefundAsync(DeskIdentity actor, int conversationId, int orderId, string reason, CancellationToken ct) =>
        work.AtomicAsync($"conversation:{conversationId}", async () =>
        {
            if (actor.Role != "Customer") throw new UnauthorizedAccessException();
            var conversation = await work.Query<Conversation>().SingleOrDefaultAsync(c => c.ConversationId == conversationId && c.CustomerId == actor.ProfileId, ct)
                ?? throw new UnauthorizedAccessException();
            var order = await work.Query<Order>().SingleOrDefaultAsync(o => o.OrderId == orderId && o.CustomerId == actor.ProfileId, ct)
                ?? throw new UnauthorizedAccessException();
            var existing = await work.Query<RefundRequest>().Where(r => r.OrderId == orderId).OrderBy(r => r.RefundId).FirstOrDefaultAsync(ct);
            if (existing is not null) return existing.RefundId;
            if (conversation.Status != "AI" || !SupportRules.Eligible(await PolicyAsync("RefundEligibility", ct),
                order.OrderStatus, order.PlacedAt, DateTime.UtcNow, order.TotalAmount))
                throw new InvalidOperationException("This refund request requires agent review.");
            var orderReference = orderId.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var cutoff = DateTime.UtcNow.AddMinutes(-30);
            if (!await work.Query<ActionLog>().AnyAsync(log => log.ConversationId == conversationId &&
                log.UserId == actor.UserId && log.ActionType == "RefundOffered" && log.Details == orderReference &&
                log.CreatedAt >= cutoff, ct))
                throw new InvalidOperationException("Request an eligibility check before confirming the refund.");
            // Serialize order-level creation even when two conversations request the same refund.
            var resource = $"refund-order:{orderId}";
            await work.LockAsync(resource, ct);
            existing = await work.Query<RefundRequest>().Where(r => r.OrderId == orderId).OrderBy(r => r.RefundId).FirstOrDefaultAsync(ct);
            if (existing is not null) return existing.RefundId;
            var refund = new RefundRequest
            {
                OrderId = orderId, ConversationId = conversationId, Amount = order.TotalAmount,
                Reason = ValidateText(reason), Eligibility = "Eligible", Status = "PendingReview", InitiatedAt = DateTime.UtcNow
            };
            work.Add(refund);
            work.Add(new RefundStatusHistory { RefundRequest = refund, NewStatus = refund.Status, ChangedAt = DateTime.UtcNow });
            await work.SaveAsync(ct);
            var ticket = await EscalateAsync(conversation, actor, "Refund approval required", $"Refund #{refund.RefundId}; order {order.OrderNumber}", ct);
            await AppendAsync(conversationId, "System",
                $"Refund request #{refund.RefundId} was submitted for review. Ticket #{ticket.TicketId} is {ticket.Status.ToLowerInvariant()}.", null, ct);
            conversation.ResolutionType = "Awaiting authorized processing";
            Audit(actor, conversationId, "RefundRequested", $"Refund #{refund.RefundId}");
            return refund.RefundId;
        }, ct);

    public Task<int> AgentActionAsync(DeskIdentity actor, int ticketId, string action, string? note, CancellationToken ct) =>
        work.AtomicAsync("agent-ticket", async () =>
        {
            if (actor.Role != "Agent" || !presence.IsAvailable(actor.ProfileId) ||
                !await work.Query<SupportAgent>().AnyAsync(a => a.AgentId == actor.ProfileId && a.Availability != "Disabled", ct))
                throw new UnauthorizedAccessException();
            var ticket = await work.Query<SupportTicket>().Include(t => t.Conversation).Include(t => t.Assignments)
                .SingleOrDefaultAsync(t => t.TicketId == ticketId, ct) ?? throw new UnauthorizedAccessException();
            await work.LockAsync($"conversation:{ticket.ConversationId}", ct);
            await work.ReloadAsync(ticket, ct);
            await work.ReloadAsync(ticket.Conversation, ct);
            var assignment = await work.Query<TicketAssignment>().SingleOrDefaultAsync(a => a.TicketId == ticketId && a.IsCurrent, ct);
            if (action == "Join" && ticket.Status is "Queued" or "Assigned")
            {
                if (assignment is not null && assignment.AgentId != actor.ProfileId)
                {
                    if (presence.IsAvailable(assignment.AgentId)) throw new UnauthorizedAccessException();
                    assignment.IsCurrent = false;
                    await work.SaveAsync(ct);
                    assignment = null;
                }
                if (assignment is null) Assign(ticket, actor.ProfileId);
                Change(ticket, actor, "InProgress");
                ticket.Conversation.Status = "Human";
            }
            else
            {
                if (assignment?.AgentId != actor.ProfileId || ticket.Status != "InProgress") throw new UnauthorizedAccessException();
                if (action == "Resolve")
                {
                    Change(ticket, actor, "Resolved");
                    ticket.Conversation.Status = "Closed";
                    ticket.Conversation.EndedAt = DateTime.UtcNow;
                }
                else if (action == "Note")
                    work.Add(new InternalNote { TicketId = ticketId, AgentId = actor.ProfileId, Content = ValidateText(note ?? ""), CreatedAt = DateTime.UtcNow });
                else throw new ArgumentException("Unknown ticket action.");
            }
            Audit(actor, ticket.ConversationId, $"Ticket{action}", $"Ticket #{ticketId}");
            return ticket.ConversationId;
        }, ct);

    public async Task<IReadOnlyList<int>> RequeueDisconnectedAgentAsync(int agentId, CancellationToken ct)
    {
        var pending = await work.Query<TicketAssignment>().AsNoTracking()
            .Where(a => a.AgentId == agentId && a.IsCurrent && (a.Ticket.Status == "Assigned" || a.Ticket.Status == "InProgress"))
            .Select(a => new { a.Ticket.ConversationId, a.TicketId, a.Agent.UserId })
            .OrderBy(a => a.TicketId).ToListAsync(ct);
        var customers = new HashSet<int>();
        foreach (var candidate in pending)
        {
            var customer = await work.AtomicAsync($"conversation:{candidate.ConversationId}", async () =>
            {
                if (presence.IsAvailable(agentId)) return (int?)null;
                var assignment = await work.Query<TicketAssignment>().Include(a => a.Ticket)
                    .ThenInclude(t => t.Conversation).SingleOrDefaultAsync(a => a.TicketId == candidate.TicketId &&
                        a.IsCurrent && a.AgentId == agentId, ct);
                if (assignment is null || assignment.Ticket.Status is not ("Assigned" or "InProgress")) return null;

                var ticket = assignment.Ticket;
                var actor = new DeskIdentity(candidate.UserId, agentId, "Agent", "");
                assignment.IsCurrent = false;
                Change(ticket, actor, "Queued");
                ticket.Conversation.Status = "Waiting";
                Audit(actor, ticket.ConversationId, "AgentDisconnected", $"Ticket #{ticket.TicketId}");
                return await CustomerUserIdAsync(ticket.ConversationId, ct);
            }, ct);
            if (customer is int userId) customers.Add(userId);
        }
        return customers.ToArray();
    }

    public Task<int> FeedbackAsync(DeskIdentity actor, int id, int rating, CancellationToken ct) =>
        work.AtomicAsync($"conversation:{id}", async () =>
        {
            if (actor.Role != "Customer" || !await CanReadAsync(actor, id, ct)) throw new UnauthorizedAccessException();
            if (rating is < 1 or > 5) throw new ArgumentException("Rating must be between 1 and 5.");
            var feedback = await work.Query<CustomerFeedback>().FirstOrDefaultAsync(f => f.ConversationId == id, ct);
            if (feedback is null)
            {
                feedback = new CustomerFeedback { ConversationId = id };
                work.Add(feedback);
            }
            feedback.Rating = rating;
            feedback.IsHelpful = rating >= 4;
            Audit(actor, id, "Feedback", rating.ToString(System.Globalization.CultureInfo.InvariantCulture));
            return rating;
        }, ct);

    private async Task<SupportTicket> EscalateAsync(Conversation conversation, DeskIdentity actor, string reason, string evidence, CancellationToken ct)
    {
        var id = conversation.ConversationId;
        var ticket = await work.Query<SupportTicket>().FirstOrDefaultAsync(t => t.ConversationId == id && t.Status != "Resolved" && t.Status != "Closed", ct);
        if (ticket is not null) return ticket;
        var context = await work.Query<Message>().Where(m => m.ConversationId == id)
            .OrderByDescending(m => m.SequenceNo).Take(5).OrderBy(m => m.SequenceNo).Select(m => m.Content).ToListAsync(ct);
        var priority = await PolicyAsync("DefaultTicketPriority", ct);
        ticket = new SupportTicket
        {
            ConversationId = id, Priority = priority is "Low" or "Normal" or "High" or "Urgent" ? priority : "Normal",
            CreatedAt = DateTime.UtcNow, EscalationReason = reason,
            ConvSummary = $"{reason}\n{evidence}\n{string.Join("\n", context)}"
        };
        work.Add(ticket);
        var candidates = await work.Query<SupportAgent>().Where(a => a.User.Status == "Active" && a.Availability != "Disabled" &&
            (a.User.Role == "Agent" || a.User.Role == "SupportAgent")).OrderBy(a => a.AgentId).Select(a => a.AgentId).ToListAsync(ct);
        var agent = candidates.FirstOrDefault(presence.IsAvailable);
        if (agent != 0) Assign(ticket, agent);
        Change(ticket, actor, agent == 0 ? "Queued" : "Assigned");
        conversation.Status = "Waiting";
        await work.SaveAsync(ct);
        Audit(actor, id, "TicketCreated", $"Ticket #{ticket.TicketId}");
        return ticket;
    }

    private void Assign(SupportTicket ticket, int agent) =>
        work.Add(new TicketAssignment { Ticket = ticket, AgentId = agent, IsCurrent = true, AssignedAt = DateTime.UtcNow });

    private void Change(SupportTicket ticket, DeskIdentity actor, string status)
    {
        work.Add(new TicketStatusHistory { Ticket = ticket, ChangedBy = actor.UserId,
            OldStatus = ticket.Status, NewStatus = status, ChangedAt = DateTime.UtcNow });
        ticket.Status = status;
    }

    private async Task<Order?> FindOrderAsync(int customerId, int conversationId, string text, CancellationToken ct)
    {
        var orders = await work.Query<Order>().AsNoTracking().Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.PlacedAt).ThenByDescending(o => o.OrderId).Take(100).ToListAsync(ct);
        var matches = orders.Where(o => SupportRules.MentionsReference(text, o.OrderNumber)).Take(2).ToArray();
        if (matches.Length != 0) return matches.Length == 1 ? matches[0] : null;
        if (string.Equals(text.TrimEnd('?','.','!'), "the last order", StringComparison.OrdinalIgnoreCase) || string.Equals(text.TrimEnd('?','.','!'), "last order", StringComparison.OrdinalIgnoreCase)) return orders.FirstOrDefault();
        if (!SupportRules.IsOrderFollowUp(text)) return null;
        var last = await work.Query<OrderStatusCheck>().Where(c => c.ConversationId == conversationId)
            .OrderByDescending(c => c.CheckId).Select(c => c.OrderId).FirstOrDefaultAsync(ct);
        return orders.FirstOrDefault(o => o.OrderId == last);
    }

    private async Task<List<KnowledgeArticle>> RetrieveAsync(string text, CancellationToken ct)
    {
        var words = System.Text.RegularExpressions.Regex.Matches(text.ToLowerInvariant(), @"[\p{L}\d]{4,}")
            .Select(m => m.Value).Except(["what", "does", "your", "have", "about", "tell", "please", "product", "policy"]).Take(12).ToArray();
        var articles = await work.Query<KnowledgeArticle>().AsNoTracking().Include(a => a.Category)
            .Where(a => a.Status == "Published").OrderByDescending(a => a.UpdatedAt).Take(200).ToListAsync(ct);
        return articles.Select(a => (article: a, score: words.Count(w =>
                a.Title.Contains(w, StringComparison.OrdinalIgnoreCase) || a.Category.Name.Contains(w, StringComparison.OrdinalIgnoreCase) ||
                a.Content.Contains(w, StringComparison.OrdinalIgnoreCase))))
            .Where(item => item.score > 0).OrderByDescending(item => item.score).Take(4).Select(item => item.article).ToList();
    }

    private Task<string?> PolicyAsync(string name, CancellationToken ct) =>
        work.Query<SystemPolicy>().Where(p => p.PolicyName == name).Select(p => p.PolicyValue).SingleOrDefaultAsync(ct);

    private async Task<Message> AppendAsync(int id, string sender, string text, int? profile, CancellationToken ct)
    {
        var last = await work.Query<Message>().Where(m => m.ConversationId == id).MaxAsync(m => (int?)m.SequenceNo, ct) ?? 0;
        var entry = new Message
        {
            ConversationId = id, SequenceNo = last + 1, SenderType = sender, Content = text, SentAt = DateTime.UtcNow,
            CustomerId = sender == "Customer" ? profile : null, AgentId = sender == "Agent" ? profile : null
        };
        work.Add(entry);
        await work.SaveAsync(ct);
        return entry;
    }

    private void Audit(DeskIdentity actor, int id, string action, string details) =>
        work.Add(new ActionLog { UserId = actor.UserId, ConversationId = id, ActionType = action, Details = details, CreatedAt = DateTime.UtcNow });

    private static string ValidateText(string value)
    {
        value = value.Trim();
        if (value.Length is < 1 or > 2000) throw new ArgumentException("Enter between 1 and 2,000 characters.");
        return value;
    }
}
