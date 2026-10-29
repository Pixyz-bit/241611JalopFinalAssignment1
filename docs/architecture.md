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
    ├── Controllers/              HomeController, ConceptsController
    ├── Models/                   ShowcaseViewModel, ConceptCatalog
    ├── Views/
    │   ├── Home/Index.cshtml      Desktop composition
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

`Controllers/ConceptsController.cs` includes these routing notes beside the relevant code:
- `[HttpGet]` means the action handles GET requests.
- `{category}` captures a route value, such as `nested-if-else`, from the URL.
- `{example:int}` requires the captured example value to be an integer. The action separately checks that it is between 1 and 5.
- `int?` means the C# parameter can contain an integer or `null`. The category-only route supplies no example number; the example route still requires its numbered segment.
- `IActionResult` lets the action return different kinds of responses, including a full view, a partial view or HTTP 404.

## Shared application surfaces
`Views/Shared/_ShowcaseWindow.cshtml` owns the brand titlebar, full-width category header, sidebar example links and the scrollable overview/example workspace. `Views/Shared/_ConceptIcon.cshtml` loads the approved transparent PNG from `wwwroot/images/concepts/<slug>.png` with cache versioning; desktop, Start, search, taskbar and category headings share it. There is no address path, file list or filename suffix.

## Rendering and navigation
All canonical URLs render complete Razor pages. Requests with `X-Showcase-Partial: true` return only the _ShowcaseWindow partial. JavaScript enhances same-origin links and pushes browser history. The initial view and desktop are cached immediately; each other route is fetched once on its first visit and kept in memory until the page reloads. Back/Forward and repeated tab selections reuse the cached view. Clicking the already active route does nothing.

Switching categories, examples or About keeps the outer application element mounted and updates its contents, so its opening animation plays only when opening a window from the desktop. The current view stays visible during first-time requests. A delayed “Opening…” notice and accessible busy state appear only for uncached navigation; aborted or superseded requests cannot replace a newer selection. Failed enhanced navigation falls back to full-page navigation. Ctrl/Cmd+K opens search; Escape closes search or the open window. Minimized windows are restored from their active taskbar shortcut, keeping the exact selected example. The taskbar remains visible when the application is maximized. Start uses native details/summary and canonical links; JavaScript adds outside/Escape dismissal, returning focus to Start on Escape. About has its own restoration check so it cannot restore a different minimized category.

The route cache is suitable for the current read-only template. When adding real CRUD or other changing server data, explicitly invalidate affected cached routes after successful mutations, or opt those views out of caching; do not serve stale records or cache POST responses.

## Adding your examples
1. Give examples real titles/descriptions in a typed view model, keyed by category and number.
2. Add each example's UI as a Razor partial beneath `Views/Concepts/<Category>/` when that real example exists. Select it within the shared application workspace; retain the shared shell.
3. Add controller actions and strongly typed input models for actual form handling. Use POST, validation and antiforgery for mutations.
4. Add data storage and CRUD services only when implementing real CRUD; the current catalog is static navigation metadata, not a persistence layer.
5. Replace “Not implemented yet” only once that example works. Do not present fake Run/Save results.

Do not create 20 empty controller/view copies now. The shared template keeps initial behavior consistent and gives each future example an explicit slot.
