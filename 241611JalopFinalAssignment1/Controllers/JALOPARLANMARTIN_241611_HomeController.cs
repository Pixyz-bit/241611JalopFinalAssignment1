using System.Diagnostics;
using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace _241611JalopFinalAssignment1.Controllers
{
    public class JALOPARLANMARTIN_241611_HomeController : Controller
    {
        private readonly ILogger<JALOPARLANMARTIN_241611_HomeController> _logger;
        public JALOPARLANMARTIN_241611_HomeController(ILogger<JALOPARLANMARTIN_241611_HomeController> logger) // ASP.NET Core supplies this controller's logger automatically.
        {
            _logger = logger; // Keep the supplied logger so controller methods can use it.
        }

        // Explicit routes preserve the familiar desktop URLs after the controller is renamed.
        [HttpGet("/")]
        [HttpGet("/Home")]
        [HttpGet("/Home/Index")]
        public IActionResult Index() // Shows the desktop page (/); IActionResult allows different kinds of responses.
        {
            var model = new JALOPARLANMARTIN_241611_ShowcaseViewModel(); // The default model selects the desktop without an open window.
            if (Request.Headers["X-Showcase-Partial"] == "true") // Enhanced navigation requests only the shared window markup.
                return PartialView("JALOPARLANMARTIN_241611_ShowcaseWindow", model); // Use the existing partial; the desktop model renders no open window.
            return View("~/Views/Home/JALOPARLANMARTIN_241611_Index.cshtml", model); // Render Views/Home/JALOPARLANMARTIN_241611_Index.cshtml with this model and the shared layout.
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Tell browsers and caches not to store this error response.
        [Route("/Home/Error")] // Keep the production exception-handler URL stable.
        public IActionResult Error() // Production exception handling sends errors to /Home/Error.
        {
            // Send a request ID to the error view; ?. safely reads the trace ID, and ?? uses the fallback if it is null.
            return View("~/Views/Shared/JALOPARLANMARTIN_241611_Error.cshtml", new JALOPARLANMARTIN_241611_ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
