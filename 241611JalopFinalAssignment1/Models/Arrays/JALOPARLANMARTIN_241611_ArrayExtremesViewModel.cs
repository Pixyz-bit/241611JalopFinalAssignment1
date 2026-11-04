using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Array Example 3 receives row text and returns server-calculated extremes in a real 2D array.
public sealed class JALOPARLANMARTIN_241611_ArrayExtremesViewModel : JALOPARLANMARTIN_241611_ArrayExampleViewModel
{
    public override int Number => 3;
    public override string Title => JALOPARLANMARTIN_241611_ExampleCatalog.GetTitle("arrays", 3);
    public override string Description => "Find the largest and smallest numbers in a two-dimensional array.";
    public override string Dimension => "Two-dimensional array";

    [Required(ErrorMessage = "Enter the rows of your array.")]
    [StringLength(6000, ErrorMessage = "Use at most 6,000 characters.")]
    public string? Values { get; set; }

    // The controller binds only Values; submitted results and indexes are never trusted.
    public decimal[,]? Numbers { get; set; }
    public decimal? Largest { get; set; }
    public decimal? Smallest { get; set; }
    public int LargestRow { get; set; }
    public int LargestColumn { get; set; }
    public int SmallestRow { get; set; }
    public int SmallestColumn { get; set; }
}
