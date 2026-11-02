# Array example selections

All five selected examples are implemented. Slot ordering follows the one-dimensional/two-dimensional grouping.

| Example | Array type |
|---|---|
| Duplicate Finder | One-dimensional |
| Array Sort | One-dimensional |
| Tic-Tac-Toe | Two-dimensional |
| Number Search in a Grid | Two-dimensional |
| Group Member Organizer | Two-dimensional |

This completes the rubric's two one-dimensional and three two-dimensional examples. Every example uses the existing shared Input/Result layout and native, antiforgery-protected MVC forms.

## Behavior and limits

- Duplicate Finder: 1–30 comma/newline-separated numbers or names, up to 40 characters per entry and 1,500 total. Names are trimmed and compared without letter case. Numeric equivalents such as 1 and 1.0 match. Output includes counts and zero-based array indexes; unique input reports No duplicates.
- Array Sort: the same input modes and limits, with ascending/descending order. C# sorts decimal values numerically or names with a case-insensitive comparer. Repeated values remain present. Numeric inputs are bounded to ±1,000,000,000 and use a decimal point without thousands separators.
- Tic-Tac-Toe: two local players, X first, on a real `char[3,3]`. Each square submits one move. A protected token carries move history; the server reconstructs turns and checks every row, column and diagonal. Occupied squares, altered/cross-example state and moves after completion are rejected. Winning squares are identified, draws are detected, and New game returns an empty board. There is no database, player account or saved score.
- Number Search in a Grid: a 3×3 `decimal[,]` with editable sample values. All nine cells and a target are required, with values bounded to ±1,000,000,000. Results show every match, one-based row/column positions and zero-based C# indexes. Match labels supplement color; missing targets report Not found.
- Group Member Organizer: three groups of three editable sample names, up to 40 characters per name. A `string[3,3]` preserves group and member positions. All nine names are required, trimmed and Razor-encoded; the result table uses rows for groups and columns for members. Repeated names are allowed because this demonstrates array placement rather than unique membership enforcement.

Canonical Clear/New game links work without JavaScript. Editing input hides stale output. Linked validation errors identify individual indexed fields. Matrix collections are normalized to nine positions before rendering invalid submissions, preventing missing or excessive fields from causing index errors.

## Validation commands

```powershell
dotnet build 241611JalopFinalAssignment1.sln
node --test tests/navigation.test.cjs tests/input-output.test.cjs tests/nested-if-else.test.cjs tests/loops.test.cjs tests/arrays.test.cjs
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http --no-build
```

The integration tests need the local app running on port 5196, or `SHOWCASE_BASE_URL`. Array tests cover routes, partials, antiforgery, numeric/name ordering, duplicates, encoded names, all eight Tic-Tac-Toe win lines, draws, turn order, occupied cells, completed games, altered state, grid matches and malformed collections.

## Verification

Validated on November 2, 2026: the solution builds with zero warnings/errors and all 39 regression tests pass. All five array pages use side-by-side panels at 1280×720, stack at 390×844 and fit 320×568 without horizontal workspace overflow. Mobile fields and board squares have a 44px minimum target. The result grid also fits a narrow phone; short landscape scrolls while clearing the taskbar. Keyboard Enter submits a board move, New game resets nine empty squares, Back/Forward restore the correct example, minimize/restore preserves the selected array example, and maximized forms remain maximized after submission. Existing reduced-motion rules remain intact; no array-specific animation was added. The app was rebuilt and rerun after each implementation batch and left running for review.
