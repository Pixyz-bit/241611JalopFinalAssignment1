using System.Globalization;
using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;
using _241611JalopFinalAssignment1.Services;

namespace _241611JalopFinalAssignment1.Controllers;

public sealed class ArraysController(ExampleStateTokens stateTokens) : Controller
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

    [HttpPost("concepts/arrays/example-3")]
    [ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult PlayMove([Bind("Cell,StateToken")] TicTacToeViewModel input)
    {
        if (!stateTokens.TryRead<TicTacToeRound>("arrays-tic-tac-toe", input.StateToken, out var round) ||
            round?.Moves is null || round.Moves.Count > 9 || !ReplayBoard(input, round.Moves))
        {
            input = new TicTacToeViewModel { RoundValid = false, StateToken = input.StateToken };
            ModelState.AddModelError(nameof(input.StateToken), "This board is invalid. Choose New game to start again.");
            return Example(input);
        }
        if (ModelState.IsValid && input.Cell is int cell)
        {
            if (input.IsFinished) ModelState.AddModelError(nameof(input.Cell), "This game has finished. Choose New game to play again.");
            else if (input.Board[cell / 3, cell % 3] != '\0') ModelState.AddModelError(nameof(input.Cell), "That square is occupied. Choose an empty square.");
            else
            {
                round.Moves.Add(cell); // The next player is derived from protected history, never a posted mark.
                input = new TicTacToeViewModel { StateToken = stateTokens.Protect("arrays-tic-tac-toe", round) };
                ReplayBoard(input, round.Moves);
                ModelState.Remove(nameof(input.StateToken)); // Render the updated round token instead of the submitted one.
                ModelState.Remove(nameof(input.Cell));
            }
        }
        if (input.Moves.Count > 0)
        {
            input.Result = input.Winner is char winner ? $"{winner} wins!" : input.IsDraw ? "It's a draw" : $"{input.CurrentPlayer} to move";
            input.Detail = input.IsFinished ? "Choose New game to start a fresh board." : "Choose an empty square. X and O alternate turns.";
        }
        return Example(input);
    }

    private static bool ReplayBoard(TicTacToeViewModel input, List<int> moves)
    {
        for (var index = 0; index < moves.Count; index++)
        {
            var cell = moves[index];
            if (cell is < 0 or > 8 || input.IsFinished || input.Board[cell / 3, cell % 3] != '\0') return false;
            var mark = index % 2 == 0 ? 'X' : 'O';
            input.Board[cell / 3, cell % 3] = mark;
            input.Moves.Add($"Move {index + 1}: {mark} at row {cell / 3 + 1}, column {cell % 3 + 1}.");
            input.WinningCells = FindWinningCells(input.Board);
            if (input.WinningCells.Length > 0) input.Winner = mark;
            input.IsDraw = index == 8 && input.Winner is null;
        }
        input.CurrentPlayer = moves.Count % 2 == 0 ? 'X' : 'O';
        return true;
    }

    private static int[] FindWinningCells(char[,] board)
    {
        for (var index = 0; index < 3; index++)
        {
            if (board[index, 0] != '\0' && board[index, 0] == board[index, 1] && board[index, 1] == board[index, 2])
                return new[] { index * 3, index * 3 + 1, index * 3 + 2 };
            if (board[0, index] != '\0' && board[0, index] == board[1, index] && board[1, index] == board[2, index])
                return new[] { index, index + 3, index + 6 };
        }
        if (board[0, 0] != '\0' && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) return new[] { 0, 4, 8 };
        if (board[0, 2] != '\0' && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) return new[] { 2, 4, 6 };
        return Array.Empty<int>();
    }

    [HttpPost("concepts/arrays/example-4")]
    [ValidateAntiForgeryToken]
    public IActionResult GenerateTable([Bind("Rows,Columns")] MultiplicationTableViewModel input)
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
    public IActionResult FindHouse([Bind("Gryffindor,Slytherin,Ravenclaw,Hufflepuff,Member")] HouseMemberFinderViewModel input)
    {
        var rosters = new[] { input.Gryffindor, input.Slytherin, input.Ravenclaw, input.Hufflepuff };
        var parsed = new List<string[]>();
        for (var row = 0; row < rosters.Length; row++)
        {
            var roster = rosters[row] ?? "";
            // Split only on commas; trim spaces while allowing a house to have no entered members.
            var names = roster.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (roster.Contains('\n') || roster.Contains('\r') || names.Length > 20 || names.Any(name => name.Length > 60))
                ModelState.AddModelError(HouseMemberFinderViewModel.Houses[row], "Enter up to 20 comma-separated names, at most 60 characters each. Use commas instead of line breaks.");
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
                    input.Matches.Add(new(HouseMemberFinderViewModel.Houses[row], members[row, column]!, row, column));
                    break; // A repeated name in one house should list that house only once.
                }
            }
            input.Result = input.Matches.Count == 0 ? "Member not found" : string.Join(", ", input.Matches.Select(match => match.House));
            input.Detail = input.Matches.Count == 0 ? $"No house contains {target}." : $"Found {target}. House rows and member columns use zero-based C# indexes.";
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
