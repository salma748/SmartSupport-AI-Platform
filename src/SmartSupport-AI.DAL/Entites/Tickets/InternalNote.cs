using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class InternalNote
    {
        public int NoteId { get; set; }

        public int TicketId { get; set; }

        public int AgentId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
