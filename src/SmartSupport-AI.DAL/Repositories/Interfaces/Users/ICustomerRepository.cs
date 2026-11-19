using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Users
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetByUserIdAsync(int userId);
    }
}
