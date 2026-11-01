using System.ComponentModel.DataAnnotations;
namespace _241611JalopFinalAssignment1.Models;

// Photos intentionally have no model property: only text details may reach the server.
public class IdCardViewModel
{
    [Required, StringLength(80)]
    public string? FullName { get; set; }
    [Required, StringLength(30)]
    public string? StudentNumber { get; set; }
    [Required, StringLength(80)]
    public string? Course { get; set; }
    [Required, Range(1, 6)]
    public int? YearLevel { get; set; }
    public bool HasResult { get; set; }
}
