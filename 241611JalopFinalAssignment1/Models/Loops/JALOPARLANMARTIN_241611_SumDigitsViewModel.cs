using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Inputs, validation rules and output labels for this loop example.
public sealed class JALOPARLANMARTIN_241611_SumDigitsViewModel : JALOPARLANMARTIN_241611_LoopExampleViewModel
{
    public override string Title => JALOPARLANMARTIN_241611_ExampleCatalog.GetTitle("loops", 2);
    public override string Description => "Use a while loop to extract each digit and add it to a running total.";
    public override string LoopKind => "while";
    public override string Code => "while (remaining > 0)\n{\n    sum += (int)(remaining % 10);\n    remaining /= 10;\n}";

    // Text binding preserves up to 18 exact decimal digits without browser number rounding.
    [Required(ErrorMessage = "Enter a whole number.")]
    [RegularExpression("^[0-9]{1,18}$", ErrorMessage = "Enter 1 to 18 digits, without signs, spaces or decimal points.")]
    public string? Number { get; set; }
}
