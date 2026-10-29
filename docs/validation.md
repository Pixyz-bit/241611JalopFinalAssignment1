# Validation — Windows redesign, 2026-10-29

This record applies to `windows-design`. The detailed review below records the earlier Explorer version; it is historical evidence for that version, not validation of the later application shell.

## Application-shell refinement
- Both isolated builds passed with zero warnings and errors: initial `dotnet build 241611JalopFinalAssignment1.sln --property:OutputPath=bin/app-check/ --property:UseAppHost=false` and final `dotnet build 241611JalopFinalAssignment1.sln --property:OutputPath=bin/app-final/ --property:UseAppHost=false`.
- All seven existing `node --test tests/navigation.test.cjs` checks passed after the final JavaScript changes.
- All 24 canonical category/example routes and four checked partial endpoints returned HTTP 200. Full canonical HTML contains no `folder.svg` reference.
- Browser checks confirmed exact Example 01 restoration with Enter, maximize/restore/minimize and correct Back/Forward URLs. The 390px example pane scrolled to expose Output using End (scrollTop 62px of 63px). Start displayed app icons; Escape dismissed it and returned focus to its summary. At 320px, measured caption targets were 44×46px and example tabs 47×44px, with no horizontal document overflow.
- The detector ran once for this refinement and saved `.impeccable/review/application/detector.json`. It reported two generic Geist warnings and documentation token/color/font/radius advisories; no other categories were reported. No second run or clean detector pass is claimed.
- The targeted documentation checks parsed the constrained YAML frontmatter and JSON, resolved component references, retained schemaVersion 2 and ten representative previews, and found no obsolete Explorer preview. Documentation diffs pass whitespace checks.
- Current captures in `.impeccable/review/application/` cover desktop app/icons/example at 1280px, phone app/search/Start at 390px, a 320px phone and 844px short landscape. Search filtering and unmatched feedback, close, About, Start Escape/focus, and keyboard scrolling were checked. Landscape has no horizontal document overflow. Reduced motion remains source-verified; browser JavaScript-disabled emulation remains unavailable.
- A fresh independent reviewer inspected all eight current originals and the contact sheet and returned **ship**, with no material source fixes. Report: `.impeccable/review/application/review.md`. The earlier Explorer review below is historical. Documentation/sidebar and selection records were reconciled to the actual source values after review; no rendered styles changed.

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

## Example sidebar and taskbar refinement

- Replaced category links inside a concept application with its five canonical example links and removed duplicate content tabs. About retains category navigation.
- Removed the taskbar search bar. Start, sidebar search and Ctrl/Cmd+K remain available; search dismissal returns focus to Start.
- Build passed with zero warnings/errors using an isolated output directory; all seven navigation tests passed.
- Browser checks covered keyboard selection, Back/Forward, search dismissal, exact-example minimize/restore and desktop/mobile rendering. At 390px all five links were at least 69 by 44px, with no horizontal overflow. Captures are in `.impeccable/review/sidebar/`.
- Generated icon proposals remain preview-only until explicit user approval.
- The detector reported existing font and palette/type-ramp advisories; this refinement preserves the approved visual system.

## Approved Fluent category artwork

- Applied the four user-approved generated PNGs through the shared category icon partial; source alpha is preserved and no enclosing CSS tile or shadow is added. About keeps its code badge.
- Build passed with zero warnings/errors; all seven navigation tests passed.
- Browser review confirmed loaded, versioned assets on desktop, Start, taskbar, search and category headings. At 390px there were no broken icons or horizontal overflow. Screenshots are in `.impeccable/review/approved-icons/`.
- Exact prompts, approval status and public asset paths are recorded in `docs/icon-preview-prompts.json`.

## Desktop shortcut rail bounds

- Fixed About overlapping the taskbar at intermediate desktop heights. Compact rail spacing now begins at 760px, retains About, and uses bounded scrolling on very short screens. Mobile keeps its existing four-column layout.
- Browser measurements: at 700px height About ended at 440px, above the taskbar at 642px; at 800px it ended at 658px, above 742px. At 420px the rail ended at 326px and scrolled above the taskbar at 362px. Mobile at 390px had no horizontal overflow. Capture: `.impeccable/review/shortcut-rail/700px.png`.

## Full-width category header

- Preserved the Windows titlebar as the first row. Moved the category icon/title/description beneath it, spanning the example sidebar and workspace. Removed sidebar search; Start and Ctrl/Cmd+K remain available.
- Build passed with zero warnings/errors; seven navigation tests passed. All 20 canonical example routes plus partial rendering and About passed.
- Browser review confirmed matching category-header/window widths, header directly below the titlebar, five canonical links, keyboard selection, Back/Forward, maximize above the taskbar and exact-example minimize/restore.
- At 390px the header and window were both 370px wide with no horizontal overflow. At 1024x500 the workspace retained 242px of scrolling height and the titlebar remained 48px tall. Existing reduced-motion CSS remains active. Captures: `.impeccable/review/category-header/`.

## Compact category header

- Reduced desktop header padding to 16px vertically, title to 28px and icon to 44px. Descriptions may use up to 100ch; workspace padding is 24px.
- At 1280x800 the Loops header is 97px tall, reclaiming about 56px compared with the previous desktop header; the workspace has 479px of height. At 390px the header still matches the window width and there is no horizontal overflow. Captures: `.impeccable/review/compact-header/`.

## Wallpaper framing

- Desktop uses centered `contain` sizing over the existing cobalt ground, preserving the complete wallpaper composition rather than enlarging and cropping it with `cover`. Portrait screens retain a focused cover crop at 65% horizontal position.
- Reviewed desktop at 1280x800 and mobile at 390x844; mobile has no horizontal overflow. The raster is unchanged and retains its aspect ratio. Captures: `.impeccable/review/wallpaper-fit/`.

## User-supplied wallpaper replacement

- Replaced the generated bloom with the user-supplied `windows-wallpaper.png`. Restored centered `cover` sizing on desktop and phone, removing the empty bands introduced by `contain`. No image stretching or extra zoom scale is used.
- Visual checks passed at approximately 1917x866 and 390x844: the wallpaper reaches every edge with no horizontal overflow. Captures: `.impeccable/review/supplied-wallpaper/`. This supersedes the earlier Wallpaper framing entry.
