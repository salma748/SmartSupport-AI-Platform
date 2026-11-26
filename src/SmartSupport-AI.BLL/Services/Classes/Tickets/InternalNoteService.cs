using SmartSupport_AI.BLL.DTOs.Tickets;
using SmartSupport_AI.BLL.Services.Interfaces.Tickets;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Tickets;

namespace SmartSupport_AI.BLL.Services.Classes.Tickets
{
    public class InternalNoteService : IInternalNoteService
    {
        private readonly IInternalNoteRepository _noteRepository;
        private readonly ISupportTicketRepository _ticketRepository;

        public InternalNoteService(
            IInternalNoteRepository noteRepository,
            ISupportTicketRepository ticketRepository)
        {
            _noteRepository = noteRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<InternalNoteDto> AddNoteAsync(AddInternalNoteDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                throw new ArgumentException("Note content cannot be empty");
            }

            var ticket = await _ticketRepository.GetByIdAsync(dto.TicketId);

            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket {dto.TicketId} was not found");
            }

            var note = new InternalNote
            {
                TicketId = dto.TicketId,
                AgentId = dto.AgentId,
                Content = dto.Content.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _noteRepository.Add(note);
            await _noteRepository.SaveChangesAsync();

            return MapToDto(note);
        }

        public async Task<List<InternalNoteDto>> GetNotesByTicketAsync(int ticketId)
        {
            var notes = await _noteRepository.GetByTicketIdAsync(ticketId);

            return notes.Select(MapToDto).ToList();
        }

        private static InternalNoteDto MapToDto(InternalNote note)
        {
            return new InternalNoteDto
            {
                NoteId = note.NoteId,
                TicketId = note.TicketId,
                AgentId = note.AgentId,
                Content = note.Content,
                CreatedAt = note.CreatedAt
            };
        }
    }
}