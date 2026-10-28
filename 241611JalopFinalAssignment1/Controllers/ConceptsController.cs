using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace _241611JalopFinalAssignment1.Controllers;

public sealed class ConceptsController : Controller
{
    [HttpGet("concepts/{category}")]
    [HttpGet("concepts/{category}/example-{example:int}")]
    public IActionResult Index(string category, int? example)
    {
        var concept = ConceptCatalog.Find(category);
        if (concept is null || example is < 1 or > 5) return NotFound();
        var model = new ShowcaseViewModel(concept, example);
        if (Request.Headers["X-Showcase-Partial"] == "true") return PartialView("_FinderWindow", model);
        return View("~/Views/Home/Index.cshtml", model);
    }
}
