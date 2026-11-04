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
}
