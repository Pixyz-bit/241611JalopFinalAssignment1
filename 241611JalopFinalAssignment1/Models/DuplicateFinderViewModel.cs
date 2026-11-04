using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Inputs and duplicate results for Arrays Example 1.
public sealed class DuplicateFinderViewModel : FlatArrayViewModel
{
    public override int Number => 1;
    public override string Title => ExampleCatalog.GetTitle("arrays", 1);
    public override string Description => "Find repeated numbers or names and see how often each value appears.";
    public List<ArrayDuplicate> Duplicates { get; } = new();
}

public sealed record ArrayDuplicate(string Value, int Count, IReadOnlyList<int> Indexes);
