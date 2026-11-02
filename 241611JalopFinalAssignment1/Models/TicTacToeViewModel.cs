using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

public sealed record TicTacToeRound(List<int> Moves);

public sealed class TicTacToeViewModel : ArrayExampleViewModel
{
    public override int Number => 3;
    public override string Title => ExampleCatalog.GetTitle("arrays", 3);
    public override string Description => "Two players take turns on a 3×3 array. Complete a row, column or diagonal to win.";
    public override string Dimension => "Two-dimensional array";
    public override string ActionName => "PlayMove";
    public override string SubmitLabel => "Play";
    public override bool ShowSubmit => false; // Each board square is a native submit button.
    public override string ClearLabel => "New game";
    public override string InputPartial => "_TicInputs";
    public override string ResultPartial => "_TicResult";
    [Required(ErrorMessage = "Start a new game before playing.")]
    [StringLength(8192, ErrorMessage = "Start a new game before playing.")]
    public string? StateToken { get; set; }
    [Required(ErrorMessage = "Choose an empty square.")]
    [Range(0, 8, ErrorMessage = "Choose a square on the board.")]
    public int? Cell { get; set; }
    public char[,] Board { get; set; } = new char[3, 3];
    public char CurrentPlayer { get; set; } = 'X';
    public char? Winner { get; set; }
    public bool IsDraw { get; set; }
    public bool IsFinished => Winner.HasValue || IsDraw;
    public bool RoundValid { get; set; } = true;
    public int[] WinningCells { get; set; } = Array.Empty<int>();
    public List<string> Moves { get; } = new();
}
