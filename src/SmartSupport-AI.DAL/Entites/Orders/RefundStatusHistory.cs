using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class RefundStatusHistory
    {
        public int HistoryId { get; set; }

        public int RefundId { get; set; }

        public string OldStatus { get; set; } = string.Empty;

        public string NewStatus { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }
    }
}
