# Architecture and extension guide

## Structure
```
241611JalopFinalAssignment1/          Solution root
├── .agents/skills/showcase-ui/     Project skill entrypoint
├── .impeccable/                   Design sidecar and visual-review evidence
├── docs/                         Architecture, plan, assets and validation
├── tests/                        Dependency-free JavaScript navigation checks
├── AGENTS.md                     Instructions for coding agents
├── PRODUCT.md                    Confirmed requirements and scope
├── DESIGN.md                     Shared visual system
├── README.md                     Setup and run instructions
├── 241611JalopFinalAssignment1.sln
└── 241611JalopFinalAssignment1/    ASP.NET Core MVC application
    ├── Controllers/              JALOPARLANMARTIN_241611_HomeController, JALOPARLANMARTIN_241611_ConceptsController
    ├── Models/                   JALOPARLANMARTIN_241611_ShowcaseViewModel, JALOPARLANMARTIN_241611_ConceptCatalog
    ├── Views/
    │   ├── Home/JALOPARLANMARTIN_241611_Index.cshtml      Desktop composition
    │   └── Shared/               Layout, application shell, concept icons and SVG symbols
    ├── Properties/               Launch profiles
    ├── wwwroot/
    │   ├── css/site.css          Shared tokens and responsive styles
    │   ├── js/site.js            Navigation, search and window controls
    │   ├── fonts/               Self-hosted Geist and license
    │   └── images/              Wallpaper, provenance and favicon (legacy folder retained)
    └── Program.cs
```

The existing wwwroot/lib starter libraries are retained but are not loaded by the new layout. bin/, obj/ and .vs/ are local build/IDE artifacts, not source.

## Routes
- `/`: desktop.
- `/concepts/{category}`: category overview.
- `/concepts/{category}/example-{number}`: one of five example slots.
- `/about`: showcase information.

Category slugs: input-output, nested-if-else, loops, arrays. Unknown categories and example numbers outside 1–5 return HTTP 404.

## Learning notes and code comments
New or changed code should include brief beginner-friendly notes beside meaningful lines explaining what the line does or what unfamiliar syntax means. Use the appropriate comment syntax for C#, Razor, JavaScript or CSS. If an inline note makes a line difficult to read, put it immediately above that line. Keep notes accurate when changing code; reserve longer explanations for this document. Braces and other purely structural lines do not need individual comments.

`Controllers/JALOPARLANMARTIN_241611_ConceptsController.cs` includes these routing notes beside the relevant code:
- `[HttpGet]` means the action handles GET requests.
- `{category}` captures a route value, such as `nested-if-else`, from the URL.
- `{example:int}` requires the captured example value to be an integer. The action separately checks that it is between 1 and 5.
- `int?` means the C# parameter can contain an integer or `null`. The category-only route supplies no example number; the example route still requires its numbered segment.
- `IActionResult` lets the action return different kinds of responses, including a full view, a partial view or HTTP 404.

## Shared application surfaces

`Models/JALOPARLANMARTIN_241611_ExampleCatalog.cs` supplies the approved title for each category/slot. Example model titles, Input/Output headings and the shared sidebar use this catalog so labels stay consistent. Input/Output slot 2 calculates rectangle area and perimeter from validated dimensions in C#.
`Views/Shared/JALOPARLANMARTIN_241611_ShowcaseWindow.cshtml` owns the brand titlebar, full-width category header, sidebar example links and the scrollable overview/example workspace. `Views/Shared/JALOPARLANMARTIN_241611_ConceptIcon.cshtml` loads the approved transparent PNG from `wwwroot/images/concepts/<slug>.png` with cache versioning; desktop, Start, search, taskbar and category headings share it. There is no address path, file list or filename suffix.

## Rendering and navigation

`window-state.js` runs in the document head before the stylesheet. It reads the current tab's maximized preference from `sessionStorage` and sets the root appearance class before paint. `site.js` initializes from that class and saves changes when the maximize/restore caption control is used. This keeps native form POSTs from shrinking a maximized application. Storage failures are caught so ordinary controls and canonical navigation still work. No input values or results are persisted.
All canonical URLs render complete Razor pages. Requests with `X-Showcase-Partial: true` return only the JALOPARLANMARTIN_241611_ShowcaseWindow partial. JavaScript enhances same-origin links and pushes browser history. The initial view and desktop are cached immediately; each other route is fetched once on its first visit and kept in memory until the page reloads. Back/Forward and repeated tab selections reuse the cached view. Clicking the already active route does nothing.

