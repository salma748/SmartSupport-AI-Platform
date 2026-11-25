namespace SmartSupport_AI.BLL.DTOs.KnowledgeBase
{
    public class AIConfigurationDto
    {
        public int ConfigId { get; set; }

        public string VersionNumber { get; set; } = string.Empty;

        public string ModelName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
