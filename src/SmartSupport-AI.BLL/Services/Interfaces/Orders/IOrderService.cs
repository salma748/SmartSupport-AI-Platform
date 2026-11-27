using SmartSupport_AI.BLL.DTOs.Orders;

namespace SmartSupport_AI.BLL.Services.Interfaces.Orders
{
    public interface IOrderService
    {
        Task<OrderStatusCheckDto> CheckOrderStatusAsync(CheckOrderStatusDto dto);

        Task<OrderDto?> GetCustomerOrderAsync(string orderNumber, int customerId);

        Task<List<OrderDto>> GetOrdersByCustomerAsync(int customerId);

        Task<List<OrderStatusCheckDto>> GetStatusChecksAsync(int orderId);
    }
}
