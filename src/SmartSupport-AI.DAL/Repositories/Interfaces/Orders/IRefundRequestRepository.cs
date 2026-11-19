using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Orders
{
    public interface IRefundRequestRepository : IGenericRepository<RefundRequest>
    {
        Task<RefundRequest?> GetWithHistoryAsync(int refundId);

        Task<List<RefundRequest>> GetByOrderIdAsync(int orderId);

        Task<List<RefundRequest>> GetByConversationIdAsync(int conversationId);

        // Customer is derived through the refund's Order.
        Task<List<RefundRequest>> GetByCustomerIdAsync(int customerId);

        Task<List<RefundRequest>> GetByStatusAsync(string status);

        // Sums the amounts of the order's refunds whose Status is in the given list
        // (the caller decides which statuses count as "refunded"). Returns 0 when none match.
        Task<decimal> GetTotalRefundedAmountAsync(int orderId, IEnumerable<string> statuses);
    }
}
