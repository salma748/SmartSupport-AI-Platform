using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Tickets;

namespace SmartSupport_AI.DAL.Repositories.Tickets
{
    public class SupportTicketRepository : GenericRepository<SupportTicket>, ISupportTicketRepository
    {
        public SupportTicketRepository(SmartSupportDbContext context) : base(context)
        {
        }

        public async Task<List<SupportTicket>> GetByStatusAsync(string status)
        {
            return await _context.SupportTickets
                .AsNoTracking()
                .Where(t => t.Status == status)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<SupportTicket?> GetWithDetailsAsync(int ticketId)
        {
            return await _context.SupportTickets
                .Include(t => t.Assignments)
                .Include(t => t.StatusHistory)
                .Include(t => t.InternalNotes)
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }
    }
}