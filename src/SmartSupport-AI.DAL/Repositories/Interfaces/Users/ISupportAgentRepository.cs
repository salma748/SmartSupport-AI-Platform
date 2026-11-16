using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Users
{
    public interface ISupportAgentRepository : IGenericRepository<SupportAgent>
    {
        SupportAgent? GetByUserId(int userId);
    }
}
