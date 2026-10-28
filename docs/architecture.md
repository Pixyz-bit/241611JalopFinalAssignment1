# Architecture and extension guide

## Structure
```
241611JalopFinalAssignment1/          Solution root
├── .agents/skills/showcase-ui/     Project skill entrypoint
├── .impeccable/                   Design sidecar and visual-review evidence
├── docs/                         Architecture, plan, assets and validation
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
    │   └── Shared/               Layout, Finder window and SVG symbols
    ├── Properties/               Launch profiles
    ├── wwwroot/
    │   ├── css/site.css          Shared tokens and responsive styles
    │   ├── js/site.js            Navigation, search and window controls
    │   ├── fonts/               Self-hosted Geist and license
    │   └── images/              Wallpaper, provenance, folder and favicon
    └── Program.cs
```

The existing wwwroot/lib starter libraries are retained but are not loaded by the new layout. bin/, obj/ and .vs/ are local build/IDE artifacts, not source.

## Routes
- `/`: desktop.
- `/concepts/{category}`: category overview.
- `/concepts/{category}/example-{number}`: one of five example slots.
- `/about`: showcase information.

Category slugs: input-output, conditional, repetition, basic-crud. Unknown categories and example numbers outside 1–5 return HTTP 404.

## Rendering and navigation
All canonical URLs render complete Razor pages. Requests with `X-Showcase-Partial: true` return only the Finder partial. JavaScript enhances same-origin links and pushes browser history; Back/Forward reload the matching partial. Failed enhanced navigation falls back to full-page navigation. Ctrl/Cmd+K opens search; Escape closes search or the open window. Minimized windows are restored from their active dock shortcut.

## Adding your examples
1. Give examples real titles/descriptions in a typed view model, keyed by category and number.
2. Add each example's UI as a Razor partial beneath `Views/Concepts/<Category>/` when that real example exists. Select it within the shared Finder detail region; retain the shared shell.
3. Add controller actions and strongly typed input models for actual form handling. Use POST, validation and antiforgery for mutations.
4. Add data storage and CRUD services only when implementing real CRUD; the current catalog is static navigation metadata, not a persistence layer.
5. Replace “Not implemented yet” only once that example works. Do not present fake Run/Save results.

Do not create 20 empty controller/view copies now. The shared template keeps initial behavior consistent and gives each future example an explicit slot.
