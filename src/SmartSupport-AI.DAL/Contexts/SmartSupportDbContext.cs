using Microsoft.EntityFrameworkCore;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Contexts
{
    public class SmartSupportDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<SupportAgent> SupportAgents { get; set; } = null!;
        public DbSet<Administrator> Administrators { get; set; } = null!;
        public DbSet<Conversation> Conversations { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Attachment> Attachments { get; set; } = null!;
        public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; } = null!;
        public DbSet<ActionLog> ActionLogs { get; set; } = null!;
        public DbSet<AIConfiguration> AIConfigurations { get; set; } = null!;
        public DbSet<Intent> Intents { get; set; } = null!;
        public DbSet<AIResponse> AIResponses { get; set; } = null!;
        public DbSet<ToolExecution> ToolExecutions { get; set; } = null!;
        public DbSet<ArticleCategory> ArticleCategories { get; set; } = null!;
        public DbSet<KnowledgeArticle> KnowledgeArticles { get; set; } = null!;
    }
}
