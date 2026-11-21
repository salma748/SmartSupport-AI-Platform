using SmartSupport_AI.DAL.Contexts;
using SmartSupport_AI.DAL.Entites;
using SmartSupport_AI.DAL.Repositories.Interfaces.KnowledgeBase;

namespace SmartSupport_AI.DAL.Repositories.Classes.KnowledgeBase
{
    public class ResponseArticleRepository : GenericRepository<ResponseArticle>, IResponseArticleRepository
    {
        public ResponseArticleRepository(SmartSupportDbContext context)
            : base(context)
        {
        }
    }
}
