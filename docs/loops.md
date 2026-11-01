# Approved loop examples

1. Pyramid Generator — nested `for` loops, rows and symbol inputs, centered text output.
2. Sum of Digits — a `while` loop extracts and adds decimal digits.
3. Prime Factorization — `while` loops repeatedly divide a positive integer into prime factors.
4. Number Guessing Game — `do…while` processes guesses until correct or the attempt limit is reached.
5. Quiz Retry Simulator — `do…while` processes answers until correct or the attempt limit is reached.

All five use the existing Input/Result panels, field styles, result typography, canonical GET reset links and server-side validation. Calculations remain in C#. Game rounds travel in protected hidden form values so native POST works without a database or JavaScript. Keep attempts bounded and reject altered round data.

## Limits and behavior

- Pyramid: 1–20 rows and one printable ASCII keyboard character; nested for loops append padding and symbols. Razor encodes the output, including `<` and `>`.
- Digit sum: 1–18 decimal digits, including zero and leading zeros. A while loop extracts digits from right to left; zero has a sum of zero and requires no loop iterations.
- Prime factors: whole numbers from 2 to 1,000,000,000,000. Nested while loops remove repeated factors, testing only odd divisors after 2 and stopping at the square root of the remaining number.
- Guessing: a random target from 1–100 and at most 10 guesses. Higher/lower feedback, history, completion and New game work through native POST/GET.
- Quiz: one randomly selected loop question from a three-question teaching bank and at most three answers. Correct or exhausted rounds reveal an explanation; New quiz resets attempts.

The games replay protected attempt lists with actual C# do…while loops. A valid submission appends one attempt; the loop stops at the current history's end, a correct response or the limit. The next form submission continues the same round. Separate protection purposes prevent cross-game token reuse. Altered/missing tokens show a linked reset message. Inputs are bound through allowlists, and every POST requires antiforgery. Completed rounds cannot add attempts. Tokens can be replayed as an educational round; there is no competitive score or authoritative saved game.

## Validation

```powershell
dotnet build 241611JalopFinalAssignment1.sln
node --test tests/navigation.test.cjs tests/input-output.test.cjs tests/nested-if-else.test.cjs tests/loops.test.cjs
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http --no-build
```

The app must be running for integration tests. `SHOWCASE_BASE_URL` overrides port 5196. The loop suite checks pyramid spacing and encoded symbols, zero/leading-zero/18-digit sums, prime and composite factors, limits, invalid inputs, antiforgery, guessing consistency, quiz retries, attempt exhaustion and altered/cross-game round data.

Browser validation on November 2, 2026: all five examples use side-by-side panels at 1280×720 and stacked panels at 390×844, without horizontal workspace overflow. Phone fields measure 44px. A 20-row pyramid at 320×568 scrolls inside its own output; short landscape retains scrolling and taskbar clearance. Guessing feedback/reset, quiz retry/success/disabled controls, keyboard focus, Back/Forward, minimize/restore and maximized submission were checked. The existing reduced-motion rule is unchanged and no loop-specific animation was added. The application was rebuilt and rerun after each code batch.
