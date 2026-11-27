namespace SmartSupport_AI.BLL.DTOs.KnowledgeBase
{
    public class KnowledgeArticleSummaryDto
    {
        public int ArticleId { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
