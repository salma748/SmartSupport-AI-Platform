using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.DAL.Repositories.KnowledgeBase
{
    public class KnowledgeArticleRepository : GenericRepository<KnowledgeArticle>, IKnowledgeArticleRepository
    {
        public KnowledgeArticleRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public async Task<List<KnowledgeArticle>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.KnowledgeArticles
                .AsNoTracking()
                .Where(article => article.CategoryId == categoryId)
                .OrderBy(article => article.ArticleId)
                .ToListAsync();
        }
    }
}
