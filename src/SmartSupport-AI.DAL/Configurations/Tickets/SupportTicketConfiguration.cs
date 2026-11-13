using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
    {
        public void Configure(EntityTypeBuilder<SupportTicket> builder)
        {
            builder.HasKey(e => e.TicketId);

            builder.Property(e => e.Priority).IsRequired();

            builder.Property(e => e.Status).IsRequired();

            builder.Property(e => e.EscalationReason).IsRequired(false);

            builder.Property(e => e.ConvSummary).IsRequired(false);

            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasMany(e => e.Assignments)
                .WithOne(e => e.Ticket)
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(e => e.StatusHistory)
                .WithOne(e => e.Ticket)
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(e => e.InternalNotes)
                .WithOne(e => e.Ticket)
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Attachments)
                .WithOne(e => e.Ticket)
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
