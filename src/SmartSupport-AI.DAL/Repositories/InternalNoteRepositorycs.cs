using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces;

namespace SmartSupport_AI.DAL.Repositories
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

        public async Task AddAsync(InternalNote note)
        {
            await _context.InternalNotes.AddAsync(note);
        }
    }
}