using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class RefundRequestConfiguration : IEntityTypeConfiguration<RefundRequest>
    {
        public void Configure(EntityTypeBuilder<RefundRequest> builder)
        {
            builder.HasKey(e => e.RefundId);

            builder.Property(e => e.Amount).HasPrecision(18, 2);

            builder.Property(e => e.Reason).IsRequired();

            builder.Property(e => e.Eligibility).IsRequired();

            builder.Property(e => e.Status).IsRequired();

            builder.Property(e => e.InitiatedAt).IsRequired();

            builder.HasMany<RefundStatusHistory>()
                .WithOne()
                .HasForeignKey(e => e.RefundId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