Switching categories, examples or About keeps the outer application element mounted and updates its contents, so its opening animation plays only when opening a window from the desktop. The current view stays visible during first-time requests. A delayed “Opening…” notice and accessible busy state appear only for uncached navigation; aborted or superseded requests cannot replace a newer selection. Failed enhanced navigation falls back to full-page navigation. Ctrl/Cmd+K opens search; Escape closes search or the open window. Minimized windows are restored from their active taskbar shortcut, keeping the exact selected example. The taskbar remains visible when the application is maximized. Start uses native details/summary and canonical links; JavaScript adds outside/Escape dismissal, returning focus to Start on Escape. About has its own restoration check so it cannot restore a different minimized category.

The route cache is suitable for the current read-only template. When adding real CRUD or other changing server data, explicitly invalidate affected cached routes after successful mutations, or opt those views out of caching; do not serve stale records or cache POST responses.

`fitStartPanel` resets and measures Start's natural width and content height when it opens, the viewport resizes, or fonts finish loading. It chooses the smaller available-width/available-height ratio and writes CSS width and height variables for the outer panel. This preserves the panel's proportions while its text, icons, controls and padding keep their natural sizes. A column flex layout and automatic top margin hold the footer at the bottom. A minimum sizing factor of 1 preserves small-screen control sizes; CSS constrains the panel height and allows scrolling when content cannot fit.

## Implemented Nested If…Else examples

All working examples share `.example-split`, `.example-layout`, `.example-panel`, `.example-input-panel` and `.example-output-panel`. The model's `HasImplementedExample` selects a compact workspace inset; placeholders retain the original template. A native form contains the two panels, with inputs first in DOM and keyboard order. `JALOPARLANMARTIN_241611_ExampleEmpty` keeps an honest empty state in the Result panel. Shared CSS stacks the panels below 1000px and sets primary results to 24–32px and explanatory text to 12px. Keep teaching notes with inputs, and put confirmed output and its explanation in the Result panel. The ID card remains a preview artifact within that panel.

`examples.js` hides stale results and restores the shared empty state after edits. ID generation and Clear explicitly toggle the card and empty state together. Temperature retains its own delegated Clear/Swap handlers so Swap can reuse the full-precision C# result before invalidating it. No calculation moves into JavaScript.

`JALOPARLANMARTIN_241611_NestedIfElseController` handles antiforgery-protected POST routes 1–5. `JALOPARLANMARTIN_241611_ConceptsController` initializes their typed GET models, and `JALOPARLANMARTIN_241611_ShowcaseWindow` selects the corresponding Razor partial. `JALOPARLANMARTIN_241611_NestedExampleViewModel` shares result text, ordered decision steps and the text flow; each concrete model defines only its input fields and descriptive metadata. Bind allowlists accept inputs only. The algorithms use explicit nested `if`/`else` blocks so the C# matches the learning diagrams.

`JALOPARLANMARTIN_241611_DecisionResult`, `JALOPARLANMARTIN_241611_DecisionErrors` and `JALOPARLANMARTIN_241611_DecisionFlow` share output, linked validation feedback and native details disclosures. `examples.js` hides outdated output after edits and focuses server feedback after native POST. Clear always performs a canonical GET. Login removes submitted passwords from ModelState and the model before returning HTML. ATM calculations are independent simulations with no saved balance. See [nested-if-else.md](nested-if-else.md) for accounts, numeric limits and test commands.

## Implemented loop examples

See [loops.md](loops.md) for the five implemented loop examples, limits, protected game rounds and validation commands. `JALOPARLANMARTIN_241611_LoopsController` calculates every result. Each loop example has its own model file and complete Razor view, containing its inputs, validation, buttons, reset URL, teaching disclosure and results. `JALOPARLANMARTIN_241611_ShowcaseWindow` selects that view by its model type. The former `_LoopExample` template, input fragments and `LoopNumber` ViewData lookup are removed. Form action names and button labels are written directly in each view instead of being stored in model metadata. Shared CSS and result/error hooks preserve the existing appearance and interactions.

