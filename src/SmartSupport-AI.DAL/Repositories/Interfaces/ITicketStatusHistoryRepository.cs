using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces
{
    public interface ITicketStatusHistoryRepository : IGenericRepository<TicketStatusHistory>
    {
        Task<List<TicketStatusHistory>> GetByTicketIdAsync(int ticketId);
    }
}