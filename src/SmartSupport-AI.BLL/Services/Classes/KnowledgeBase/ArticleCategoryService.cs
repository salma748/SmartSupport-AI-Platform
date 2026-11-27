using SmartSupport_AI.BLL.DTOs.KnowledgeBase;
using SmartSupport_AI.BLL.Services.Interfaces.KnowledgeBase;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.BLL.Services.Classes.KnowledgeBase
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        private const int MaxNameLength = 256;

        private readonly IArticleCategoryRepository _categoryRepository;

        public ArticleCategoryService(IArticleCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ArticleCategoryDto?> GetByIdAsync(int categoryId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId);

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            return category is null ? null : MapToDto(category);
        }

        public async Task<ArticleCategoryDto?> GetByNameAsync(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            if (name.Length > MaxNameLength)
            {
                throw new ArgumentException($"Category name must be at most {MaxNameLength} characters.", nameof(name));
            }

            var category = await _categoryRepository.GetByNameAsync(name);
            return category is null ? null : MapToDto(category);
        }

        private static ArticleCategoryDto MapToDto(ArticleCategory category)
        {
            return new ArticleCategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
