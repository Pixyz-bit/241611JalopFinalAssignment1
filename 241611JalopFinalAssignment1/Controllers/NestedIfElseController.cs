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

    [HttpPost("concepts/nested-if-else/example-3")]
    [ValidateAntiForgeryToken]
    public IActionResult CalculateBmi([Bind("Weight,Height")] BmiCalculatorViewModel input)
    {
        if (ModelState.IsValid && input.Weight is decimal weight && input.Height is decimal height)
        {
            input.Decisions.Add("Valid positive measurements? Yes.");
            var bmi = weight / (height * height); // Height must be in meters, and is squared here.
            string category;
            if (bmi < 18.5m)
            {
                input.Decisions.Add("BMI below 18.5? Yes.");
                category = "Underweight";
            }
            else
            {
                input.Decisions.Add("BMI below 18.5? No.");
                if (bmi < 25m)
                {
                    input.Decisions.Add("BMI below 25? Yes.");
                    category = "Healthy weight";
                }
                else
                {
                    input.Decisions.Add("BMI below 25? No.");
                    if (bmi < 30m)
                    {
                        input.Decisions.Add("BMI below 30? Yes.");
                        category = "Overweight";
                    }
                    else
                    {
                        input.Decisions.Add("BMI below 30? No.");
                        category = "Obesity";
                    }
                }
            }
            // Classify first, then round for display so boundary decisions use the full value.
            input.Result = category;
            input.Detail = $"BMI: {bmi:0.00} kg/m². Formula: {weight:0.######} / ({height:0.######} × {height:0.######}). Category uses the unrounded value.";
        }
        return Example(3, input);
    }

    [HttpPost("concepts/nested-if-else/example-4")]
    [ValidateAntiForgeryToken]
    public IActionResult ClassifyTriangle([Bind("A,B,C")] TriangleClassifierViewModel input)
    {
        if (ModelState.IsValid && input.A is decimal a && input.B is decimal b && input.C is decimal c)
        {
            input.Decisions.Add("All sides positive? Yes.");
            // && means every inequality must pass; equality describes a flat, invalid triangle.
            if (a + b > c && a + c > b && b + c > a)
            {
                input.Decisions.Add("Every pair sums to more than the third side? Yes.");
                if (a == b && b == c)
                {
                    input.Decisions.Add("All three sides equal? Yes.");
                    input.Result = "Equilateral";
                    input.Detail = "All three sides have the same length.";
                }
                else
                {
                    input.Decisions.Add("All three sides equal? No.");
                    if (a == b || a == c || b == c) // || means at least one pair is equal.
                    {
                        input.Decisions.Add("Any two sides equal? Yes.");
                        input.Result = "Isosceles";
                        input.Detail = "Exactly two sides have the same length.";
                    }
                    else
                    {
                        input.Decisions.Add("Any two sides equal? No.");
                        input.Result = "Scalene";
                        input.Detail = "All three sides have different lengths.";
                    }
                }
            }
            else
            {
                input.Decisions.Add("Every pair sums to more than the third side? No.");
                input.Result = "Not a valid triangle";
                input.Detail = "The sum of any two sides must be greater than the remaining side.";
            }
        }
        return Example(4, input);
    }

    [HttpPost("concepts/nested-if-else/example-5")]
    [ValidateAntiForgeryToken]
    public IActionResult FindLargest([Bind("A,B,C")] LargestNumbersViewModel input)
    {
        if (ModelState.IsValid && input.A is decimal a && input.B is decimal b && input.C is decimal c)
        {
            decimal largest;
            if (a >= b) // >= includes equality, so tied values follow a predictable branch.
            {
                input.Decisions.Add("A >= B? Yes.");
                if (a >= c)
                {
                    input.Decisions.Add("A >= C? Yes. Select A.");
                    largest = a;
                }
                else
                {
                    input.Decisions.Add("A >= C? No. Select C.");
                    largest = c;
                }
            }
            else
            {
                input.Decisions.Add("A >= B? No.");
                if (b >= c)
                {
                    input.Decisions.Add("B >= C? Yes. Select B.");
                    largest = b;
                }
                else
                {
                    input.Decisions.Add("B >= C? No. Select C.");
                    largest = c;
                }
            }
            input.Result = $"Largest: {largest:G29}"; // G29 preserves significant decimal digits.
            input.Detail = $"Compared A = {a:G29}, B = {b:G29} and C = {c:G29}. Tied maximum values share the same result.";
        }
        return Example(5, input);
    }

    private IActionResult Example(int number, NestedExampleViewModel input)
    {
        // Reuse the desktop shell for both results and validation feedback.
        var page = new ShowcaseViewModel(ConceptCatalog.Find("nested-if-else"), number) { Nested = input };
        return View("~/Views/Home/Index.cshtml", page);
    }
}
