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

            builder.Property(e => e.VersionNumber)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(e => e.VersionNumber).IsUnique();

            builder.Property(e => e.ModelName).IsRequired();

            builder.Property(e => e.MinConfidence).HasPrecision(18, 2);

            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasMany<Conversation>()
                .WithOne(e => e.Configuration)
                .HasForeignKey(e => e.ConfigId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<AIResponse>()
                .WithOne(e => e.Configuration)
                .HasForeignKey(e => e.ConfigId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<PerformanceMetric>()
                .WithOne(e => e.Configuration)
                .HasForeignKey(e => e.ConfigId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
