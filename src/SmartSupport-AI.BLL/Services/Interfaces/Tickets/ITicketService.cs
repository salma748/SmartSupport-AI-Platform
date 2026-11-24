using SmartSupport_AI.BLL.DTOs.Tickets;

namespace SmartSupport_AI.BLL.Services.Interfaces.Tickets
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(CreateTicketDto dto);
        Task<TicketDto> ChangeStatusAsync(ChangeTicketStatusDto dto);
    }
}