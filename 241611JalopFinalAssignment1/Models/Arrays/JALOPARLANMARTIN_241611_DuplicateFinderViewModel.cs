using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Inputs and duplicate results for Arrays Example 1.
public sealed class JALOPARLANMARTIN_241611_DuplicateFinderViewModel : JALOPARLANMARTIN_241611_FlatArrayViewModel
{
    public override int Number => 1;
    public override string Title => JALOPARLANMARTIN_241611_ExampleCatalog.GetTitle("arrays", 1);
    public override string Description => "Find repeated numbers or names and see how often each value appears.";
    public List<JALOPARLANMARTIN_241611_ArrayDuplicate> Duplicates { get; } = new();
}

public sealed record JALOPARLANMARTIN_241611_ArrayDuplicate(string Value, int Count, IReadOnlyList<int> Indexes);
