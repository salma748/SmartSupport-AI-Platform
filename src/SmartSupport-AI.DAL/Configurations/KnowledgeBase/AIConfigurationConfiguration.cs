using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class AIConfigurationConfiguration : IEntityTypeConfiguration<AIConfiguration>
    {
        public void Configure(EntityTypeBuilder<AIConfiguration> builder)
        {
            builder.HasKey(e => e.ConfigId);
        }
    }
}
