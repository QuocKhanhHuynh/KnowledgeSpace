using KnowledgeSpace.Model.Contents;

namespace KnowledgeSpace.WebPortal.Services
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryViewModel>> GetCategories();
        Task<CategoryViewModel> GetCategoryById(int id);
    }
}
