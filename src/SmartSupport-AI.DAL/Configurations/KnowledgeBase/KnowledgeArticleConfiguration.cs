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
        }
    }
}
