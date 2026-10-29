using System.Diagnostics;
using _241611JalopFinalAssignment1.Models; // Makes this project's model types available here.
using Microsoft.AspNetCore.Mvc; // Provides Controller, IActionResult and MVC attributes.

namespace _241611JalopFinalAssignment1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Stores a logger for this controller; readonly prevents reassignment after construction.

        public HomeController(ILogger<HomeController> logger) // Constructor: ASP.NET Core supplies the logger automatically (dependency injection).
        {
            _logger = logger; // Keep the supplied logger so controller methods can use it.
        }

        public IActionResult Index() // Handles the desktop route (/); IActionResult allows different kinds of responses.
        {
            var model = new ShowcaseViewModel(); // Default values select the desktop with no open window.
            if (Request.Headers["X-Showcase-Partial"] == "true") // Check whether JavaScript requested only the window content.
                return PartialView("_ShowcaseWindow", model); // Render without the page layout; the desktop model produces no window HTML.
            return View(model); // Render Views/Home/Index.cshtml with this model and the shared layout.
        }

        [HttpGet("about")] // A GET request to /about calls the method below.
        public IActionResult About() // Prepares the About window.
        {
            var model = new ShowcaseViewModel(IsAbout: true); // Named argument: set IsAbout to true and keep the other defaults.
            if (Request.Headers["X-Showcase-Partial"] == "true") // Enhanced navigation needs only part of the page.
                return PartialView("_ShowcaseWindow", model); // Return the About window HTML without the full page layout.
            return View("Index", model); // Reuse the desktop view, with this model selecting the About window.
        }

        public IActionResult Privacy() // The default route exposes this starter page at /Home/Privacy.
        {
            return View(); // With no view name, MVC uses the action name: Views/Home/Privacy.cshtml.
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Tell browsers and caches not to store this error response.
        public IActionResult Error() // Production exception handling sends errors to /Home/Error.
        {
            // Send a request ID to the error view; ?. safely reads the trace ID, and ?? uses the fallback if it is null.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
