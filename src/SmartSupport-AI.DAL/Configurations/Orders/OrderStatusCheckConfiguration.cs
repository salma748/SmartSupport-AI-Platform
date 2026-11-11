using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class OrderStatusCheckConfiguration : IEntityTypeConfiguration<OrderStatusCheck>
    {
        public void Configure(EntityTypeBuilder<OrderStatusCheck> builder)
        {
            builder.HasKey(e => e.CheckId);
        }
    }
}
