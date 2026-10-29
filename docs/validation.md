# Validation — Windows redesign, 2026-10-29

This record applies to `windows-design`. Earlier macOS layout results belong to that version's history and do not describe the current taskbar or Explorer geometry.

## Build and regression checks
- `dotnet build 241611JalopFinalAssignment1.sln --property:OutputPath=bin/windows-check/ --property:UseAppHost=false` succeeded with zero warnings and errors.
- A normal build was attempted but the original app on port 5196 held the default output binaries open. The isolated build above verified this branch without stopping the user's existing app; the standard build command is not claimed to have passed in this session.
- All seven existing `node --test tests/navigation.test.cjs` tests passed. They cover cached requests/history, active-route no-op, cancellation, late failures and retry using the actual script with a minimal deterministic DOM/network boundary.
- Asset provenance scan: two rasters, zero missing prompt records.
- Documentation checks parsed the constrained YAML frontmatter and JSON, verified all eight canonical sections, resolved component token references, and checked the schemaVersion 2 sidecar structure, ten preview snippets and eight-step color ramps. Documentation diffs pass whitespace checks.

## Routes and fallback
- All 24 canonical category/example routes returned HTTP 200: four category overviews and twenty example slots.
- Four checked enhanced-navigation partial endpoints returned HTTP 200; full-page canonical HTML contains server-rendered anchors.
- Unknown category, example 0 and example 6 returned HTTP 404.
- Canonical full-page HTML was inspected for navigation and honest example state. Browser JavaScript-disabled emulation was unavailable, so this is source/HTTP evidence rather than a browser no-JavaScript test. Native Start and anchors work through their standard HTML behavior; noscript hides JavaScript-only search/minimize/maximize controls.

## Browser checks
Category/example switching, browser Back/Forward, close, maximize/restore, minimize and taskbar restoration were exercised. Restoration by Enter retained the exact selected example. About navigation from another minimized category opened About instead of restoring the category. Start opened and closed with Escape, returning focus to its summary. Search accepted input, filtered categories, displayed unmatched-query feedback and returned focus on dismissal. Home and About were checked.

The nominal checked viewports were 1440×900, 1280×720, 390×844, 320×568 and 844×390. Captures can differ by one pixel due to browser scaling. Phone and landscape pages had no horizontal document overflow. At 320px, measured caption/back/example/taskbar hit boxes were at least 44px. Content panes scroll within the bounded window; maximized Explorer reserves the persistent taskbar.

Reduced-motion handling was verified in CSS: the preference rule disables animations, transitions and smooth scrolling. Operating-system preference emulation was unavailable, so runtime reduced-motion behavior was not emulated. This is a focused implementation review, not a cross-browser or full accessibility certification.

## Independent review
The independent file-only finish review returned **ship**, opened all twelve required screenshots and the contact sheet, and found no material source fixes. It judged fidelity against the human-approved Windows direction; no generated UI comp or reproduction measurement was supplied. Dynamic interaction/build facts in that report are parent-reported, while screenshot validity and visual judgments are independent.

The detector ran once before documentation reconciliation. Its advisories largely reflected stale macOS token/ramp records, plus generic Geist warnings; the user explicitly preserved the local Geist font. This is not claimed as a clean detector pass or a second detector run. The final documenter extracted the Windows tokens and component patterns from the actual stylesheet, Razor and script, then updated DESIGN.md, the schemaVersion 2 sidecar and branch guidance.

## Evidence
- `.impeccable/review/windows/`: twelve required capture files.
- `.impeccable/review/windows-contact.jpg`: contact sheet.
- `.impeccable/review/windows-review.md`: independent disposition, evidence and scope.
- `docs/design-brief.md`: approved direction, modes and recorded `seed:0ef4e066c55ebfc6`.
