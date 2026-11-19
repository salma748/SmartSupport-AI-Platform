using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase
{
    public interface ISystemPolicyRepository : IGenericRepository<SystemPolicy>
    {
        Task<SystemPolicy?> GetByPolicyNameAsync(string policyName);
    }
}
