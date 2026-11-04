using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Inputs and sorting direction for Arrays Example 2.
public enum ArraySortDirection { Ascending, Descending }

public sealed class ArraySortViewModel : FlatArrayViewModel
{
    public override int Number => 2;
    public override string Title => ExampleCatalog.GetTitle("arrays", 2);
    public override string Description => "Sort numbers numerically or names alphabetically, in either direction.";
    [EnumDataType(typeof(ArraySortDirection), ErrorMessage = "Choose Ascending or Descending.")]
    public ArraySortDirection Direction { get; set; }
}
