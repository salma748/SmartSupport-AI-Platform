using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasKey(e => e.ConversationId);

            builder.Property(e => e.Channel).IsRequired();

            builder.Property(e => e.Status).IsRequired();

            builder.Property(e => e.ResolutionType).IsRequired(false);

            builder.Property(e => e.Summary).IsRequired(false);

            builder.Property(e => e.StartedAt).IsRequired();

            builder.Property(e => e.EndedAt).IsRequired(false);

            builder.HasMany(e => e.Messages)
                .WithOne(e => e.Conversation)
                .HasForeignKey(e => e.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Feedback)
                .WithOne(e => e.Conversation)
                .HasForeignKey(e => e.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Tickets)
                .WithOne(e => e.Conversation)
                .HasForeignKey(e => e.ConversationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<OrderStatusCheck>()
                .WithOne(e => e.Conversation)
                .HasForeignKey(e => e.ConversationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<RefundRequest>()
                .WithOne(e => e.Conversation)
                .HasForeignKey(e => e.ConversationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
