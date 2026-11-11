using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class TicketAssignmentConfiguration : IEntityTypeConfiguration<TicketAssignment>
    {
        public void Configure(EntityTypeBuilder<TicketAssignment> builder)
        {
            builder.HasKey(e => e.AssignmentId);
        }
    }
}
