using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class PerformanceMetricConfiguration : IEntityTypeConfiguration<PerformanceMetric>
    {
        public void Configure(EntityTypeBuilder<PerformanceMetric> builder)
        {
            builder.HasKey(e => e.MetricId);

            builder.Property(e => e.MetricType).IsRequired();

            builder.Property(e => e.MetricValue).HasPrecision(18, 2);

            builder.Property(e => e.PeriodStart).IsRequired();

            builder.Property(e => e.PeriodEnd).IsRequired();
        }
    }
}
