using System;

namespace SmartSupport_AI.DAL.Entites
{
    public class KnowledgeArticle
    {
        public int ArticleId { get; set; }

        public int CategoryId { get; set; }

        public int CreatedBy { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }

        public ArticleCategory Category { get; set; } = null!;

        public Administrator CreatedByAdministrator { get; set; } = null!;

        public ICollection<ResponseArticle> ResponseArticles { get; set; } = new List<ResponseArticle>();
    }
}
