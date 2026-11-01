using System.Globalization;
using System.Text;
using _241611JalopFinalAssignment1.Models;
using Microsoft.AspNetCore.Mvc;
using _241611JalopFinalAssignment1.Services;

namespace _241611JalopFinalAssignment1.Controllers;

public sealed class LoopsController(LoopRoundTokens roundTokens) : Controller
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

    [HttpPost("concepts/loops/example-4")]
    [ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult GuessNumber([Bind("Guess,StateToken")] GuessGameViewModel input)
    {
        if (!roundTokens.TryRead<GuessRound>("guess", input.StateToken, out var round) || round is null ||
            round.Target is < 1 or > 100 || round.Guesses is null || round.Guesses.Count > input.AttemptLimit)
        {
            ModelState.AddModelError(nameof(input.StateToken), "This round is invalid. Choose New game to start again.");
            return Example(4, input);
        }
        input.AttemptsUsed = round.Guesses.Count;
        var finished = round.Guesses.Contains(round.Target) || round.Guesses.Count >= input.AttemptLimit;
        if (ModelState.IsValid && input.Guess is int guess && !finished)
        {
            round.Guesses.Add(guess); // Append one attempt; existing history comes only from the protected token.
        }
        if (ModelState.IsValid || finished)
        {
            var index = 0;
            int current;
            do
            {
                current = round.Guesses[index];
                var feedback = current == round.Target ? "Correct" : current < round.Target ? "Too low" : "Too high";
                input.Steps.Add($"Attempt {index + 1}: {current} — {feedback}.");
                index++;
            }
            while (index < round.Guesses.Count && current != round.Target && index < input.AttemptLimit);
            input.AttemptsUsed = index;
            input.IsFinished = current == round.Target || index >= input.AttemptLimit;
            input.Result = current == round.Target ? "You found it!" : input.IsFinished ? "No attempts left" : current < round.Target ? "Too low" : "Too high";
            input.Detail = input.IsFinished ? $"The number was {round.Target}. Choose New game to play again." : $"Try a {(current < round.Target ? "higher" : "lower")} number. {input.AttemptsRemaining} attempts remaining.";
            input.StateToken = roundTokens.Protect("guess", round);
            input.Guess = input.IsFinished ? current : null;
            ModelState.Remove(nameof(input.Guess)); // Render a fresh field for the next attempt.
            ModelState.Remove(nameof(input.StateToken)); // Render the updated token rather than the submitted one.
        }
        return Example(4, input);
    }

    [HttpPost("concepts/loops/example-5")]
    [ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult RetryQuiz([Bind("Answer,StateToken")] QuizRetryViewModel input)
    {
        if (!roundTokens.TryRead<QuizRound>("quiz", input.StateToken, out var round) || round is null ||
            round.QuestionId < 0 || round.QuestionId >= LoopQuizCatalog.Questions.Count || round.Answers is null || round.Answers.Count > input.AttemptLimit)
        {
            ModelState.AddModelError(nameof(input.StateToken), "This round is invalid. Choose New quiz to start again.");
            return Example(5, input);
        }
        input.QuestionId = round.QuestionId;
        input.AttemptsUsed = round.Answers.Count;
        var correct = input.Question.CorrectAnswer;
        var finished = round.Answers.Contains(correct) || round.Answers.Count >= input.AttemptLimit;
        if (ModelState.IsValid && input.Answer is int answer && !finished) round.Answers.Add(answer);
        if (ModelState.IsValid || finished)
        {
            var index = 0;
            int current;
            do
            {
                current = round.Answers[index];
                input.Steps.Add($"Attempt {index + 1}: {input.Question.Options[current]} — {(current == correct ? "Correct" : "Incorrect")}.");
                index++;
            }
            while (index < round.Answers.Count && current != correct && index < input.AttemptLimit);
            input.AttemptsUsed = index;
            input.IsFinished = current == correct || index >= input.AttemptLimit;
            input.Result = current == correct ? "Correct!" : input.IsFinished ? "Attempts used" : "Try again";
            input.Detail = input.IsFinished ? $"Answer: {input.Question.Options[correct]}. {input.Question.Explanation}" : $"That answer is incorrect. {input.AttemptsRemaining} attempts remaining.";
            input.StateToken = roundTokens.Protect("quiz", round);
            input.Answer = input.IsFinished ? current : null;
            ModelState.Remove(nameof(input.Answer));
            ModelState.Remove(nameof(input.StateToken));
        }
        return Example(5, input);
    }

    private IActionResult Example(int number, LoopExampleViewModel input) =>
        View("~/Views/Home/Index.cshtml", new ShowcaseViewModel(ConceptCatalog.Find("loops"), number) { Loop = input });
}
