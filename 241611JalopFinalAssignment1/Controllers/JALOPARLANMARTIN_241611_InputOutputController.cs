using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;
namespace _241611JalopFinalAssignment1.Controllers;

public sealed class JALOPARLANMARTIN_241611_InputOutputController : Controller
{
    [HttpPost("concepts/input-output/example-2")]
    [ValidateAntiForgeryToken]
    public IActionResult CalculateRectangle([Bind("Length,Width")] JALOPARLANMARTIN_241611_RectangleCalculatorViewModel input)
    {
        if (ModelState.IsValid && input.Length is decimal length && input.Width is decimal width)
        {
            input.Area = length * width; 
            input.Perimeter = 2 * (length + width); 
        }
        return View("~/Views/Home/JALOPARLANMARTIN_241611_Index.cshtml", new JALOPARLANMARTIN_241611_ShowcaseViewModel(JALOPARLANMARTIN_241611_ConceptCatalog.Find("input-output"), 2) { Rectangle = input });
    }

    // Keep Input/Output submissions together while preserving the canonical URL.
    [HttpPost("concepts/input-output/example-3")] // Handles a submitted form at this URL.
    [ValidateAntiForgeryToken] // Checks the form's antiforgery token.
    public IActionResult ConvertTemperature(
        [Bind("Value,FromUnit,ToUnit")] JALOPARLANMARTIN_241611_TemperatureConverterViewModel input, string? command)
    {
        // Binding accepts only editable inputs; users cannot submit their own result or formula.
        if (ModelState.IsValid && input.Value is decimal value)
        {
            // Absolute zero has a different numeric value on each temperature scale.
            var minimum = input.FromUnit switch
            {
                JALOPARLANMARTIN_241611_TemperatureUnit.Celsius => -273.15m,
                JALOPARLANMARTIN_241611_TemperatureUnit.Fahrenheit => -459.67m,
                _ => 0m // Kelvin cannot be negative.
            };
            if (value < minimum)
            {
                ModelState.AddModelError(nameof(input.Value), "Temperature cannot be below absolute zero in the selected unit.");
            }
            else
            {
                // Normalize to Celsius, then convert to the destination to cover every unit pair.
                var celsius = input.FromUnit switch
                {
                    JALOPARLANMARTIN_241611_TemperatureUnit.Fahrenheit => (value - 32m) * 5m / 9m,
                    JALOPARLANMARTIN_241611_TemperatureUnit.Kelvin => value - 273.15m,
                    _ => value
                };
                input.Result = input.ToUnit switch
                {
                    JALOPARLANMARTIN_241611_TemperatureUnit.Fahrenheit => celsius * 9m / 5m + 32m,
                    JALOPARLANMARTIN_241611_TemperatureUnit.Kelvin => celsius + 273.15m,
                    _ => celsius
                };
                if (input.FromUnit == input.ToUnit) input.Result = value; // Same units preserve the exact input.

                // Tuple matching selects the explanation for the chosen source/destination pair.
                input.Formula = (input.FromUnit, input.ToUnit) switch
                {
                    (JALOPARLANMARTIN_241611_TemperatureUnit.Celsius, JALOPARLANMARTIN_241611_TemperatureUnit.Fahrenheit) => $"({value:0.##}°C × 9/5) + 32 = {input.Result:0.##}°F",
                    (JALOPARLANMARTIN_241611_TemperatureUnit.Celsius, JALOPARLANMARTIN_241611_TemperatureUnit.Kelvin) => $"{value:0.##}°C + 273.15 = {input.Result:0.##} K",
                    (JALOPARLANMARTIN_241611_TemperatureUnit.Fahrenheit, JALOPARLANMARTIN_241611_TemperatureUnit.Celsius) => $"({value:0.##}°F − 32) × 5/9 = {input.Result:0.##}°C",
                    (JALOPARLANMARTIN_241611_TemperatureUnit.Fahrenheit, JALOPARLANMARTIN_241611_TemperatureUnit.Kelvin) => $"({value:0.##}°F − 32) × 5/9 + 273.15 = {input.Result:0.##} K",
                    (JALOPARLANMARTIN_241611_TemperatureUnit.Kelvin, JALOPARLANMARTIN_241611_TemperatureUnit.Celsius) => $"{value:0.##} K − 273.15 = {input.Result:0.##}°C",
                    (JALOPARLANMARTIN_241611_TemperatureUnit.Kelvin, JALOPARLANMARTIN_241611_TemperatureUnit.Fahrenheit) => $"({value:0.##} K − 273.15) × 9/5 + 32 = {input.Result:0.##}°F",
                    _ => $"{value:0.##} {input.FromUnit} = {input.Result:0.##} {input.ToUnit}"
                };
            }
        }

        // Without JavaScript, Swap still works as a server action using the full-precision result.
        if (command == "swap" && input.Result is decimal converted)
        {
            (input.FromUnit, input.ToUnit) = (input.ToUnit, input.FromUnit);
            input.Value = converted;
            input.Result = null;
            input.Formula = null;
            ModelState.Clear(); // Let Razor display the swapped values instead of the original POST values.
        }

        // Rebuild the same application window with the submitted data and result.
        var page = new JALOPARLANMARTIN_241611_ShowcaseViewModel(
            JALOPARLANMARTIN_241611_ConceptCatalog.Find("input-output"),
            Example: 3)
        {
            Temperature = input
        };

        return View("~/Views/Home/JALOPARLANMARTIN_241611_Index.cshtml", page);
    }

