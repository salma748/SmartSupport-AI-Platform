using SmartSupport_AI.BLL.DTOs.Orders;
using SmartSupport_AI.BLL.Services.Interfaces.Orders;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;
using SmartSupport_AI.DAL.Repositories.Interfaces.Orders;

namespace SmartSupport_AI.BLL.Services.Classes.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderStatusCheckRepository _checkRepository;
        private readonly IConversationRepository _conversationRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderStatusCheckRepository checkRepository,
            IConversationRepository conversationRepository)
        {
            _orderRepository = orderRepository;
            _checkRepository = checkRepository;
            _conversationRepository = conversationRepository;
        }

        public async Task<OrderStatusCheckDto> CheckOrderStatusAsync(CheckOrderStatusDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (string.IsNullOrWhiteSpace(dto.OrderNumber))
            {
                throw new ArgumentException("Order number cannot be empty");
            }

            // Same failure for an unknown order and for another customer's order,
            // so the response never reveals whether the order exists.
            var order = await _orderRepository.GetCustomerOrderAsync(dto.OrderNumber.Trim(), dto.CustomerId);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order {dto.OrderNumber.Trim()} was not found");
            }

            var conversation = await _conversationRepository.GetByIdAsync(dto.ConversationId);

            if (conversation == null || conversation.CustomerId != dto.CustomerId)
            {
                throw new KeyNotFoundException($"Conversation {dto.ConversationId} was not found");
            }

            var check = new OrderStatusCheck
            {
                OrderId = order.OrderId,
                ConversationId = dto.ConversationId,
                ExecutionId = dto.ExecutionId,
                StatusReturned = order.OrderStatus,
                CheckedAt = DateTime.UtcNow
            };

            _checkRepository.Add(check);
            await _checkRepository.SaveChangesAsync();

            return MapToDto(check);
        }

        public async Task<OrderDto?> GetCustomerOrderAsync(string orderNumber, int customerId)
        {
            ArgumentNullException.ThrowIfNull(orderNumber);

            var order = await _orderRepository.GetCustomerOrderAsync(orderNumber.Trim(), customerId);

            return order == null ? null : MapToDto(order);
        }

        public async Task<List<OrderDto>> GetOrdersByCustomerAsync(int customerId)
        {
            var orders = await _orderRepository.GetByCustomerIdAsync(customerId);

            return orders.Select(o => MapToDto(o)).ToList();
        }

        public async Task<List<OrderStatusCheckDto>> GetStatusChecksAsync(int orderId)
        {
            var checks = await _checkRepository.GetByOrderIdAsync(orderId);

            return checks.Select(c => MapToDto(c)).ToList();
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus,
                TotalAmount = order.TotalAmount,
                PlacedAt = order.PlacedAt
            };
        }

        private static OrderStatusCheckDto MapToDto(OrderStatusCheck check)
        {
            return new OrderStatusCheckDto
            {
                CheckId = check.CheckId,
                OrderId = check.OrderId,
                ConversationId = check.ConversationId,
                ExecutionId = check.ExecutionId,
                StatusReturned = check.StatusReturned,
                CheckedAt = check.CheckedAt
            };
        }
    }
}
