using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// An enum limits the unit choices to the three supported temperature scales.
public enum TemperatureUnit { Celsius, Fahrenheit, Kelvin }

public class TemperatureConverterViewModel
{
    [Required(ErrorMessage = "Enter a temperature.")]
    [Range(-1000000, 1000000, ErrorMessage = "Enter a value between -1,000,000 and 1,000,000.")]
    public decimal? Value { get; set; } // The input can now represent either selected unit.

    [EnumDataType(typeof(TemperatureUnit))]
    public TemperatureUnit FromUnit { get; set; } = TemperatureUnit.Celsius;

    [EnumDataType(typeof(TemperatureUnit))]
    public TemperatureUnit ToUnit { get; set; } = TemperatureUnit.Fahrenheit;

    public decimal? Result { get; set; } // Only the controller supplies the confirmed output.
    public string? Formula { get; set; } // Explains the calculation used for the confirmed result.
}
