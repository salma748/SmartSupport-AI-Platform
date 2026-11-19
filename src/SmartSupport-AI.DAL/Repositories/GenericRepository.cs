using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Repositories.Interfaces;

namespace SmartSupport_AI.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly SmartSupportDbContext _context;
        private readonly DbSet<T> _entities;

        public GenericRepository(SmartSupportDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(params object[] keyValues)
        {
            return await _entities.FindAsync(keyValues);
        }

        public Task<List<T>> GetAllAsync()
        {
            return _entities.ToListAsync();
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
