namespace SmartSupport_AI.BLL.DTOs.KnowledgeBase
{
    public class CreateKnowledgeArticleDraftDto
    {
        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}
