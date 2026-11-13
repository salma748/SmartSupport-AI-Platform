using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserId);

            builder.Property(e => e.FullName).IsRequired();

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(e => e.Email).IsUnique();

            builder.Property(e => e.PasswordHash).IsRequired();

            builder.Property(e => e.Phone).IsRequired(false);

            builder.Property(e => e.Role).IsRequired();

            builder.Property(e => e.Status).IsRequired();

            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasMany<Attachment>()
                .WithOne(e => e.Uploader)
                .HasForeignKey(e => e.UploadedBy)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<TicketStatusHistory>()
                .WithOne(e => e.ChangedByUser)
                .HasForeignKey(e => e.ChangedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
