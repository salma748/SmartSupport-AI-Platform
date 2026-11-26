using SmartSupport_AI.BLL.DTOs.Orders;

namespace SmartSupport_AI.BLL.Services.Interfaces.Orders
{
    public interface IRefundService
    {
        Task<RefundRequestDto> RequestRefundAsync(CreateRefundRequestDto dto);

        Task<RefundRequestDto> ChangeStatusAsync(ChangeRefundStatusDto dto);

        Task<RefundRequestDto?> GetRefundAsync(int refundId);

        Task<List<RefundRequestDto>> GetRefundsByOrderAsync(int orderId);

        Task<List<RefundRequestDto>> GetRefundsByCustomerAsync(int customerId);

        Task<List<RefundRequestDto>> GetRefundsByStatusAsync(string status);

        Task<List<RefundStatusHistoryDto>> GetStatusHistoryAsync(int refundId);
    }
}
