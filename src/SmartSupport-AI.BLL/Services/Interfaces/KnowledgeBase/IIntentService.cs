using SmartSupport_AI.BLL.DTOs.KnowledgeBase;

namespace SmartSupport_AI.BLL.Services.Interfaces.KnowledgeBase
{
    public interface IIntentService
    {
        Task<IntentDto?> GetByIdAsync(int intentId);

        Task<IntentDto?> GetByNameAsync(string name);
    }
}
