using SmartSupport_AI.BLL.DTOs.KnowledgeBase;
using SmartSupport_AI.BLL.Services.Interfaces.KnowledgeBase;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.BLL.Services.Classes.KnowledgeBase
{
    public class IntentService : IIntentService
    {
        private const int MaxNameLength = 256;

        private readonly IIntentRepository _intentRepository;

        public IntentService(IIntentRepository intentRepository)
        {
            _intentRepository = intentRepository;
        }

        public async Task<IntentDto?> GetByIdAsync(int intentId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(intentId);

            var intent = await _intentRepository.GetByIdAsync(intentId);
            return intent is null ? null : MapToDto(intent);
        }

        public async Task<IntentDto?> GetByNameAsync(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            if (name.Length > MaxNameLength)
            {
                throw new ArgumentException($"Intent name must be at most {MaxNameLength} characters.", nameof(name));
            }

            var intent = await _intentRepository.GetByNameAsync(name);
            return intent is null ? null : MapToDto(intent);
        }

        private static IntentDto MapToDto(Intent intent)
        {
            return new IntentDto
            {
                IntentId = intent.IntentId,
                Name = intent.Name,
                Description = intent.Description
            };
        }
    }
}
