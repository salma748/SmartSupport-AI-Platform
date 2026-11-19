using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase
{
    public interface IIntentRepository : IGenericRepository<Intent>
    {
        Task<Intent?> GetByNameAsync(string name);
    }
}
