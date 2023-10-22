using KnowledgeSpace.Model;
using KnowledgeSpace.Model.Contents;

namespace KnowledgeSpace.WebPortal.Models
{
    public class ListByCategoryIdViewModel
    {
        public Pagination<KnowledgeBaseQuickViewModel> Data { set; get; }

        public CategoryViewModel Category { set; get; }
    }
}
