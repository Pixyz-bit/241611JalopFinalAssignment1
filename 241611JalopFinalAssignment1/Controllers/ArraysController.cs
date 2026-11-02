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

    [HttpPost("concepts/arrays/example-4")]
    [ValidateAntiForgeryToken]
    public IActionResult SearchGrid([Bind("Cells,SearchValue")] GridSearchViewModel input)
    {
        var count = input.Cells?.Count ?? 0;
        if (count != 9) ModelState.AddModelError(nameof(input.Cells), "Enter all nine grid values.");
        input.Cells = NormalizeCells(input.Cells); // Even invalid collections must remain safe to render as nine inputs.
        var grid = new decimal[3, 3];
        for (var index = 0; index < 9; index++)
        {
            if (!TryNumber(input.Cells[index]?.Trim(), out var value))
                ModelState.AddModelError($"Cells[{index}]", $"Row {index / 3 + 1}, column {index % 3 + 1}: enter a number from −1,000,000,000 to 1,000,000,000.");
            else grid[index / 3, index % 3] = value;
        }
        if (ModelState.IsValid && input.SearchValue is decimal target)
        {
            input.Grid = grid;
            for (var row = 0; row < grid.GetLength(0); row++)
                for (var column = 0; column < grid.GetLength(1); column++)
                    if (grid[row, column] == target) input.Matches.Add(new(row, column));
            input.Result = input.Matches.Count == 0 ? "Not found" : $"{input.Matches.Count} {(input.Matches.Count == 1 ? "match" : "matches")}";
            input.Detail = $"Searching for {FormatNumber(target)}. Row and column labels start at 1; C# indexes start at 0.";
        }
        return Example(input);
    }

    [HttpPost("concepts/arrays/example-5")]
    [ValidateAntiForgeryToken]
    public IActionResult OrganizeGroups([Bind("Members")] GroupOrganizerViewModel input)
    {
        if (input.Members?.Count != 9) ModelState.AddModelError(nameof(input.Members), "Enter all nine member names.");
        input.Members = NormalizeCells(input.Members);
        var groups = new string[3, 3];
        for (var index = 0; index < 9; index++)
        {
            var name = input.Members[index]?.Trim();
            if (string.IsNullOrEmpty(name) || name.Length > 40)
                ModelState.AddModelError($"Members[{index}]", $"Group {index / 3 + 1}, member {index % 3 + 1}: enter a name of 1–40 characters.");
            else groups[index / 3, index % 3] = name;
        }
        if (ModelState.IsValid)
        {
            input.Groups = groups;
            input.Result = "3 groups organized";
            input.Detail = "9 member slots. Names stay in the groups and positions you entered.";
        }
        return Example(input);
    }

    private static List<string?> NormalizeCells(List<string?>? cells) =>
        (cells ?? new()).Take(9).Concat(Enumerable.Repeat<string?>(null, 9)).Take(9).ToList();

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
