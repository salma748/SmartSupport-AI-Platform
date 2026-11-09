using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class SupportTicket
    {
        public int TicketId { get; set; }

        public int ConversationId { get; set; }

        public int CustomerId { get; set; }

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? EscalationReason { get; set; }

        public string? ConvSummary { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
