using System.ComponentModel.DataAnnotations;
namespace _241611JalopFinalAssignment1.Models;

public enum TimeUnit { Seconds, Minutes, Hours }
public class TimeConverterViewModel
{
    [Required(ErrorMessage = "Enter an amount of time.")]
    [Range(0, 1000000000, ErrorMessage = "Enter a value between 0 and 1,000,000,000.")]
    public decimal? Value { get; set; }
    [EnumDataType(typeof(TimeUnit))]
    public TimeUnit FromUnit { get; set; } = TimeUnit.Hours;
    [EnumDataType(typeof(TimeUnit))]
    public TimeUnit ToUnit { get; set; } = TimeUnit.Seconds;
    public decimal? Result { get; set; } // Calculated by C#, never trusted from a submitted form.

    // A tuple switch selects the explanation for the chosen source and destination units.
    public string Formula => (FromUnit, ToUnit) switch
    {
        (TimeUnit.Hours, TimeUnit.Seconds) => "Seconds = Hours × 3,600",
        (TimeUnit.Hours, TimeUnit.Minutes) => "Minutes = Hours × 60",
        (TimeUnit.Minutes, TimeUnit.Seconds) => "Seconds = Minutes × 60",
        (TimeUnit.Minutes, TimeUnit.Hours) => "Hours = Minutes ÷ 60",
        (TimeUnit.Seconds, TimeUnit.Minutes) => "Minutes = Seconds ÷ 60",
        (TimeUnit.Seconds, TimeUnit.Hours) => "Hours = Seconds ÷ 3,600",
        _ => "Output = Input (same unit)"
    };
}
