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
    }
}
