namespace SmartSupport_AI.BLL.DTOs.KnowledgeBase
{
    public class KnowledgeArticleDto
    {
        public int ArticleId { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
