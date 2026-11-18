using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces
{
    public interface ITicketStatusHistoryRepository
    {
        Task<List<TicketStatusHistory>> GetByTicketIdAsync(int ticketId);

        Task AddAsync(TicketStatusHistory history);
    }
}