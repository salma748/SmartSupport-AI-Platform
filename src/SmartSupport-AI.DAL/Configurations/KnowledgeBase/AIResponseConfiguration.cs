using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class AIResponseConfiguration : IEntityTypeConfiguration<AIResponse>
    {
        public void Configure(EntityTypeBuilder<AIResponse> builder)
        {
            builder.HasKey(e => e.ResponseId);
        }
    }
}
