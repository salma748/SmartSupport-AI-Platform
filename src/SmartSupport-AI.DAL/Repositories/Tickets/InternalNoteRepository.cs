using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Tickets;

namespace SmartSupport_AI.DAL.Repositories.Tickets
{
    public class InternalNoteRepository : GenericRepository<InternalNote>, IInternalNoteRepository
    {
        public InternalNoteRepository(SmartSupportDbContext context) : base(context)
        {
        }

        public async Task<List<InternalNote>> GetByTicketIdAsync(int ticketId)
        {
            return await _context.InternalNotes
                .AsNoTracking()
                .Where(n => n.TicketId == ticketId)
                .OrderBy(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}