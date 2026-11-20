using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.DAL.Repositories.KnowledgeBase
{
    public class PerformanceMetricRepository : GenericRepository<PerformanceMetric>, IPerformanceMetricRepository
    {
        public PerformanceMetricRepository(SmartSupportDbContext context)
            : base(context)
        {
        }
    }
}
