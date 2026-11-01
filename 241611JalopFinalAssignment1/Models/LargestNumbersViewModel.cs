using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

public sealed class LargestNumbersViewModel : NestedExampleViewModel
{
    public override string Title => "Largest of Three Numbers";
    public override string Description => "Compare the first two numbers, then compare the larger one with the third. Negative numbers, decimals and ties are welcome.";
    public override string Flow => "A >= B?\n+-- Yes: A >= C?\n|   +-- Yes: Largest = A\n|   +-- No: Largest = C\n+-- No: B >= C?\n    +-- Yes: Largest = B\n    +-- No: Largest = C";

    // Nullable decimals distinguish an empty input from a valid zero.
    [Required(ErrorMessage = "Enter number A.")]
    [Range(typeof(decimal), "-1000000000000", "1000000000000", ErrorMessage = "Number A must be between -1,000,000,000,000 and 1,000,000,000,000.")]
    public decimal? A { get; set; }

    [Required(ErrorMessage = "Enter number B.")]
    [Range(typeof(decimal), "-1000000000000", "1000000000000", ErrorMessage = "Number B must be between -1,000,000,000,000 and 1,000,000,000,000.")]
    public decimal? B { get; set; }

    [Required(ErrorMessage = "Enter number C.")]
    [Range(typeof(decimal), "-1000000000000", "1000000000000", ErrorMessage = "Number C must be between -1,000,000,000,000 and 1,000,000,000,000.")]
    public decimal? C { get; set; }
}
