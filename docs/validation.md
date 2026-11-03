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

## Temperature converter Razor repair

- Fixed the malformed conditional in `_ShowcaseWindow.cshtml`: converter first, numbered placeholder second, category overview last. Restored the placeholder content for the other 19 examples.
- Build passed with zero warnings/errors; all seven navigation tests passed.
- Local HTTP checks verified 0, 25, 100, -40 and 12.5 Celsius conversions; missing, nonnumeric and out-of-range inputs showed validation without a result.
- All 20 example routes, partial converter rendering, category overview and About returned their expected content.


## Paired temperature converter with Confirm
- Build passed with zero warnings/errors; all seven navigation tests passed.
- HTTP checks covered seven conversion cases (forward, reverse, decimals, same units and both absolute-zero boundaries), six invalid submissions (missing/nonnumeric/out-of-range/unsupported units) and all 20 canonical example routes. Client-supplied Result is excluded from binding.
- Browser checks confirmed 0 Celsius gives 32 Fahrenheit and keyboard-confirmed 212 Fahrenheit gives 100 Celsius. Invalid temperatures show validation and no output. Desktop at 1280x800 and phone at 390x844 render the paired fields correctly; mobile has no horizontal overflow and selectors/Confirm are 44px high. Captures: `.impeccable/review/temperature-converter/`.


## Compact light temperature converter and Kelvin
- Replaced charcoal panels and the yellow formula badge with existing Paper/Ink/Blue styling, visible From/To labels and inline field errors. Width is 440px (previously 680px); number fields are 48px high (previously 72px). Mobile selectors and Confirm remain at least 44px high.
- Added Kelvin to both selectors. Server-side validation enforces each scale's absolute-zero limit; calculations normalize through Celsius and retain decimal precision. Kelvin is displayed as K without a degree sign.
- Build passed with zero warnings/errors; all seven navigation tests passed. HTTP checks passed all nine unit combinations, additional decimal/absolute-zero cases (14 valid cases total), eight invalid submissions, all 20 example routes and converter partial rendering. Submitted output values are excluded from binding.
- Browser review passed at 1280x800, 375x812, 320x740 and 1024x500, with no horizontal overflow. Confirmed 0 Celsius -> 273.15 Kelvin and 0 Kelvin -> -273.15 Celsius, negative Kelvin validation, keyboard focus and Back navigation. No motion was added; global reduced-motion styling is preserved.
- Captures are in `.impeccable/review/temperature-light/`. The detector reports existing shared CSS font/token-ramp advisories; the approved app font and established shell are preserved. This entry supersedes the previous dark converter presentation.


## Temperature converter actions and feedback
- Added Swap, Clear, pale-blue output with a unit symbol, confirmed equivalence feedback and tighter action/formula spacing.
- Build passed with zero warnings/errors; all seven navigation tests passed. HTTP checks verified seven representative conversions, validation, the server Swap fallback and Clear GET reset.
- Browser checks covered 23 Fahrenheit -> -5 Celsius, Swap reusing -5 then Confirm returning 23 Fahrenheit, input/unit edits clearing stale result/formula, Clear resetting fields and focus, Enter confirming, and Swap after cached Back navigation.
- Desktop at 1280x800 and phone at 375x812 were inspected. Mobile has no horizontal overflow; Swap, Clear and Confirm retain 44px touch targets. The stacked mobile workspace scrolls normally. Existing reduced-motion rules remain unchanged. Captures: `.impeccable/review/temperature-actions/`.

## Time Unit Converter
Build and seven navigation tests passed. Checked hours/seconds, fractional minutes, zero, missing/negative input and invalid units. Canonical form submissions calculate on the server.

## Character and Word Counter
Build and seven navigation tests passed. Confirmed empty text, repeated whitespace, CRLF normalization, combining accents, emoji and the 10,000 input-length limit. Browser confirmed Hello world -> 11 characters / 2 words.

## ID Card Generator
Build and all seven navigation tests passed. Checked valid text-only MVC and JSON responses, trimming and required/year validation. All 20 canonical routes passed. Browser generated a card from sample details and a local test image; the image source was a blob URL and the file picker was absent from named form fields. Clear removed the image source, hid the card and emptied details. No-photo preview also passed.

Final example checks: desktop/mobile forms fit without horizontal overflow and phone inputs are 44px high. Synthetic file submissions to the ID endpoint were rejected with HTTP 400. The photo decode-ready guard prevents generating a card before a selected image is ready; cleanup also hides photo output on page exit.

## Five Nested If…Else examples — November 1, 2026

