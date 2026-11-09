namespace SmartSupport_AI.DAL.Entites
{
    public class ResponseArticle
    {
        public int ResponseId { get; set; }

        public int ArticleId { get; set; }

        public decimal RelevanceScore { get; set; }

        public int RankPosition { get; set; }

        public bool WasUsed { get; set; }
    }
}
