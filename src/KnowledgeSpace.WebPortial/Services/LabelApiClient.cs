using KnowledgeSpace.Model.Contents;
using Newtonsoft.Json;

namespace KnowledgeSpace.WebPortal.Services
{
    public class LabelApiClient : BaseApiClient, ILabelApiClient
    {

        public LabelApiClient(IHttpClientFactory httpClientFactory,
         IConfiguration configuration, IHttpContextAccessor httpContext) : base(httpClientFactory,configuration,httpContext)
        {
        }

        public async Task<LabelViewModel> GetLabelById(string labelId)
        {
            var label = await base.GetAsync<LabelViewModel>($"/api/labels/{labelId}");
            return label;
        }

        public async Task<List<LabelViewModel>> GetPopularLabels(int take)
        {
            var labels = await base.GetListAsync<LabelViewModel>($"/api/labels/popular/{take}");
            return labels;
        }
    }
}
