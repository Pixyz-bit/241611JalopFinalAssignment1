using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// The quiz example chooses questions from this shared teaching bank.
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
