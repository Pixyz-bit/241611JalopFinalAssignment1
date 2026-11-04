using System.Globalization;
using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace _241611JalopFinalAssignment1.Controllers;

public sealed class JALOPARLANMARTIN_241611_ArraysController : Controller
{
    [HttpPost("concepts/arrays/example-1")]
    [ValidateAntiForgeryToken] // Native requests must include this site's form token.
    public IActionResult FindDuplicates([Bind("Values")] JALOPARLANMARTIN_241611_DuplicateFinderViewModel input)
    {
        if (ModelState.IsValid && TryReadValues(input, out var values, out _)) //splits text at commas or line breaks, trims spaces, and checks that every entry is a valid number.
        {
            input.OutputValues = values;
            // Parsed numbers have canonical text, so numerically equal values use the same dictionary key.
            var positions = new Dictionary<string, List<int>>(StringComparer.Ordinal);
            for (var index = 0; index < values.Length; index++) //increments the index position of duplicates
            {
                if (!positions.TryGetValue(values[index], out var indexes)) positions[values[index]] = indexes = new();
                indexes.Add(index); // C# array indexes start at zero.
            }
            for (var index = 0; index < values.Length; index++) //counts the number of index for each
            {
                var indexes = positions[values[index]];
                if (indexes.Count > 1 && indexes[0] == index) input.Duplicates.Add(new(values[index], indexes.Count, indexes));
            }
            input.Result = input.Duplicates.Count == 0 ? "No duplicates" : $"{input.Duplicates.Count} repeated {(input.Duplicates.Count == 1 ? "value" : "values")}";
            input.Detail = "Numbers are compared by their numeric value, so 1 and 1.0 match.";
        }
        return Example(input);
    }

    [HttpPost("concepts/arrays/example-2")]
    [ValidateAntiForgeryToken]
    public IActionResult SortValues([Bind("Values,Direction")] JALOPARLANMARTIN_241611_ArraySortViewModel input)
    {
        if (ModelState.IsValid && TryReadValues(input, out var values, out var numbers))
        {
            Array.Sort(numbers); // Sort numeric values, so 2 comes before 10.
            if (input.Direction == JALOPARLANMARTIN_241611_ArraySortDirection.Descending) Array.Reverse(numbers);
            values = numbers.Select(FormatNumber).ToArray();
            input.OutputValues = values;
            input.Result = $"{input.Direction} order";
            input.Detail = $"{values.Length} numbers sorted numerically. Repeated values are kept.";
        }
        return Example(input);
    }

    [HttpPost("concepts/arrays/example-3")]
    [ValidateAntiForgeryToken]
    public IActionResult FindExtremes([Bind("Row1,Row2,Row3")] JALOPARLANMARTIN_241611_ArrayExtremesViewModel input)
    {
        if (ModelState.IsValid && TryReadMatrix(input, out var numbers))
        {
            input.Numbers = numbers;
            // Start with a real cell so all-negative and all-positive arrays both work.
            var largest = numbers[0, 0];
            var smallest = numbers[0, 0];
            for (var row = 0; row < numbers.GetLength(0); row++)
            {
                for (var column = 0; column < numbers.GetLength(1); column++)
                {
                    var value = numbers[row, column]; // Two indexes locate a cell in a rectangular array.
                    if (value > largest)
                    {
                        largest = value;
                        input.LargestRow = row;
                        input.LargestColumn = column;
                    }
                    if (value < smallest)
                    {
                        smallest = value;
                        input.SmallestRow = row;
                        input.SmallestColumn = column;
                    }
                }
            }
            // Strict comparisons keep the first position when an extreme appears more than once.
            input.Largest = largest;
            input.Smallest = smallest;
            input.Result = $"Largest: {FormatNumber(largest)}; Smallest: {FormatNumber(smallest)}";
            input.Detail = $"{numbers.GetLength(0)} rows × {numbers.GetLength(1)} columns. Indexes start at 0; repeated extremes show their first index.";
        }
        return Example(input);
    }

    private bool TryReadMatrix(JALOPARLANMARTIN_241611_ArrayExtremesViewModel input, out decimal[,] numbers)
    {
        numbers = new decimal[0, 0];
        // Each submitted field becomes one of the three rows in the rectangular array.
        var rows = new[] { input.Row1 ?? "", input.Row2 ?? "", input.Row3 ?? "" };
        var fields = new[] { nameof(input.Row1), nameof(input.Row2), nameof(input.Row3) };
        var columns = rows[0].Split(',').Length;
        if (columns is < 1 or > 12)
        {
            ModelState.AddModelError(nameof(input.Row1), "Enter 1–12 comma-separated numbers in each row.");
            return false;
        }
        numbers = new decimal[3, columns]; // All three rows must have the same column count.
        for (var row = 0; row < rows.Length; row++)
        {
            var cells = rows[row].Split(',', StringSplitOptions.TrimEntries); // Keep missing entries for validation.
            if (rows[row].IndexOfAny(new[] { '\r', '\n' }) >= 0 || cells.Length != columns)
            {
                ModelState.AddModelError(fields[row], $"Row {row + 1} must contain {columns} comma-separated numbers on one line.");
                return false;
            }
            for (var column = 0; column < columns; column++)
            {
                if (cells[column].Length > 40 || !TryNumber(cells[column], out numbers[row, column]))
                {
                    ModelState.AddModelError(fields[row], $"Row {row + 1}, column {column + 1}: enter a number from −1,000,000,000 to 1,000,000,000.");
                    return false;
                }
            }
        }
        return true;
    }

