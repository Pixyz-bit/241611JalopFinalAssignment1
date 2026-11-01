using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;
namespace _241611JalopFinalAssignment1.Controllers;

public sealed class InputOutputController : Controller
{
    [HttpPost("concepts/input-output/example-4")]
    [ValidateAntiForgeryToken]
    public IActionResult ConvertTime([Bind("Value,FromUnit,ToUnit")] TimeConverterViewModel input)
    {
        if (ModelState.IsValid && input.Value is decimal value)
        {
            // Normalize to seconds, then divide by the destination unit's number of seconds.
            decimal SecondsPerUnit(TimeUnit unit) => unit switch { TimeUnit.Hours => 3600m, TimeUnit.Minutes => 60m, _ => 1m };
            input.Result = value * SecondsPerUnit(input.FromUnit) / SecondsPerUnit(input.ToUnit);
        }
        return View("~/Views/Home/Index.cshtml", new ShowcaseViewModel(ConceptCatalog.Find("input-output"), 4) { Time = input });
    }
}
