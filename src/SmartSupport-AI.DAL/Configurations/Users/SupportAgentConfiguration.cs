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

            builder.HasOne(e => e.User)
                .WithOne(e => e.SupportAgent)
                .HasForeignKey<SupportAgent>(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(e => e.TicketAssignments)
                .WithOne(e => e.Agent)
                .HasForeignKey(e => e.AgentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<InternalNote>()
                .WithOne(e => e.Agent)
                .HasForeignKey(e => e.AgentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<PerformanceMetric>()
                .WithOne(e => e.Agent)
                .HasForeignKey(e => e.AgentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<Message>()
                .WithOne(e => e.Agent)
                .HasForeignKey(e => e.AgentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
