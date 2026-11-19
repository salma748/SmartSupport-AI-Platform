using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Orders;

namespace SmartSupport_AI.DAL.Repositories.Orders
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(SmartSupportDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<Order?> GetCustomerOrderAsync(string orderNumber, int customerId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber && o.CustomerId == customerId);
        }

        public async Task<List<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.PlacedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetWithRefundRequestsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.RefundRequests)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<bool> OrderNumberExistsAsync(string orderNumber)
        {
            return await _context.Orders
                .AnyAsync(o => o.OrderNumber == orderNumber);
        }
    }
}
