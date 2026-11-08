using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class Conversation
    {
        public int ConversationId { get; set; }

        public int CustomerId { get; set; }

        public int ConfigId { get; set; }

        public string Channel { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? ResolutionType { get; set; }

        public string? Summary { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }
    }
}
