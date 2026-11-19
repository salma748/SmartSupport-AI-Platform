using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces
{
    public interface ITicketAssignmentRepository : IGenericRepository<TicketAssignment>
    {
        Task<TicketAssignment?> GetCurrentByTicketIdAsync(int ticketId);

        Task<List<TicketAssignment>> GetByTicketIdAsync(int ticketId);
    }
}