using System.ComponentModel.DataAnnotations;
namespace _241611JalopFinalAssignment1.Models;

public enum JALOPARLANMARTIN_241611_TimeUnit { Seconds, Minutes, Hours }
public class JALOPARLANMARTIN_241611_TimeConverterViewModel
{
    [Required(ErrorMessage = "Enter an amount of time.")]
    [Range(0, 1000000000, ErrorMessage = "Enter a value between 0 and 1,000,000,000.")]
    public decimal? Value { get; set; }
    [EnumDataType(typeof(JALOPARLANMARTIN_241611_TimeUnit))]
    public JALOPARLANMARTIN_241611_TimeUnit FromUnit { get; set; } = JALOPARLANMARTIN_241611_TimeUnit.Hours;
    [EnumDataType(typeof(JALOPARLANMARTIN_241611_TimeUnit))]
    public JALOPARLANMARTIN_241611_TimeUnit ToUnit { get; set; } = JALOPARLANMARTIN_241611_TimeUnit.Seconds;
    public decimal? Result { get; set; } // Calculated by C#, never trusted from a submitted form.

    // A tuple switch selects the explanation for the chosen source and destination units.
    public string Formula => (FromUnit, ToUnit) switch
    {
        (JALOPARLANMARTIN_241611_TimeUnit.Hours, JALOPARLANMARTIN_241611_TimeUnit.Seconds) => "Seconds = Hours × 3,600",
        (JALOPARLANMARTIN_241611_TimeUnit.Hours, JALOPARLANMARTIN_241611_TimeUnit.Minutes) => "Minutes = Hours × 60",
        (JALOPARLANMARTIN_241611_TimeUnit.Minutes, JALOPARLANMARTIN_241611_TimeUnit.Seconds) => "Seconds = Minutes × 60",
        (JALOPARLANMARTIN_241611_TimeUnit.Minutes, JALOPARLANMARTIN_241611_TimeUnit.Hours) => "Hours = Minutes ÷ 60",
        (JALOPARLANMARTIN_241611_TimeUnit.Seconds, JALOPARLANMARTIN_241611_TimeUnit.Minutes) => "Minutes = Seconds ÷ 60",
        (JALOPARLANMARTIN_241611_TimeUnit.Seconds, JALOPARLANMARTIN_241611_TimeUnit.Hours) => "Hours = Seconds ÷ 3,600",
        _ => "Output = Input (same unit)"
    };
}
