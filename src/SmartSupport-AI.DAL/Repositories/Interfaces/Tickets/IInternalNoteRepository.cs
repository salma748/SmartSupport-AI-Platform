using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Tickets
{
    public interface IInternalNoteRepository : IGenericRepository<InternalNote>
    {
        Task<List<InternalNote>> GetByTicketIdAsync(int ticketId);
    }
}