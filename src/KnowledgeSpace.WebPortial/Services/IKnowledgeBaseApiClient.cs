using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model;
using KnowledgeSpace.Model.Contents;

namespace KnowledgeSpace.WebPortal.Services
{
    public interface IKnowledgeBaseApiClient
    {
        Task<List<KnowledgeBaseQuickViewModel>> GetPopularKnowledgeBases(int take);
        Task<List<KnowledgeBaseQuickViewModel>> GetLatestKnowledgeBases(int take);
        Task<Pagination<KnowledgeBaseQuickViewModel>> GetKnowledgeBasesByCategoryId(int categoryId, int pageIndex, int pageSize);
        Task<Pagination<KnowledgeBaseQuickViewModel>> SearchKnowledgeBase(string keyword, int pageIndex, int pageSize);
        Task<Pagination<KnowledgeBaseQuickViewModel>> GetKnowledgeBasesByTagId(string tagId, int pageIndex, int pageSize);
        Task<KnowledgeBaseViewModel> GetKnowledgeBaseDetail(int id);
        Task<List<LabelViewModel>> GetLabelsByKnowledgeBaseId(int id);
        Task<List<CommentViewModel>> GetRecentComments(int take);
        Task<List<CommentViewModel>> GetCommentsTree(int knowledgeBaseId);
        Task<bool> PostComment(CommentCreateRequest request);
        Task<bool> PostKnowlegdeBase(KnowledgeBaseCreateRequest request);
        Task<bool> PutKnowlegdeBase(int id, KnowledgeBaseCreateRequest request);
        Task<bool> UpdateViewCount(int id);
        Task<int> Vote(VoteCreateRequest request);
        Task<bool> PostReport(ReportCreateRequest request);
    }
}
