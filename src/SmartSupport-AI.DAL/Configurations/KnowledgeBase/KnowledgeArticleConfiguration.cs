using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class KnowledgeArticleConfiguration : IEntityTypeConfiguration<KnowledgeArticle>
    {
        public void Configure(EntityTypeBuilder<KnowledgeArticle> builder)
        {
            builder.HasKey(e => e.ArticleId);

            builder.Property(e => e.Title).IsRequired();

            builder.Property(e => e.Content).IsRequired();

            builder.Property(e => e.Status).IsRequired();

            builder.Property(e => e.UpdatedAt).IsRequired();

            builder.HasMany(e => e.ResponseArticles)
                .WithOne(e => e.Article)
                .HasForeignKey(e => e.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
