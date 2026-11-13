namespace SmartSupport_AI.DAL.Entites
{
    public class ArticleCategory
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<KnowledgeArticle> Articles { get; set; } = new List<KnowledgeArticle>();
    }
}
