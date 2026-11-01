using System.Globalization;
using System.Text;
using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace _241611JalopFinalAssignment1.Controllers;

public sealed class LoopsController : Controller
{
    [HttpPost("concepts/loops/example-1")]
    [ValidateAntiForgeryToken] // Native forms must include this site's antiforgery token.
    public IActionResult GeneratePyramid([Bind("Rows,Symbol")] PyramidViewModel input)
    {
        if (ModelState.IsValid && input.Rows is int rows)
        {
            var pattern = new StringBuilder(); // Build the text without repeatedly copying the whole pyramid.
            var symbols = 0;
            for (var row = 1; row <= rows; row++)
            {
                for (var space = 0; space < rows - row; space++) pattern.Append(' ');
                for (var column = 0; column < 2 * row - 1; column++)
                {
                    pattern.Append(input.Symbol);
                    symbols++;
                }
                if (row < rows) pattern.Append('\n');
            }
            input.Pattern = pattern.ToString();
            input.Result = $"{rows} {(rows == 1 ? "row" : "rows")}";
            input.Detail = $"{symbols} symbols. Each row adds two symbols to the previous row.";
        }
        return Example(1, input);
    }

    [HttpPost("concepts/loops/example-2")]
    [ValidateAntiForgeryToken]
    public IActionResult SumDigits([Bind("Number")] SumDigitsViewModel input)
    {
        if (ModelState.IsValid)
        {
            var remaining = ulong.Parse(input.Number!, CultureInfo.InvariantCulture);
            var sum = 0;
            while (remaining > 0)
            {
                var digit = (int)(remaining % 10); // Remainder after division by 10 gives the final digit.
                sum += digit;
                input.Steps.Add($"Add {digit}: total = {sum}.");
                remaining /= 10; // Integer division removes the digit just processed.
            }
            if (input.Steps.Count == 0) input.Steps.Add("The number is zero, so the while loop does not run.");
            input.Result = $"Digit sum: {sum}";
            input.Detail = $"{input.Number} → {sum}. Digits are processed from right to left.";
        }
        return Example(2, input);
    }

    [HttpPost("concepts/loops/example-3")]
    [ValidateAntiForgeryToken]
    public IActionResult Factorize([Bind("Number")] PrimeFactorsViewModel input)
    {
        if (ModelState.IsValid && input.Number is long number)
        {
            var remaining = number;
            long divisor = 2;
            var factors = new List<long>();
            while (divisor * divisor <= remaining) // No factor pair can have both values above the square root.
            {
                while (remaining % divisor == 0)
                {
                    factors.Add(divisor);
                    input.Steps.Add($"{remaining:N0} ÷ {divisor:N0} = {remaining / divisor:N0}");
                    remaining /= divisor;
                }
                divisor = divisor == 2 ? 3 : divisor + 2; // After 2, test only odd divisors.
            }
            if (remaining > 1)
            {
                factors.Add(remaining);
                input.Steps.Add($"The remaining {remaining:N0} is prime.");
            }
            input.Result = $"{number:N0} = {string.Join(" × ", factors.Select(factor => factor.ToString("N0")))}";
            input.Detail = factors.Count == 1 ? "This number is prime." : $"{factors.Count} prime factors, including repeated factors.";
        }
        return Example(3, input);
    }

    private IActionResult Example(int number, LoopExampleViewModel input) =>
        View("~/Views/Home/Index.cshtml", new ShowcaseViewModel(ConceptCatalog.Find("loops"), number) { Loop = input });
}
