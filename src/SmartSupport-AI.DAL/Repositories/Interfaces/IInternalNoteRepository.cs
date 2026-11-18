using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces
{
    public interface IInternalNoteRepository
    {
        Task<List<InternalNote>> GetByTicketIdAsync(int ticketId);

        Task AddAsync(InternalNote note);
    }
}