using SmartSupport_AI.BLL.Constants;
using SmartSupport_AI.BLL.DTOs.Tickets;
using SmartSupport_AI.BLL.Services.Interfaces.Tickets;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces;
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
        private readonly ISupportTicketRepository _ticketRepository;

        public TicketService(ISupportTicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
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