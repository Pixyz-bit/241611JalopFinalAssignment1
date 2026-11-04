using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Duplicate Finder and Array Sort share the same list input and validation.
public enum JALOPARLANMARTIN_241611_ArrayValueKind { Numbers, Names }

public abstract class JALOPARLANMARTIN_241611_FlatArrayViewModel : JALOPARLANMARTIN_241611_ArrayExampleViewModel
{
    public override string Dimension => "One-dimensional array";
    [Required(ErrorMessage = "Enter some values.")]
    [StringLength(1500, ErrorMessage = "Use at most 1,500 characters.")]
    public string? Values { get; set; }
    [EnumDataType(typeof(JALOPARLANMARTIN_241611_ArrayValueKind), ErrorMessage = "Choose Numbers or Names.")]
    public JALOPARLANMARTIN_241611_ArrayValueKind ValueKind { get; set; }
    public string[] OutputValues { get; set; } = Array.Empty<string>();
}
