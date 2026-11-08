using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class AIConfiguration
    {
        public int ConfigId { get; set; }

        public int CreatedBy { get; set; }

        public string VersionNumber { get; set; } = string.Empty;

        public string ModelName { get; set; } = string.Empty;

        public decimal MinConfidence { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
