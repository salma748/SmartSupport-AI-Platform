using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.DAL.Repositories.KnowledgeBase
{
    public class ArticleCategoryRepository : GenericRepository<ArticleCategory>, IArticleCategoryRepository
    {
        public ArticleCategoryRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public async Task<ArticleCategory?> GetByNameAsync(string name)
        {
            return await _context.ArticleCategories
                .FirstOrDefaultAsync(category => category.Name == name);
        }
    }
}
