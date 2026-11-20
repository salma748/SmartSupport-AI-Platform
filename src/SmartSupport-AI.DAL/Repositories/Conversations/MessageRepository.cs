using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;

namespace SmartSupport_AI.DAL.Repositories.Conversations
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(SmartSupportDbContext context)
            : base(context)
        {
        }
    }
}
