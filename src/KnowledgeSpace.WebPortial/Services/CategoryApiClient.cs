using KnowledgeSpace.Model.Contents;
using Newtonsoft.Json;

namespace KnowledgeSpace.WebPortal.Services
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<List<CategoryViewModel>> GetCategories()
        {
            var categories = await base.GetListAsync<CategoryViewModel>("/api/categories");
            return categories;
        }

        public async Task<CategoryViewModel> GetCategoryById(int id)
        {
            var category = await base.GetAsync<CategoryViewModel>($"/api/categories/{id}");
            return category;
        }
    }
}
