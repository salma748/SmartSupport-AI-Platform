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
        }
    }
}
