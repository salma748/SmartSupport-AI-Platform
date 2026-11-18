using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces
{
    public interface ITicketAssignmentRepository
    {
        Task<TicketAssignment?> GetCurrentByTicketIdAsync(int ticketId);

        Task<List<TicketAssignment>> GetByTicketIdAsync(int ticketId);

        Task AddAsync(TicketAssignment assignment);
    }
}