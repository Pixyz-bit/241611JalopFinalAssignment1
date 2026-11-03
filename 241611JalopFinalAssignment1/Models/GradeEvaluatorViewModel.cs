using System.ComponentModel.DataAnnotations;
namespace _241611JalopFinalAssignment1.Models;


public sealed class GradeEvaluatorViewModel : NestedExampleViewModel
{
    public override string Title => ExampleCatalog.GetTitle("nested-if-else", 2);
    public override string Description => "Enter your final percentage to check whether you passed. The passing score is 75.";
    [Required(ErrorMessage = "Enter your final percentage.")]
    [Range(0, 100, ErrorMessage = "Enter a whole-number percentage from 0 to 100.")]
    public int? Percentage { get; set; }

    // Input validation is the outer decision; the passing check is nested inside it.
    public override string Flow => "Valid whole-number percentage (0–100)?\n├── Yes: Percentage >= 75?\n│   ├── Yes: Passed\n│   └── No: Failed\n└── No: Show input errors";
}