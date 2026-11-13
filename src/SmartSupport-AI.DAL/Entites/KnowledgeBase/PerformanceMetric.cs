using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class PerformanceMetric
    {
        public int MetricId { get; set; }

        public int? AgentId { get; set; }

        public int? ConfigId { get; set; }

        public string MetricType { get; set; } = string.Empty;

        public decimal MetricValue { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public AIConfiguration? Configuration { get; set; }

        public SupportAgent? Agent { get; set; }
    }
}
