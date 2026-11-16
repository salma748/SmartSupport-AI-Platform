using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User? GetByEmail(string email);

        bool EmailExists(string email);
    }
}
