namespace _241611JalopFinalAssignment1.Models;

// Shared properties inherited by the five dedicated loop example models.
public abstract class LoopExampleViewModel
{
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string LoopKind { get; }
    public abstract string ActionName { get; }
    public abstract string SubmitLabel { get; }
    public abstract string InputPartial { get; }
    public abstract string Code { get; }
    public virtual string ClearLabel => "Clear";
    public string? Result { get; set; } // Calculated output is never accepted from the submitted form.
    public string? Detail { get; set; }
    public string? Pattern { get; set; }
    public List<string> Steps { get; } = new();
    public bool IsFinished { get; set; }
}
