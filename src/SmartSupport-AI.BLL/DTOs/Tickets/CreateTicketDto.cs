namespace SmartSupport_AI.BLL.DTOs.Tickets
{
    public class CreateTicketDto
    {
        public int ConversationId { get; set; }

        public string Priority { get; set; } = string.Empty;

        public string? EscalationReason { get; set; }

        public string? ConvSummary { get; set; }
    }
}