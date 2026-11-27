using SmartSupport_AI.BLL.Constants;
using SmartSupport_AI.BLL.DTOs.Orders;
using SmartSupport_AI.BLL.Services.Interfaces.Orders;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;
using SmartSupport_AI.DAL.Repositories.Interfaces.Orders;

namespace SmartSupport_AI.BLL.Services.Classes.Orders
{
    public class RefundService : IRefundService
    {
        // Order states that can be refunded (to be confirmed with the business owner).
        private static readonly string[] _refundableOrderStatuses =
        {
            OrderStatuses.Delivered
        };

        // Refund states whose amount is still counted against the order total.
        private static readonly string[] _amountHoldingStatuses =
        {
            RefundStatuses.Pending,
            RefundStatuses.Approved,
            RefundStatuses.Completed
        };

        private static readonly Dictionary<string, string[]> _allowedTransitions = new()
        {
            [RefundStatuses.Pending] = new[] { RefundStatuses.Approved, RefundStatuses.Rejected, RefundStatuses.Cancelled },
            [RefundStatuses.Approved] = new[] { RefundStatuses.Completed, RefundStatuses.Failed },
            [RefundStatuses.Rejected] = Array.Empty<string>(),
            [RefundStatuses.Completed] = Array.Empty<string>(),
            [RefundStatuses.Failed] = Array.Empty<string>(),
            [RefundStatuses.Cancelled] = Array.Empty<string>()
        };

        private readonly IRefundRequestRepository _refundRepository;
        private readonly IRefundStatusHistoryRepository _historyRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IConversationRepository _conversationRepository;

        public RefundService(
            IRefundRequestRepository refundRepository,
            IRefundStatusHistoryRepository historyRepository,
            IOrderRepository orderRepository,
            IConversationRepository conversationRepository)
        {
            _refundRepository = refundRepository;
            _historyRepository = historyRepository;
            _orderRepository = orderRepository;
            _conversationRepository = conversationRepository;
        }

        public async Task<RefundRequestDto> RequestRefundAsync(CreateRefundRequestDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (string.IsNullOrWhiteSpace(dto.OrderNumber))
            {
                throw new ArgumentException("Order number cannot be empty");
            }

            if (dto.Amount <= 0)
            {
                throw new ArgumentException("Refund amount must be greater than zero");
            }

            if (string.IsNullOrWhiteSpace(dto.Reason))
            {
                throw new ArgumentException("A reason is required for a refund request");
            }

            // Same failure for an unknown order and for another customer's order.
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

            if (!_refundableOrderStatuses.Contains(order.OrderStatus))
            {
                throw new InvalidOperationException(
                    $"Order {order.OrderNumber} is not eligible for a refund (status: '{order.OrderStatus}')");
            }

            var alreadyRefunded = await _refundRepository.GetTotalRefundedAmountAsync(
                order.OrderId, _amountHoldingStatuses);

            if (alreadyRefunded >= order.TotalAmount)
            {
                throw new InvalidOperationException($"Order {order.OrderNumber} has already been fully refunded");
            }

            if (alreadyRefunded + dto.Amount > order.TotalAmount)
            {
                throw new InvalidOperationException(
                    $"Refund amount exceeds the remaining refundable amount ({order.TotalAmount - alreadyRefunded})");
            }

            var now = DateTime.UtcNow;

            var refund = new RefundRequest
            {
                OrderId = order.OrderId,
                ConversationId = dto.ConversationId,
                ExecutionId = dto.ExecutionId,
                Amount = dto.Amount,
                Reason = dto.Reason.Trim(),
                Eligibility = RefundEligibilities.Eligible,
                Status = RefundStatuses.Pending,
                InitiatedAt = now
            };

            // The first history row is saved together with the request in a single SaveChanges.
            refund.StatusHistory.Add(new RefundStatusHistory
            {
                OldStatus = string.Empty,
                NewStatus = RefundStatuses.Pending,
                ChangedAt = now
            });

            _refundRepository.Add(refund);
            await _refundRepository.SaveChangesAsync();

            return MapToDto(refund);
        }

        public async Task<RefundRequestDto> ChangeStatusAsync(ChangeRefundStatusDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var refund = await _refundRepository.GetByIdAsync(dto.RefundId);

            if (refund == null)
            {
                throw new KeyNotFoundException($"Refund request {dto.RefundId} was not found");
            }

            if (!_allowedTransitions.TryGetValue(refund.Status, out var allowed)
                || !allowed.Contains(dto.NewStatus))
            {
                throw new InvalidOperationException(
                    $"Cannot change status from '{refund.Status}' to '{dto.NewStatus}'");
            }

            var history = new RefundStatusHistory
            {
                RefundId = refund.RefundId,
                OldStatus = refund.Status,
                NewStatus = dto.NewStatus,
                ChangedAt = DateTime.UtcNow
            };

            refund.Status = dto.NewStatus;

            _historyRepository.Add(history);
            await _refundRepository.SaveChangesAsync();

            return MapToDto(refund);
        }

        public async Task<RefundRequestDto?> GetRefundAsync(int refundId)
        {
            var refund = await _refundRepository.GetByIdAsync(refundId);

            return refund == null ? null : MapToDto(refund);
        }

        public async Task<List<RefundRequestDto>> GetRefundsByOrderAsync(int orderId)
        {
            var refunds = await _refundRepository.GetByOrderIdAsync(orderId);

            return refunds.Select(r => MapToDto(r)).ToList();
        }

        public async Task<List<RefundRequestDto>> GetRefundsByCustomerAsync(int customerId)
        {
            var refunds = await _refundRepository.GetByCustomerIdAsync(customerId);

            return refunds.Select(r => MapToDto(r)).ToList();
        }

        public async Task<List<RefundRequestDto>> GetRefundsByStatusAsync(string status)
        {
            ArgumentNullException.ThrowIfNull(status);

            if (!_allowedTransitions.ContainsKey(status))
            {
                throw new ArgumentException($"Invalid status: {status}");
            }

            var refunds = await _refundRepository.GetByStatusAsync(status);

            return refunds.Select(r => MapToDto(r)).ToList();
        }

        public async Task<List<RefundStatusHistoryDto>> GetStatusHistoryAsync(int refundId)
        {
            var history = await _historyRepository.GetByRefundIdAsync(refundId);

            return history.Select(h => MapToDto(h)).ToList();
        }

        private static RefundRequestDto MapToDto(RefundRequest refund)
        {
            return new RefundRequestDto
            {
                RefundId = refund.RefundId,
                OrderId = refund.OrderId,
                ConversationId = refund.ConversationId,
                ExecutionId = refund.ExecutionId,
                Amount = refund.Amount,
                Reason = refund.Reason,
                Eligibility = refund.Eligibility,
                Status = refund.Status,
                InitiatedAt = refund.InitiatedAt
            };
        }

        private static RefundStatusHistoryDto MapToDto(RefundStatusHistory history)
        {
            return new RefundStatusHistoryDto
            {
                HistoryId = history.HistoryId,
                RefundId = history.RefundId,
                OldStatus = history.OldStatus,
                NewStatus = history.NewStatus,
                ChangedAt = history.ChangedAt
            };
        }
    }
}
