using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Orders;

namespace SmartSupport_AI.DAL.Repositories.Orders
{
    public class RefundStatusHistoryRepository : GenericRepository<RefundStatusHistory>, IRefundStatusHistoryRepository
    {
        public RefundStatusHistoryRepository(SmartSupportDbContext context) : base(context)
        {
        }

        public async Task<List<RefundStatusHistory>> GetByRefundIdAsync(int refundId)
        {
            return await _context.RefundStatusHistories
                .AsNoTracking()
                .Where(h => h.RefundId == refundId)
                .OrderBy(h => h.ChangedAt)
                .ToListAsync();
        }

        public async Task<RefundStatusHistory?> GetLatestByRefundIdAsync(int refundId)
        {
            return await _context.RefundStatusHistories
                .AsNoTracking()
                .Where(h => h.RefundId == refundId)
                .OrderByDescending(h => h.ChangedAt)
                .FirstOrDefaultAsync();
        }
    }
}
