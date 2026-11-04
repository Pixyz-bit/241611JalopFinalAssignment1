using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Loop Example 5 counts numeric digits, including one iteration for the number zero.
public sealed class JALOPARLANMARTIN_241611_DigitCounterViewModel : JALOPARLANMARTIN_241611_LoopExampleViewModel
{
    public override string Title => JALOPARLANMARTIN_241611_ExampleCatalog.GetTitle("loops", 5);
    public override string Description => "Count the digits in a whole number using a do…while loop.";
    public override string LoopKind => "do…while";
    public override string Code => "int count = 0;\ndo\n{\n    count++;\n    remaining /= 10;\n}\nwhile (remaining > 0);";

    // Text input preserves all 18 digits without browser number rounding; validation rejects signs and decimals.
    [Required(ErrorMessage = "Enter a whole number.")]
    [RegularExpression("^[0-9]{1,18}$", ErrorMessage = "Enter 1 to 18 digits, without signs, spaces or decimal points.")]
    public string? Number { get; set; }
}
