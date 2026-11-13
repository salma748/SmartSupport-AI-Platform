using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class RefundStatusHistoryConfiguration : IEntityTypeConfiguration<RefundStatusHistory>
    {
        public void Configure(EntityTypeBuilder<RefundStatusHistory> builder)
        {
            builder.HasKey(e => e.HistoryId);

            builder.Property(e => e.OldStatus).IsRequired();

            builder.Property(e => e.NewStatus).IsRequired();

            builder.Property(e => e.ChangedAt).IsRequired();
        }
    }
}
