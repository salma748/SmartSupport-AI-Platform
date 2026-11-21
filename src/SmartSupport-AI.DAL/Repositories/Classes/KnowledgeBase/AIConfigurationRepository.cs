using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.DAL.Repositories.Classes.KnowledgeBase
{
    public class AIConfigurationRepository : GenericRepository<AIConfiguration>, IAIConfigurationRepository
    {
        public AIConfigurationRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public async Task<AIConfiguration?> GetByVersionNumberAsync(string versionNumber)
        {
            return await _context.AIConfigurations
                .FirstOrDefaultAsync(configuration => configuration.VersionNumber == versionNumber);
        }
    }
}
