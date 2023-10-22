
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.BackendServer.Extensions;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.WebPortal.Helpers;
using KnowledgeSpace.WebPortal.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSpace.WebPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IKnowledgeBaseApiClient _knowledgeBaseApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public AccountController(IUserApiClient userApiClient, IKnowledgeBaseApiClient knowledgeBaseApiClient, ICategoryApiClient categoryApiClient)
        {
            _userApiClient = userApiClient;
            _knowledgeBaseApiClient = knowledgeBaseApiClient;
            _categoryApiClient = categoryApiClient;

        }
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "oidc");
        }

        public IActionResult SignOut()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" }, "Cookies", "oidc");
        }

        [Authorize]
        public async Task<ActionResult> MyProfile()
        {
             var user = await _userApiClient.GetById(User.GetUserId());
            return View(user);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateNewKnowledgeBase()
        {
            await SetCategoriesViewBag();
            return View();
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateNewKnowledgeBase([FromForm] KnowledgeBaseCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                await SetCategoriesViewBag();
                return View();
            }
            if (!Captcha.ValidateCaptchaCode(request.CaptchaCode, HttpContext))
            {
                await SetCategoriesViewBag();
                ModelState.AddModelError("", "Mã xác nhận không đúng");
                return View();
            }

            var result = await _knowledgeBaseApiClient.PostKnowlegdeBase(request);
            if (result)
            {
                TempData["message"] = "Thêm bài viết thành công";
                return Redirect("/my-kbs");
            }

            await SetCategoriesViewBag();
            return View(request);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyKnowledgeBases(int page = 1, int pageSize = 10)
        {
            var kbs = await _userApiClient.GetKnowledgeBasesByUserId(User.GetUserId(), page, pageSize);
            return View(kbs);
        }

        [HttpGet]
        public async Task<IActionResult> EditKnowledgeBase(int id)
        {
            var knowledgeBase = await _knowledgeBaseApiClient.GetKnowledgeBaseDetail(id);
            await SetCategoriesViewBag();
            return View(new KnowledgeBaseCreateRequest()
            {
                CategoryId = knowledgeBase.CategoryId,
                Description = knowledgeBase.Description,
                Environment = knowledgeBase.Environment,
                ErrorMessage = knowledgeBase.ErrorMessage,
              //  Labels = knowledgeBase.Labels,
                Note = knowledgeBase.Note,
                Problem = knowledgeBase.Problem,
                StepToReproduce = knowledgeBase.StepToReproduce,
                Title = knowledgeBase.Title,
                Workaround = knowledgeBase.Workaround,
                Id = knowledgeBase.Id
            });
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditKnowledgeBase(KnowledgeBaseCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                await SetCategoriesViewBag();
                return View();
            }

            var result = await _knowledgeBaseApiClient.PutKnowlegdeBase(request.Id.Value,request);
            if (result)
            {
                TempData["message"] = "Cập nhật bài viết thành công";
                return Redirect("/my-kbs");
            }

            await SetCategoriesViewBag();
            return View(request);
        }

        private async Task SetCategoriesViewBag()
        {
            var categories = await _categoryApiClient.GetCategories();
            categories.Insert(0, new CategoryViewModel()
            {
                Id = 0,
                Name = "--Hãy chọn danh mục--"
            });
            ViewBag.Categories = categories;
        }
    }
}
