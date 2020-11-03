using LocalizationAndGlobalization.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ResourceApp.Resources;
using System;
using System.Diagnostics;

namespace LocalizationAndGlobalization.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //private readonly IHtmlLocalizer<SharedResources> _localizer;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

  

        public IActionResult Index()
        {
            ViewData["HelloWorld"] = _localizer["HelloWorld"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            //manage culture by cookies
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                             CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                             new CookieOptions { Expires = DateTimeOffset.Now.AddDays(10) });
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}