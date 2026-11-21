using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Tickets;

namespace SmartSupport_AI.DAL.Repositories.Classes.Tickets
{
    public class TicketStatusHistoryRepository : GenericRepository<TicketStatusHistory>, ITicketStatusHistoryRepository
    {
        public TicketStatusHistoryRepository(SmartSupportDbContext context) : base(context)
        {
        }

        public async Task<List<TicketStatusHistory>> GetByTicketIdAsync(int ticketId)
        {
            return await _context.TicketStatusHistories
                .AsNoTracking()
                .Where(h => h.TicketId == ticketId)
                .OrderBy(h => h.ChangedAt)
                .ToListAsync();
        }
    }
}