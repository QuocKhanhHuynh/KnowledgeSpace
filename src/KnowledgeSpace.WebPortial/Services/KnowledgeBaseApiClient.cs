using Azure.Core;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model;
using KnowledgeSpace.Model.Contents;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace KnowledgeSpace.WebPortal.Services
{
    public class KnowledgeBaseApiClient : BaseApiClient, IKnowledgeBaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public KnowledgeBaseApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<KnowledgeBaseQuickViewModel>> GetLatestKnowledgeBases(int take)
        {
            var latestKnowledgeBases = await base.GetListAsync<KnowledgeBaseQuickViewModel>($"/api/knowledgeBases/latest/{take}");
            return latestKnowledgeBases;
        }

        public async Task<List<KnowledgeBaseQuickViewModel>> GetPopularKnowledgeBases(int take)
        {
            var latestKnowledgeBases = await base.GetListAsync<KnowledgeBaseQuickViewModel>($"/api/knowledgeBases/popular/{take}");
            return latestKnowledgeBases;
        }

        public async Task<Pagination<KnowledgeBaseQuickViewModel>> GetKnowledgeBasesByCategoryId(int categoryId, int pageIndex, int pageSize)
        {
            var knowledgeBases = await base.GetAsync<Pagination<KnowledgeBaseQuickViewModel>>($"/api/knowledgeBases/filter?categoryId={categoryId}&pageIndex={pageIndex}&pageSize={pageSize}");
            return knowledgeBases;
        }

        public async Task<KnowledgeBaseViewModel> GetKnowledgeBaseDetail(int id)
        {
            var knowledgeBase = await base.GetAsync<KnowledgeBaseViewModel>($"/api/knowledgeBases/{id}");
            return knowledgeBase;
        }

        public async Task<List<LabelViewModel>> GetLabelsByKnowledgeBaseId(int id)
        {
            var labels = await base.GetListAsync <LabelViewModel>($"/api/knowledgeBases/{id}/labels");
            return labels;
        }

        public async Task<Pagination<KnowledgeBaseQuickViewModel>> SearchKnowledgeBase(string keyword, int pageIndex, int pageSize)
        {
            var knowledgeBases = await base.GetAsync<Pagination<KnowledgeBaseQuickViewModel>>($"/api/knowledgeBases/filter?filter={keyword}&pageIndex={pageIndex}&pageSize={pageSize}");
            return knowledgeBases;
        }

        public async Task<Pagination<KnowledgeBaseQuickViewModel>> GetKnowledgeBasesByTagId(string tagId, int pageIndex, int pageSize)
        {
            var knowledgeBases = await base.GetAsync<Pagination<KnowledgeBaseQuickViewModel>>($"/api/knowledgeBases/tags/{tagId}?pageIndex={pageIndex}&pageSize={pageSize}");
            return knowledgeBases;
        }

        public async Task<List<CommentViewModel>> GetRecentComments(int take)
        {
            var comments = await base.GetListAsync<CommentViewModel>($"/api/knowledgeBases/comments/recent/{take}");
            return comments;
        }

        public async Task<List<CommentViewModel>> GetCommentsTree(int knowledgeBaseId)
        {
            return await GetListAsync<CommentViewModel>($"/api/knowledgeBases/{knowledgeBaseId}/comments/tree");
        }

        public async Task<bool> PostComment(CommentCreateRequest request)
        {
            return await PostAsync<CommentCreateRequest,bool>($"/api/knowledgeBases/{request.KnowledgeBaseId}/comments", request);
        }

        public async Task<bool> PostKnowlegdeBase(KnowledgeBaseCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);

            using var form = new MultipartFormDataContent();
            if (request.Attachments?.Count > 0)
            {
                foreach (var file in request.Attachments)
                {
                    //form.Add(new StreamContent(file.OpenReadStream()), "Attachments");
                    byte[] data;
                    using (var br = new BinaryReader(file.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)file.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    form.Add(bytes, "Attachments", file.FileName);
                }
            }
            form.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");
            form.Add(new StringContent(request.Environment.ToString()), "Environment");
            form.Add(new StringContent(request.Note.ToString()), "Note");
            form.Add(new StringContent(request.Problem.ToString()), "Problem");
            form.Add(new StringContent(request.StepToReproduce.ToString()), "StepToReproduce");
            form.Add(new StringContent(request.Workaround.ToString()), "Workaround");
            form.Add(new StringContent(request.ErrorMessage.ToString()), "ErrorMessage");
            form.Add(new StringContent(request.Description.ToString()), "Description");
            form.Add(new StringContent(request.Title.ToString()), "Title");
            form.Add(new StringContent(request.CaptchaCode.ToString()), "CaptchaCode");
            // if (request.Labels != null)
            //       form.Add(new StringContent("Labels"), request.Labels.ToString());

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"/api/knowledgebases/", form);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutKnowlegdeBase(int id, KnowledgeBaseCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);

            using var form = new MultipartFormDataContent();
            if (request.Attachments?.Count > 0)
            {
                foreach (var file in request.Attachments)
                {
                    //form.Add(new StreamContent(file.OpenReadStream()), "Attachments");
                    byte[] data;
                    using (var br = new BinaryReader(file.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)file.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    form.Add(bytes, "Attachments", file.FileName);
                }
            }
            form.Add(new StringContent(request.Id.ToString()), "Id");
            form.Add(new StringContent(request.CategoryId.ToString()), "CategoryId");
            form.Add(new StringContent(request.Environment.ToString()), "Environment");
            form.Add(new StringContent(request.Note.ToString()), "Note");
            form.Add(new StringContent(request.Problem.ToString()), "Problem");
            form.Add(new StringContent(request.StepToReproduce.ToString()), "StepToReproduce");
            form.Add(new StringContent(request.Workaround.ToString()), "Workaround");
            form.Add(new StringContent(request.ErrorMessage.ToString()), "ErrorMessage");
            form.Add(new StringContent(request.Description.ToString()), "Description");
            form.Add(new StringContent(request.Title.ToString()), "Title");
            // if (request.Labels != null)
            //       form.Add(new StringContent("Labels"), request.Labels.ToString());

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            /* if (request.Labels?.Length > 0)
             {
                 foreach (var label in request.Labels)
                 {
                     requestContent.Add(new StringContent(label), "labels");
                 }
             }*/

            var response = await client.PutAsync($"/api/knowledgeBases/{id}", form);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateViewCount(int id)
        {
            return await PutAsync<object, bool>($"/api/knowledgeBases/{id}/view-count", null, false);
        }

        public async Task<int> Vote(VoteCreateRequest request)
        {
            return await PostAsync<VoteCreateRequest, int>($"/api/knowledgeBases/{request.KnowledgeBaseId}/votes", request);
        }

        public async Task<bool> PostReport(ReportCreateRequest request)
        {
            return await PostAsync<ReportCreateRequest, bool>($"/api/knowledgeBases/{request.KnowledgeBaseId}/reports", request);
        }
    }
}
