namespace SmartSupport_AI.BLL.DTOs.KnowledgeBase
{
    public class IntentDto
    {
        public int IntentId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
