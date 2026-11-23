using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.DTOs.Conversations
{
    public class SendMessageDto
    {
        public int ConversationId { get; set; }

        public string Content { get; set; } = string.Empty;
    }
}
