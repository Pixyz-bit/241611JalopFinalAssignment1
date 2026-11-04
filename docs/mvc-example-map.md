# Example file map

## Required naming convention

Controller, model and view filenames start with `JALOPARLANMARTIN_241611_`. Controller and model type names carry the same prefix. Controller classes retain the final `Controller` suffix, and form `asp-controller` values use the prefixed class name without that suffix. Example: `JALOPARLANMARTIN_241611_ArraysController.cs` handles the form whose `asp-controller` is `JALOPARLANMARTIN_241611_Arrays`.

Razor views use prefixed filenames, including shared partials and the layout. `_ViewImports.cshtml` and `_ViewStart.cshtml` are framework-discovered setup files and retain their reserved names. Public example URLs remain `/concepts/<category>/example-<number>`; explicit desktop/error routes preserve `/`, `/Home`, `/Home/Index` and `/Home/Error`. Controller view paths and partial references are updated to the renamed views.

Naming-refactor verification on November 5, 2026: build passed with zero warnings/errors; all 44 regression tests passed; `/`, `/Home`, `/Home/Index` and `/Home/Error` returned full pages with HTTP 200. Browser checks covered keyboard submission of Duplicate Finder and Back/Forward between array examples. The Razor detector reported no findings. All controller/model filenames match a prefixed declared type; the two reserved setup files are the only unprefixed Razor filenames.

Each example has a dedicated model file under `241611JalopFinalAssignment1/Models/<category>/`. Its view lives under `241611JalopFinalAssignment1/Views/Concepts/<category>/`. Controllers live under `241611JalopFinalAssignment1/Controllers/`.

```text
Models/
├── InputOutput/    Five Input/Output example models
├── NestedIfElse/   Five decision models and their shared base
├── Loops/          Five loop models and shared bases
├── Arrays/         Five array models and their shared bases
├── JALOPARLANMARTIN_241611_ShowcaseViewModel.cs
├── JALOPARLANMARTIN_241611_ExampleCatalog.cs
└── JALOPARLANMARTIN_241611_ErrorViewModel.cs
```

All model files retain the `_241611JalopFinalAssignment1.Models` namespace. C# folders organize the source files; they do not automatically change a type's namespace. Existing controller and Razor imports therefore continue to work.

The model holds input, validation rules and results. The view displays them. The controller action handles the submitted form and calculates the result.

## Input/Output

Models are in `Models/InputOutput/`; views are in `Views/Concepts/InputOutput/`. All five POST methods are in `JALOPARLANMARTIN_241611_InputOutputController.cs`.

| Example | Model file | Razor view | POST method |
|---|---|---|---|
| 1 — ID preview | `JALOPARLANMARTIN_241611_IdCardViewModel.cs` | `JALOPARLANMARTIN_241611_IdCard.cshtml` | `GenerateId()` |
| 2 — Rectangle | `JALOPARLANMARTIN_241611_RectangleCalculatorViewModel.cs` | `JALOPARLANMARTIN_241611_RectangleCalculator.cshtml` | `CalculateRectangle()` |
| 3 — Temperature | `JALOPARLANMARTIN_241611_TemperatureConverterViewModel.cs` | `JALOPARLANMARTIN_241611_TemperatureConverter.cshtml` | `ConvertTemperature()` |
| 4 — Time | `JALOPARLANMARTIN_241611_TimeConverterViewModel.cs` | `JALOPARLANMARTIN_241611_TimeConverter.cshtml` | `ConvertTime()` |
| 5 — Text counting | `JALOPARLANMARTIN_241611_TextCounterViewModel.cs` | `JALOPARLANMARTIN_241611_TextCounter.cshtml` | `CountText()` |

## Nested If…Else

Models are in `Models/NestedIfElse/`; views are in `Views/Concepts/NestedIfElse/`. POST methods are in `JALOPARLANMARTIN_241611_NestedIfElseController.cs`. These views still reuse shared decision-result, error and flow fragments.

| Example | Model file | Razor view | POST method |
|---|---|---|---|
| 1 — Login and Access Checker | `JALOPARLANMARTIN_241611_LoginAccessViewModel.cs` | `JALOPARLANMARTIN_241611_LoginAccess.cshtml` | `CheckAccess()` |
| 2 — Latin Honors | `JALOPARLANMARTIN_241611_LatinHonorsViewModel.cs` | `JALOPARLANMARTIN_241611_LatinHonors.cshtml` | `EvaluateHonors()` |
| 3 — BMI | `JALOPARLANMARTIN_241611_BmiCalculatorViewModel.cs` | `JALOPARLANMARTIN_241611_BmiCalculator.cshtml` | `CalculateBmi()` |
| 4 — Triangle | `JALOPARLANMARTIN_241611_TriangleClassifierViewModel.cs` | `JALOPARLANMARTIN_241611_TriangleClassifier.cshtml` | `ClassifyTriangle()` |
| 5 — Largest of Three | `JALOPARLANMARTIN_241611_LargestNumbersViewModel.cs` | `JALOPARLANMARTIN_241611_LargestNumbers.cshtml` | `FindLargest()` |

