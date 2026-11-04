using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// The two retry games share a protected round token and attempt counters.
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
