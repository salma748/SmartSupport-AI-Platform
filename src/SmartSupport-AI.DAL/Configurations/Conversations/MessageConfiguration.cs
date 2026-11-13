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

            builder.HasOne<AIResponse>()
                .WithOne()
                .HasForeignKey<AIResponse>(e => e.MessageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<AIResponse>()
                .WithOne()
                .HasForeignKey(e => e.TriggerMsgId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<Attachment>()
                .WithOne()
                .HasForeignKey(e => e.MessageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
