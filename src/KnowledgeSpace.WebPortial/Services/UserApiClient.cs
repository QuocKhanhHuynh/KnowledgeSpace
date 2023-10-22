﻿using KnowledgeSpace.Model;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.Model.Systems;

namespace KnowledgeSpace.WebPortal.Services
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor): base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<UserViewModel> GetById(string id)
        {
            return await GetAsync<UserViewModel>($"/api/users/{id}", true);
        }

        public async Task<Pagination<KnowledgeBaseQuickViewModel>> GetKnowledgeBasesByUserId(string userId, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/users/{userId}/knowledgeBases?pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickViewModel>>(apiUrl, true);
        }
    }
}
