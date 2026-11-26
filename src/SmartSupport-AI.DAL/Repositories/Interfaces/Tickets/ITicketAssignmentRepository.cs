using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Tickets
{
    public interface ITicketAssignmentRepository : IGenericRepository<TicketAssignment>
    {
        Task<TicketAssignment?> GetCurrentByTicketIdAsync(int ticketId);
        Task<TicketAssignment> ReassignAsync(int ticketId, int newAgentId);
        Task<List<TicketAssignment>> GetByTicketIdAsync(int ticketId);
    }
}