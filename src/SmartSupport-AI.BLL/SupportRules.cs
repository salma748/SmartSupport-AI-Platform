using System.Text.Json;
using System.Text.RegularExpressions;

namespace SmartSupport_AI.BLL;

public enum SupportIntent { OutOfScope, OrderStatus, RefundRequest, RefundStatus, Knowledge, Complaint, Human, Clarification }

public static partial class SupportRules
{
    public const string Refusal = "I'm here to help with customer support for this business, including orders, refunds, products, and company policies. How can I assist you with one of these?";

    public static SupportIntent Classify(string text, SupportIntent? previous = null)
    {
        text = NormalizeForScope(text);
        if (ContainsRoleOverride(text) || Matches(text,
            @"\b(homework|capital of|tell (me )?a joke|weather in|human rights|political (party|campaign)|immigration policy|monetary policy|foreign policy|purchase order template)\b|\b(write|generate|debug)\b.{0,34}\b(code|script|poem|essay)\b"))
            return SupportIntent.OutOfScope;

        var serviceRole = @"(human( agent)?|support agent|customer service|representative|real person|someone from support)";
        if (Matches(text, $@"\A{serviceRole}( please)?[?.! ]*\z|\b(connect|transfer|put|send)\b.{{0,22}}\bme\b.{{0,18}}\b{serviceRole}\b|\b(speak|talk|chat) (to|with) (a |an |the )?{serviceRole}\b|\b(i need|i want|can i get) (a |an )?{serviceRole}\b"))
            return SupportIntent.Human;
        if (previous is SupportIntent.OrderStatus or SupportIntent.RefundRequest or SupportIntent.RefundStatus &&
            (IsOrderFollowUp(text) || Matches(text, @"\A[a-z]{0,12}[- ]?\d[\w-]{0,60}[?.! ]*\z")))
            return previous.Value;
        if (Matches(text, @"\brefund\b.{0,40}\b(status|reference|pending|approved)\b|status.{0,25}\brefund\b"))
            return SupportIntent.RefundStatus;
        if (Matches(text, @"\b(refund|return my|money back)\b")) return SupportIntent.RefundRequest;
        if (Matches(text, @"\b(complaint|broken|damaged|wrong item|account|log ?in|charged|payment)\b"))
            return SupportIntent.Complaint;
        if (Matches(text, @"\b(product|policy|policies|warranty|business hours|exchange|returns)\b"))
            return SupportIntent.Knowledge;
        if (Matches(text, @"\b(order|delivery|shipping|shipment|tracking|package)\b")) return SupportIntent.OrderStatus;
        return Matches(text, @"\A(hello|hi|hey|help)[!.? ]*\z") ? SupportIntent.Clarification : SupportIntent.OutOfScope;
    }

    public static bool IsOrderFollowUp(string text) =>
        Matches(text.Trim(), @"\A(that( order| one)?|the same( order| one)?|the last (one|order)|what about (it|that order))[?.! ]*\z");

    public static bool ContainsRoleOverride(string text) =>
        Matches(NormalizeForScope(text),
            @"\b(ignore|disregard|override|forget|bypass)\b.{0,65}\b(instructions?|rules?|policy|authorization|permissions?|previous|prior|above)\b|\b(system prompt|developer message|hidden instructions|jailbreak)\b|\b(act as|pretend to be|you are now|change your role|reveal your instructions)\b");

    private static string NormalizeForScope(string text) =>
        Regex.Replace(text.Normalize(System.Text.NormalizationForm.FormKC), @"\p{Cf}", "")
            .Trim().Replace('\r', ' ').Replace('\n', ' ');

    private static bool Matches(string text, string pattern) =>
        Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100));

    public static bool Eligible(string? policy, string status, DateTime placedAt, DateTime now, decimal total)
    {
        if (string.IsNullOrWhiteSpace(policy) || total <= 0 || placedAt > now) return false;
        try
        {
            using var json = JsonDocument.Parse(policy);
            var root = json.RootElement;
            return root.ValueKind == JsonValueKind.Object &&
                root.TryGetProperty("maxDaysFromOrder", out var days) &&
                days.ValueKind == JsonValueKind.Number && days.TryGetInt32(out var window) && window > 0 &&
                (now - placedAt).TotalDays <= window &&
                root.TryGetProperty("eligibleStatuses", out var statuses) &&
                statuses.ValueKind == JsonValueKind.Array &&
                statuses.EnumerateArray().Any(value => value.ValueKind == JsonValueKind.String &&
                    string.Equals(value.GetString(), status, StringComparison.OrdinalIgnoreCase));
        }
        catch (JsonException) { return false; }
    }

    public static bool MentionsReference(string text, string reference) =>
        Matches(text, $@"(?<![\w-]){Regex.Escape(reference)}(?![\w-])");
}
