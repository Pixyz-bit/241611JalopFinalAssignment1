namespace _241611JalopFinalAssignment1.Models;

// Common labels and results inherited by the five dedicated array models.
public abstract class JALOPARLANMARTIN_241611_ArrayExampleViewModel
{
    public abstract int Number { get; }
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string Dimension { get; }
    public string? Result { get; set; } // Controllers supply output; form binding accepts inputs only.
    public string? Detail { get; set; }
}