- All 14 regression tests passed: seven navigation tests plus seven HTTP suites covering 59 POST cases, all 20 canonical slots, five partial forms and invalid routes. Coverage includes every decision outcome, invalid/malformed/oversized input, missing antiforgery tokens, forged output, password clearing, BMI cutoffs before rounding, triangle inequality permutations, negative numbers, decimals and ties. These HTTP submissions also verify the native server form behavior without JavaScript.
- Standard `dotnet build 241611JalopFinalAssignment1.sln` passed with zero warnings/errors. Initial review used an isolated preview build at port 5198 because an existing project instance locked the normal output. After verifying and stopping that exact executable, the standard build succeeded and the app was restarted with the HTTP launch profile at port 5196.
- Browser confirmed student login, ATM success and denomination rejection, editing hides stale output, Clear empties fields, keyboard BMI submission, triangle classification, negative largest-number results and expandable text flows. Back/Forward restored the correct examples; minimize/restore preserved the selected result; maximize reserved the taskbar. Ctrl+K search filtered to Nested If…Else, Close returned to the desktop, and browser logs contained no errors.
- Inspected desktop 1280×800, phone 375×812, narrow phone 320×740 and short landscape 1024×500. All five forms fit at 320px without document overflow. Mobile fields/actions use 44px CSS minimum heights. Long flow lines scroll inside their preformatted block. Short landscape workspace scrolls; keyboard focus is visible.
- No motion was added. Source and browser CSS inspection confirmed the existing reduced-motion rule disables animations/transitions and smooth scrolling. The browser tool does not expose media preference emulation, so an OS-preference toggle was not exercised.
- The Impeccable detector ran over changed surfaces. It reports the existing shared Geist and design token/ramp advisories; the approved font, palette and shell are preserved. Its context loader also identified pre-existing legacy PRODUCT metadata and an outdated design sidecar; their separate `init`/`document` migrations are outside this feature.
- Review captures: [desktop](review/nested-if-else/desktop.jpg), [mobile](review/nested-if-else/mobile.jpg). Example behavior and commands: [nested-if-else.md](nested-if-else.md).

## Landscape ID card — November 2, 2026

- The ID card fills the Result panel width at a fixed 85.6:54 landscape ratio. Removed the subtitle and programming-example footer. Container queries stack panels according to the workspace width and pair student number/year only when the Input panel has enough room.
- Browser checks covered 1440×960, 1280×720, 1024×768, 844×390, 390×844 and 320×640, including restored and maximized windows. No document horizontal overflow was measured; the portrait remained inside the card. The workspace scrolls vertically on short screens and keeps the taskbar clear.
- Maximum-length text wraps and scrolls inside the keyboard-focusable details region without changing the card ratio. End scrolls the details; regenerating returns them to the name. Confirmed local photo decoding/display, validation feedback, Generate, Clear and browser Back/Forward.
- Build passed with zero warnings/errors; all 39 existing regression cases passed. The layout detector reported no findings. Existing visible focus and reduced-motion rules remain intact; no motion was added. Rebuilt/reran after each code batch; the app remains on http://localhost:5196.

### Compact ID spacing follow-up

- Replaced the 85.6:54 ratio with a fixed 11:5 ratio, making the same-width card approximately 28% shorter. Reduced header/body padding, made the photo fill its body height, and replaced the percentage-based label column with a shared content-sized column. Metadata rows distribute within the available height.
- Rebuilt/reran successfully with zero warnings/errors. All nine navigation tests and three Input/Output integration tests passed; the layout detector reported no findings.
- Inspected 1440×960, 1024×768, 390×844 and 320×640. The ratio remains 2.2:1 without horizontal overflow. A decoded local photo stays inside the card. Maximum-length text wraps without horizontal overflow; the keyboard can scroll to the final detail. Generate and Clear remain functional. Existing reduced-motion styling is preserved.

### Portrait ID follow-up

- Implemented the approved 4:5 portrait wireframe: centered header, photo and name above a left-aligned vertical list of student details. The card is centered at up to 420px wide. Its own container controls type/spacing, and separate photo/detail rows prevent overlap. Cards narrower than 280px reserve more space for text.
- Browser measurements at 1440×960, 1024×768, 390×844 and 320×640 confirmed a constant 0.8 width/height ratio, centered photo, no photo/detail overlap and no document horizontal overflow. Ordinary details fit at 320px; maximum-length names/courses wrap and scroll locally with keyboard access. Generate and Clear were verified.
- Both builds passed with zero warnings/errors, the 12 navigation/Input-Output regression cases passed, and the layout detector reported no findings. Existing focus and reduced-motion rules remain intact. Rebuilt/reran after each code batch; the app remains at http://localhost:5196.

### Student number beneath the photo

- Swapped the prominent name and student number: the number is centered immediately beneath the larger photo, while Full name begins the labeled detail list. The number retains a screen-reader label. Tightened header/body padding and the photo gap; detail groups fill the available height rather than leaving a blank footer area.
- Checked 1440×960, 1024×768, 390×844 and 320×640. The card retains its 4:5 ratio; ordinary details fit without internal scrolling, photo-to-number spacing is about 6px, and bottom inset is about 10–13px. No overlap or horizontal overflow was measured. Maximum-length fields still wrap and support keyboard scrolling.
- Build passed with zero warnings/errors; all 12 navigation/Input-Output tests passed. The layout detector reported no findings. The app was rebuilt/rerun and remains on http://localhost:5196.

### Consistent ID value typography and ordinal years

