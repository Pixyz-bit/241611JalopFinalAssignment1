# Validation — 2026-10-28

## Welcome centering refinement
- Centered the complete welcome block on the viewport, replacing unequal side insets and the upward desktop offset.
- Browser geometry confirmed zero horizontal/vertical center error at 1440×900, 1920×1080 and 1024×768.
- Existing portrait flow and compact landscape composition remain separate responsive layouts.
- Screenshot: `.impeccable/review/centered-welcome.png`.

## Build and source
- `dotnet build 241611JalopFinalAssignment1.sln --no-restore`: passed, zero warnings and errors after final changes.
- `node --check` on wwwroot/js/site.js: passed.
- Project skill checked for frontmatter, required name/description fields, valid naming and unfinished placeholders. The bundled skill validator could not run because its Python environment lacked PyYAML; the simple two-field frontmatter was checked using Python's built-in parser instead.
- Wallpaper provenance scan: one raster, zero missing provenance records. Font, license, icons and wallpaper are local assets.

## HTTP checks
- All 20 category/example URLs returned 200 and the honest unimplemented state.
- Unknown category, example 0 and example 6 returned 404.
- Enhanced-navigation responses contain the Finder partial and omit the document shell.
- Final canonical page includes server-rendered has-window and welcome inert state; the home page omits inert. A noscript stylesheet hides JavaScript-only search/minimize/maximize controls.
- Final font and wallpaper requests returned 200.

## Browser checks
- Open category, choose example, change category and return to desktop.
- Minimize and restore from active dock preserves the selected example.
- Maximize/restore state updates correctly.
- Search finds Repetition for “loop”; an unmatched query shows the empty state; close returns focus to the page.
- Browser Back and Forward restore category/example routes.
- No browser console errors observed in the checked navigation flow.
- Desktop 1440×900, shorter desktop 1280×720, phone 390×844 and smaller phone 375×667 checked. No document horizontal overflow in checked portrait/desktop sizes; 375px category navigation fits without horizontal clipping.
- Landscape 844×390: final page fits viewport; Finder bottom 305.4px, dock top 318.4px. Detail region scrolls internally, including with the keyboard.
- Visible focus and search-field keyboard input exercised. Reduced-motion handling was inspected in the stylesheet; browser motion preference was not emulated. This is a focused implementation check, not a full WCAG certification or cross-browser audit.

## Independent review
The Impeccable finish reviewer initially requested landscape sizing, supporting-text contrast, and canonical no-JavaScript state fixes. After a batch correction and recapture, all three were scored resolved with disposition **ship**. This verdict covers those three fixes, not an unrestricted accessibility audit.

Supporting text now uses #566b83: measured contrast ranges from 4.71:1 to 5.25:1 on the identified solid surfaces. A static detector also reported the intentionally selected Geist typeface as “overused” and advisory coverage gaps for fine-grained literals outside the initial token frontmatter. These are not reported as a clean detector pass; the final design record documents the implemented system.

## Evidence
Local screenshots are in `.impeccable/review/`: desktop-home, desktop-finder, desktop-example, desktop-about, mobile-home, mobile-example, mobile-search, landscape-category and landscape-example PNG files. The original landscape failure is retained as landscape-before.png for comparison. These are review artifacts, not public website assets.
