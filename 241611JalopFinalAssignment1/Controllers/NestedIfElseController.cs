using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace _241611JalopFinalAssignment1.Controllers;

public sealed class NestedIfElseController : Controller
{
    [HttpPost("concepts/nested-if-else/example-1")]
    [ValidateAntiForgeryToken] // Reject submissions that lack this site's form token.
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult CheckAccess([Bind("Username,Password")] LoginAccessViewModel input)
    {
        if (ModelState.IsValid)
        {
            // Public teaching accounts only: the matching account determines the role.
            var role = input.Username switch
            {
                "admin" when input.Password == "admin123" => "Admin",
                "student" when input.Password == "student123" => "Student",
                "guest" when input.Password == "guest123" => "Guest",
                _ => null
            };
            if (role is null)
            {
                input.Decisions.Add("Credentials match? No.");
                input.Result = "Invalid credentials";
                input.Detail = "Try one of the sample accounts shown below.";
            }
            else
            {
                input.Decisions.Add("Credentials match? Yes.");
                if (role == "Admin") // This role check is nested inside the successful match.
                {
                    input.Decisions.Add("Admin role? Yes.");
                    input.Result = "Welcome, Admin";
                    input.Detail = "Sample permissions: manage users and view reports.";
                }
                else
                {
                    input.Decisions.Add("Admin role? No.");
                    if (role == "Student")
                    {
                        input.Decisions.Add("Student role? Yes.");
                        input.Result = "Welcome, Student";
                        input.Detail = "Sample permissions: view lessons and submit assignments.";
                    }
                    else
                    {
                        input.Decisions.Add("Student role? No.");
                        input.Result = "Access denied";
                        input.Detail = "The sample Guest role has no admin or student permissions.";
                    }
                }
            }
        }
        // Never render the submitted password back into HTML or the navigation cache.
        input.Password = null;
        var passwordErrors = ModelState[nameof(input.Password)]?.Errors.Select(error => error.ErrorMessage).ToArray() ?? Array.Empty<string>();
        ModelState.Remove(nameof(input.Password));
        foreach (var error in passwordErrors) ModelState.AddModelError(nameof(input.Password), error);
        return Example(1, input);
    }

    [HttpPost("concepts/nested-if-else/example-2")]
    [ValidateAntiForgeryToken]
    public IActionResult Withdraw([Bind("Balance,Amount")] AtmWithdrawalViewModel input)
    {
        if (ModelState.IsValid && input.Balance is decimal balance && input.Amount is decimal amount)
        {
            input.Decisions.Add("Valid amounts? Yes.");
            if (amount <= balance)
            {
                input.Decisions.Add("Enough balance? Yes.");
                if (amount % 100m == 0m) // % gives the remainder; zero means complete ₱100 bills.
                {
                    input.Decisions.Add("Multiple of ₱100? Yes.");
                    input.Result = "Withdrawal successful";
                    input.Detail = $"Withdrew ₱{amount:N2}. Remaining balance: ₱{balance - amount:N2}.";
                }
                else
                {
                    input.Decisions.Add("Multiple of ₱100? No.");
                    input.Result = "Use multiples of ₱100";
                    input.Detail = $"Balance stays at ₱{balance:N2}. Try 100, 200 or another multiple of 100.";
                }
            }
            else
            {
                input.Decisions.Add("Enough balance? No.");
                input.Result = "Insufficient balance";
                input.Detail = $"Balance stays at ₱{balance:N2}. Choose an amount within the balance.";
            }
        }
        return Example(2, input);
    }

    private IActionResult Example(int number, NestedExampleViewModel input)
    {
        // Reuse the desktop shell for both results and validation feedback.
        var page = new ShowcaseViewModel(ConceptCatalog.Find("nested-if-else"), number) { Nested = input };
        return View("~/Views/Home/Index.cshtml", page);
    }
}
