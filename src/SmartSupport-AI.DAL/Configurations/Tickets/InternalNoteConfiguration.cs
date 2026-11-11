using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class InternalNoteConfiguration : IEntityTypeConfiguration<InternalNote>
    {
        public void Configure(EntityTypeBuilder<InternalNote> builder)
        {
            builder.HasKey(e => e.NoteId);
        }
    }
}
