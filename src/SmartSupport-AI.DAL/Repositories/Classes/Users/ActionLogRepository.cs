using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Users;

namespace SmartSupport_AI.DAL.Repositories.Classes.Users
{
    public class ActionLogRepository : GenericRepository<ActionLog>, IActionLogRepository
    {
        public ActionLogRepository(SmartSupportDbContext context)
            : base(context)
        {
        }
    }
}
