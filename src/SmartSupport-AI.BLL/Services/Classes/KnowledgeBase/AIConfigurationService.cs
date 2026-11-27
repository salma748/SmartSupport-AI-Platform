using SmartSupport_AI.BLL.DTOs.KnowledgeBase;
using SmartSupport_AI.BLL.Services.Interfaces.KnowledgeBase;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.BLL.Services.Classes.KnowledgeBase
{
    public class AIConfigurationService : IAIConfigurationService
    {
        private const int MaxVersionNumberLength = 256;

        private readonly IAIConfigurationRepository _configurationRepository;

        public AIConfigurationService(IAIConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task<AIConfigurationDto?> GetByIdAsync(int configId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(configId);

            var configuration = await _configurationRepository.GetByIdAsync(configId);
            return configuration is null ? null : MapToDto(configuration);
        }

        public async Task<AIConfigurationDto?> GetByVersionNumberAsync(string versionNumber)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(versionNumber);
            if (versionNumber.Length > MaxVersionNumberLength)
            {
                throw new ArgumentException($"Version number must be at most {MaxVersionNumberLength} characters.", nameof(versionNumber));
            }

            var configuration = await _configurationRepository.GetByVersionNumberAsync(versionNumber);
            return configuration is null ? null : MapToDto(configuration);
        }

        private static AIConfigurationDto MapToDto(AIConfiguration configuration)
        {
            return new AIConfigurationDto
            {
                ConfigId = configuration.ConfigId,
                VersionNumber = configuration.VersionNumber,
                ModelName = configuration.ModelName,
                IsActive = configuration.IsActive,
                CreatedAt = configuration.CreatedAt
            };
        }
    }
}
