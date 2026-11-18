using Microsoft.EntityFrameworkCore;
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

        public Task<SupportAgent?> GetByUserIdAsync(int userId)
        {
            return _context.SupportAgents.FirstOrDefaultAsync(agent => agent.UserId == userId);
        }
    }
}
