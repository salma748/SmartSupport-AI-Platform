namespace SmartSupport_AI.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(params object[] keyValues);

        Task<List<T>> GetAllAsync();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<int> SaveChangesAsync();
    }
}
