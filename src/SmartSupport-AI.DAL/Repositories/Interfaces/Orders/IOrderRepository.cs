using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Orders
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetByOrderNumberAsync(string orderNumber);

        // Returns null when the order does not exist OR belongs to another customer,
        // so the service can return a non-disclosing failure.
        Task<Order?> GetCustomerOrderAsync(string orderNumber, int customerId);

        Task<List<Order>> GetByCustomerIdAsync(int customerId);

        Task<Order?> GetWithRefundRequestsAsync(int orderId);

        Task<bool> OrderNumberExistsAsync(string orderNumber);
    }
}
