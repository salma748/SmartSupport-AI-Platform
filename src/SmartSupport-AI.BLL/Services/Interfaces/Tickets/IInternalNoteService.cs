using SmartSupport_AI.BLL.DTOs.Tickets;

namespace SmartSupport_AI.BLL.Services.Interfaces.Tickets
{
    public interface IInternalNoteService
    {
        Task<InternalNoteDto> AddNoteAsync(AddInternalNoteDto dto);

        Task<List<InternalNoteDto>> GetNotesByTicketAsync(int ticketId);
    }
}