- Name, course and year values now share the name's responsive font size and 550 weight. The model supplies one ordinal year label (1st through 6th year) to both Razor and the enhanced JSON preview.
- Build passed with zero warnings/errors. All 13 navigation/Input-Output tests passed, including native and JSON label agreement for all six years. The layout detector reported no findings.
- Browser checks at 1440×960, 390×844 and 320×640 confirmed equal computed sizes/weights across all three values, the “2nd year” result, no horizontal overflow and a preserved 4:5 ratio. Ordinary details fit inside the card. The app was rebuilt/rerun and remains at http://localhost:5196.

## Named example navigation

- Centralized the 19 approved titles and the honest Available slot label in Models/ExampleCatalog.cs. Headings and sidebar links share the catalog. Canonical URLs and selected-state semantics remain unchanged.
- Desktop labels wrap beside fixed-size icons. Below 720px, full names appear in a horizontal scroll row with at least 44px link targets. Existing focus and reduced-motion styling is preserved.
- The final build passed with zero warnings/errors after stopping the app to release its executable. All 41 regression cases passed, including names, destinations and selected states on all 20 full and partial routes. The layout detector reported no findings.
- Local browser use was disabled in Work settings during this task. Desktop/mobile rendered inspection and interactive keyboard review could not be completed; no visual verification is claimed. The app was rerun at http://localhost:5196 for review.

## Rectangle area and perimeter — November 3, 2026

- Input/Output Example 2 calculates rectangle area (length × width) and perimeter (2 × (length + width)) with decimal dimensions from 0.01 to 1,000,000. Results are excluded from model binding, antiforgery is required, and invalid inputs retain the empty Result panel. Sidebar and heading share the approved title.
- Build passed with zero warnings/errors after stopping the verified old executable to release its file lock. The app was rebuilt/rerun on http://localhost:5196. All 42 regression tests passed, including decimal/boundary calculations, invalid/missing dimensions, forged result values, missing antiforgery and all named canonical/partial routes. Layout detection found no issues.
- Browser review covered 1280×800, 390×844 and 320×640. Panels stack on mobile without document horizontal overflow; phone inputs/buttons are 44px tall. Verified keyboard Enter (5 × 3 → area 15, perimeter 16), decimal results (2.5 × 1.2 → area 3, perimeter 7.4), Clear, edits hiding stale output, and browser Back/Forward. Existing reduced-motion and focus CSS is preserved; no motion was added.

## Login decision flow pilot — November 3, 2026

- Added a responsive flowchart only to Nested If…Else Example 1. Question/outcome nodes and Yes/No branch labels use the full workspace width below the form; highlighting is derived from the controller's recorded checks. Submitted outcomes open the disclosure and show Your result on one endpoint. Editing removes the previous path highlights.
- Build passed with zero warnings/errors, layout detection found no issues, and all 43 regression tests passed. Added coverage for active route lengths on Admin, Student, Guest and invalid credentials, the neutral GET state and unchanged text trees in Examples 2–5.
- Browser review at 1440×1000, 390×844 and 320×640 confirmed branching desktop and stacked phone layouts without document horizontal overflow. Verified Student/Guest endpoints, clearing highlights after edits, and keyboard disclosure toggling. No motion added. Rebuilt/reran at http://localhost:5196. Desktop capture is in .tmp/login-flow-desktop.png.

## Compact text decision trees — November 4, 2026

- Replaced the Login flowchart pilot and all four earlier ASCII trees with shared compact text trees matching the user's reference: ├── / └── branches, connected nesting, no vertical-only spacer lines, and a dark monospace panel. Login's disclosure returns beneath its inputs. Removed unused flowchart styling and stale-highlight JavaScript; calculations and Result decision paths remain unchanged.
- Build passed with zero warnings/errors. All 43 regression cases passed, including compact-tree checks on all five full and partial routes. Layout detector reported no findings. Rebuilt/reran at http://localhost:5196.
- Browser reviewed the desktop Login tree at 1440×960 and all five expanded trees at 320×640. None widened the document; larger trees scroll inside their keyboard-focusable preformatted panels. Keyboard disclosure opening and tree focus were verified. Existing reduced-motion styling remains unchanged. Capture: .tmp/compact-decision-tree.png.

## White decision-tree surfaces — November 4, 2026

- Replaced the dark text-tree surface with the app's existing Paper background, Ink text, Line border and field-border scrollbar color across all five decision examples. Compact branch text and behavior remain unchanged.
- Build passed with zero warnings/errors and layout detection reported no findings. The app was rebuilt/rerun at http://localhost:5196.
- Browser reviewed Login at 1440×960 and all five expanded diagrams at 320×640: each uses the shared white/dark-text/border colors and none causes document horizontal overflow. Keyboard disclosure opening remains functional. Capture: .tmp/white-decision-tree.png.

## Branch-label emphasis

Only Yes and No are now bold in all five decision trees; conditions, outcomes and branch lines use regular weight. HtmlContentBuilder encodes the source text and inserts only fixed strong tags without introducing whitespace. Build passed with zero warnings/errors; all eight decision regression tests passed, including bold-label and preserved-tree checks. App rebuilt/rerun at http://localhost:5196.
