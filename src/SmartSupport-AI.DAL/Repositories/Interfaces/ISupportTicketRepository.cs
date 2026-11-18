using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces
{
    public interface ISupportTicketRepository
    {
        Task<SupportTicket?> GetByIdAsync(int ticketId);

        Task<List<SupportTicket>> GetAllAsync();

        Task AddAsync(SupportTicket ticket);

        Task<int> SaveChangesAsync();
    }
}