using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;

namespace SmartSupport_AI.DAL.Repositories.Conversations
{
    public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
    {
        public ConversationRepository(SmartSupportDbContext context)
            : base(context)
        {
        }
    }
}
