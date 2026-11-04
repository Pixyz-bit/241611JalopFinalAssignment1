using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Duplicate Finder and Array Sort share the same list input and validation.
public enum ArrayValueKind { Numbers, Names }

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
