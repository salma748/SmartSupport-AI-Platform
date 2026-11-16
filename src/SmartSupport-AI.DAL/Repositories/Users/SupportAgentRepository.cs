using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Users;

namespace SmartSupport_AI.DAL.Repositories.Users
{
    public class SupportAgentRepository : GenericRepository<SupportAgent>, ISupportAgentRepository
    {
        public SupportAgentRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public SupportAgent? GetByUserId(int userId)
        {
            return _context.SupportAgents.FirstOrDefault(agent => agent.UserId == userId);
        }
    }
}
