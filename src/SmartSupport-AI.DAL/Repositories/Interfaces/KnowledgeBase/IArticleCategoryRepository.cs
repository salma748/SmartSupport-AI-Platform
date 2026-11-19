using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase
{
    public interface IArticleCategoryRepository : IGenericRepository<ArticleCategory>
    {
        Task<ArticleCategory?> GetByNameAsync(string name);
    }
}
