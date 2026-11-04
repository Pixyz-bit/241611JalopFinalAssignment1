using System.Diagnostics;
using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace _241611JalopFinalAssignment1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger) // ASP.NET Core supplies this controller's logger automatically.
        {
            _logger = logger; // Keep the supplied logger so controller methods can use it.
        }

        public IActionResult Index() // Shows the desktop page (/); IActionResult allows different kinds of responses.
        {
            var model = new ShowcaseViewModel(); // The default model selects the desktop without an open window.
            if (Request.Headers["X-Showcase-Partial"] == "true") // Enhanced navigation requests only the shared window markup.
                return PartialView("_ShowcaseWindow", model); // Use the existing partial; the desktop model renders no open window.
            return View(model); // Render Views/Home/Index.cshtml with this model and the shared layout.
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Tell browsers and caches not to store this error response.
        public IActionResult Error() // Production exception handling sends errors to /Home/Error.
        {
            // Send a request ID to the error view; ?. safely reads the trace ID, and ?? uses the fallback if it is null.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
