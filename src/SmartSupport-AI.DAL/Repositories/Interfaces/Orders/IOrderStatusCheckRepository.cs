using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Orders
{
    public interface IOrderStatusCheckRepository : IGenericRepository<OrderStatusCheck>
    {
        Task<List<OrderStatusCheck>> GetByOrderIdAsync(int orderId);

        Task<List<OrderStatusCheck>> GetByConversationIdAsync(int conversationId);

        Task<OrderStatusCheck?> GetLatestByOrderIdAsync(int orderId);
    }
}
