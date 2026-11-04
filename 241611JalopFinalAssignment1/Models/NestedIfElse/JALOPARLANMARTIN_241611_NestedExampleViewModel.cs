namespace _241611JalopFinalAssignment1.Models;

// The five dedicated decision models inherit these common result properties.
public abstract class JALOPARLANMARTIN_241611_NestedExampleViewModel
{
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string Flow { get; }
    public string? Result { get; set; } // Only the controller supplies an outcome.
    public string? Detail { get; set; }
    public List<string> Decisions { get; } = new(); // Ordered checks explain the branch taken.
}
