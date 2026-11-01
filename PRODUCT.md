# C# Programming Showcase

## Purpose and audience
A student programming showcase for visitors and assignment reviewers. The owner supplies and approves the example titles, explanations, forms and algorithms; remaining slots await their next examples.

## Platform and stack
ASP.NET Core MVC targeting .NET 8. Razor, plain CSS and JavaScript; no React, Node build pipeline, database or authentication for this template.

## Confirmed requirements
- Title: C# Programming Showcase.
- Input/Output, Nested If…Else, Loops, Arrays; five example slots each. Input/Output implements ID preview (1), temperature conversion (3), time conversion (4) and text counting (5); slot 2 stays available and links to BMI's new location. Nested If…Else implements Login and Access Checker (1), ATM Withdrawal Simulator (2), BMI Calculator (3), Triangle Classifier (4) and Largest of Three Numbers (5). Loops and Arrays remain placeholders.
- On `windows-design`: Windows 11 inspired desktop with the user-supplied blue wallpaper, shared concept app icons, left shortcut rail and welcome, persistent full-width taskbar, native Start navigation and two-line clock.
- One light Windows application with brand titlebar, right caption controls, full-width category header beneath the titlebar, sidebar with five canonical example links and an example workspace.
- Minimize preserves the selected example; its active taskbar shortcut restores it. Maximize keeps the taskbar visible.
- Functional canonical URLs without JavaScript; enhanced cached navigation, history and search.
- Mobile, keyboard, visible focus and reduced-motion support.
- `main` retains the earlier macOS design for comparison; this redesign does not merge into it automatically.

## Scope
UI, shared templates, navigation and the owner-approved examples. Implement the supplied decision flows in C# with real validation and results. Login and ATM are explicitly educational simulations with no authentication, bank connection or saved state. Keep other slots honestly unimplemented; never invent persistence or CRUD.

## Repository layout
Solution root: AGENTS.md, PRODUCT.md, DESIGN.md and README.md. Detailed notes: docs/. Project skill: .agents/skills/showcase-ui/. App: 241611JalopFinalAssignment1/ with standard Controllers, Models, Views and wwwroot folders. Installed design skills stay in their global folders.
