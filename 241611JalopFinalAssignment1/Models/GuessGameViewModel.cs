using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Inputs and validation for the Number Guessing Game.
public sealed class GuessGameViewModel : RetryLoopViewModel
{
    public override string Title => ExampleCatalog.GetTitle("loops", 4);
    public override string Description => "Guess the number from 1 to 100. A do…while loop checks your attempts and gives higher/lower feedback.";
    public override int AttemptLimit => 10;
    public override string Code => "int index = 0;\ndo\n{\n    guess = guesses[index];\n    // Compare the guess with the target.\n    index++;\n}\nwhile (index < guesses.Count && guess != target\n       && index < 10);";

    [Required(ErrorMessage = "Enter your guess.")]
    [Range(1, 100, ErrorMessage = "Guess a whole number from 1 to 100.")]
    public int? Guess { get; set; }
}

// Protected round data records the target and guesses; it is not editable form input.
public sealed record GuessRound(int Target, List<int> Guesses);
