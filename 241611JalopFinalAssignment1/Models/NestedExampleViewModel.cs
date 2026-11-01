using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Shared presentation data lets every decision example reuse its result and flow views.
public abstract class NestedExampleViewModel
{
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract string Flow { get; }
    public string? Result { get; set; } // Only the controller supplies an outcome.
    public string? Detail { get; set; }
    public List<string> Decisions { get; } = new(); // Ordered checks explain the branch taken.
}

public sealed class LoginAccessViewModel : NestedExampleViewModel
{
    public override string Title => "Login and Access Checker";
    public override string Description => "Match a sample account, then check its role. This simulator does not sign you in.";
    public override string Flow => "Credentials match?\n+-- No: Invalid credentials\n+-- Yes: Admin role?\n    +-- Yes: Admin options\n    +-- No: Student role?\n        +-- Yes: Student options\n        +-- No: Access denied";

    [Required(ErrorMessage = "Enter a sample username.")]
    [StringLength(40, ErrorMessage = "Use at most 40 characters.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Enter the sample password.")]
    [StringLength(80, ErrorMessage = "Use at most 80 characters.")]
    [DataType(DataType.Password)] // Razor renders a masked password field.
    public string? Password { get; set; }
}
