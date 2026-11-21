using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Orders;

namespace SmartSupport_AI.DAL.Repositories.Classes.Orders
{
    public class RefundRequestRepository : GenericRepository<RefundRequest>, IRefundRequestRepository
    {
        public RefundRequestRepository(SmartSupportDbContext context) : base(context)
        {
        }

        public async Task<RefundRequest?> GetWithHistoryAsync(int refundId)
        {
            return await _context.RefundRequests
                .Include(r => r.StatusHistory)
                .FirstOrDefaultAsync(r => r.RefundId == refundId);
        }

        public async Task<List<RefundRequest>> GetByOrderIdAsync(int orderId)
        {
            return await _context.RefundRequests
                .AsNoTracking()
                .Where(r => r.OrderId == orderId)
                .OrderByDescending(r => r.InitiatedAt)
                .ToListAsync();
        }

        public async Task<List<RefundRequest>> GetByConversationIdAsync(int conversationId)
        {
            return await _context.RefundRequests
                .AsNoTracking()
                .Where(r => r.ConversationId == conversationId)
                .OrderBy(r => r.InitiatedAt)
                .ToListAsync();
        }

        public async Task<List<RefundRequest>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.RefundRequests
                .AsNoTracking()
                .Where(r => r.Order.CustomerId == customerId)
                .OrderByDescending(r => r.InitiatedAt)
                .ToListAsync();
        }

        public async Task<List<RefundRequest>> GetByStatusAsync(string status)
        {
            return await _context.RefundRequests
                .AsNoTracking()
                .Where(r => r.Status == status)
                .OrderBy(r => r.InitiatedAt)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRefundedAmountAsync(int orderId, IEnumerable<string> statuses)
        {
            var statusList = statuses.ToList();

            return await _context.RefundRequests
                .Where(r => r.OrderId == orderId && statusList.Contains(r.Status))
                .SumAsync(r => r.Amount);
        }
    }
}
