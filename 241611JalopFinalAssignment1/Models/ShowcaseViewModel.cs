namespace _241611JalopFinalAssignment1.Models;

// A record groups related data into properties; sealed prevents other types from inheriting from it.
public sealed record ConceptCategory(
    string Slug, // Category name used in the URL, such as "input-output".
    string Name, // Display name shown on the page; can contain spaces or punctuation.
    string Description,
    string Icon,
    string Accent
    );

public static class ConceptCatalog // A shared category list; static means no new ConceptCatalog() is needed.
{
    // A read-only list of category records; { get; } lets code read the list without replacing it.
    // Array.AsReadOnly wraps the array so callers cannot add, remove or replace its entries.
    public static IReadOnlyList<ConceptCategory> Categories { get; } = Array.AsReadOnly(new[]
    {
        new ConceptCategory("input-output", "Input/Output", "Start a conversation with your program. Read input, work with values, and display the result.", "terminal", "cyan"),
        // These names and URL slugs are reused by folders, navigation, search and example links.
        new ConceptCategory("nested-if-else", "Nested If…Else", "Give your code a choice. Explore nested if…else statements and decisions within decisions.", "branch", "amber"),
        new ConceptCategory("loops", "Loops", "Find the rhythm in your code. Explore loops and the things worth doing more than once.", "repeat", "violet"),
        new ConceptCategory("arrays", "Arrays", "Keep related values together. Explore array elements, indexes, and working with collections of values.", "grid", "mint")
    });
    
    // Find a category by its URL slug; ? allows the input and result to be null.
    public static ConceptCategory? Find(string? slug)
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
public sealed record ShowcaseViewModel(
    ConceptCategory? Category = null, // Selected category; null means no category is selected.
    int? Example = null, // Selected example number; null means no example is selected.
    bool IsAbout = false) // True shows About; these default values let the controller omit arguments.
{
    public bool HasWindow => Category is not null || IsAbout; // Computed value: show a window if a category is selected OR About is open.

    public string WindowTitle => IsAbout ? "About this showcase" : Category?.Name ?? "Desktop"; // Title is About, the selected category's name, or Desktop.

    // Example 3 can carry its form data alongside the existing page selection.
    public TemperatureConverterViewModel? Temperature { get; init; }
    public IdCardViewModel? IdCard { get; init; } // Example 1 text-only ID data.
    public TextCounterViewModel? TextCounter { get; init; }
    public TimeConverterViewModel? Time { get; init; } // Example 4 form and calculated output.
    public NestedExampleViewModel? Nested { get; init; } // One of the five nested-decision forms.
}
