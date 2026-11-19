using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase
{
    public interface IKnowledgeArticleRepository : IGenericRepository<KnowledgeArticle>
    {
        Task<List<KnowledgeArticle>> GetByCategoryIdAsync(int categoryId);
    }
}
