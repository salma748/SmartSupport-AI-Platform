using SmartSupport_AI.BLL.Constants;
using SmartSupport_AI.BLL.DTOs.Tickets;
using SmartSupport_AI.BLL.Services.Interfaces.Tickets;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Tickets;

namespace SmartSupport_AI.BLL.Services.Classes.Tickets
{
    public class TicketService : ITicketService
    {
        private static readonly string[] _validPriorities =
        {
            TicketPriorities.Low,
            TicketPriorities.Medium,
            TicketPriorities.High
        };

        private static readonly Dictionary<string, string[]> _allowedTransitions = new()
        {
            [TicketStatuses.Open] = new[] { TicketStatuses.InProgress },
            [TicketStatuses.InProgress] = new[] { TicketStatuses.Resolved },
            [TicketStatuses.Resolved] = new[] { TicketStatuses.Closed, TicketStatuses.InProgress },
            [TicketStatuses.Closed] = Array.Empty<string>()
        };

        private readonly ISupportTicketRepository _ticketRepository;
        private readonly ITicketStatusHistoryRepository _historyRepository;

        public TicketService(
            ISupportTicketRepository ticketRepository,
            ITicketStatusHistoryRepository historyRepository)
        {
            _ticketRepository = ticketRepository;
            _historyRepository = historyRepository;
        }

        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto dto)
        {
            if (!_validPriorities.Contains(dto.Priority))
            {
                throw new ArgumentException($"Invalid priority: {dto.Priority}");
            }

            var ticket = new SupportTicket
            {
                ConversationId = dto.ConversationId,
                Priority = dto.Priority,
                Status = TicketStatuses.Open,
                EscalationReason = dto.EscalationReason,
                ConvSummary = dto.ConvSummary,
                CreatedAt = DateTime.UtcNow
            };

            _ticketRepository.Add(ticket);
            await _ticketRepository.SaveChangesAsync();

            return MapToDto(ticket);
        }

        public async Task<TicketDto> ChangeStatusAsync(ChangeTicketStatusDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(dto.TicketId);

            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket {dto.TicketId} was not found");
            }

            if (!_allowedTransitions.TryGetValue(ticket.Status, out var allowed)
                || !allowed.Contains(dto.NewStatus))
            {
                throw new InvalidOperationException(
                    $"Cannot change status from '{ticket.Status}' to '{dto.NewStatus}'");
            }

            bool isReopen = ticket.Status == TicketStatuses.Resolved
                            && dto.NewStatus == TicketStatuses.InProgress;

            if (isReopen && string.IsNullOrWhiteSpace(dto.Reason))
            {
                throw new ArgumentException("A reason is required to reopen a ticket");
            }

            var history = new TicketStatusHistory
            {
                TicketId = ticket.TicketId,
                ChangedBy = dto.ChangedByUserId,
                OldStatus = ticket.Status,
                NewStatus = dto.NewStatus,
                ChangedAt = DateTime.UtcNow
            };

            ticket.Status = dto.NewStatus;

            _historyRepository.Add(history);
            await _ticketRepository.SaveChangesAsync();

            return MapToDto(ticket);
        }

        private static TicketDto MapToDto(SupportTicket ticket)
        {
            return new TicketDto
            {
                TicketId = ticket.TicketId,
                ConversationId = ticket.ConversationId,
                Priority = ticket.Priority,
                Status = ticket.Status,
                EscalationReason = ticket.EscalationReason,
                ConvSummary = ticket.ConvSummary,
                CreatedAt = ticket.CreatedAt
            };
        }
    }
}