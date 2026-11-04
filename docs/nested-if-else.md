# Nested If…Else examples

All five forms run their calculations in C# and work with JavaScript disabled. Each has a canonical Clear GET link, input validation, an expandable text decision tree and an ordered trace of the checks actually executed. Invalid input shows linked errors without a calculated result.

| Slot | Example | Try it |
| --- | --- | --- |
| 1 | Login and Access Checker | `student` / `student123` → student permissions |
| 2 | Latin Honors Evaluator | GWA 1.50 → Magna Cum Laude |
| 3 | BMI Calculator | Weight 60 kg, height 1.65 m → BMI 22.04, healthy weight |
| 4 | Triangle Classifier | 3, 4, 5 → scalene |
| 5 | Largest of Three Numbers | 12, 25, 18 → 25 |

Routes follow `/concepts/nested-if-else/example-1` through `example-5`. Input/Output slot 2 remains available and links to BMI in its new category.

## Decision rules

- Login checks the username/password pair before the account's role. Public accounts are `admin` / `admin123`, `student` / `student123`, and `guest` / `guest123`. Matching is case-sensitive; Guest demonstrates a valid account with an unsupported role. Results describe sample permissions only. No real authentication or protected actions are created. Submitted passwords are removed from both the view model and ModelState before rendering; these responses send `Cache-Control: no-store`.
- Latin Honors Evaluator accepts one decimal GWA from 1.00 to 5.00. Nested No branches check GWA ≤ 1.25 for Summa Cum Laude, then ≤ 1.50 for Magna Cum Laude, then ≤ 1.75 for Cum Laude; higher valid values produce No Latin Honors. It uses the unrounded decimal value and displays Based on GWA only. It does not evaluate subject grades or other graduation requirements.
- BMI validates positive measurements, computes `weight / (height * height)`, and nests checks at 18.5, 25 and 30. These are the [CDC categories for adults aged 20 and older](https://www.cdc.gov/bmi/adult-calculator/bmi-categories.html). Category selection uses the unrounded decimal value; display rounds to two decimal places. BMI is a screening measure, not a diagnosis.
- Triangle checks positive sides, then all three strict triangle inequalities. Only a valid triangle proceeds to equal-side checks: all equal → equilateral; exactly two equal → isosceles; otherwise scalene. All sides use the same unit. Decimal equality is exact for the entered values.
- Largest compares A with B, then compares the larger candidate with C using `>=`. This handles zero, negative numbers, decimals and tied maxima without requiring loops or arrays.

Numeric limits keep decimal arithmetic safe. Latin Honors Evaluator accepts decimal GWA from 1.00 to 5.00. BMI measurements are 0.000001–1,000,000,000. Triangle sides are 0.000001–1,000,000,000,000. Largest inputs are −1,000,000,000,000–1,000,000,000,000. The server reports missing, malformed and out-of-range values even when browser validation is bypassed.

## Nested-logic review

All five POST actions in `NestedIfElseController` contain actual nested decisions, rather than relying on their illustrated trees alone:

| Example | Outer decision | Inner decision |
| --- | --- | --- |
| Login and Access Checker | Credentials match | Admin role, then Student role |
| Latin Honors Evaluator | GWA at most 1.25 | In the No branch, GWA at most 1.50, then at most 1.75 |
| BMI Calculator | BMI below 18.5 | In the No branch, BMI below 25, then below 30 |
| Triangle Classifier | Triangle inequalities pass | All sides equal, then any pair equal |
| Largest of Three Numbers | A is at least B | Compare A or B with C |

Latin Honors Evaluator demonstrates nested subject-specific thresholds beyond its initial input validation. All five examples contain nested decisions in their example logic.

## Running regression checks

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
