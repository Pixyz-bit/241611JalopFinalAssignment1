using System.ComponentModel.DataAnnotations;
namespace _241611JalopFinalAssignment1.Models;

public enum GradeStatus { Graded, Incomplete, Withdrawn }
public sealed record GradeBand(int Minimum, string Percentage, string Grade, string Equivalent);

public sealed class GradeEvaluatorViewModel : NestedExampleViewModel
{
    public override string Title => ExampleCatalog.GetTitle("nested-if-else", 2);
    public override string Description => "Convert a whole-number percentage using the supplied grading scale, or select Incomplete or Withdrawn.";
    [EnumDataType(typeof(GradeStatus), ErrorMessage = "Choose a valid assessment status.")]
    public GradeStatus Status { get; set; }
    [Range(0, 100, ErrorMessage = "Enter a whole-number percentage from 0 to 100.")]
    public int? Percentage { get; set; }

    // One reference list supplies both the displayed scale and the compact decision explanation.
    public static IReadOnlyList<GradeBand> Bands { get; } = Array.AsReadOnly(new[] {
        new GradeBand(97, "97–100", "1.0", "Excellent"), new GradeBand(94, "94–96", "1.25", "Excellent"),
        new GradeBand(91, "91–93", "1.5", "Very Good"), new GradeBand(88, "88–90", "1.75", "Very Good"),
        new GradeBand(85, "85–87", "2.0", "Good"), new GradeBand(82, "82–84", "2.25", "Good"),
        new GradeBand(79, "79–81", "2.5", "Satisfactory"), new GradeBand(76, "76–78", "2.75", "Satisfactory"),
        new GradeBand(75, "75", "3.0", "Passing"), new GradeBand(65, "65–74", "4.0", "Conditional"),
        new GradeBand(0, "0–64", "5.0", "Failure")
    });
    public override string Flow
    {
        get
        {
            var lines = new List<string> { "Incomplete?", "├── Yes: INC — Incomplete", "└── No: Withdrawn?", "    ├── Yes: W — Withdrawn", "    └── No: Valid whole-number percentage?", "        ├── No: Show input errors", "        └── Yes: Evaluate percentage" };
            // Each unsuccessful threshold leads to the next nested check, without blank spacer rows.
            var indent = "            ";
            foreach (var band in Bands.Take(Bands.Count - 1))
            {
                lines.Add($"{indent}Percentage >= {band.Minimum}?");
                lines.Add($"{indent}├── Yes: {band.Grade} — {band.Equivalent}");
                lines.Add($"{indent}└── No:");
                indent += "    ";
            }
            lines.Add($"{indent}5.0 — Failure");
            return string.Join("\n", lines);
        }
    }
}
