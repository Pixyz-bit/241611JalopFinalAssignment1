# Example file map

Each example has a dedicated model file under `241611JalopFinalAssignment1/Models/<category>/`. Its view lives under `241611JalopFinalAssignment1/Views/Concepts/<category>/`. Controllers live under `241611JalopFinalAssignment1/Controllers/`.

```text
Models/
├── InputOutput/    Five Input/Output example models
├── NestedIfElse/   Five decision models and their shared base
├── Loops/          Five loop models, shared bases and quiz catalog
├── Arrays/         Five array models and their shared bases
├── ShowcaseViewModel.cs
├── ExampleCatalog.cs
└── ErrorViewModel.cs
```

All model files retain the `_241611JalopFinalAssignment1.Models` namespace. C# folders organize the source files; they do not automatically change a type's namespace. Existing controller and Razor imports therefore continue to work.

The model holds input, validation rules and results. The view displays them. The controller action handles the submitted form and calculates the result.

## Input/Output

Models are in `Models/InputOutput/`; views are in `Views/Concepts/InputOutput/`. POST methods are in `InputOutputController.cs`, except temperature conversion, which is in `ConceptsController.cs`.

| Example | Model file | Razor view | POST method |
|---|---|---|---|
| 1 — ID preview | `IdCardViewModel.cs` | `_IdCard.cshtml` | `GenerateId()` |
| 2 — Rectangle | `RectangleCalculatorViewModel.cs` | `_RectangleCalculator.cshtml` | `CalculateRectangle()` |
| 3 — Temperature | `TemperatureConverterViewModel.cs` | `_TemperatureConverter.cshtml` | `ConvertTemperature()` |
| 4 — Time | `TimeConverterViewModel.cs` | `_TimeConverter.cshtml` | `ConvertTime()` |
| 5 — Text counting | `TextCounterViewModel.cs` | `_TextCounter.cshtml` | `CountText()` |

## Nested If…Else

Models are in `Models/NestedIfElse/`; views are in `Views/Concepts/NestedIfElse/`. POST methods are in `NestedIfElseController.cs`. These views still reuse shared decision-result, error and flow fragments.

| Example | Model file | Razor view | POST method |
|---|---|---|---|
| 1 — Login and Access Checker | `LoginAccessViewModel.cs` | `_LoginAccess.cshtml` | `CheckAccess()` |
| 2 — Latin Honors | `LatinHonorsViewModel.cs` | `_LatinHonors.cshtml` | `EvaluateHonors()` |
| 3 — BMI | `BmiCalculatorViewModel.cs` | `_BmiCalculator.cshtml` | `CalculateBmi()` |
| 4 — Triangle | `TriangleClassifierViewModel.cs` | `_TriangleClassifier.cshtml` | `ClassifyTriangle()` |
| 5 — Largest of Three | `LargestNumbersViewModel.cs` | `_LargestNumbers.cshtml` | `FindLargest()` |

## Loops

Models are in `Models/Loops/`; views are in `Views/Concepts/Loops/`. POST methods are in `LoopsController.cs`. Each view contains the complete form, validation feedback, teaching disclosure and result display for its example.

| Example | Model file | Razor view | POST method |
|---|---|---|---|
| 1 — Pyramid Generator | `PyramidViewModel.cs` | `_PyramidGenerator.cshtml` | `GeneratePyramid()` |
| 2 — Sum of Digits | `SumDigitsViewModel.cs` | `_SumOfDigits.cshtml` | `SumDigits()` |
| 3 — Prime Factorization | `PrimeFactorsViewModel.cs` | `_PrimeFactorization.cshtml` | `Factorize()` |
| 4 — Number Guessing Game | `GuessGameViewModel.cs` | `_NumberGuessingGame.cshtml` | `GuessNumber()` |
| 5 — Quiz Retry Simulator | `QuizRetryViewModel.cs` | `_QuizRetrySimulator.cshtml` | `RetryQuiz()` |

## Arrays

Models are in `Models/Arrays/`; views are in `Views/Concepts/Arrays/`. POST methods are in `ArraysController.cs`. Each view contains its own form and results.

| Example | Model file | Razor view | POST method |
|---|---|---|---|
| 1 — Duplicate Finder | `DuplicateFinderViewModel.cs` | `_DuplicateFinder.cshtml` | `FindDuplicates()` |
| 2 — Array Sort | `ArraySortViewModel.cs` | `_ArraySort.cshtml` | `SortValues()` |
| 3 — Tic-Tac-Toe | `TicTacToeViewModel.cs` | `_TicTacToe.cshtml` | `PlayMove()` |
| 4 — Multiplication Table | `MultiplicationTableViewModel.cs` | `_MultiplicationTable.cshtml` | `GenerateTable()` |
| 5 — House Member Finder | `HouseMemberFinderViewModel.cs` | `_HouseMemberFinder.cshtml` | `FindHouse()` |

## Follow one example

Opening the pyramid page:

```text
GET /concepts/loops/example-1
    → [Controller] ConceptsController.Index()
    → [Model] Create PyramidViewModel, attach it to ShowcaseViewModel.Loop
    → [View] Index.cshtml → _ShowcaseWindow.cshtml → _PyramidGenerator.cshtml
```

Submitting the pyramid form:

```text
POST /concepts/loops/example-1
    → MVC binds and validates [Model] PyramidViewModel
    → [Controller] LoopsController.GeneratePyramid()
    → Store results in that model
    → [Controller] LoopsController.Example() returns the page model
    → [View] Index.cshtml → _ShowcaseWindow.cshtml → _PyramidGenerator.cshtml
```

Enhanced GET navigation can return only `_ShowcaseWindow`; direct visits return the full page. Razor files with an underscore are partial views embedded within that page.

Shared base models such as `LoopExampleViewModel`, `RetryLoopViewModel`, `ArrayExampleViewModel`, `FlatArrayViewModel` and `NestedExampleViewModel` hold common properties. For example, `DuplicateFinderViewModel` inherits the `Values` input from `FlatArrayViewModel`. Its dedicated file holds the properties specific to finding duplicates.

## Verification — November 5, 2026

- The solution builds with zero warnings and errors.
- All 44 existing regression tests pass across the six test files in `tests/`.
- Browser checks cover pyramid submission using Enter, all five loop links, Back/Forward, guessing/reset, quiz completion, and minimize/restore. Pyramid results were reviewed at 1366×900; factorization and quiz forms were reviewed at 390×844.
- Shared CSS, focus hooks, canonical links and reduced-motion rules are retained. Reduced motion was checked in the stylesheet, not separately emulated in the browser.
- The Impeccable detector reports no findings for the loop views and shared window selection.
