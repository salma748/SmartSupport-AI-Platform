using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class OrderStatusCheck
    {
        public int CheckId { get; set; }

        public int OrderId { get; set; }

        public int ConversationId { get; set; }

        public int? ExecutionId { get; set; }

        public string StatusReturned { get; set; } = string.Empty;

        public DateTime CheckedAt { get; set; }

        public Conversation Conversation { get; set; } = null!;

        public ToolExecution? ToolExecution { get; set; }

        public Order Order { get; set; } = null!;
    }
}
