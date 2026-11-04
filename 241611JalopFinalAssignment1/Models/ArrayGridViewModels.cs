using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

public sealed class MultiplicationTableViewModel : ArrayExampleViewModel
{
    public override int Number => 4;
    public override string Title => ExampleCatalog.GetTitle("arrays", 4);
    public override string Description => "Generate products in a two-dimensional array.";
    public override string Dimension => "Two-dimensional array";
    [Required(ErrorMessage = "Enter the number of rows.")]
    [Range(1, 12, ErrorMessage = "Use 1–12 rows.")]
    public int? Rows { get; set; }
    [Required(ErrorMessage = "Enter the number of columns.")]
    [Range(1, 12, ErrorMessage = "Use 1–12 columns.")]
    public int? Columns { get; set; }
    public int[,]? Products { get; set; } // Each cell stores its row factor multiplied by its column factor.
}

public sealed class HouseMemberFinderViewModel : ArrayExampleViewModel
{
    public override int Number => 5;
    public override string Title => ExampleCatalog.GetTitle("arrays", 5);
    public override string Description => "Find a member in four house rosters stored as a two-dimensional array.";
    public override string Dimension => "Two-dimensional array";
    public static IReadOnlyList<string> Houses { get; } = Array.AsReadOnly(new[] { "Gryffindor", "Slytherin", "Ravenclaw", "Hufflepuff" });
    [StringLength(1220, ErrorMessage = "Use at most 1,220 characters for Gryffindor.")]
    public string? Gryffindor { get; set; }
    [StringLength(1220, ErrorMessage = "Use at most 1,220 characters for Slytherin.")]
    public string? Slytherin { get; set; }
    [StringLength(1220, ErrorMessage = "Use at most 1,220 characters for Ravenclaw.")]
    public string? Ravenclaw { get; set; }
    [StringLength(1220, ErrorMessage = "Use at most 1,220 characters for Hufflepuff.")]
    public string? Hufflepuff { get; set; }
    [Required(ErrorMessage = "Enter a member to find.")]
    [StringLength(60, ErrorMessage = "Use at most 60 characters for the member name.")]
    public string? Member { get; set; }
    public string?[,]? HouseMembers { get; set; } // Rows are houses; unused member columns stay null.
    public List<HouseMatch> Matches { get; } = new();
}

public sealed record HouseMatch(string House, string Member, int Row, int Column);
