using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

public sealed class BmiCalculatorViewModel : NestedExampleViewModel
{
    public override string Title => "BMI Calculator";
    public override string Description => "Calculate adult BMI from kilograms and meters, then follow the nested checks to find its category.";
    public override string Flow => "Valid positive measurements?\n+-- No: Show input errors\n+-- Yes: BMI = weight / (height * height)\n    BMI < 18.5?\n    +-- Yes: Underweight\n    +-- No: BMI < 25?\n        +-- Yes: Healthy weight\n        +-- No: BMI < 30?\n            +-- Yes: Overweight\n            +-- No: Obesity";

    // Explicit bounds keep decimal multiplication/division safe, including crafted requests.
    [Required(ErrorMessage = "Enter weight in kilograms.")]
    [Range(typeof(decimal), "0.000001", "1000000000", ErrorMessage = "Enter a positive weight from 0.000001 to 1,000,000,000 kg.")]
    public decimal? Weight { get; set; }

    [Required(ErrorMessage = "Enter height in meters.")]
    [Range(typeof(decimal), "0.000001", "1000000000", ErrorMessage = "Enter a positive height from 0.000001 to 1,000,000,000 m.")]
    public decimal? Height { get; set; }
}
