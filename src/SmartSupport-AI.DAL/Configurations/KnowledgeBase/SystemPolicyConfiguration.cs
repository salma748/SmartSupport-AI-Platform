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

            builder.Property(e => e.PolicyName)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(e => e.PolicyName).IsUnique();

            builder.Property(e => e.PolicyValue).IsRequired();

            builder.Property(e => e.UpdatedAt).IsRequired();
        }
    }
}
