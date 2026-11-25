using SmartSupport_AI.BLL.DTOs.KnowledgeBase;

namespace SmartSupport_AI.BLL.Services.Interfaces.KnowledgeBase
{
    public interface IKnowledgeArticleService
    {
        Task<KnowledgeArticleDto?> GetByIdAsync(int articleId);

        Task<IReadOnlyList<KnowledgeArticleSummaryDto>> GetByCategoryIdAsync(int categoryId);
    }
}
