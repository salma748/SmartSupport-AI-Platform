using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class SystemPolicyConfiguration : IEntityTypeConfiguration<SystemPolicy>
    {
        public void Configure(EntityTypeBuilder<SystemPolicy> builder)
        {
            builder.HasKey(e => e.PolicyId);
        }
    }
}
