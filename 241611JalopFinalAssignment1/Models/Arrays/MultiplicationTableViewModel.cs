using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Dimensions, validation and the product grid for Arrays Example 4.
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
