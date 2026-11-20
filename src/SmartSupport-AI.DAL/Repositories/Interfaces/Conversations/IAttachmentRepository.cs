using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Repositories.Interfaces.Conversations
{
    public interface IAttachmentRepository : IGenericRepository<Attachment>
    {
        Task<List<Attachment>> GetByMessageIdAsync(int messageId);

        Task<List<Attachment>> GetByTicketIdAsync(int ticketId);
    }
}
