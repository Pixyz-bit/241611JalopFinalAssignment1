# Nested If…Else examples

All five forms run their calculations in C# and work with JavaScript disabled. Each has a canonical Clear GET link, input validation, an expandable text decision tree and an ordered trace of the checks actually executed. Invalid input shows linked errors without a calculated result.

| Slot | Example | Try it |
| --- | --- | --- |
| 1 | Login and Access Checker | `student` / `student123` → student permissions |
| 2 | Grade Evaluator | Final percentage 98 → Passed |
| 3 | BMI Calculator | Weight 60 kg, height 1.65 m → BMI 22.04, healthy weight |
| 4 | Triangle Classifier | 3, 4, 5 → scalene |
| 5 | Largest of Three Numbers | 12, 25, 18 → 25 |

Routes follow `/concepts/nested-if-else/example-1` through `example-5`. Input/Output slot 2 remains available and links to BMI in its new category.

## Decision rules

- Login checks the username/password pair before the account's role. Public accounts are `admin` / `admin123`, `student` / `student123`, and `guest` / `guest123`. Matching is case-sensitive; Guest demonstrates a valid account with an unsupported role. Results describe sample permissions only. No real authentication or protected actions are created. Submitted passwords are removed from both the view model and ModelState before rendering; these responses send `Cache-Control: no-store`.
- Grade Evaluator accepts one whole-number final percentage from 0 to 100. An outer valid-input check contains an inner `if/else`: 75 or higher is Passed; below 75 is Failed. Invalid input shows errors without a pass/fail result. There is no decimal-grade conversion.
- BMI validates positive measurements, computes `weight / (height * height)`, and nests checks at 18.5, 25 and 30. These are the [CDC categories for adults aged 20 and older](https://www.cdc.gov/bmi/adult-calculator/bmi-categories.html). Category selection uses the unrounded decimal value; display rounds to two decimal places. BMI is a screening measure, not a diagnosis.
- Triangle checks positive sides, then all three strict triangle inequalities. Only a valid triangle proceeds to equal-side checks: all equal → equilateral; exactly two equal → isosceles; otherwise scalene. All sides use the same unit. Decimal equality is exact for the entered values.
- Largest compares A with B, then compares the larger candidate with C using `>=`. This handles zero, negative numbers, decimals and tied maxima without requiring loops or arrays.

Numeric limits keep decimal arithmetic safe. Grade Evaluator accepts integer percentages from 0 to 100. BMI measurements are 0.000001–1,000,000,000. Triangle sides are 0.000001–1,000,000,000,000. Largest inputs are −1,000,000,000,000–1,000,000,000,000. The server reports missing, malformed and out-of-range values even when browser validation is bypassed.

## Nested-logic review

All five POST actions in `NestedIfElseController` contain actual nested decisions, rather than relying on their illustrated trees alone:

| Example | Outer decision | Inner decision |
| --- | --- | --- |
| Login and Access Checker | Credentials match | Admin role, then Student role |
| Grade Evaluator | Valid percentage | Percentage is at least 75 |
| BMI Calculator | BMI below 18.5 | In the No branch, BMI below 25, then below 30 |
| Triangle Classifier | Triangle inequalities pass | All sides equal, then any pair equal |
| Largest of Three Numbers | A is at least B | Compare A or B with C |

Grade Evaluator deliberately uses validation as its outer condition to keep the single-input Passed/Failed example simple. The other examples include nested subject-specific decisions beyond validation.

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
