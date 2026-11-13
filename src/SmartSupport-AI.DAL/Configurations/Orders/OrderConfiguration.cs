using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.OrderId);

            builder.Property(e => e.OrderNumber)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(e => e.OrderNumber).IsUnique();

            builder.Property(e => e.OrderStatus).IsRequired();

            builder.Property(e => e.TotalAmount).HasPrecision(18, 2);

            builder.Property(e => e.PlacedAt).IsRequired();

            builder.HasMany(e => e.StatusChecks)
                .WithOne(e => e.Order)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.RefundRequests)
                .WithOne(e => e.Order)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
