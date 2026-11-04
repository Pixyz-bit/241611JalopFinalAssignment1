---
name: showcase-ui
description: Build or refine this C# Programming Showcase MVC interface while preserving its approved Windows 11 inspired desktop design, reusable example templates, and project structure.
---

# Showcase UI

Read the solution-root PRODUCT.md, DESIGN.md and AGENTS.md first. Use docs/architecture.md to locate templates and extension points.

## Coordinate the installed skills
- Impeccable: `C:/Users/arlan/.agents/skills/impeccable/SKILL.md` for hierarchy, interaction design and rendered review.
- Frontend God Mode: `C:/Users/arlan/.codex/skills/frontend-god-mode/SKILL.md` for shared visual consistency and preflight checks.
- UI/UX Pro Max: `C:/Users/arlan/.codex/skills/ui-ux-pro-max/SKILL.md` for targeted accessibility and usability guidance. Its search script is inside that skill, not this project.

These are dependencies in this workstation's skill catalog, not copied project source. Resolve by skill name through the available catalog on another machine. If absent, follow the project's durable design rules and report the missing augmentation.

## Project-specific decisions
The approved Windows direction on this branch and user-approved MVC stack override React, Tailwind, generic landing-page layouts and other conflicting defaults. Keep the user-supplied blue Windows wallpaper, shared transparent PNG concept app icons, left desktop rail and credits, persistent full-width taskbar, native Start panel and one light Windows application with a brand titlebar, category navigation and five canonical example tabs. The maximized application fills the area above the taskbar. Keep the current example when minimizing and restore it from its active taskbar shortcut. Use one shared CSS token set, not a parallel generated design system.

Preserve all 20 implemented owner-approved examples. Every link must have a server-rendered route. Add examples through the existing Models/Controllers/Views structure; preserve shared sidebar and shell partials.

## Validation
Build the solution; check affected canonical and partial routes, history, search, close/minimize/maximize, keyboard focus and reduced motion. Inspect the home desktop and an open application at desktop and mobile sizes. Keep documentation in docs/, public assets in wwwroot/, and project skills here.
