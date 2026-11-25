using SmartSupport_AI.BLL.DTOs.KnowledgeBase;

namespace SmartSupport_AI.BLL.Services.Interfaces.KnowledgeBase
{
    public interface IAIConfigurationService
    {
        Task<AIConfigurationDto?> GetByIdAsync(int configId);

        Task<AIConfigurationDto?> GetByVersionNumberAsync(string versionNumber);
    }
}
