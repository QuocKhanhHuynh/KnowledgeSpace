﻿using KnowledgeSpace.WebPortal.Helpers;
using KnowledgeSpace.WebPortal.Models;
using KnowledgeSpace.WebPortal.Services;
using KnowledgeSpace.WebPortial.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KnowledgeSpace.WebPortial.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IKnowledgeBaseApiClient _knowledgeBaseApiClient;
        private readonly ILabelApiClient _labelApiClient;

        public HomeController(ILogger<HomeController> logger, IKnowledgeBaseApiClient knowledgeBaseApiClient, ILabelApiClient labelApiClient)
        {
            _logger = logger;
            _knowledgeBaseApiClient = knowledgeBaseApiClient;
            _labelApiClient = labelApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var latestKbs = await _knowledgeBaseApiClient.GetLatestKnowledgeBases(6);
            var popularKbs = await _knowledgeBaseApiClient.GetPopularKnowledgeBases(6);
            var popularLbs = await _labelApiClient.GetPopularLabels(6);
            var model = new HomeViewModel()
            {
                LatestKnowledgeBases = latestKbs,
                PopularKnowledgeBases = popularKbs,
                PopularLabels = popularLbs,
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }
    }
}