using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces;

namespace SmartSupport_AI.DAL.Repositories
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
                .Where(a => a.TicketId == ticketId)
                .OrderByDescending(a => a.AssignedAt)
                .ToListAsync();
        }

        public async Task AddAsync(TicketAssignment assignment)
        {
            await _context.TicketAssignments.AddAsync(assignment);
        }
    }
}