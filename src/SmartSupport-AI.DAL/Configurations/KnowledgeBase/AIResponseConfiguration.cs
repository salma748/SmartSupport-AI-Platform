using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class AIResponseConfiguration : IEntityTypeConfiguration<AIResponse>
    {
        public void Configure(EntityTypeBuilder<AIResponse> builder)
        {
            builder.HasKey(e => e.ResponseId);

            builder.Property(e => e.Confidence).HasPrecision(18, 2);

            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasMany<CustomerFeedback>()
                .WithOne(e => e.Response)
                .HasForeignKey(e => e.ResponseId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(e => e.ToolExecutions)
                .WithOne(e => e.Response)
                .HasForeignKey(e => e.ResponseId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(e => e.ResponseArticles)
                .WithOne(e => e.Response)
                .HasForeignKey(e => e.ResponseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