    [HttpPost("concepts/arrays/example-4")]
    [ValidateAntiForgeryToken]
    public IActionResult GenerateTable([Bind("Rows,Columns")] JALOPARLANMARTIN_241611_MultiplicationTableViewModel input)
    {
        if (ModelState.IsValid && input.Rows is int rows && input.Columns is int columns)
        {
            var products = new int[rows, columns]; // A rectangular array has a row index and a column index.
            for (var row = 0; row < rows; row++)
                for (var column = 0; column < columns; column++)
                    products[row, column] = (row + 1) * (column + 1); // Displayed factors start at 1, array indexes at 0.
            input.Products = products;
            input.Result = $"{rows} × {columns} table";
            input.Detail = "Each cell contains its row number multiplied by its column number.";
        }
        return Example(input);
    }

    [HttpPost("concepts/arrays/example-5")]
    [ValidateAntiForgeryToken]
    public IActionResult FindHouse([Bind("Gryffindor,Slytherin,Ravenclaw,Hufflepuff,Member")] JALOPARLANMARTIN_241611_HouseMemberFinderViewModel input)
    {
        var rosters = new[] { input.Gryffindor, input.Slytherin, input.Ravenclaw, input.Hufflepuff };
        var parsed = new List<string[]>();
        for (var row = 0; row < rosters.Length; row++)
        {
            var roster = rosters[row] ?? "";
            // Split only on commas; trim spaces while allowing a house to have no entered members.
            var names = roster.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (roster.Contains('\n') || roster.Contains('\r') || names.Length > 20 || names.Any(name => name.Length > 60))
                ModelState.AddModelError(JALOPARLANMARTIN_241611_HouseMemberFinderViewModel.Houses[row], "Enter up to 20 comma-separated names, at most 60 characters each. Use commas instead of line breaks.");
            parsed.Add(names);
        }
        if (parsed.All(names => names.Length == 0))
            ModelState.AddModelError(nameof(input.Gryffindor), "Enter at least one house member.");
        var target = input.Member?.Trim();
        if (!string.IsNullOrEmpty(target) && target.IndexOfAny(new[] { ',', '\r', '\n' }) >= 0)
            ModelState.AddModelError(nameof(input.Member), "Search for one complete member name.");
        if (ModelState.IsValid && !string.IsNullOrEmpty(target))
        {
            // Pad shorter rosters with null cells so all four houses fit a genuine rectangular 2D array.
            var members = new string?[4, parsed.Max(names => names.Length)];
            for (var row = 0; row < members.GetLength(0); row++)
                for (var column = 0; column < parsed[row].Length; column++)
                    members[row, column] = parsed[row][column];
            input.HouseMembers = members;
            for (var row = 0; row < members.GetLength(0); row++)
            {
                for (var column = 0; column < members.GetLength(1); column++)
                {
                    if (!string.Equals(members[row, column], target, StringComparison.OrdinalIgnoreCase)) continue;
                    input.Matches.Add(new(JALOPARLANMARTIN_241611_HouseMemberFinderViewModel.Houses[row], members[row, column]!, row, column));
                    break; // A repeated name in one house should list that house only once.
                }
            }
            input.Result = input.Matches.Count == 0 ? "Member not found" : string.Join(", ", input.Matches.Select(match => match.House));
            input.Detail = input.Matches.Count == 0 ? $"No house contains {target}." : $"Found {target}. House rows and member columns use zero-based C# indexes.";
        }
        return Example(input);
    }

    private bool TryReadValues(JALOPARLANMARTIN_241611_FlatArrayViewModel input, out string[] values, out decimal[] numbers)
    {
        values = (input.Values ?? "").Split(new[] { ',', '\r', '\n' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        numbers = Array.Empty<decimal>();
        if (values.Length is < 1 or > 30 || values.Any(value => value.Length > 40))
        {
            ModelState.AddModelError(nameof(input.Values), "Enter 1–30 values, with at most 40 characters per value.");
            return false;
        }
        // Always parse numbers; a submitted legacy Names option cannot bypass numeric validation.
        numbers = new decimal[values.Length];
        for (var index = 0; index < values.Length; index++)
        {
            if (!TryNumber(values[index], out numbers[index]))
            {
                ModelState.AddModelError(nameof(input.Values), "Use numbers from −1,000,000,000 to 1,000,000,000, with a decimal point and no thousands separators.");
                return false;
            }
            values[index] = FormatNumber(numbers[index]); // Canonical text groups equivalent numbers together.
        }
        return true;
    }

    private static bool TryNumber(string? value, out decimal number) =>
        decimal.TryParse(value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out number) && number >= -1000000000m && number <= 1000000000m;

    private static string FormatNumber(decimal number) => number.ToString("0.############################", CultureInfo.InvariantCulture);

    private IActionResult Example(JALOPARLANMARTIN_241611_ArrayExampleViewModel input) =>
        View("~/Views/Home/JALOPARLANMARTIN_241611_Index.cshtml", new JALOPARLANMARTIN_241611_ShowcaseViewModel(JALOPARLANMARTIN_241611_ConceptCatalog.Find("arrays"), input.Number) { ArrayExample = input });
}
