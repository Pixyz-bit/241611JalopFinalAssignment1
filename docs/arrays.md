# Array example selections

All five selected examples are implemented. Slot ordering follows the one-dimensional/two-dimensional grouping.

| Example | Array type |
|---|---|
| Duplicate Finder | One-dimensional |
| Array Sort | One-dimensional |
| Largest and Smallest | Two-dimensional |
| Multiplication Table | Two-dimensional |
| House Member Finder | Two-dimensional |

This completes the rubric's two one-dimensional and three two-dimensional examples. Every example uses the existing shared Input/Result layout and native, antiforgery-protected MVC forms.

## Behavior and limits

- Duplicate Finder: 1–30 comma/newline-separated numbers, up to 40 characters per entry and 1,500 total. Numeric equivalents such as 1 and 1.0 match. Output includes counts and zero-based array indexes; unique input reports No duplicates.
- Array Sort: the same numeric input and limits, with ascending/descending order. C# sorts decimal values numerically; names are rejected. Repeated values remain present. Numeric inputs are bounded to ±1,000,000,000 and use a decimal point without thousands separators.
- Largest and Smallest: enter comma-separated numbers into three separate fields labeled Row 1, Row 2 and Row 3. All fields are required and must contain the same number of values (1–12), with at most 500 characters per row and 40 per number. Numbers use a decimal point and a value between −1,000,000,000 and 1,000,000,000. Empty cells, line breaks, unequal row lengths and invalid numbers are rejected with errors linked to the affected row. Nested loops scan a real decimal[3, columns] initialized from its first cell, supporting all-negative values and decimals. Results show largest/smallest values, their first zero-based indexes and the entered array. Strict comparisons retain the first matching index for ties. Clear empties all three fields. Only Row1, Row2 and Row3 are bound.
- Multiplication Table: choose 1–12 rows and columns (default 5×5). Nested loops fill an actual int[,] with (row + 1) × (column + 1). The result uses row/column headers, and large tables scroll locally without splitting numbers.
- House Member Finder: enter comma-separated members for Gryffindor, Slytherin, Ravenclaw and Hufflepuff, then one complete name to find. Each house accepts up to 20 names of 1–60 characters and 1,220 total characters. Houses may be empty, but at least one roster must contain a member. Line breaks are rejected as separators; empty comma entries are ignored. An actual rectangular string?[,] uses four house rows and enough columns for the longest roster; shorter rows contain null padding. Search trims outer spaces, ignores letter case, returns each matching house once, and reports Member not found otherwise. A matching table shows the stored spelling and zero-based array position. Names in multiple houses return all matching houses. Input and output are Razor-encoded.

Canonical Clear links work without JavaScript. Editing input hides stale output. Linked validation errors identify individual indexed fields. Invalid dimensions or roster entries show linked errors without computed results. House rosters start with editable sample members; Member to find starts empty without a placeholder. Clear restores those sample rosters and clears the search, or restores the default table dimensions.

## Validation commands

```powershell
dotnet build 241611JalopFinalAssignment1.sln
node --test tests/navigation.test.cjs tests/input-output.test.cjs tests/nested-if-else.test.cjs tests/loops.test.cjs tests/arrays.test.cjs
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http --no-build
```

The integration tests need the local app running on port 5196, or `SHOWCASE_BASE_URL`. Array tests cover routes, partials, antiforgery, numeric ordering, duplicates, rejection of names and forged input modes, three-row/3×12 extrema, all-negative values, decimals, ties, first indexes, forged output rejection and invalid matrix input, multiplication products and dimensions, all four houses, unequal rosters, shared names, no matches, comma-only parsing and malformed inputs.

## Verification

Numeric-only and three-row revision verified on November 5, 2026: Duplicate Finder and Array Sort no longer expose or bind `ValueKind`; all list entries are parsed numerically, including requests containing a forged legacy Names option. Largest and Smallest binds only `Row1`, `Row2` and `Row3`, with linked per-row validation and a real three-row rectangular array. The build succeeds with zero warnings/errors and all 44 regression tests pass. Tests cover required rows, unequal lengths, per-row limits, three-by-twelve arrays, numeric equivalents, decimal sorting, forged modes/results and antiforgery.

Browser review confirms name rejection, numeric sorting, a correct 3×3 extrema result, error recovery for Row 2, and Clear emptying all three row fields. Desktop was reviewed at 1366×900; the 390px layout has no document-width overflow. Shared CSS, keyboard/error focus hooks and reduced-motion rules remain in use. No new animation or styling was introduced, and the Razor detector reported no findings. The rebuilt app is running locally.

Replacement verified on November 5, 2026: Arrays Example 3 is Largest and Smallest in a 2D Array. The build has zero warnings/errors and all 44 regression checks pass. The 10 array checks also pass after the final metric-spacing adjustment. New cases cover mixed signs, all-negative arrays, decimals, equal values, first indexes, one cell, one row/column, CRLF input, 12×12 input, numeric bounds, ragged rows, empty cells, malformed numbers, oversized text and forged results. Native requests and partial routes remain covered by the integration tests.

Browser review covered keyboard submission, a correct 2×3 result, linked validation for uneven rows, Clear and Back navigation. Desktop was reviewed at 1366×900; the 390px layout has no document-width overflow. Existing window controls, result/error hooks and reduced-motion rules remain in use; no animation was added. The app is rebuilt and running at `http://localhost:5196/concepts/arrays/example-3`.

Validated on November 2, 2026: the solution builds with zero warnings/errors and all 39 regression tests pass. All five array pages use side-by-side panels at 1280×720, stack at 390×844 and fit 320×568 without horizontal workspace overflow. Mobile fields and board squares have a 44px minimum target. The result grid also fits a narrow phone; short landscape scrolls while clearing the taskbar. Keyboard Enter submits a board move, New game resets nine empty squares, Back/Forward restore the correct example, minimize/restore preserves the selected array example, and maximized forms remain maximized after submission. Existing reduced-motion rules remain intact; no array-specific animation was added. The app was rebuilt and rerun after each implementation batch and left running for review.
