using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.DAL.Repositories.KnowledgeBase
{
    public class AIResponseRepository : GenericRepository<AIResponse>, IAIResponseRepository
    {
        public AIResponseRepository(SmartSupportDbContext context)
            : base(context)
        {
        }
    }
}
