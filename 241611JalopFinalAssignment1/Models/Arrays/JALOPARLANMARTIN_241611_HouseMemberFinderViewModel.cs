using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Rosters, search input and matching houses for Arrays Example 5.
public sealed class JALOPARLANMARTIN_241611_HouseMemberFinderViewModel : JALOPARLANMARTIN_241611_ArrayExampleViewModel
{
    public override int Number => 5;
    public override string Title => JALOPARLANMARTIN_241611_ExampleCatalog.GetTitle("arrays", 5);
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
    public List<JALOPARLANMARTIN_241611_HouseMatch> Matches { get; } = new();
}

public sealed record JALOPARLANMARTIN_241611_HouseMatch(string House, string Member, int Row, int Column);
