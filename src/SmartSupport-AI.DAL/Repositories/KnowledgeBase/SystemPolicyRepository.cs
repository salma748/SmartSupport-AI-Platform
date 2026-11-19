using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.DAL.Repositories.KnowledgeBase
{
    public class SystemPolicyRepository : GenericRepository<SystemPolicy>, ISystemPolicyRepository
    {
        public SystemPolicyRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public async Task<SystemPolicy?> GetByPolicyNameAsync(string policyName)
        {
            return await _context.SystemPolicies
                .FirstOrDefaultAsync(policy => policy.PolicyName == policyName);
        }
    }
}
