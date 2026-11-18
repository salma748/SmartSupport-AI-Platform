using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces;

namespace SmartSupport_AI.DAL.Repositories
{
    public class SupportTicketRepository
        : GenericRepository<SupportTicket>, ISupportTicketRepository
    {
        public SupportTicketRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public async Task<SupportTicket?> GetByIdAsync(int ticketId)
        {
            return await _context.SupportTickets.FindAsync(ticketId);
        }

        public async Task<List<SupportTicket>> GetAllAsync()
        {
            return await _context.SupportTickets.ToListAsync();
        }

        public async Task AddAsync(SupportTicket ticket)
        {
            await _context.SupportTickets.AddAsync(ticket);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}