using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Users;

namespace SmartSupport_AI.DAL.Repositories.Classes.Users
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return _context.Users.AnyAsync(user => user.Email == email);
        }
    }
}
