namespace SmartSupport_AI.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T? GetById(params object[] keyValues);

        List<T> GetAll();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        int SaveChanges();
    }
}
