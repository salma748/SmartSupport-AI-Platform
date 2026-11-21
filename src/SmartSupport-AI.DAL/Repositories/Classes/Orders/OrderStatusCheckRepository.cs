using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Orders;

namespace SmartSupport_AI.DAL.Repositories.Classes.Orders
{
    public class OrderStatusCheckRepository : GenericRepository<OrderStatusCheck>, IOrderStatusCheckRepository
    {
        public OrderStatusCheckRepository(SmartSupportDbContext context) : base(context)
        {
        }

        public async Task<List<OrderStatusCheck>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderStatusChecks
                .AsNoTracking()
                .Where(c => c.OrderId == orderId)
                .OrderByDescending(c => c.CheckedAt)
                .ToListAsync();
        }

        public async Task<List<OrderStatusCheck>> GetByConversationIdAsync(int conversationId)
        {
            return await _context.OrderStatusChecks
                .AsNoTracking()
                .Where(c => c.ConversationId == conversationId)
                .OrderBy(c => c.CheckedAt)
                .ToListAsync();
        }

        public async Task<OrderStatusCheck?> GetLatestByOrderIdAsync(int orderId)
        {
            return await _context.OrderStatusChecks
                .AsNoTracking()
                .Where(c => c.OrderId == orderId)
                .OrderByDescending(c => c.CheckedAt)
                .FirstOrDefaultAsync();
        }
    }
}
