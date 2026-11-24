using SmartSupport_AI.BLL.DTOs.Tickets;

namespace SmartSupport_AI.BLL.Services.Interfaces.Tickets
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(CreateTicketDto dto);
        Task<TicketDto> ChangeStatusAsync(ChangeTicketStatusDto dto);
        Task<TicketAssignmentDto> AssignTicketAsync(AssignTicketDto dto);
        Task<TicketDto?> GetTicketAsync(int ticketId);

        Task<List<TicketDto>> GetTicketsByStatusAsync(string status);

        Task<List<TicketStatusHistoryDto>> GetStatusHistoryAsync(int ticketId);

        Task<List<TicketAssignmentDto>> GetAssignmentsAsync(int ticketId);
    }
}