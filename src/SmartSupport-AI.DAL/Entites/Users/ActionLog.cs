using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class ActionLog
    {
        public int LogId { get; set; }

        public int? UserId { get; set; }

        public int? ConversationId { get; set; }

        public int? TicketId { get; set; }

        public string ActionType { get; set; } = string.Empty;

        public string? Details { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
