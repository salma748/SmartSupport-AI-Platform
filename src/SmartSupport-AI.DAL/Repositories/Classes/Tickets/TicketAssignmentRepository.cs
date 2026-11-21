using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Tickets;

namespace SmartSupport_AI.DAL.Repositories.Classes.Tickets
{
    public class TicketAssignmentRepository : GenericRepository<TicketAssignment>, ITicketAssignmentRepository
    {
        public TicketAssignmentRepository(SmartSupportDbContext context) : base(context)
        {
        }

        public async Task<TicketAssignment?> GetCurrentByTicketIdAsync(int ticketId)
        {
            return await _context.TicketAssignments
                .FirstOrDefaultAsync(a => a.TicketId == ticketId && a.IsCurrent);
        }

        public async Task<List<TicketAssignment>> GetByTicketIdAsync(int ticketId)
        {
            return await _context.TicketAssignments
                .AsNoTracking()
                .Where(a => a.TicketId == ticketId)
                .OrderByDescending(a => a.AssignedAt)
                .ToListAsync();
        }
    }
}