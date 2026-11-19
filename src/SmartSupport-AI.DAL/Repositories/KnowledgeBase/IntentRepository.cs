using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.DAL.Repositories.KnowledgeBase
{
    public class IntentRepository : GenericRepository<Intent>, IIntentRepository
    {
        public IntentRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public async Task<Intent?> GetByNameAsync(string name)
        {
            return await _context.Intents.FirstOrDefaultAsync(intent => intent.Name == name);
        }
    }
}
