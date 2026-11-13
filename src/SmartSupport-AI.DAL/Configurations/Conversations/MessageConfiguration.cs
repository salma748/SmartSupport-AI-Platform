using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(e => e.MessageId);

            builder.Property(e => e.SenderType).IsRequired();

            builder.Property(e => e.Content).IsRequired();

            builder.Property(e => e.SentAt).IsRequired();

            builder.HasIndex(e => new { e.ConversationId, e.SequenceNo }).IsUnique();

            builder.HasOne(e => e.AIResponse)
                .WithOne(e => e.Message)
                .HasForeignKey<AIResponse>(e => e.MessageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.TriggeredAIResponses)
                .WithOne(e => e.TriggerMessage)
                .HasForeignKey(e => e.TriggerMsgId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(e => e.Attachments)
                .WithOne(e => e.Message)
                .HasForeignKey(e => e.MessageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
