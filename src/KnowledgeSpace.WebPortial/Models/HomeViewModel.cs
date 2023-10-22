using KnowledgeSpace.Model.Contents;

namespace KnowledgeSpace.WebPortal.Models
{
    public class HomeViewModel
    {
        public List<KnowledgeBaseQuickViewModel> LatestKnowledgeBases { get; set; }
        public List<KnowledgeBaseQuickViewModel> PopularKnowledgeBases { get; set; }

        public List<LabelViewModel> PopularLabels { get; set; }
    }
}
