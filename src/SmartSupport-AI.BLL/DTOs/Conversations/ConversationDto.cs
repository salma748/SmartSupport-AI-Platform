using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.DTOs.Conversations
{
    public class ConversationDto
    {
        public int ConversationId { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Summary { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }
    }
}
