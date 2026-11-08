using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class AIResponse
    {
        public int ResponseId { get; set; }

        public int MessageId { get; set; }

        public int TriggerMsgId { get; set; }

        public int IntentId { get; set; }

        public int ConfigId { get; set; }

        public decimal Confidence { get; set; }

        public bool Escalated { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
