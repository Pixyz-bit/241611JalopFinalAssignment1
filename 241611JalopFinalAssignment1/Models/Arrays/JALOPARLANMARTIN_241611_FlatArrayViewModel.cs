using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Duplicate Finder and Array Sort share the same list input and validation.

public abstract class JALOPARLANMARTIN_241611_FlatArrayViewModel : JALOPARLANMARTIN_241611_ArrayExampleViewModel
{
    public override string Dimension => "One-dimensional array";
    [Required(ErrorMessage = "Enter some numbers.")]
    [StringLength(1500, ErrorMessage = "Use at most 1,500 characters.")]
    public string? Values { get; set; }
    public string[] OutputValues { get; set; } = Array.Empty<string>();
}
