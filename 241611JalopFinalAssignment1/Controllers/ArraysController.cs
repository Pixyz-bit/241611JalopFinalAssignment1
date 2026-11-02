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
