using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);
    }
}
