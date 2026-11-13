using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class SupportTicket
    {
        public int TicketId { get; set; }

        public int ConversationId { get; set; }

        public string Priority { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? EscalationReason { get; set; }

        public string? ConvSummary { get; set; }

        public DateTime CreatedAt { get; set; }

        public Conversation Conversation { get; set; } = null!;

        public ICollection<TicketAssignment> Assignments { get; set; } = new List<TicketAssignment>();

        public ICollection<TicketStatusHistory> StatusHistory { get; set; } = new List<TicketStatusHistory>();

        public ICollection<InternalNote> InternalNotes { get; set; } = new List<InternalNote>();

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
