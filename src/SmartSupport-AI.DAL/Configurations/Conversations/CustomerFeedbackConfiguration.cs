using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class CustomerFeedbackConfiguration : IEntityTypeConfiguration<CustomerFeedback>
    {
        public void Configure(EntityTypeBuilder<CustomerFeedback> builder)
        {
            builder.HasKey(e => e.FeedbackId);
        }
    }
}
