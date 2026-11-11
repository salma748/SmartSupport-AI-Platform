using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class SupportAgentConfiguration : IEntityTypeConfiguration<SupportAgent>
    {
        public void Configure(EntityTypeBuilder<SupportAgent> builder)
        {
            builder.HasKey(e => e.AgentId);

            builder.Property(e => e.Department).IsRequired();

            builder.Property(e => e.SkillLevel).IsRequired();

            builder.Property(e => e.Availability).IsRequired();

            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<SupportAgent>(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<TicketAssignment>()
                .WithOne()
                .HasForeignKey(e => e.AgentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<InternalNote>()
                .WithOne()
                .HasForeignKey(e => e.AgentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<PerformanceMetric>()
                .WithOne()
                .HasForeignKey(e => e.AgentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<Message>()
                .WithOne()
                .HasForeignKey(e => e.AgentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
