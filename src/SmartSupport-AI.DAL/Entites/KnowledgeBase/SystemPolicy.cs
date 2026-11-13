using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class SystemPolicy
    {
        public int PolicyId { get; set; }

        public int UpdatedBy { get; set; }

        public string PolicyName { get; set; } = string.Empty;

        public string PolicyValue { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }

        public Administrator UpdatedByAdministrator { get; set; } = null!;
    }
}
