using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Shared presentation metadata keeps all array examples in the approved Input/Result layout.
public abstract class ArrayExampleViewModel
{
    public abstract int Number { get; }
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string Dimension { get; }
    public abstract string ActionName { get; }
    public abstract string SubmitLabel { get; }
    public abstract string InputPartial { get; }
    public abstract string ResultPartial { get; }
    public virtual bool ShowSubmit => true;
    public virtual string ClearLabel => "Clear";
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
    public override string Title => "Duplicate Finder";
    public override string Description => "Find repeated numbers or names and see how often each value appears.";
    public override string ActionName => "FindDuplicates";
    public override string SubmitLabel => "Find duplicates";
    public override string InputPartial => "_FlatInputs";
    public override string ResultPartial => "_DuplicateResult";
    public List<ArrayDuplicate> Duplicates { get; } = new();
}

public sealed class ArraySortViewModel : FlatArrayViewModel
{
    public override int Number => 2;
    public override string Title => "Array Sort";
    public override string Description => "Sort numbers numerically or names alphabetically, in either direction.";
    public override string ActionName => "SortValues";
    public override string SubmitLabel => "Sort";
    public override string InputPartial => "_FlatInputs";
    public override string ResultPartial => "_SortResult";
    [EnumDataType(typeof(ArraySortDirection), ErrorMessage = "Choose Ascending or Descending.")]
    public ArraySortDirection Direction { get; set; }
}
