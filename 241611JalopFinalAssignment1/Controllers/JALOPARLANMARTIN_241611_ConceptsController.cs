using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;
using _241611JalopFinalAssignment1.Services;
using System.Security.Cryptography;

namespace _241611JalopFinalAssignment1.Controllers;

public sealed class JALOPARLANMARTIN_241611_ConceptsController(ExampleStateTokens roundTokens) : Controller
{
    [HttpGet("concepts/{category}")] // Handles GET requests; {category} captures the category (page name) from the URL.
    [HttpGet("concepts/{category}/example-{example:int}")] // Handles GET requests; {example:int} requires an integer example number.
    
    public IActionResult Index(string category, int? example) // GET Request: Prepares the page
    {
        var concept = JALOPARLANMARTIN_241611_ConceptCatalog.Find(category); // Look up the category using its URL slug.
        if (concept is null || example is < 1 or > 5) return NotFound(); // Return HTTP 404 for an unknown category or an example outside 1-5.

        // Old category URLs still work, but now open the first example immediately.
        if (!example.HasValue) return Redirect($"/concepts/{concept.Slug}/example-1");

        var model = new JALOPARLANMARTIN_241611_ShowcaseViewModel(concept, example)
        {
            IdCard = category == "input-output" && example == 1 ? new JALOPARLANMARTIN_241611_IdCardViewModel() : null,
            Rectangle = category == "input-output" && example == 2 ? new JALOPARLANMARTIN_241611_RectangleCalculatorViewModel() : null,
            Temperature = category == "input-output" && example == 3 ? new JALOPARLANMARTIN_241611_TemperatureConverterViewModel() : null,
            Time = category == "input-output" && example == 4 ? new JALOPARLANMARTIN_241611_TimeConverterViewModel() : null,
            TextCounter = category == "input-output" && example == 5 ? new JALOPARLANMARTIN_241611_TextCounterViewModel() : null,
            ArrayExample = category == "arrays" ? example switch
            {
                1 => new JALOPARLANMARTIN_241611_DuplicateFinderViewModel(),
                2 => new JALOPARLANMARTIN_241611_ArraySortViewModel(),
                3 => new JALOPARLANMARTIN_241611_ArrayExtremesViewModel(),
                4 => new JALOPARLANMARTIN_241611_MultiplicationTableViewModel { Rows = 5, Columns = 5 },
                // Seed editable examples on GET only; POST must preserve the user's submitted rosters.
                5 => new JALOPARLANMARTIN_241611_HouseMemberFinderViewModel
                {
                    Gryffindor = "Harry Potter, Hermione Granger, Ron Weasley",
                    Slytherin = "Draco Malfoy, Pansy Parkinson",
                    Ravenclaw = "Luna Lovegood, Cho Chang",
                    Hufflepuff = "Cedric Diggory, Nymphadora Tonks"
                },
                _ => null
            } : null,
            // Match each loop slot to its own validated input model.
            Loop = category == "loops" ? example switch
            {
                1 => new JALOPARLANMARTIN_241611_PyramidViewModel { Rows = 5, Symbol = "*" },
                2 => new JALOPARLANMARTIN_241611_SumDigitsViewModel(),
                3 => new JALOPARLANMARTIN_241611_PrimeFactorsViewModel(),
                4 => new JALOPARLANMARTIN_241611_GuessGameViewModel { StateToken = roundTokens.Protect("guess", new JALOPARLANMARTIN_241611_GuessRound(RandomNumberGenerator.GetInt32(1, 101), new())) },
                5 => new JALOPARLANMARTIN_241611_DigitCounterViewModel(),
                _ => null
            } : null,
            // A switch chooses the typed form for each implemented decision slot.
            Nested = category == "nested-if-else" ? example switch
            {
                1 => new JALOPARLANMARTIN_241611_LoginAccessViewModel(),
                2 => new JALOPARLANMARTIN_241611_LatinHonorsViewModel(),
                3 => new JALOPARLANMARTIN_241611_BmiCalculatorViewModel(),
                4 => new JALOPARLANMARTIN_241611_TriangleClassifierViewModel(),
                5 => new JALOPARLANMARTIN_241611_LargestNumbersViewModel(),
                _ => null
            } : null,
            
        };

        if (Request.Headers["X-Showcase-Partial"] == "true")
            return PartialView("JALOPARLANMARTIN_241611_ShowcaseWindow", model); // Enhanced navigation needs only the window HTML.

        return View("~/Views/Home/JALOPARLANMARTIN_241611_Index.cshtml", model); // Direct visits receive the complete page using the shared desktop view.
    }   

    //POST Request: processes the form for Temperature Converter
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
}
