using System.ComponentModel.DataAnnotations;
namespace _241611JalopFinalAssignment1.Models;

public class TextCounterViewModel
{
    [StringLength(10000, ErrorMessage = "Keep your text within the 10,000 input-length limit.")]
    public string? Text { get; set; }
    public int Characters { get; set; } // Output counts are assigned by the controller.
    public int Words { get; set; }
    public bool HasResult { get; set; } // Distinguishes an empty form from a confirmed count of zero.
}
