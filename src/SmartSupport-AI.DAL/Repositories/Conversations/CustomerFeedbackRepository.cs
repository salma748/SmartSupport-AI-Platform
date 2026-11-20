using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.Conversations;

namespace SmartSupport_AI.DAL.Repositories.Conversations
{
    public class CustomerFeedbackRepository : GenericRepository<CustomerFeedback>, ICustomerFeedbackRepository
    {
        public CustomerFeedbackRepository(SmartSupportDbContext context)
            : base(context)
        {
        }

        public async Task<List<CustomerFeedback>> GetByConversationIdAsync(int conversationId)
        {
            return await _context.CustomerFeedbacks
                .AsNoTracking()
                .Where(feedback => feedback.ConversationId == conversationId)
                .OrderBy(feedback => feedback.FeedbackId)
                .ToListAsync();
        }
    }
}
