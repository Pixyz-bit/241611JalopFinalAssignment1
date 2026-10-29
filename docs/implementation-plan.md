# Approved implementation record — Windows branch

The user approved replacing the macOS visual world on `windows-design` while retaining the macOS version on `main`. This file records the Windows implementation and the user-requested application-shell refinement.

1. Preserve ASP.NET Core MVC, net8.0, Razor, CSS and plain JavaScript, all canonical routes and the existing route cache.
2. Replace the desktop material with an original cobalt bloom raster and shared CSS gradient concept app badges; arrange a left shortcut rail and welcome.
3. Build a persistent full-width taskbar, centered app group, native Start navigation and two-line local clock.
4. Refine the shared shell to `_ShowcaseWindow.cshtml`: brand titlebar, right caption controls, sidebar with five canonical example links, category heading and one main workspace. Reuse `_ConceptIcon.cshtml` across desktop and navigation.
5. Preserve category/example switching, history, search and exact-example minimize/restore. Maximization reserves the taskbar.
6. Adapt phone and short landscape layouts, keeping all five examples and reachable controls.
7. Validate build, routes, navigation and visual evidence; obtain independent review; reconcile DESIGN.md, the sidecar and project guidance with the shipped system.

All four categories still have five unimplemented slots. No algorithms, database, authentication or execution endpoint are included in this phase. Build and browser verification limits are explicit in docs/validation.md. The approved direction, seed reference and modes are recorded in docs/design-brief.md.
