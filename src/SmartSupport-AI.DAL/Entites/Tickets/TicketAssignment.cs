using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class TicketAssignment
    {
        public int AssignmentId { get; set; }

        public int TicketId { get; set; }

        public int AgentId { get; set; }

        public DateTime AssignedAt { get; set; }

        public bool IsCurrent { get; set; }

        public SupportTicket Ticket { get; set; } = null!;

        public SupportAgent Agent { get; set; } = null!;
    }
}
