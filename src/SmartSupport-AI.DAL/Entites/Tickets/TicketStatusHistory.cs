using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class TicketStatusHistory
    {
        public int HistoryId { get; set; }

        public int TicketId { get; set; }

        public int ChangedBy { get; set; }

        public string OldStatus { get; set; } = string.Empty;

        public string NewStatus { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }

        public SupportTicket Ticket { get; set; } = null!;

        public User ChangedByUser { get; set; } = null!;
    }
}
