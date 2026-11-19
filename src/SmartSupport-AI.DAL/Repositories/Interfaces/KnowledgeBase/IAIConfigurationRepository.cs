using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase
{
    public interface IAIConfigurationRepository : IGenericRepository<AIConfiguration>
    {
        Task<AIConfiguration?> GetByVersionNumberAsync(string versionNumber);
    }
}
