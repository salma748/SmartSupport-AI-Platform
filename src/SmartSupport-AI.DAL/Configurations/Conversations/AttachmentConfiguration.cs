using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.HasKey(e => e.AttachmentId);

            builder.Property(e => e.FileName).IsRequired();

            builder.Property(e => e.FileType).IsRequired();

            builder.Property(e => e.UploadedAt).IsRequired();

            builder.ToTable(t => t.HasCheckConstraint(
                "CK_Attachment_MessageOrTicket",
                "[MessageId] IS NOT NULL OR [TicketId] IS NOT NULL"
            ));
        }
    }
}
