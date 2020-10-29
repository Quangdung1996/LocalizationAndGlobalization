using LocalizationAndGlobalization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ResourceApp.Resources;
using System.Diagnostics;

namespace LocalizationAndGlobalization.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //private readonly IStringLocalizer<ResourceApp.Resources.SharedResources> _sharedLocalizer;
        //private readonly ISharedResource _sharedResource;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        //public HomeController(ILogger<HomeController> logger, IStringLocalizer<ResourceApp.Resources.SharedResources> sharedLocalizer, ISharedResource sharedResource)
        //{
        //    _logger = logger;
        //    _sharedLocalizer = sharedLocalizer;
        //    _sharedResource = sharedResource;
        //}

        public IActionResult Index()
        {
            ViewData["HelloWorld"] = _localizer["HelloWorld"];
            return View();
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
    }
}