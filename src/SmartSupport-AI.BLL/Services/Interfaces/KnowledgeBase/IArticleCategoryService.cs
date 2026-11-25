using SmartSupport_AI.BLL.DTOs.KnowledgeBase;

namespace SmartSupport_AI.BLL.Services.Interfaces.KnowledgeBase
{
    public interface IArticleCategoryService
    {
        Task<ArticleCategoryDto?> GetByIdAsync(int categoryId);

        Task<ArticleCategoryDto?> GetByNameAsync(string name);
    }
}
