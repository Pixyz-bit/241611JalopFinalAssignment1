using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

public sealed class TriangleClassifierViewModel : NestedExampleViewModel
{
    public override string Title => "Triangle Classifier";
    public override string Description => "Check that three sides form a triangle, then classify it by equal side lengths. Use the same unit for every side.";
    public override string Flow => "All sides positive?\n+-- No: Show input errors\n+-- Yes: Every pair sums to more than the third?\n    +-- No: Not a valid triangle\n    +-- Yes: All three sides equal?\n        +-- Yes: Equilateral\n        +-- No: Any two sides equal?\n            +-- Yes: Isosceles\n            +-- No: Scalene";

    // Bounded decimal sides allow exact equality and safe sums for triangle inequalities.
    [Required(ErrorMessage = "Enter side A.")]
    [Range(typeof(decimal), "0.000001", "1000000000000", ErrorMessage = "Side A must be from 0.000001 to 1,000,000,000,000.")]
    public decimal? A { get; set; }

    [Required(ErrorMessage = "Enter side B.")]
    [Range(typeof(decimal), "0.000001", "1000000000000", ErrorMessage = "Side B must be from 0.000001 to 1,000,000,000,000.")]
    public decimal? B { get; set; }

    [Required(ErrorMessage = "Enter side C.")]
    [Range(typeof(decimal), "0.000001", "1000000000000", ErrorMessage = "Side C must be from 0.000001 to 1,000,000,000,000.")]
    public decimal? C { get; set; }
}
