using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class Message
    {
        public int MessageId { get; set; }

        public int ConversationId { get; set; }

        public int SequenceNo { get; set; }

        public string SenderType { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }

        public int? CustomerId { get; set; }

        public int? AgentId { get; set; }

        public Conversation Conversation { get; set; } = null!;

        public AIResponse? AIResponse { get; set; }

        public Customer? Customer { get; set; }

        public SupportAgent? Agent { get; set; }

        public ICollection<AIResponse> TriggeredAIResponses { get; set; } = new List<AIResponse>();

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
