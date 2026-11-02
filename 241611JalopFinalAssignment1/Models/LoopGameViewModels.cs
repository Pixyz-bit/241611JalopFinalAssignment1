using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

public abstract class RetryLoopViewModel : LoopExampleViewModel
{
    public override string LoopKind => "do…while";
    [Required(ErrorMessage = "Start a new round before submitting.")]
    [StringLength(8192, ErrorMessage = "Start a new round before submitting.")]
    public string? StateToken { get; set; }
    public int AttemptsUsed { get; set; }
    public abstract int AttemptLimit { get; }
    public int AttemptsRemaining => AttemptLimit - AttemptsUsed;
}

// Only the protected token carries these records; editable form values never choose the target or history.
public sealed record GuessRound(int Target, List<int> Guesses);
public sealed record QuizRound(int QuestionId, List<int> Answers);

public sealed class GuessGameViewModel : RetryLoopViewModel
{
    public override string Title => ExampleCatalog.GetTitle("loops", 4);
    public override string Description => "Guess the number from 1 to 100. A do…while loop checks your attempts and gives higher/lower feedback.";
    public override string ActionName => "GuessNumber";
    public override string SubmitLabel => "Guess";
    public override string ClearLabel => "New game";
    public override string InputPartial => "_GuessInputs";
    public override int AttemptLimit => 10;
    public override string Code => "int index = 0;\ndo\n{\n    guess = guesses[index];\n    // Compare the guess with the target.\n    index++;\n}\nwhile (index < guesses.Count && guess != target\n       && index < 10);";

    [Required(ErrorMessage = "Enter your guess.")]
    [Range(1, 100, ErrorMessage = "Guess a whole number from 1 to 100.")]
    public int? Guess { get; set; }
}

public sealed record LoopQuizQuestion(string Prompt, string[] Options, int CorrectAnswer, string Explanation);

public static class LoopQuizCatalog
{
    // A small public teaching bank gives each new quiz one clear loop question.
    public static IReadOnlyList<LoopQuizQuestion> Questions { get; } = Array.AsReadOnly(new[]
    {
        new LoopQuizQuestion("Which loop executes its body at least once?", new[] { "for", "while", "do…while" }, 2,
            "A do…while loop checks its condition after executing the body."),
        new LoopQuizQuestion("When does a while loop check its condition?", new[] { "Before each iteration", "Only after the first iteration", "Only when the loop finishes" }, 0,
            "A while loop checks before each iteration, so its body may run zero times."),
        new LoopQuizQuestion("How many iterations does for (int i = 1; i <= 4; i++) perform?", new[] { "3", "4", "5" }, 1,
            "The loop runs for i = 1, 2, 3 and 4: four iterations.")
    });
}

public sealed class QuizRetryViewModel : RetryLoopViewModel
{
    public override string Title => ExampleCatalog.GetTitle("loops", 5);
    public override string Description => "Answer a C# loop question. A do…while loop checks your answers until correct or three attempts are used.";
    public override string ActionName => "RetryQuiz";
    public override string SubmitLabel => "Check answer";
    public override string ClearLabel => "New quiz";
    public override string InputPartial => "_QuizInputs";
    public override int AttemptLimit => 3;
    public override string Code => "int index = 0;\ndo\n{\n    answer = answers[index];\n    // Check this answer and record feedback.\n    index++;\n}\nwhile (index < answers.Count && answer != correctAnswer\n       && index < 3);";
    public int QuestionId { get; set; } // Set from trusted round data, never from a posted field.
    public LoopQuizQuestion Question => LoopQuizCatalog.Questions[QuestionId];

    [Required(ErrorMessage = "Choose an answer.")]
    [Range(0, 2, ErrorMessage = "Choose one of the listed answers.")]
    public int? Answer { get; set; }
}