    [HttpPost("concepts/input-output/example-4")]
    [ValidateAntiForgeryToken]
    public IActionResult ConvertTime([Bind("Value,FromUnit,ToUnit")] JALOPARLANMARTIN_241611_TimeConverterViewModel input)
    {
        if (ModelState.IsValid && input.Value is decimal value)
        {
            // Normalize to seconds, then divide by the destination unit's number of seconds.
            decimal SecondsPerUnit(JALOPARLANMARTIN_241611_TimeUnit unit) => unit switch { JALOPARLANMARTIN_241611_TimeUnit.Hours => 3600m, JALOPARLANMARTIN_241611_TimeUnit.Minutes => 60m, _ => 1m };
            input.Result = value * SecondsPerUnit(input.FromUnit) / SecondsPerUnit(input.ToUnit);
        }
        return View("~/Views/Home/JALOPARLANMARTIN_241611_Index.cshtml", new JALOPARLANMARTIN_241611_ShowcaseViewModel(JALOPARLANMARTIN_241611_ConceptCatalog.Find("input-output"), 4) { Time = input });
    }

    [HttpPost("concepts/input-output/example-5")]
    [ValidateAntiForgeryToken]
    public IActionResult CountText([Bind("Text")] JALOPARLANMARTIN_241611_TextCounterViewModel input)
    {
        if (ModelState.IsValid)
        {
            // Normalize browser line endings so each line break counts once.
            var text = (input.Text ?? "").Replace("\r\n", "\n").Replace("\r", "\n");
            input.Characters = System.Globalization.StringInfo.ParseCombiningCharacters(text).Length;
            input.Words = System.Text.RegularExpressions.Regex.Matches(text, @"\S+").Count; // Ignore repeated spaces, tabs and blank lines.
            input.HasResult = true;
        }
        return View("~/Views/Home/JALOPARLANMARTIN_241611_Index.cshtml", new JALOPARLANMARTIN_241611_ShowcaseViewModel(JALOPARLANMARTIN_241611_ConceptCatalog.Find("input-output"), 5) { TextCounter = input });
    }

    [HttpPost("concepts/input-output/example-1")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(16384)] // The endpoint accepts a small text form, not photo uploads.
    public IActionResult GenerateId([Bind("FullName,StudentNumber,Course,YearLevel")] JALOPARLANMARTIN_241611_IdCardViewModel input)
    {
        if (Request.Form.Files.Count > 0) return BadRequest(); // Reject files even if a custom client submits one.
        if (ModelState.IsValid)
        {
            input.FullName = input.FullName!.Trim();
            input.StudentNumber = input.StudentNumber!.Trim();
            input.Course = input.Course!.Trim();
            input.HasResult = true;
        }
        if (Request.Headers["X-Id-Preview"] == "true")
        {
            if (!ModelState.IsValid) return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            // JSON carries validated text only; the selected photo remains in the browser.
            return Json(new { fullName = input.FullName, studentNumber = input.StudentNumber, course = input.Course, yearLevel = input.YearLevel, yearLabel = input.YearLevelLabel });
        }
        return View("~/Views/Home/JALOPARLANMARTIN_241611_Index.cshtml", new JALOPARLANMARTIN_241611_ShowcaseViewModel(JALOPARLANMARTIN_241611_ConceptCatalog.Find("input-output"), 1) { IdCard = input });
    }
}
