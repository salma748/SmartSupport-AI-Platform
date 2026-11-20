using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Conversations
{
    public interface IConversationRepository : IGenericRepository<Conversation>
    {
        Task<List<Conversation>> GetByCustomerIdAsync(int customerId);
    }
}
