using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class TicketStatusHistoryConfiguration : IEntityTypeConfiguration<TicketStatusHistory>
    {
        public void Configure(EntityTypeBuilder<TicketStatusHistory> builder)
        {
            builder.HasKey(e => e.HistoryId);

            builder.Property(e => e.OldStatus).IsRequired();

            builder.Property(e => e.NewStatus).IsRequired();

            builder.Property(e => e.ChangedAt).IsRequired();
        }
    }
}
