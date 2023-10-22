using KnowledgeSpace.BackendServer.Extensions;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.WebPortal.Models;
using KnowledgeSpace.WebPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace KnowledgeSpace.WebPortal.Controllers
{
    public class KnowledgeBaseController : Controller
    {
        private readonly IKnowledgeBaseApiClient _knowledgeBaseApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly ILabelApiClient _labelApiClient;
        private readonly IConfiguration _configuration;
        private readonly IUserApiClient _userApiClient;

        public KnowledgeBaseController(IKnowledgeBaseApiClient knowledgeBaseApiClient,
            ICategoryApiClient categoryApiClient,
            ILabelApiClient labelApiClient,
            IConfiguration configuration,
            IUserApiClient userApiClient)
        {
            _knowledgeBaseApiClient = knowledgeBaseApiClient;
            _categoryApiClient = categoryApiClient;
            _labelApiClient = labelApiClient;
            _configuration = configuration;
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> ListByCategoryId(int id, int pageIndex = 1, int pageSize = 10)
        {
            var category = await _categoryApiClient.GetCategoryById(id);
            var data = await _knowledgeBaseApiClient.GetKnowledgeBasesByCategoryId(id, pageIndex, pageSize);
            var viewModel = new ListByCategoryIdViewModel()
            {
                Data = data,
                Category = category

            };
            return View(viewModel);
        }

        public async Task<IActionResult> Search(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var data = await _knowledgeBaseApiClient.SearchKnowledgeBase(keyword, pageIndex, pageSize);
            var viewModel = new SearchKnowledgeBaseViewModel()
            {
                Data = data,
                Keyword = keyword
            };
            return View(viewModel);
        }

        public async Task<IActionResult> ListByTag(string tagId, int pageIndex = 1, int pageSize = 10)
        {
            var data = await _knowledgeBaseApiClient.GetKnowledgeBasesByTagId(tagId, pageIndex, pageSize);
            var label = await _labelApiClient.GetLabelById(tagId);
            var viewModel = new ListByTagIdViewModel()
            {
                Data = data,
                Label = label
            };
            return View(viewModel);
        }
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var knowledgeBase = await _knowledgeBaseApiClient.GetKnowledgeBaseDetail(id);
            var category = await _categoryApiClient.GetCategoryById(knowledgeBase.CategoryId);
            var labels = await _knowledgeBaseApiClient.GetLabelsByKnowledgeBaseId(id);
            var viewModel = new KnowledgeBaseDetailViewModel()
            {
                Detail = knowledgeBase,
                Category = category,
                Labels = labels,
                CurrentUser = await _userApiClient.GetById(User.GetUserId())
            };
            await _knowledgeBaseApiClient.UpdateViewCount(id);
            return View(viewModel);
        }

        public async Task<IActionResult> GetCommentByKnowledgeBaseId(int knowledgeBaseId)
        {
            var data = await _knowledgeBaseApiClient.GetCommentsTree(knowledgeBaseId);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewComment([FromForm] CommentCreateRequest request)
        {
            var result = await _knowledgeBaseApiClient.PostComment(request);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Vote([FromForm] VoteCreateRequest request)
        {
            var result = await _knowledgeBaseApiClient.Vote(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostReport([FromForm] ReportCreateRequest request)
        {
            var result = await _knowledgeBaseApiClient.PostReport(request);
            return Ok(result);
        }
    }
}