`JALOPARLANMARTIN_241611_LoopExampleViewModel` holds common result properties; `JALOPARLANMARTIN_241611_RetryLoopViewModel` shares protected state and attempt counters between the games. `JALOPARLANMARTIN_241611_GuessGameViewModel.cs` also contains its `JALOPARLANMARTIN_241611_GuessRound` record, while `JALOPARLANMARTIN_241611_QuizRetryViewModel.cs` contains `JALOPARLANMARTIN_241611_QuizRound`; `JALOPARLANMARTIN_241611_LoopQuizCatalog.cs` holds the question bank. `Services/ExampleStateTokens` uses ASP.NET Core Data Protection with separate guessing/quiz purposes to protect hidden round data. The two do…while actions recheck the submitted attempt history, stopping at a correct answer or attempt limit; each valid request appends only one attempt. No database or server session is used.

## Dedicated example models

All 20 examples have a model file named after their model type. Model files are grouped into `Models/InputOutput/`, `Models/NestedIfElse/`, `Models/Loops/` and `Models/Arrays/`. Each category's base models and supporting catalogs live in that category folder. `JALOPARLANMARTIN_241611_ShowcaseViewModel.cs`, `JALOPARLANMARTIN_241611_ExampleCatalog.cs` and `JALOPARLANMARTIN_241611_ErrorViewModel.cs` stay at the root of `Models/` because they serve the application as a whole.

The array, loop and login models were moved out of grouped files; their namespaces, validation rules, type names and controller contracts are preserved. All models continue to use the `_241611JalopFinalAssignment1.Models` namespace; the concept folders organize files without requiring new imports. Shared base models remain separate because their properties apply to several examples. Moving a type into another file does not change how MVC binds or validates it. Small supporting enums and result/round records stay beside the example that uses them.

Use [mvc-example-map.md](mvc-example-map.md) to find each example's model, view and POST method. `JALOPARLANMARTIN_241611_ShowcaseViewModel` still carries the selected example model to the desktop view; it does not replace those dedicated models.

## Implemented array examples

`JALOPARLANMARTIN_241611_ArraysController` handles the five array POST routes; `JALOPARLANMARTIN_241611_ConceptsController` initializes their GET models. `JALOPARLANMARTIN_241611_ArrayExampleViewModel` provides shared labels and result data. Each example has one complete, strongly typed Razor view in `Views/Concepts/Arrays/`, containing its form, validation feedback, buttons, empty state and results:

| Example | Razor view | POST action in `JALOPARLANMARTIN_241611_ArraysController` |
|---|---|---|
| 1 — Duplicate Finder | `JALOPARLANMARTIN_241611_DuplicateFinder.cshtml` | `FindDuplicates()` |
| 2 — Array Sort | `JALOPARLANMARTIN_241611_ArraySort.cshtml` | `SortValues()` |
| 3 — Tic-Tac-Toe | `JALOPARLANMARTIN_241611_TicTacToe.cshtml` | `PlayMove()` |
| 4 — Multiplication Table | `JALOPARLANMARTIN_241611_MultiplicationTable.cshtml` | `GenerateTable()` |
| 5 — House Member Finder | `JALOPARLANMARTIN_241611_HouseMemberFinder.cshtml` | `FindHouse()` |

`JALOPARLANMARTIN_241611_ShowcaseWindow` selects the view directly from the concrete `ArrayExample` model type. There is no shared `_ArrayExample` view or filename-based input/result selection. This keeps each example readable in one place while preserving the shared CSS, desktop, window, routes and JavaScript hooks. The models no longer carry form-action names or partial filenames; these are explicit in each view.

For Example 1, follow `JALOPARLANMARTIN_241611_ConceptsController.Index()` → `JALOPARLANMARTIN_241611_DuplicateFinderViewModel` → `JALOPARLANMARTIN_241611_ShowcaseWindow` → `JALOPARLANMARTIN_241611_DuplicateFinder.cshtml`. Submitting its form calls `JALOPARLANMARTIN_241611_ArraysController.FindDuplicates()`, which parses the input through `TryReadValues()`, finds repeated values, and calls `Example()` to return the same page with results. One-dimensional examples use actual string/decimal arrays. The multiplication table stores products in an `int[,]`; House Member Finder copies validated rosters into a rectangular string array.

