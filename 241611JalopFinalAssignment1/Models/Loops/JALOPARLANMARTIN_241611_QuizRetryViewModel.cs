using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Inputs and validation for the Quiz Retry Simulator.
public sealed class JALOPARLANMARTIN_241611_QuizRetryViewModel : JALOPARLANMARTIN_241611_RetryLoopViewModel
{
    public override string Title => JALOPARLANMARTIN_241611_ExampleCatalog.GetTitle("loops", 5);
    public override string Description => "Answer a C# loop question. A do…while loop checks your answers until correct or three attempts are used.";
    public override int AttemptLimit => 3;
    public override string Code => "int index = 0;\ndo\n{\n    answer = answers[index];\n    // Check this answer and record feedback.\n    index++;\n}\nwhile (index < answers.Count && answer != correctAnswer\n       && index < 3);";
    public int QuestionId { get; set; } // Set from trusted round data, never from a posted field.
    public JALOPARLANMARTIN_241611_LoopQuizQuestion Question => JALOPARLANMARTIN_241611_LoopQuizCatalog.Questions[QuestionId];

    [Required(ErrorMessage = "Choose an answer.")]
    [Range(0, 2, ErrorMessage = "Choose one of the listed answers.")]
    public int? Answer { get; set; }
}

// Protected round data records the question and submitted answers.
public sealed record JALOPARLANMARTIN_241611_QuizRound(int QuestionId, List<int> Answers);
