# Surface brief — Windows branch

## Confirmed direction
The user explicitly approved a Windows 11 inspired replacement on `windows-design`; `main` preserves the prior macOS implementation. The approved direction predates implementation. No generated UI comp serves as a pixel specification: the bloom is a standalone wallpaper asset. The existing ASP.NET Core MVC, net8.0, Razor/CSS/plain JavaScript stack remains required.

## Direction contract
- **THESIS:** explore four C# concepts as applications on a familiar Windows desktop.
- **OWN-WORLD:** original dark cobalt bloom raster, shared gradient concept app badges, pale taskbar glass, a compact light Windows application and the existing self-hosted Geist face with Segoe UI fallback.
- **STORY:** desktop → category overview → one of five clearly unimplemented example templates. Ordinary links and browser history remain meaningful.
- **FIRST VIEWPORT:** left shortcut rail and left-aligned C# Programming Showcase welcome; persistent full-width taskbar with centered Start/search/category shortcuts and a two-line clock. No irrelevant system apps.
- **FORM:** one application window with brand titlebar, right caption controls, sidebar with five canonical example links, category heading and Description/Interface/Output workspace. Maximization fills the space above the visible taskbar. Mobile reflows categories into two columns and examples into five numbered targets. The recorded direction roll is `seed:0ef4e066c55ebfc6`; the user's pinned Windows metaphor overrides alternative worlds.
- **SIGNATURE:** minimize retains the current example and its active taskbar shortcut restores that exact example; About navigation must not restore a different minimized category.

## Modes
Home: Experience. Application and search: Operate. About: Read. Mode defaults serve the approved desktop metaphor.

## Interaction and adaptation
Start uses native details/summary and canonical anchors, with JavaScript for outside/Escape dismissal and dismissal after link selection. Search uses native dialog and retained filtering, keyboard and route-cache behavior. At 720px and below the taskbar is 64px plus bottom safe area, mobile controls use 44px targets and application content scrolls. At 360px and below, About remains in Start while its taskbar icon hides to preserve fit.

## Refinement
The user explicitly refined the same Windows world into an application rather than File Explorer, with category app icons instead of folders. This is a targeted refinement of the pinned direction, not a new concept/comp round.

## Evidence
The earlier Explorer-version screenshots are in `.impeccable/review/windows/`, with `.impeccable/review/windows-contact.jpg` and the independent `.impeccable/review/windows-review.md` ship report. That reviewer opened all twelve historical captures; they do not validate the later application-shell refinement. See docs/validation.md for build, route, browser and verification limits.

Current application refinement evidence: `.impeccable/review/application/`, including `contact.jpg` and the independent `review.md` ship verdict. These captures supersede the historical Explorer captures for the current application shell.
