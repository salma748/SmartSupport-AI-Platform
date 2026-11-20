using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.DAL.Repositories.KnowledgeBase
{
    public class ToolExecutionRepository : GenericRepository<ToolExecution>, IToolExecutionRepository
    {
        public ToolExecutionRepository(SmartSupportDbContext context)
            : base(context)
        {
        }
    }
}
