using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Tickets
{
    public interface ISupportTicketRepository : IGenericRepository<SupportTicket>
    {
        Task<List<SupportTicket>> GetByStatusAsync(string status);

        Task<SupportTicket?> GetWithDetailsAsync(int ticketId);
    }
}