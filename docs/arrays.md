# Array example selections

All five selected examples are implemented. Slot ordering follows the one-dimensional/two-dimensional grouping.

| Example | Array type |
|---|---|
| Duplicate Finder | One-dimensional |
| Array Sort | One-dimensional |
| Tic-Tac-Toe | Two-dimensional |
| Multiplication Table | Two-dimensional |
| House Member Finder | Two-dimensional |

This completes the rubric's two one-dimensional and three two-dimensional examples. Every example uses the existing shared Input/Result layout and native, antiforgery-protected MVC forms.

## Behavior and limits

- Duplicate Finder: 1–30 comma/newline-separated numbers or names, up to 40 characters per entry and 1,500 total. Names are trimmed and compared without letter case. Numeric equivalents such as 1 and 1.0 match. Output includes counts and zero-based array indexes; unique input reports No duplicates.
- Array Sort: the same input modes and limits, with ascending/descending order. C# sorts decimal values numerically or names with a case-insensitive comparer. Repeated values remain present. Numeric inputs are bounded to ±1,000,000,000 and use a decimal point without thousands separators.
- Tic-Tac-Toe: two local players, X first, on a real `char[3,3]`. Each square submits one move. A protected token carries move history; the server reconstructs turns and checks every row, column and diagonal. Occupied squares, altered/cross-example state and moves after completion are rejected. Winning squares are identified, draws are detected, and New game returns an empty board. There is no database, player account or saved score.
- Multiplication Table: choose 1–12 rows and columns (default 5×5). Nested loops fill an actual int[,] with (row + 1) × (column + 1). The result uses row/column headers, and large tables scroll locally without splitting numbers.
- House Member Finder: enter comma-separated members for Gryffindor, Slytherin, Ravenclaw and Hufflepuff, then one complete name to find. Each house accepts up to 20 names of 1–60 characters and 1,220 total characters. Houses may be empty, but at least one roster must contain a member. Line breaks are rejected as separators; empty comma entries are ignored. An actual rectangular string?[,] uses four house rows and enough columns for the longest roster; shorter rows contain null padding. Search trims outer spaces, ignores letter case, returns each matching house once, and reports Member not found otherwise. A matching table shows the stored spelling and zero-based array position. Names in multiple houses return all matching houses. Input and output are Razor-encoded.

Canonical Clear/New game links work without JavaScript. Editing input hides stale output. Linked validation errors identify individual indexed fields. Invalid dimensions or roster entries show linked errors without computed results. House rosters start with editable sample members; Member to find starts empty without a placeholder. Clear restores those sample rosters and clears the search, or restores the default table dimensions.

## Validation commands

```powershell
dotnet build 241611JalopFinalAssignment1.sln
node --test tests/navigation.test.cjs tests/input-output.test.cjs tests/nested-if-else.test.cjs tests/loops.test.cjs tests/arrays.test.cjs
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http --no-build
```

The integration tests need the local app running on port 5196, or `SHOWCASE_BASE_URL`. Array tests cover routes, partials, antiforgery, numeric/name ordering, duplicates, encoded names, all eight Tic-Tac-Toe win lines, draws, turn order, occupied cells, completed games, altered state, multiplication products and dimensions, all four houses, unequal rosters, shared names, no matches, comma-only parsing and malformed inputs.

## Verification

Validated on November 2, 2026: the solution builds with zero warnings/errors and all 39 regression tests pass. All five array pages use side-by-side panels at 1280×720, stack at 390×844 and fit 320×568 without horizontal workspace overflow. Mobile fields and board squares have a 44px minimum target. The result grid also fits a narrow phone; short landscape scrolls while clearing the taskbar. Keyboard Enter submits a board move, New game resets nine empty squares, Back/Forward restore the correct example, minimize/restore preserves the selected array example, and maximized forms remain maximized after submission. Existing reduced-motion rules remain intact; no array-specific animation was added. The app was rebuilt and rerun after each implementation batch and left running for review.
