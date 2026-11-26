namespace SmartSupport_AI.BLL.DTOs.Tickets
{
    public class TicketDto
    {
        public int TicketId { get; set; }

        public int ConversationId { get; set; }

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? EscalationReason { get; set; }

        public string? ConvSummary { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}