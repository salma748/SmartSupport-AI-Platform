using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;

namespace SmartSupport_AI.DAL.Repositories.Classes.Conversations
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public async Task<List<Message>> GetByConversationIdAsync(int conversationId)
        {
            return await _context.Messages
                .AsNoTracking()
                .Where(message => message.ConversationId == conversationId)
                .OrderBy(message => message.SequenceNo)
                .ToListAsync();
        }
    }
}
