namespace SmartSupport_AI.BLL.DTOs.KnowledgeBase
{
    public class ArticleCategoryDto
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
