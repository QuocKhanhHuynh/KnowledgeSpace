using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.Model.Systems;

namespace KnowledgeSpace.WebPortal.Models
{
    public class KnowledgeBaseDetailViewModel
    {
        public CategoryViewModel Category { set; get; }
        public KnowledgeBaseViewModel Detail { get; set; }
        public List<LabelViewModel> Labels { get; set; }
        public UserViewModel CurrentUser { get; set; }
    }
}