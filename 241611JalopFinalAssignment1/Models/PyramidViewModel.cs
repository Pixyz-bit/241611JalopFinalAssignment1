using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Inputs, validation rules and output labels for this loop example.
public sealed class PyramidViewModel : LoopExampleViewModel
{
    public override string Title => ExampleCatalog.GetTitle("loops", 1);
    public override string Description => "Build a centered pyramid one row at a time using nested for loops.";
    public override string LoopKind => "for";
    public override string ActionName => "GeneratePyramid";
    public override string SubmitLabel => "Generate";
    public override string InputPartial => "_PyramidInputs";
    public override string Code => "for (int row = 1; row <= rows; row++)\n{\n    // Add rows - row leading spaces.\n    // Add row symbols, with one space between them.\n}";

    [Required(ErrorMessage = "Enter the number of rows.")]
    [Range(1, 20, ErrorMessage = "Choose between 1 and 20 rows.")]
    public int? Rows { get; set; }

    [Required(ErrorMessage = "Enter a symbol.")]
    [RegularExpression("^[!-~]$", ErrorMessage = "Use one printable keyboard symbol or letter, without spaces.")]
    public string? Symbol { get; set; }
}
