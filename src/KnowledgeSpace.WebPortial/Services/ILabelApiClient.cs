using KnowledgeSpace.Model.Contents;

namespace KnowledgeSpace.WebPortal.Services
{
    public interface ILabelApiClient
    {
        Task<List<LabelViewModel>> GetPopularLabels(int take);
        Task<LabelViewModel> GetLabelById(string labelId);
    }
}
