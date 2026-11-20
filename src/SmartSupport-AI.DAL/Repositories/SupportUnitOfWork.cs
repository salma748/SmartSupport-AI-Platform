using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;

namespace SmartSupport_AI.DAL.Repositories;

public sealed class SupportUnitOfWork(SmartSupportDbContext db)
{
    public IQueryable<T> Query<T>() where T : class => db.Set<T>();
    public void Add<T>(T entity) where T : class => new GenericRepository<T>(db).Add(entity);
    public Task<int> SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);

    public async Task<T> AtomicAsync<T>(string resource, Func<Task<T>> operation, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        try
        {
            await LockAsync(resource, ct);
            var result = await operation();
            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return result;
        }
        catch
        {
            db.ChangeTracker.Clear();
            throw;
        }
    }
    public Task ReloadAsync<T>(T entity, CancellationToken ct) where T : class => db.Entry(entity).ReloadAsync(ct);

    public Task<int> LockAsync(string resource, CancellationToken ct) =>
        db.Database.ExecuteSqlInterpolatedAsync($"""
                DECLARE @result int;
                EXEC @result = sp_getapplock @Resource = {resource}, @LockMode = 'Exclusive',
                    @LockOwner = 'Transaction', @LockTimeout = 5000;
                IF @result < 0 THROW 51000, 'Support operation lock unavailable', 1;
                """, ct);
}
