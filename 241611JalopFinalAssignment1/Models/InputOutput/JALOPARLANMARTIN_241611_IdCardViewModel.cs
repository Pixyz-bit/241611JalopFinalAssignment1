using System.ComponentModel.DataAnnotations;
namespace _241611JalopFinalAssignment1.Models;

// Photos intentionally have no model property: only text details may reach the server.
public class JALOPARLANMARTIN_241611_IdCardViewModel
{
    [Required, StringLength(80)]
    public string? FullName { get; set; }
    [Required, StringLength(30)]
    public string? StudentNumber { get; set; }
    [Required, StringLength(80)]
    public string? Course { get; set; }
    [Required, Range(1, 6)]
    public int? YearLevel { get; set; }
    // One display label keeps native Razor and enhanced JSON previews consistent.
    public string YearLevelLabel => YearLevel switch
    {
        1 => "1st year",
        2 => "2nd year",
        3 => "3rd year",
        4 => "4th year",
        5 => "5th year",
        6 => "6th year",
        _ => string.Empty
    };
    public bool HasResult { get; set; }
}
