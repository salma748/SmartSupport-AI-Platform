using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class ToolExecution
    {
        public int ExecutionId { get; set; }

        public int ResponseId { get; set; }

        public string ToolName { get; set; } = string.Empty;

        public string InputParams { get; set; } = string.Empty;

        public string OutputResult { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime ExecutedAt { get; set; }
    }
}
