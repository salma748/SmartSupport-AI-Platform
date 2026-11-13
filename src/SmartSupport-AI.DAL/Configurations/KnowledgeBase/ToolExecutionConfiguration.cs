using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class ToolExecutionConfiguration : IEntityTypeConfiguration<ToolExecution>
    {
        public void Configure(EntityTypeBuilder<ToolExecution> builder)
        {
            builder.HasKey(e => e.ExecutionId);

            builder.Property(e => e.ToolName).IsRequired();

            builder.Property(e => e.InputParams).IsRequired();

            builder.Property(e => e.OutputResult).IsRequired();

            builder.Property(e => e.Status).IsRequired();

            builder.Property(e => e.ExecutedAt).IsRequired();

            builder.HasMany<OrderStatusCheck>()
                .WithOne(e => e.ToolExecution)
                .HasForeignKey(e => e.ExecutionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany<RefundRequest>()
                .WithOne(e => e.ToolExecution)
                .HasForeignKey(e => e.ExecutionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
