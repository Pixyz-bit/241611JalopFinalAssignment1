using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Inputs, validation rules and output labels for this loop example.
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
