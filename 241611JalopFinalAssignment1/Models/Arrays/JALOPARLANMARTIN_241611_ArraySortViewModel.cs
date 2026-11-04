using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Inputs and sorting direction for Arrays Example 2.
public enum JALOPARLANMARTIN_241611_ArraySortDirection { Ascending, Descending }

public sealed class JALOPARLANMARTIN_241611_ArraySortViewModel : JALOPARLANMARTIN_241611_FlatArrayViewModel
{
    public override int Number => 2;
    public override string Title => JALOPARLANMARTIN_241611_ExampleCatalog.GetTitle("arrays", 2);
    public override string Description => "Sort numbers numerically in ascending or descending order.";
    [EnumDataType(typeof(JALOPARLANMARTIN_241611_ArraySortDirection), ErrorMessage = "Choose Ascending or Descending.")]
    public JALOPARLANMARTIN_241611_ArraySortDirection Direction { get; set; }
}
