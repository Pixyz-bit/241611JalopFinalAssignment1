using System.Globalization;
using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace _241611JalopFinalAssignment1.Controllers;

public sealed class ArraysController : Controller
{
    [HttpPost("concepts/arrays/example-1")]
    [ValidateAntiForgeryToken] // Native requests must include this site's form token.
    public IActionResult FindDuplicates([Bind("Values,ValueKind")] DuplicateFinderViewModel input)
    {
        if (ModelState.IsValid && TryReadValues(input, out var values, out _))
        {
            input.OutputValues = values;
            var comparer = input.ValueKind == ArrayValueKind.Names ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            var positions = new Dictionary<string, List<int>>(comparer);
            for (var index = 0; index < values.Length; index++)
            {
                if (!positions.TryGetValue(values[index], out var indexes)) positions[values[index]] = indexes = new();
                indexes.Add(index); // C# array indexes start at zero.
            }
            for (var index = 0; index < values.Length; index++)
            {
                var indexes = positions[values[index]];
                if (indexes.Count > 1 && indexes[0] == index) input.Duplicates.Add(new(values[index], indexes.Count, indexes));
            }
            input.Result = input.Duplicates.Count == 0 ? "No duplicates" : $"{input.Duplicates.Count} repeated {(input.Duplicates.Count == 1 ? "value" : "values")}";
            input.Detail = input.ValueKind == ArrayValueKind.Names ? "Names are compared without letter case. Spaces around each name are ignored." : "Numbers are compared by their numeric value, so 1 and 1.0 match.";
        }
        return Example(input);
    }

    [HttpPost("concepts/arrays/example-2")]
    [ValidateAntiForgeryToken]
    public IActionResult SortValues([Bind("Values,ValueKind,Direction")] ArraySortViewModel input)
    {
        if (ModelState.IsValid && TryReadValues(input, out var values, out var numbers))
        {
            if (input.ValueKind == ArrayValueKind.Numbers)
            {
                Array.Sort(numbers); // Sort the actual decimal array, not the text representation of each number.
                if (input.Direction == ArraySortDirection.Descending) Array.Reverse(numbers);
                values = numbers.Select(FormatNumber).ToArray();
            }
            else
            {
                Array.Sort(values, StringComparer.OrdinalIgnoreCase);
                if (input.Direction == ArraySortDirection.Descending) Array.Reverse(values);
            }
            input.OutputValues = values;
            input.Result = $"{input.Direction} order";
            input.Detail = $"{values.Length} values sorted {(input.ValueKind == ArrayValueKind.Numbers ? "numerically" : "alphabetically, without letter case")}. Repeated values are kept.";
        }
        return Example(input);
    }

    private bool TryReadValues(FlatArrayViewModel input, out string[] values, out decimal[] numbers)
    {
        values = (input.Values ?? "").Split(new[] { ',', '\r', '\n' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        numbers = Array.Empty<decimal>();
        if (values.Length is < 1 or > 30 || values.Any(value => value.Length > 40))
        {
            ModelState.AddModelError(nameof(input.Values), "Enter 1–30 values, with at most 40 characters per value.");
            return false;
        }
        if (input.ValueKind == ArrayValueKind.Numbers)
        {
            numbers = new decimal[values.Length];
            for (var index = 0; index < values.Length; index++)
            {
                if (!TryNumber(values[index], out numbers[index]))
                {
                    ModelState.AddModelError(nameof(input.Values), "Use numbers from −1,000,000,000 to 1,000,000,000, with a decimal point and no thousands separators.");
                    return false;
                }
                values[index] = FormatNumber(numbers[index]); // Canonical text groups numerically equal values together.
            }
        }
        return true;
    }

    private static bool TryNumber(string? value, out decimal number) =>
        decimal.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out number) && number >= -1000000000m && number <= 1000000000m;

    private static string FormatNumber(decimal number) => number.ToString("0.############################", CultureInfo.InvariantCulture);

    private IActionResult Example(ArrayExampleViewModel input) =>
        View("~/Views/Home/Index.cshtml", new ShowcaseViewModel(ConceptCatalog.Find("arrays"), input.Number) { ArrayExample = input });
}
