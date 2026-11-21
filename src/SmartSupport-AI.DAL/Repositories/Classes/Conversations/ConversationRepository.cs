using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;

namespace SmartSupport_AI.DAL.Repositories.Classes.Conversations
{
    public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
    {
        public ConversationRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public async Task<List<Conversation>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Conversations
                .AsNoTracking()
                .Where(conversation => conversation.CustomerId == customerId)
                .OrderByDescending(conversation => conversation.StartedAt)
                .ThenByDescending(conversation => conversation.ConversationId)
                .ToListAsync();
        }
    }
}
