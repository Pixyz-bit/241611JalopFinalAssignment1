using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Each array view reads these shared labels and results; its form markup stays in its own Razor file.
public abstract class ArrayExampleViewModel
{
    public abstract int Number { get; }
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string Dimension { get; }
    public string? Result { get; set; } // Controllers supply output; form binding accepts inputs only.
    public string? Detail { get; set; }
}

public enum ArrayValueKind { Numbers, Names }
public enum ArraySortDirection { Ascending, Descending }

public abstract class FlatArrayViewModel : ArrayExampleViewModel
{
    public override string Dimension => "One-dimensional array";
    [Required(ErrorMessage = "Enter some values.")]
    [StringLength(1500, ErrorMessage = "Use at most 1,500 characters.")]
    public string? Values { get; set; }
    [EnumDataType(typeof(ArrayValueKind), ErrorMessage = "Choose Numbers or Names.")]
    public ArrayValueKind ValueKind { get; set; }
    public string[] OutputValues { get; set; } = Array.Empty<string>();
}

public sealed record ArrayDuplicate(string Value, int Count, IReadOnlyList<int> Indexes);

public sealed class DuplicateFinderViewModel : FlatArrayViewModel
{
    public override int Number => 1;
    public override string Title => ExampleCatalog.GetTitle("arrays", 1);
    public override string Description => "Find repeated numbers or names and see how often each value appears.";
    public List<ArrayDuplicate> Duplicates { get; } = new();
}

public sealed class ArraySortViewModel : FlatArrayViewModel
{
    public override int Number => 2;
    public override string Title => ExampleCatalog.GetTitle("arrays", 2);
    public override string Description => "Sort numbers numerically or names alphabetically, in either direction.";
    [EnumDataType(typeof(ArraySortDirection), ErrorMessage = "Choose Ascending or Descending.")]
    public ArraySortDirection Direction { get; set; }
}
