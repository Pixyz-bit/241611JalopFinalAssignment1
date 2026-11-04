using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

public sealed class LatinHonorsViewModel : NestedExampleViewModel
{
    public override string Title => ExampleCatalog.GetTitle("nested-if-else", 2);
    public override string Description => "Evaluate Latin honors using GWA only.";
    [Required(ErrorMessage = "Enter your GWA.")]
    [Range(typeof(decimal), "1.00", "5.00", ErrorMessage = "Enter a GWA from 1.00 to 5.00.")]
    public decimal? Gwa { get; set; } // Nullable decimal distinguishes an empty field from a numeric grade.

    // Every later threshold is nested inside the previous check's No branch.
    public override string Flow => "Valid GWA (1.00–5.00)?\n├── Yes: GWA <= 1.25?\n│   ├── Yes: Summa Cum Laude\n│   └── No: GWA <= 1.50?\n│       ├── Yes: Magna Cum Laude\n│       └── No: GWA <= 1.75?\n│           ├── Yes: Cum Laude\n│           └── No: No Latin Honors\n└── No: Show input errors";
}
