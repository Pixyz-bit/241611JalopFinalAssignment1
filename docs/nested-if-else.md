# Nested If…Else examples

All five forms run their calculations in C# and work with JavaScript disabled. Each has a canonical Clear GET link, input validation, an expandable text decision tree and an ordered trace of the checks actually executed. Invalid input shows linked errors without a calculated result.

| Slot | Example | Try it |
| --- | --- | --- |
| 1 | Login and Access Checker | `student` / `student123` → student permissions |
| 2 | ATM Withdrawal Simulator | Balance 5000, amount 1200 → remaining ₱3800 |
| 3 | BMI Calculator | Weight 60 kg, height 1.65 m → BMI 22.04, healthy weight |
| 4 | Triangle Classifier | 3, 4, 5 → scalene |
| 5 | Largest of Three Numbers | 12, 25, 18 → 25 |

Routes follow `/concepts/nested-if-else/example-1` through `example-5`. Input/Output slot 2 remains available and links to BMI in its new category.

## Decision rules

- Login checks the username/password pair before the account's role. Public accounts are `admin` / `admin123`, `student` / `student123`, and `guest` / `guest123`. Matching is case-sensitive; Guest demonstrates a valid account with an unsupported role. Results describe sample permissions only. No real authentication or protected actions are created. Submitted passwords are removed from both the view model and ModelState before rendering; these responses send `Cache-Control: no-store`.
- ATM checks valid nonnegative balance and positive withdrawal, then sufficient funds, then divisibility by 100 using `%`. Success subtracts the amount exactly with decimal arithmetic. Each submission independently uses the entered balance, with no stored transaction history. The simulator assumes enough ₱100 bills are available.
- BMI validates positive measurements, computes `weight / (height * height)`, and nests checks at 18.5, 25 and 30. These are the [CDC categories for adults aged 20 and older](https://www.cdc.gov/bmi/adult-calculator/bmi-categories.html). Category selection uses the unrounded decimal value; display rounds to two decimal places. BMI is a screening measure, not a diagnosis.
- Triangle checks positive sides, then all three strict triangle inequalities. Only a valid triangle proceeds to equal-side checks: all equal → equilateral; exactly two equal → isosceles; otherwise scalene. All sides use the same unit. Decimal equality is exact for the entered values.
- Largest compares A with B, then compares the larger candidate with C using `>=`. This handles zero, negative numbers, decimals and tied maxima without requiring loops or arrays.

Numeric limits keep decimal arithmetic safe. ATM balance is 0–1,000,000,000,000 and withdrawal is 0.01–1,000,000,000,000. BMI measurements are 0.000001–1,000,000,000. Triangle sides are 0.000001–1,000,000,000,000. Largest inputs are −1,000,000,000,000–1,000,000,000,000. The server reports missing, malformed and out-of-range values even when browser validation is bypassed.

## Regression checks

Start the app, then run from the solution root:

```powershell
node --test tests/navigation.test.cjs
node --test tests/nested-if-else.test.cjs
```

For an alternate local port:

```powershell
$env:SHOWCASE_BASE_URL = 'http://localhost:5198'
node --test tests/nested-if-else.test.cjs
```

The HTTP suite covers all 20 canonical slots, partial rendering, every outcome branch, BMI boundaries before rounding, triangle permutations, tied/negative/decimal maxima, bad inputs, forged output fields and missing antiforgery tokens. It requires Node.js with built-in `fetch` and `Headers.getSetCookie`; no npm installation is needed.
