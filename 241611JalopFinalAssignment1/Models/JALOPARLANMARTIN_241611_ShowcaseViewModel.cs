namespace _241611JalopFinalAssignment1.Models;

// A record groups related data into properties; sealed prevents other types from inheriting from it.
public sealed record JALOPARLANMARTIN_241611_ConceptCategory(
    string Slug, // Category name used in the URL, such as "input-output".
    string Name, // Display name shown on the page; can contain spaces or punctuation.
    string Description,
    string Icon,
    string Accent
    );

public static class JALOPARLANMARTIN_241611_ConceptCatalog // A shared category list; static means no new JALOPARLANMARTIN_241611_ConceptCatalog() is needed.
{
    // A read-only list of category records; { get; } lets code read the list without replacing it.
    // Array.AsReadOnly wraps the array so callers cannot add, remove or replace its entries.
    public static IReadOnlyList<JALOPARLANMARTIN_241611_ConceptCategory> Categories { get; } = Array.AsReadOnly(new[]
    {
        new JALOPARLANMARTIN_241611_ConceptCategory("input-output", "Input/Output", "Start a conversation with your program. Read input, work with values, and display the result.", "terminal", "cyan"),
        // These names and URL slugs are reused by folders, navigation, search and example links.
        new JALOPARLANMARTIN_241611_ConceptCategory("nested-if-else", "Nested If…Else", "Give your code a choice. Explore nested if…else statements and decisions within decisions.", "branch", "amber"),
        new JALOPARLANMARTIN_241611_ConceptCategory("loops", "Loops", "Find the rhythm in your code. Explore loops and the things worth doing more than once.", "repeat", "violet"),
        new JALOPARLANMARTIN_241611_ConceptCategory("arrays", "Arrays", "Keep related values together. Explore array elements, indexes, and working with collections of values.", "grid", "mint")
    });

    // Find a category by its URL slug; ? allows the input and result to be null.
    public static JALOPARLANMARTIN_241611_ConceptCategory? Find(string? slug)
    {
        foreach (var category in Categories) // Check each category in the list.
        {
            if (category.Slug == slug) // Compare this category's slug with the supplied slug.
            {
                return category; // Found a match.
            }
        }

        return null; // No match found.
    }
}

// Carries the current page selection from a controller to a Razor view.
public sealed record JALOPARLANMARTIN_241611_ShowcaseViewModel(
    JALOPARLANMARTIN_241611_ConceptCategory? Category = null, // Selected category; null means no category is selected.
    int? Example = null) // Selected example number; null means no example is selected.
{
    public bool HasWindow => Category is not null; // This line doesn’t open a window itself. The view reads this value to decide what to display.
    public string WindowTitle => Category?.Name ?? "Desktop"; // Use the category name, or Desktop when no window is open.

    public JALOPARLANMARTIN_241611_TemperatureConverterViewModel? Temperature { get; init; }
    public JALOPARLANMARTIN_241611_IdCardViewModel? IdCard { get; init; } // Example 1 text-only ID data.
    public JALOPARLANMARTIN_241611_RectangleCalculatorViewModel? Rectangle { get; init; } // Example 2 dimensions and calculated measurements.
    public JALOPARLANMARTIN_241611_TextCounterViewModel? TextCounter { get; init; }
    public JALOPARLANMARTIN_241611_TimeConverterViewModel? Time { get; init; } // Example 4 form and calculated output.
    public JALOPARLANMARTIN_241611_NestedExampleViewModel? Nested { get; init; } // One of the five nested-decision forms.
    public JALOPARLANMARTIN_241611_LoopExampleViewModel? Loop { get; init; } // Typed inputs and server-generated loop output.
    public JALOPARLANMARTIN_241611_ArrayExampleViewModel? ArrayExample { get; init; } // One- or two-dimensional array presentation data.

    // “Return true if Temperature exists, OR IdCard exists, OR Rectangle exists, and so on.”.
    public bool HasImplementedExample => Temperature is not null || IdCard is not null || Rectangle is not null || TextCounter is not null || Time is not null || Nested is not null || Loop is not null || ArrayExample is not null;
}
