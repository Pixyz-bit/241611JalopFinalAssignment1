namespace _241611JalopFinalAssignment1.Models;

public sealed record ConceptCategory(string Slug, string Name, string Description, string Icon, string Accent);

public static class ConceptCatalog
{
    public static IReadOnlyList<ConceptCategory> Categories { get; } = Array.AsReadOnly(new[]
    {
        new ConceptCategory("input-output", "Input/Output", "Start a conversation with your program. Read input, work with values, and display the result.", "terminal", "cyan"),
        new ConceptCategory("conditional", "Conditional", "Give your code a choice. Explore decisions, comparisons, and the paths a program can take.", "branch", "amber"),
        new ConceptCategory("repetition", "Repetition", "Find the rhythm in your code. Explore loops and the things worth doing more than once.", "repeat", "violet"),
        new ConceptCategory("basic-crud", "Basic CRUD", "Give your data a place to live. Explore creating, reading, updating, and deleting records.", "database", "mint")
    });
    public static ConceptCategory? Find(string? slug) => Categories.FirstOrDefault(category => category.Slug == slug);
}

public sealed record ShowcaseViewModel(ConceptCategory? Category = null, int? Example = null, bool IsAbout = false)
{
    public bool HasWindow => Category is not null || IsAbout;
    public string WindowTitle => IsAbout ? "About this showcase" : Category?.Name ?? "Desktop";
}
