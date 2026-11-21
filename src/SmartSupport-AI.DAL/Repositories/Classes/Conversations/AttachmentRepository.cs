using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;

namespace SmartSupport_AI.DAL.Repositories.Classes.Conversations
{
    public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public async Task<List<Attachment>> GetByMessageIdAsync(int messageId)
        {
            return await _context.Attachments
                .AsNoTracking()
                .Where(attachment => attachment.MessageId == messageId)
                .OrderBy(attachment => attachment.UploadedAt)
                .ThenBy(attachment => attachment.AttachmentId)
                .ToListAsync();
        }

        public async Task<List<Attachment>> GetByTicketIdAsync(int ticketId)
        {
            return await _context.Attachments
                .AsNoTracking()
                .Where(attachment => attachment.TicketId == ticketId)
                .OrderBy(attachment => attachment.UploadedAt)
                .ThenBy(attachment => attachment.AttachmentId)
                .ToListAsync();
        }
    }
}
