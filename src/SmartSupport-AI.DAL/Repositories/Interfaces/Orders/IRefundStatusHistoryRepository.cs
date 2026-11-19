using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Orders
{
    public interface IRefundStatusHistoryRepository : IGenericRepository<RefundStatusHistory>
    {
        Task<List<RefundStatusHistory>> GetByRefundIdAsync(int refundId);

        Task<RefundStatusHistory?> GetLatestByRefundIdAsync(int refundId);
    }
}
