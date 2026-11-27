using SmartSupport_AI.BLL.DTOs.KnowledgeBase;
using SmartSupport_AI.BLL.Services.Interfaces.KnowledgeBase;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.BLL.Services.Classes.KnowledgeBase
{
    public class KnowledgeArticleService : IKnowledgeArticleService
    {
        private readonly IKnowledgeArticleRepository _knowledgeArticleRepository;

        public KnowledgeArticleService(IKnowledgeArticleRepository knowledgeArticleRepository)
        {
            _knowledgeArticleRepository = knowledgeArticleRepository;
        }

        // Future AI Agent retrieval will enforce article eligibility in its own workflow.
        public async Task<KnowledgeArticleDto?> GetByIdAsync(int articleId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(articleId);

            var article = await _knowledgeArticleRepository.GetByIdAsync(articleId);
            if (article is null)
            {
                return null;
            }

            return new KnowledgeArticleDto
            {
                ArticleId = article.ArticleId,
                CategoryId = article.CategoryId,
                Title = article.Title,
                Content = article.Content,
                Status = article.Status,
                UpdatedAt = article.UpdatedAt
            };
        }

        public async Task<IReadOnlyList<KnowledgeArticleSummaryDto>> GetByCategoryIdAsync(int categoryId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId);

            var articles = await _knowledgeArticleRepository.GetByCategoryIdAsync(categoryId);
            return articles.Select(article => new KnowledgeArticleSummaryDto
            {
                ArticleId = article.ArticleId,
                CategoryId = article.CategoryId,
                Title = article.Title,
                Status = article.Status,
                UpdatedAt = article.UpdatedAt
            }).ToList();
        }
    }
}
