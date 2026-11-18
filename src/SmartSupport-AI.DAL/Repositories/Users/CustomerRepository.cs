using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Users;

namespace SmartSupport_AI.DAL.Repositories.Users
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public Task<Customer?> GetByUserIdAsync(int userId)
        {
            return _context.Customers.FirstOrDefaultAsync(customer => customer.UserId == userId);
        }
    }
}
