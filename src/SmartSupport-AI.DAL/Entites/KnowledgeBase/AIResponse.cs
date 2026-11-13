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

        public Message Message { get; set; } = null!;

        public Message TriggerMessage { get; set; } = null!;

        public AIConfiguration Configuration { get; set; } = null!;

        public Intent Intent { get; set; } = null!;

        public ICollection<ToolExecution> ToolExecutions { get; set; } = new List<ToolExecution>();

        public ICollection<ResponseArticle> ResponseArticles { get; set; } = new List<ResponseArticle>();
    }
}
