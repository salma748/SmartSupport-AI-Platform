using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Conversations
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<List<Message>> GetByConversationIdAsync(int conversationId);
    }
}
