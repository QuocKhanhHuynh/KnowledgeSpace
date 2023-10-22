using KnowledgeSpace.Model;
using KnowledgeSpace.Model.Contents;

namespace KnowledgeSpace.WebPortal.Models
{
    public class ListByTagIdViewModel
    {
        public Pagination<KnowledgeBaseQuickViewModel> Data { set; get; }

        public LabelViewModel Label { set; get; }
    }
}
