using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

// Inputs and validation for the Login and Access Checker simulation.
public sealed class JALOPARLANMARTIN_241611_LoginAccessViewModel : JALOPARLANMARTIN_241611_NestedExampleViewModel
{
    public override string Title => JALOPARLANMARTIN_241611_ExampleCatalog.GetTitle("nested-if-else", 1);
    public override string Description => "Match a sample account, then check its role. This simulator does not sign you in.";
    public override string Flow => "Credentials match?\n├── Yes: Admin role?\n│   ├── Yes: Admin options\n│   └── No: Student role?\n│       ├── Yes: Student options\n│       └── No: Access denied\n└── No: Invalid credentials";

    [Required(ErrorMessage = "Enter a sample username.")]
    [StringLength(40, ErrorMessage = "Use at most 40 characters.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Enter the sample password.")]
    [StringLength(80, ErrorMessage = "Use at most 80 characters.")]
    [DataType(DataType.Password)] // Razor renders a masked password field.
    public string? Password { get; set; }
}
