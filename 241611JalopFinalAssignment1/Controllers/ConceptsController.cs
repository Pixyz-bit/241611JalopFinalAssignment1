using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace _241611JalopFinalAssignment1.Controllers;

public sealed class ConceptsController : Controller
{
    [HttpGet("concepts/{category}")] // Handles GET requests; {category} captures the category (page name) from the URL.
    [HttpGet("concepts/{category}/example-{example:int}")] // Handles GET requests; {example:int} requires an integer example number.
    public IActionResult Index(string category, int? example) // IActionResult supports responses such as a view or 404; int? allows example to be absent (null).
    {
        var concept = ConceptCatalog.Find(category); // Look up the category using its URL slug.
        if (concept is null || example is < 1 or > 5) return NotFound(); // Return HTTP 404 for an unknown category or an example outside 1-5.
        var model = new ShowcaseViewModel(concept, example); // Pass the selected category and optional example to Razor.
        if (Request.Headers["X-Showcase-Partial"] == "true") return PartialView("_FinderWindow", model); // Enhanced navigation needs only the window HTML.
        return View("~/Views/Home/Index.cshtml", model); // Direct visits receive the complete page using the shared desktop view.
    }
}