`ExampleStateTokens` is the shared Data Protection codec for loop games and Tic-Tac-Toe. Its original root protection purpose remains unchanged so existing loop tokens continue to work; the separate `arrays-tic-tac-toe` purpose prevents token reuse across examples. The Tic-Tac-Toe controller reconstructs a `char[,]` from protected moves and derives the next player, winner and draw state server-side. Buttons submit cell positions through normal forms, including without JavaScript. See [arrays.md](arrays.md) for limits and validation.

### Separate array views: verification (2026-11-05)

- `dotnet build 241611JalopFinalAssignment1.sln`: passed with zero warnings/errors.
- `node --test tests/arrays.test.cjs tests/navigation.test.cjs`: all 19 checks passed, including native/partial routes, antiforgery, valid/invalid submissions, duplicate normalization, sorting, game rounds, table products, house lookup and navigation history.
- Browser review: duplicate results at 1366×900, multiplication and house forms at 390×844, all five example links, Back/Forward, Enter to play a Tic-Tac-Toe move, and maximize/minimize/restore/close.
- Existing shared CSS, focus hooks and reduced-motion rules are reused; no new styles or animations were introduced. Reduced motion was not separately emulated in the browser.
- Impeccable detector: no findings in the changed array views or window view.

## Adding your examples

1. Give examples real titles/descriptions in a typed view model, keyed by category and number.
2. Add each example's UI as a Razor partial beneath `Views/Concepts/<Category>/` when that real example exists. Select it within the shared application workspace; retain the shared shell.
3. Add controller actions and strongly typed input models for actual form handling. Use POST, validation and antiforgery for mutations.
4. Add data storage and CRUD services only when implementing real CRUD; the current catalog is static navigation metadata, not a persistence layer.
5. Replace “Not implemented yet” only once that example works. Do not present fake Run/Save results.

Do not create 20 empty controller/view copies now. The shared template keeps initial behavior consistent and gives each future example an explicit slot.


## Temperature converter
`JALOPARLANMARTIN_241611_TemperatureConverterViewModel` carries Value, FromUnit and ToUnit as validated inputs, with Result and Formula as controller-generated output. The JALOPARLANMARTIN_241611_TemperatureUnit enum constrains the three available scales: Celsius, Fahrenheit and Kelvin. `JALOPARLANMARTIN_241611_ConceptsController.ConvertTemperature` binds only the three input properties, validates the selected scale's absolute-zero limit, and normalizes through Celsius before calculating the chosen destination using decimal arithmetic. Same-unit conversions preserve the exact input. A tuple switch selects the explanatory formula, and Kelvin uses K without a degree sign. The canonical GET route initializes the form; POST returns the complete desktop view with validation or results. The shared window chooses the temperature partial before falling back to numbered placeholders. Confirm is a normal antiforgery-protected form submission and works without JavaScript.


Converter interactions use delegated input/change/click handlers so they survive cached navigation. Editing a value/unit clears the result, its symbol, formula and outdated validation feedback. Swap exchanges units and, when available, copies the full-precision confirmed result into the input; C# still performs every calculation on Confirm. Clear resets to an empty Celsius-to-Fahrenheit form and focuses the input. The raw confirmed result is only a browser convenience, never a trusted posted output. A noscript Swap submit action recalculates on the server, swaps fields and clears ModelState so Razor renders the new inputs. Clear falls back to the canonical GET route.


## Implemented Input/Output examples
`JALOPARLANMARTIN_241611_InputOutputController` handles canonical POST routes for time (4), text (5) and ID (1), while `JALOPARLANMARTIN_241611_ConceptsController` initializes their GET view models. Time normalizes through seconds; text uses Unicode text elements and whitespace-separated word groups after newline normalization. All form output is server calculated and Razor encoded.

ID generation posts only named text fields and an antiforgery token. The photo picker has no name or matching model property; JavaScript keeps a single blob URL in memory, validates type/size/decoding and revokes it on Clear, navigation and page exit. No storage API, database or server photo path is used. AJAX returns validated text as JSON; textContent inserts it safely into the preview. Native POST still renders a text-only card. Revision checks prevent old responses from restoring a preview after edits or Clear. The server rejects file submissions and limits the request to 16 KB.
