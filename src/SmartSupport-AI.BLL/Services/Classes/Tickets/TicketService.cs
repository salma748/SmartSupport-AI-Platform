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
        private readonly ITicketAssignmentRepository _assignmentRepository;

        public TicketService(
            ISupportTicketRepository ticketRepository,
            ITicketStatusHistoryRepository historyRepository,
            ITicketAssignmentRepository assignmentRepository)
        {
            _ticketRepository = ticketRepository;
            _historyRepository = historyRepository;
            _assignmentRepository = assignmentRepository;
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

        public async Task<TicketAssignmentDto> AssignTicketAsync(AssignTicketDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(dto.TicketId);

            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket {dto.TicketId} was not found");
            }

            var current = await _assignmentRepository.GetCurrentByTicketIdAsync(dto.TicketId);

            if (current != null)
            {
                if (current.AgentId == dto.AgentId)
                {
                    return MapToDto(current);
                }

                current.IsCurrent = false;
                await _assignmentRepository.SaveChangesAsync();
            }

            var assignment = new TicketAssignment
            {
                TicketId = dto.TicketId,
                AgentId = dto.AgentId,
                AssignedAt = DateTime.UtcNow,
                IsCurrent = true
            };

            _assignmentRepository.Add(assignment);
            await _assignmentRepository.SaveChangesAsync();

            return MapToDto(assignment);
        }

        public async Task<TicketDto?> GetTicketAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);

            return ticket == null ? null : MapToDto(ticket);
        }

        public async Task<List<TicketDto>> GetTicketsByStatusAsync(string status)
        {
            if (!_allowedTransitions.ContainsKey(status))
            {
                throw new ArgumentException($"Invalid status: {status}");
            }

            var tickets = await _ticketRepository.GetByStatusAsync(status);

            return tickets.Select(t => MapToDto(t)).ToList();
        }

        public async Task<List<TicketStatusHistoryDto>> GetStatusHistoryAsync(int ticketId)
        {
            var history = await _historyRepository.GetByTicketIdAsync(ticketId);

            return history.Select(h => MapToDto(h)).ToList();
        }

        public async Task<List<TicketAssignmentDto>> GetAssignmentsAsync(int ticketId)
        {
            var assignments = await _assignmentRepository.GetByTicketIdAsync(ticketId);

            return assignments.Select(a => MapToDto(a)).ToList();
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

        private static TicketAssignmentDto MapToDto(TicketAssignment assignment)
        {
            return new TicketAssignmentDto
            {
                AssignmentId = assignment.AssignmentId,
                TicketId = assignment.TicketId,
                AgentId = assignment.AgentId,
                AssignedAt = assignment.AssignedAt,
                IsCurrent = assignment.IsCurrent
            };
        }

        private static TicketStatusHistoryDto MapToDto(TicketStatusHistory history)
        {
            return new TicketStatusHistoryDto
            {
                HistoryId = history.HistoryId,
                TicketId = history.TicketId,
                ChangedByUserId = history.ChangedBy,
                OldStatus = history.OldStatus,
                NewStatus = history.NewStatus,
                ChangedAt = history.ChangedAt
            };
        }
    }
}