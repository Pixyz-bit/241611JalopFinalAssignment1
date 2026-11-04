using System.ComponentModel.DataAnnotations;
namespace _241611JalopFinalAssignment1.Models;

public sealed class JALOPARLANMARTIN_241611_RectangleCalculatorViewModel
{
    [Required(ErrorMessage = "Enter a length.")]
    [Range(typeof(decimal), "0.01", "1000000", ErrorMessage = "Enter a length from 0.01 to 1,000,000.")]
    public decimal? Length { get; set; }

    [Required(ErrorMessage = "Enter a width.")]
    [Range(typeof(decimal), "0.01", "1000000", ErrorMessage = "Enter a width from 0.01 to 1,000,000.")]
    public decimal? Width { get; set; }

    // Only the controller calculates these values; submitted results are excluded from binding.
    public decimal? Area { get; set; }
    public decimal? Perimeter { get; set; }
}
