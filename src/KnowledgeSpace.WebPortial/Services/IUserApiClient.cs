using KnowledgeSpace.Model;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.Model.Systems;

namespace KnowledgeSpace.WebPortal.Services
{
    public interface IUserApiClient
    {
        Task<UserViewModel> GetById(string id);
        Task<Pagination<KnowledgeBaseQuickViewModel>> GetKnowledgeBasesByUserId(string userId, int pageIndex, int pageSize);
    }
}
