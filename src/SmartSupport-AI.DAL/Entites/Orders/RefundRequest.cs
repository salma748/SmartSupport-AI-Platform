using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class RefundRequest
    {
        public int RefundId { get; set; }

        public int OrderId { get; set; }

        public int ConversationId { get; set; }

        public int? ExecutionId { get; set; }

        public decimal Amount { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string Eligibility { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime InitiatedAt { get; set; }

        public Conversation Conversation { get; set; } = null!;

        public ToolExecution? ToolExecution { get; set; }

        public Order Order { get; set; } = null!;

        public ICollection<RefundStatusHistory> StatusHistory { get; set; } = new List<RefundStatusHistory>();
    }
}
