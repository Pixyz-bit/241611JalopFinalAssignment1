using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Common presentation data lets all loop examples share the existing two-panel layout.
public abstract class LoopExampleViewModel
{
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string LoopKind { get; }
    public abstract string ActionName { get; }
    public abstract string SubmitLabel { get; }
    public abstract string InputPartial { get; }
    public abstract string Code { get; }
    public virtual string ClearLabel => "Clear";
    public string? Result { get; set; } // Calculated output is never accepted from the submitted form.
    public string? Detail { get; set; }
    public string? Pattern { get; set; }
    public List<string> Steps { get; } = new();
    public bool IsFinished { get; set; }
}

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

public sealed class SumDigitsViewModel : LoopExampleViewModel
{
    public override string Title => ExampleCatalog.GetTitle("loops", 2);
    public override string Description => "Use a while loop to extract each digit and add it to a running total.";
    public override string LoopKind => "while";
    public override string ActionName => "SumDigits";
    public override string SubmitLabel => "Calculate";
    public override string InputPartial => "_SumDigitsInputs";
    public override string Code => "while (remaining > 0)\n{\n    sum += (int)(remaining % 10);\n    remaining /= 10;\n}";

    // Text binding preserves up to 18 exact decimal digits without browser number rounding.
    [Required(ErrorMessage = "Enter a whole number.")]
    [RegularExpression("^[0-9]{1,18}$", ErrorMessage = "Enter 1 to 18 digits, without signs, spaces or decimal points.")]
    public string? Number { get; set; }
}

public sealed class PrimeFactorsViewModel : LoopExampleViewModel
{
    public override string Title => ExampleCatalog.GetTitle("loops", 3);
    public override string Description => "Use while loops to repeatedly divide a whole number into its prime factors.";
    public override string LoopKind => "while";
    public override string ActionName => "Factorize";
    public override string SubmitLabel => "Factorize";
    public override string InputPartial => "_PrimeFactorsInputs";
    public override string Code => "while (divisor * divisor <= remaining)\n{\n    while (remaining % divisor == 0)\n    {\n        factors.Add(divisor);\n        remaining /= divisor;\n    }\n    divisor = divisor == 2 ? 3 : divisor + 2;\n}\n// Any remaining value above 1 is also prime.";

    [Required(ErrorMessage = "Enter a whole number.")]
    [Range(typeof(long), "2", "1000000000000", ErrorMessage = "Enter a whole number between 2 and 1,000,000,000,000.")]
    public long? Number { get; set; }
}
