using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
    {
        public void Configure(EntityTypeBuilder<Administrator> builder)
        {
            builder.HasKey(e => e.AdminId);

            builder.Property(e => e.AccessLevel).IsRequired();

            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<Administrator>(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<AIConfiguration>()
                .WithOne()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<KnowledgeArticle>()
                .WithOne()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<SystemPolicy>()
                .WithOne()
                .HasForeignKey(e => e.UpdatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
