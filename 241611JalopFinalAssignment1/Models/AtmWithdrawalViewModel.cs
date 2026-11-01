using System.ComponentModel.DataAnnotations;

namespace _241611JalopFinalAssignment1.Models;

public sealed class AtmWithdrawalViewModel : NestedExampleViewModel
{
    public override string Title => "ATM Withdrawal Simulator";
    public override string Description => "Check the balance before checking the denomination. This sample ATM dispenses ₱100 bills only; nothing is saved.";
    public override string Flow => "Valid amounts?\n+-- No: Show input errors\n+-- Yes: Enough balance?\n    +-- No: Insufficient balance\n    +-- Yes: Multiple of 100?\n        +-- No: Use multiples of P100\n        +-- Yes: Withdraw and show remaining balance";

    [Required(ErrorMessage = "Enter the starting balance.")]
    [Range(typeof(decimal), "0", "1000000000000", ErrorMessage = "Enter a balance from 0 to 1,000,000,000,000.")]
    public decimal? Balance { get; set; } // Decimal keeps peso calculations exact.

    [Required(ErrorMessage = "Enter a withdrawal amount.")]
    [Range(typeof(decimal), "0.01", "1000000000000", ErrorMessage = "Enter an amount from 0.01 to 1,000,000,000,000.")]
    public decimal? Amount { get; set; }
}
