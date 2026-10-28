using System.Diagnostics;
using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace _241611JalopFinalAssignment1.Controllers
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
            var model = new ShowcaseViewModel();
            if (Request.Headers["X-Showcase-Partial"] == "true")
                return PartialView("_FinderWindow", model);
            return View(model);
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            var model = new ShowcaseViewModel(IsAbout: true);
            if (Request.Headers["X-Showcase-Partial"] == "true")
                return PartialView("_FinderWindow", model);
            return View("Index", model);
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
