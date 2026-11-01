using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace _241611JalopFinalAssignment1.Controllers;

public sealed class ConceptsController : Controller
{
    [HttpGet("concepts/{category}")] // Handles GET requests; {category} captures the category (page name) from the URL.
    [HttpGet("concepts/{category}/example-{example:int}")] // Handles GET requests; {example:int} requires an integer example number.
    
    public IActionResult Index(string category, int? example) // GET Request: Prepares the page
    {
        var concept = ConceptCatalog.Find(category); // Look up the category using its URL slug.
        if (concept is null || example is < 1 or > 5) return NotFound(); // Return HTTP 404 for an unknown category or an example outside 1-5.

        var model = new ShowcaseViewModel(concept, example)
        {
            TextCounter = category == "input-output" && example == 5 ? new TextCounterViewModel() : null,
            Time = category == "input-output" && example == 4 ? new TimeConverterViewModel() : null,
            Temperature =
                category == "input-output" && example == 3 ? new TemperatureConverterViewModel() : null,
        };

        if (Request.Headers["X-Showcase-Partial"] == "true")
            return PartialView("_ShowcaseWindow", model); // Enhanced navigation needs only the window HTML.

        return View("~/Views/Home/Index.cshtml", model); // Direct visits receive the complete page using the shared desktop view.
    }   

    //POST Request: processes the form for Temperature Converter
    [HttpPost("concepts/input-output/example-3")] // Handles a submitted form at this URL.
    [ValidateAntiForgeryToken] // Checks the form's antiforgery token.
    public IActionResult ConvertTemperature(
        [Bind("Value,FromUnit,ToUnit")] TemperatureConverterViewModel input, string? command)
    {
        // Binding accepts only editable inputs; users cannot submit their own result or formula.
        if (ModelState.IsValid && input.Value is decimal value)
        {
            // Absolute zero has a different numeric value on each temperature scale.
            var minimum = input.FromUnit switch
            {
                TemperatureUnit.Celsius => -273.15m,
                TemperatureUnit.Fahrenheit => -459.67m,
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
                    TemperatureUnit.Fahrenheit => (value - 32m) * 5m / 9m,
                    TemperatureUnit.Kelvin => value - 273.15m,
                    _ => value
                };
                input.Result = input.ToUnit switch
                {
                    TemperatureUnit.Fahrenheit => celsius * 9m / 5m + 32m,
                    TemperatureUnit.Kelvin => celsius + 273.15m,
                    _ => celsius
                };
                if (input.FromUnit == input.ToUnit) input.Result = value; // Same units preserve the exact input.

                // Tuple matching selects the explanation for the chosen source/destination pair.
                input.Formula = (input.FromUnit, input.ToUnit) switch
                {
                    (TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit) => $"({value:0.##}°C × 9/5) + 32 = {input.Result:0.##}°F",
                    (TemperatureUnit.Celsius, TemperatureUnit.Kelvin) => $"{value:0.##}°C + 273.15 = {input.Result:0.##} K",
                    (TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius) => $"({value:0.##}°F − 32) × 5/9 = {input.Result:0.##}°C",
                    (TemperatureUnit.Fahrenheit, TemperatureUnit.Kelvin) => $"({value:0.##}°F − 32) × 5/9 + 273.15 = {input.Result:0.##} K",
                    (TemperatureUnit.Kelvin, TemperatureUnit.Celsius) => $"{value:0.##} K − 273.15 = {input.Result:0.##}°C",
                    (TemperatureUnit.Kelvin, TemperatureUnit.Fahrenheit) => $"({value:0.##} K − 273.15) × 9/5 + 32 = {input.Result:0.##}°F",
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
        var page = new ShowcaseViewModel(
            ConceptCatalog.Find("input-output"),
            Example: 3)
        {
            Temperature = input
        };

        return View("~/Views/Home/Index.cshtml", page);
    }
}