## Loops

Models are in `Models/Loops/`; views are in `Views/Concepts/Loops/`. POST methods are in `JALOPARLANMARTIN_241611_LoopsController.cs`. Each view contains the complete form, validation feedback, teaching disclosure and result display for its example.

| Example | Model file | Razor view | POST method |
|---|---|---|---|
| 1 — Pyramid Generator | `JALOPARLANMARTIN_241611_PyramidViewModel.cs` | `JALOPARLANMARTIN_241611_PyramidGenerator.cshtml` | `GeneratePyramid()` |
| 2 — Sum of Digits | `JALOPARLANMARTIN_241611_SumDigitsViewModel.cs` | `JALOPARLANMARTIN_241611_SumOfDigits.cshtml` | `SumDigits()` |
| 3 — Prime Factorization | `JALOPARLANMARTIN_241611_PrimeFactorsViewModel.cs` | `JALOPARLANMARTIN_241611_PrimeFactorization.cshtml` | `Factorize()` |
| 4 — Number Guessing Game | `JALOPARLANMARTIN_241611_GuessGameViewModel.cs` | `JALOPARLANMARTIN_241611_NumberGuessingGame.cshtml` | `GuessNumber()` |
| 5 — Digit Counter | `JALOPARLANMARTIN_241611_DigitCounterViewModel.cs` | `JALOPARLANMARTIN_241611_DigitCounter.cshtml` | `CountDigits()` |

## Arrays

Models are in `Models/Arrays/`; views are in `Views/Concepts/Arrays/`. POST methods are in `JALOPARLANMARTIN_241611_ArraysController.cs`. Each view contains its own form and results.

| Example | Model file | Razor view | POST method |
|---|---|---|---|
| 1 — Duplicate Finder | `JALOPARLANMARTIN_241611_DuplicateFinderViewModel.cs` | `JALOPARLANMARTIN_241611_DuplicateFinder.cshtml` | `FindDuplicates()` |
| 2 — Array Sort | `JALOPARLANMARTIN_241611_ArraySortViewModel.cs` | `JALOPARLANMARTIN_241611_ArraySort.cshtml` | `SortValues()` |
| 3 — Largest and Smallest | `JALOPARLANMARTIN_241611_ArrayExtremesViewModel.cs` | `JALOPARLANMARTIN_241611_ArrayExtremes.cshtml` | `FindExtremes()` |
| 4 — Multiplication Table | `JALOPARLANMARTIN_241611_MultiplicationTableViewModel.cs` | `JALOPARLANMARTIN_241611_MultiplicationTable.cshtml` | `GenerateTable()` |
| 5 — House Member Finder | `JALOPARLANMARTIN_241611_HouseMemberFinderViewModel.cs` | `JALOPARLANMARTIN_241611_HouseMemberFinder.cshtml` | `FindHouse()` |

## Follow one example

Opening the pyramid page:

```text
GET /concepts/loops/example-1
    → [Controller] JALOPARLANMARTIN_241611_ConceptsController.Index()
    → [Model] Create JALOPARLANMARTIN_241611_PyramidViewModel, attach it to JALOPARLANMARTIN_241611_ShowcaseViewModel.Loop
    → [View] JALOPARLANMARTIN_241611_Index.cshtml → JALOPARLANMARTIN_241611_ShowcaseWindow.cshtml → JALOPARLANMARTIN_241611_PyramidGenerator.cshtml
```

Submitting the pyramid form:

```text
POST /concepts/loops/example-1
    → MVC binds and validates [Model] JALOPARLANMARTIN_241611_PyramidViewModel
    → [Controller] JALOPARLANMARTIN_241611_LoopsController.GeneratePyramid()
    → Store results in that model
    → [Controller] JALOPARLANMARTIN_241611_LoopsController.Example() returns the page model
    → [View] JALOPARLANMARTIN_241611_Index.cshtml → JALOPARLANMARTIN_241611_ShowcaseWindow.cshtml → JALOPARLANMARTIN_241611_PyramidGenerator.cshtml
```

Enhanced GET navigation can return only `JALOPARLANMARTIN_241611_ShowcaseWindow`; direct visits return the full page. The example and shared views are rendered as partial views embedded within that page; partial views do not require an underscore in their filename.

Shared base models such as `JALOPARLANMARTIN_241611_LoopExampleViewModel`, `JALOPARLANMARTIN_241611_RetryLoopViewModel`, `JALOPARLANMARTIN_241611_ArrayExampleViewModel`, `JALOPARLANMARTIN_241611_FlatArrayViewModel` and `JALOPARLANMARTIN_241611_NestedExampleViewModel` hold common properties. For example, `JALOPARLANMARTIN_241611_DuplicateFinderViewModel` inherits the `Values` input from `JALOPARLANMARTIN_241611_FlatArrayViewModel`. Its dedicated file holds the properties specific to finding duplicates.

## Verification records

See [validation.md](validation.md) for dated checks and limitations. The tables above and PRODUCT.md define the current example catalog.
