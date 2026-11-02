using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

public sealed record GridMatch(int Row, int Column);

public sealed class GridSearchViewModel : ArrayExampleViewModel
{
    public override int Number => 4;
    public override string Title => ExampleCatalog.GetTitle("arrays", 4);
    public override string Description => "Search a 3×3 number array and find every matching row and column.";
    public override string Dimension => "Two-dimensional array";
    public override string ActionName => "SearchGrid";
    public override string SubmitLabel => "Search";
    public override string InputPartial => "_GridSearchInputs";
    public override string ResultPartial => "_GridSearchResult";
    // Flat form fields are validated and copied into an actual rectangular C# array.
    public List<string?> Cells { get; set; } = Enumerable.Repeat<string?>(null, 9).ToList();
    [Required(ErrorMessage = "Enter a number to find.")]
    [Range(typeof(decimal), "-1000000000", "1000000000", ErrorMessage = "Search for a number between −1,000,000,000 and 1,000,000,000.")]
    public decimal? SearchValue { get; set; }
    public decimal[,]? Grid { get; set; }
    public List<GridMatch> Matches { get; } = new();
}

public sealed class GroupOrganizerViewModel : ArrayExampleViewModel
{
    public override int Number => 5;
    public override string Title => ExampleCatalog.GetTitle("arrays", 5);
    public override string Description => "Arrange three groups of three names. Rows represent groups; columns represent members.";
    public override string Dimension => "Two-dimensional array";
    public override string ActionName => "OrganizeGroups";
    public override string SubmitLabel => "Organize";
    public override string InputPartial => "_GroupInputs";
    public override string ResultPartial => "_GroupResult";
    public List<string?> Members { get; set; } = Enumerable.Repeat<string?>(null, 9).ToList();
    public string[,]? Groups { get; set; } // The displayed table comes from this rectangular string array.
}
