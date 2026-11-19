using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Users;

namespace SmartSupport_AI.DAL.Repositories.Users
{
    public class AdministratorRepository : GenericRepository<Administrator>, IAdministratorRepository
    {
        public AdministratorRepository(SmartSupportDbContext context)
            : base(context)
        {
        }
    }
}
