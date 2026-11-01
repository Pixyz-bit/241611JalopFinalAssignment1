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

    [HttpPost("concepts/input-output/example-5")]
    [ValidateAntiForgeryToken]
    public IActionResult CountText([Bind("Text")] TextCounterViewModel input)
    {
        if (ModelState.IsValid)
        {
            // Normalize browser line endings so each line break counts once.
            var text = (input.Text ?? "").Replace("\r\n", "\n").Replace("\r", "\n");
            input.Characters = System.Globalization.StringInfo.ParseCombiningCharacters(text).Length;
            input.Words = System.Text.RegularExpressions.Regex.Matches(text, @"\S+").Count; // Ignore repeated spaces, tabs and blank lines.
            input.HasResult = true;
        }
        return View("~/Views/Home/Index.cshtml", new ShowcaseViewModel(ConceptCatalog.Find("input-output"), 5) { TextCounter = input });
    }
}
