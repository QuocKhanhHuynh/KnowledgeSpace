using KnowledgeSpace.Model;
using KnowledgeSpace.Model.Contents;

namespace KnowledgeSpace.WebPortal.Models
{
    public class SearchKnowledgeBaseViewModel
    {
        public Pagination<KnowledgeBaseQuickViewModel> Data { set; get; }

        public string Keyword { set; get; }
    }
}
