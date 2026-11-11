using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSupport_AI.DAL.Entites;

namespace SmartSupport_AI.DAL.Configurations
{
    public class ResponseArticleConfiguration : IEntityTypeConfiguration<ResponseArticle>
    {
        public void Configure(EntityTypeBuilder<ResponseArticle> builder)
        {
            builder.HasKey(e => new { e.ResponseId, e.ArticleId });
        }
    }
}
