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
        }
    }
}
