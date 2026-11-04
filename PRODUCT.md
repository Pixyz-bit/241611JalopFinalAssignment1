# C# Programming Showcase

## Purpose and audience
A student programming showcase for visitors and assignment reviewers. The owner supplies and approves the example titles, explanations, forms and algorithms; remaining slots await their next examples.

## Platform and stack
ASP.NET Core MVC targeting .NET 8. Razor, plain CSS and JavaScript; no React, Node build pipeline, database or authentication for this template.

## Confirmed requirements
- Title: C# Programming Showcase.
- Input/Output, Nested If…Else, Loops, Arrays; five example slots each. Input/Output implements ID preview (1), rectangle area and perimeter (2), temperature conversion (3), time conversion (4) and text counting (5). Nested If…Else implements Login and Access Checker (1), Latin Honors Evaluator (2), BMI Calculator (3), Triangle Classifier (4) and Largest of Three Numbers (5). Loops and Arrays implement the five approved examples listed below for each category.
- On `windows-design`: Windows 11 inspired desktop with the user-supplied blue wallpaper, shared concept app icons, left shortcut rail and welcome, persistent full-width taskbar, native Start navigation and two-line clock.
- One light Windows application with brand titlebar, right caption controls, category icon and title above the left sidebar with five canonical example links, and an example workspace alongside it.
- Minimize preserves the selected example; its active taskbar shortcut restores it. Maximize keeps the taskbar visible.
- Functional canonical URLs without JavaScript; enhanced cached navigation, history and search. Category entry links open Example 1 directly; category-only URLs redirect there. The About page and its navigation entries are removed.
- Approved Loops slots: Pyramid Generator (`for`), Sum of Digits (`while`), Prime Factorization (`while`), Number Guessing Game (`do…while`) and Quiz Retry Simulator (`do…while`). Use the shared Input/Result layout. The two games retain a round across submissions and provide an explicit reset.
- Implemented Arrays slots: Duplicate Finder (1) and Array Sort (2), using one-dimensional arrays; Tic-Tac-Toe (3), Multiplication Table (4) and House Member Finder (5), using two-dimensional arrays. These complete the required two one-dimensional and three two-dimensional examples. Duplicate Finder and Array Sort support numbers or names; Tic-Tac-Toe is a local two-player simulation. Details are in docs/arrays.md.
- Mobile, keyboard, visible focus and reduced-motion support.
- `main` retains the earlier macOS design for comparison; this redesign does not merge into it automatically.

## Scope
UI, shared templates, navigation and the owner-approved examples. Implement the supplied decision flows in C# with real validation and results. Login is an educational simulation with no authentication or saved state. Latin Honors Evaluator accepts one decimal GWA from 1.00–5.00 and uses nested thresholds at 1.25, 1.50 and 1.75. Results are based on GWA only. Keep other slots honestly unimplemented; never invent persistence or CRUD.

## Repository layout
Solution root: AGENTS.md, PRODUCT.md, DESIGN.md and README.md. Detailed notes: docs/. Project skill: .agents/skills/showcase-ui/. App: 241611JalopFinalAssignment1/ with standard Controllers, Models, Views and wwwroot folders. Installed design skills stay in their global folders.
