using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class IntentConfiguration : IEntityTypeConfiguration<Intent>
    {
        public void Configure(EntityTypeBuilder<Intent> builder)
        {
            builder.HasKey(e => e.IntentId);
        }
    }
}
