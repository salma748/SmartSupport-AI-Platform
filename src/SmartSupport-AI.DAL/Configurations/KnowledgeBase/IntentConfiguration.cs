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

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(e => e.Name).IsUnique();

            builder.Property(e => e.Description).IsRequired(false);

            builder.HasMany(e => e.Responses)
                .WithOne(e => e.Intent)
                .HasForeignKey(e => e.IntentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
