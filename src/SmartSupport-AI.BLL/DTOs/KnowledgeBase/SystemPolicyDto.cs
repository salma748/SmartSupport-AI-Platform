namespace SmartSupport_AI.BLL.DTOs.KnowledgeBase
{
    public class SystemPolicyDto
    {
        public int PolicyId { get; set; }

        public string PolicyName { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
