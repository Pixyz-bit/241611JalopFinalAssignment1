# Approved loop examples

1. Pyramid Generator — nested `for` loops, rows and symbol inputs, centered text output.
2. Sum of Digits — a `while` loop extracts and adds decimal digits.
3. Prime Factorization — `while` loops repeatedly divide a positive integer into prime factors.
4. Number Guessing Game — `do…while` processes guesses until correct or the attempt limit is reached.
5. Digit Counter — a `do…while` loop removes one digit per iteration and counts zero as one digit.

All five use the existing Input/Result panels, field styles, result typography, canonical GET reset links and server-side validation. Calculations remain in C#. The guessing-game round travels in protected hidden form values so native POST works without a database or JavaScript. Keep guessing attempts bounded and reject altered round data.

## Limits and behavior

- Pyramid: 1–20 rows and one printable ASCII keyboard character; nested for loops append one leading space per missing row and one space between symbols, without trailing spaces. Rows contain 1, 2, 3, … symbols and stay centered. Razor encodes the output, including `<` and `>`.
- Digit sum: 1–18 decimal digits, including zero and leading zeros. A while loop extracts digits from right to left; zero has a sum of zero and requires no loop iterations.
- Prime factors: whole numbers from 2 to 1,000,000,000,000. Nested while loops remove repeated factors, testing only odd divisors after 2 and stopping at the square root of the remaining number.
- Guessing: a random target from 1–100 and at most 10 guesses. Higher/lower feedback, history, completion and New game work through native POST/GET.
- Digit counter: enter 1–18 digits for a non-negative whole number. A do…while loop increments the count and divides by ten before testing whether digits remain. Zero and all-zero input count as one digit; leading zeros are ignored (000472 has three digits). The Result panel shows the count, that convention and the ordered loop steps. Signs, spaces, decimals, separators, non-ASCII digits and longer values are rejected. Only Number is bound; forged results are ignored. Clear returns an empty form. No protected state is needed.

The guessing game replays its protected attempt list with an actual C# do…while loop. A valid submission appends one guess; the loop stops at the history's end, a correct guess or the limit. Later submissions continue the same round. Altered/missing tokens show a linked reset message. Inputs are bound through allowlists, and every POST requires antiforgery. Completed rounds cannot add attempts. Tokens can be replayed as an educational round; there is no competitive score or authoritative saved game.

## Validation

Digit Counter replaced Quiz Retry Simulator on November 5, 2026. Its dedicated files are `Models/Loops/JALOPARLANMARTIN_241611_DigitCounterViewModel.cs` and `Views/Concepts/Loops/JALOPARLANMARTIN_241611_DigitCounter.cshtml`; `JALOPARLANMARTIN_241611_LoopsController.CountDigits()` handles POST. The canonical URL remains `/concepts/loops/example-5`. The quiz model, question bank, view and action are removed; the guessing game retains its existing protected round.

Verification: build succeeded with zero warnings/errors and all 44 regression tests passed. Digit-counter cases cover zero, all-zero input, leading zeros, one and 18 digits, invalid text, forged output and missing antiforgery. HTML-encoding checks inspect the original response before decoding text. Browser checks covered `4725 → 4`, `0 → 1`, ordered loop steps, keyboard Tab/Enter submission, Clear, the desktop layout at 1366×900 and a 390px mobile layout with no document-width overflow. Existing focus hooks and reduced-motion rules are reused; no animation was added. The Razor detector returned no findings, and the rebuilt app is running locally.

```powershell
dotnet build 241611JalopFinalAssignment1.sln
node --test tests/navigation.test.cjs tests/input-output.test.cjs tests/nested-if-else.test.cjs tests/loops.test.cjs
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http --no-build
```

The app must be running for integration tests. `SHOWCASE_BASE_URL` overrides port 5196. The loop suite checks pyramid spacing and encoded symbols, zero/leading-zero/18-digit sums, prime and composite factors, limits, invalid inputs, antiforgery, guessing consistency, attempt exhaustion, altered round data, digit counts, zero, leading zeros, 18-digit inputs, forged results and malformed inputs.

Browser validation on November 2, 2026: all five examples use side-by-side panels at 1280×720 and stacked panels at 390×844, without horizontal workspace overflow. Phone fields measure 44px. A 20-row pyramid at 320×568 scrolls inside its own output; short landscape retains scrolling and taskbar clearance. Guessing feedback/reset, quiz retry/success/disabled controls, keyboard focus, Back/Forward, minimize/restore and maximized submission were checked. The existing reduced-motion rule is unchanged and no loop-specific animation was added. The application was rebuilt and rerun after each code batch.
