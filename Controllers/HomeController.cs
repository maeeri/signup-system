using Microsoft.AspNetCore.Mvc;
using SignUpProject.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SignUpProject.Data;

namespace SignUpProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetAppLanguage(string culture, string returnUrl)
        {
            HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                // making cookie valid for the actual app root path (which is not necessarily "/" e.g. if we're behind a reverse proxy)
                new CookieOptions { Path = Url.Content("~/") });

            return Redirect(returnUrl);
        }
    }
}