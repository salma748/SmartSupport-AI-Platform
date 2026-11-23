using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSupport_AI.BLL.DTOs.Conversations
{
    public class MessageDto
    {
        public int MessageId { get; set; }

        public int ConversationId { get; set; }

        public int SequenceNo { get; set; }

        public string SenderType { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }
    }
}
