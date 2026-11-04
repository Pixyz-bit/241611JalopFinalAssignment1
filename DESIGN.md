---
name: C# Programming Showcase
description: A Windows 11 inspired desktop for exploring C# fundamentals.
colors:
  primary: "#0067c0"
  primary-soft: "#e5f1fb"
  ink: "#202a35"
  muted: "#526170"
  paper: "#fcfdff"
  surface: "#f3f5f8"
  line: "#dde3eb"
  desktop-text: "#f5f8ff"
  desktop-ground: "#061a45"
  sidebar: "#eaf0f8"
  selection: "#e0edfa"
  sidebar-selected: "#d7e6f7"
  taskbar: "rgb(234 241 252 / .94)"
  start-panel: "#eef3fb"
  status: "#edf1f6"
  status-ink: "#47596e"
  app-cyan-top: "#159acb"
  app-cyan-bottom: "#0b5ba9"
  app-amber-top: "#f3ca78"
  app-amber-bottom: "#dca039"
  app-violet-top: "#9183df"
  app-violet-bottom: "#5a47ac"
  app-mint-top: "#1b9b8e"
  app-mint-bottom: "#0b6d65"
  focus: "#0067c0"
  field-border: "#7d8da2"
  danger: "#a4262c"
typography:
  display:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "clamp(40px, 4.7vw, 72px)"
    fontWeight: 450
    lineHeight: 1.12
    letterSpacing: "-0.04em"
  headline:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "42px"
    fontWeight: 500
    lineHeight: 1.16
    letterSpacing: "-0.035em"
  title:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "22px"
    fontWeight: 550
    lineHeight: 1.4
    letterSpacing: "-0.02em"
  body:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "13px"
    fontWeight: 400
    lineHeight: 1.6
  detail:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "12px"
    fontWeight: 400
    lineHeight: 1.8
  label:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "12px"
    fontWeight: 550
    lineHeight: 1.6
  supporting:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "10px"
    fontWeight: 400
    lineHeight: 1.6
rounded:
  row: "4px"
  control: "5px"
  app-icon: "12px"
  app-icon-compact: "10px"
  app-icon-taskbar: "6px"
  window: "8px"
spacing:
  xs: "4px"
  compact: "6px"
  sm: "8px"
  control-gap: "10px"
  inset: "12px"
  md: "16px"
  panel: "20px"
  lg: "24px"
components:
  welcome-action:
    backgroundColor: "#eef6ff"
    textColor: "#123a65"
    rounded: "{rounded.control}"
    padding: "10px 18px"
  primary-link:
    textColor: "{colors.primary}"
    typography: "{typography.label}"
  sidebar-link:
    textColor: "{colors.ink}"
    rounded: "{rounded.control}"
    padding: "10px 12px"
  sidebar-link-selected:
    backgroundColor: "{colors.sidebar-selected}"
    rounded: "{rounded.control}"
  status-pill:
    backgroundColor: "{colors.status}"
    textColor: "{colors.status-ink}"
    rounded: "{rounded.row}"
    padding: "4px 8px"
  empty-interface:
    backgroundColor: "{colors.surface}"
    rounded: "{rounded.control}"
    padding: "18px"
  search-field:
    backgroundColor: "{colors.paper}"
    textColor: "{colors.ink}"
    rounded: "{rounded.control}"
    padding: "10px 14px"
  desktop-shortcut:
    textColor: "{colors.desktop-text}"
    rounded: "{rounded.row}"
    padding: "10px 3px 8px"
  taskbar:
    backgroundColor: "{colors.taskbar}"
    padding: "4px 18px"
    height: "58px"
  start-panel:
    backgroundColor: "{colors.start-panel}"
    rounded: "{rounded.window}"
    padding: "30px 34px 0"
  application-icon:
    rounded: "{rounded.app-icon}"
    size: "58px"
  application-shell:
    backgroundColor: "{colors.surface}"
    textColor: "{colors.ink}"
    rounded: "{rounded.window}"
---

# Design System: C# Programming Showcase

## Overview

**Creative North Star: "A desktop for curious minds."**

The approved Windows 11 inspired world pairs the user-supplied blue Windows wallpaper with colored application badges, a full-width translucent taskbar and a light Windows application workspace. Desktop familiarity invites exploration; compact application typography keeps the examples easy to navigate.

The Windows direction is deliberate on `windows-design`; `main` retains the earlier macOS version. The user-approved metaphor and existing MVC implementation govern this system. Surface composition and visitor modes live in docs/design-brief.md.

**Key Characteristics:**
- Dark cobalt raster bloom with readable light desktop text.
- Shared concept app icons, a centered taskbar app group and a left-aligned welcome.
- One light application with brand titlebar, right caption controls, five canonical example links in the sidebar.
- Local Geist typography, canonical links and visible keyboard focus.

## Colors

Application Blue marks interactive links, active indicators and focus. Soft Blue supports the About emblem. Blue/charcoal, amber, violet and teal artwork gives the four concepts consistent app identities across desktop, Start, search, taskbar and page headings.

The light workspace uses Paper for detail, Surface for the application ground and empty interfaces, Sidebar for navigation, Line for pane boundaries, and dark Ink with Muted supporting text. Sidebar Selected marks the current example with a pale fill and blue leading indicator. Taskbar is deliberately translucent; Start Panel remains a readable pale surface.

**The Readable Desktop Rule.** Check light labels against the wallpaper beneath them; an average image color cannot establish text contrast.

## Typography

The self-hosted Geist variable face is preserved, with Segoe UI and sans-serif fallbacks. The welcome uses the display role with a stronger second line (650); About uses the headline role. Example titles use the title role; navigation and template sections use compact label/detail roles. Small metadata stays subordinate to category and example names.

Category headings use 28px type (25px on phones), weight 600 and line height 1.25. The header uses 16px vertical/24px horizontal padding and a 44px icon. Its description can span up to 100ch, avoiding unnecessary wrapping on desktops; template text remains bounded at 60ch. Implemented examples use a 12px pane inset and shared panel padding; empty templates retain their original spacing. The taskbar clock uses tabular numerals and two lines.

**The Two Registers Rule.** Keep expressive welcome and About headings separate from compact application navigation and example detail.

## Layout

Desktop shortcuts form a left rail (108px wide); the welcome begins at 20% of viewport width. The taskbar spans the full bottom edge and uses three columns to center Start and category shortcuts independently of the brand and clock. Its desktop height is defined by the frontmatter taskbar component.

The ordinary application window reserves the shortcut rail and taskbar: top max(24px, 6vh), left max(146px, (100vw - 1150px) / 2), right max(32px, (100vw - 1280px) / 2), bottom taskbar height + max(28px, 5vh). Its sidebar/main columns are 218px / flexible, becoming 194px / flexible at 1100px. The main content scrolls within the bounded window. The brand titlebar remains the top row. A full-width category icon, heading and description form the next row, above both the example sidebar and the scrollable workspace. The sidebar shows the five example titles from ExampleCatalog; long names wrap within the existing width.

At 720px and below, taskbar height becomes 64px plus the bottom safe-area inset, the tray/brand disappear, home app shortcuts form four columns, and the application sits 10px from the sides and 12px above the taskbar. The example sidebar becomes a horizontally scrollable row of full example names with 44px targets; About retains a two-column category menu. Caption, taskbar, search and category controls use at least 44px mobile targets. At 360px and below, About stays available in Start while its taskbar shortcut hides to preserve control width. At desktop heights up to 760px the shortcut rail uses compact icons and hides counts while retaining About. The rail is bounded above the taskbar and scrolls when necessary. Rules at 500px further reduce application spacing.

**The Persistent Taskbar Rule.** The maximized application fills the viewport above the taskbar; the taskbar remains visible and never covers application content.

The chosen window size persists within the current browser tab across native form submissions and reloads. A small early script restores maximized geometry before the page paints; the regular navigation script synchronizes the caption controls. Browser storage holds only the window-size preference. When storage is unavailable, controls still work for the current page.

## Elevation & Depth

The bloom carries the sculptural material as a raster asset. Desktop wallpaper uses centered `cover` sizing to fill every viewport edge without empty bands or distortion. This is the smallest proportional scale that covers the screen; some edge cropping is necessary when the screen and image aspect ratios differ. Portrait layouts also use centered `cover` sizing. The active asset is `wwwroot/images/windows-wallpaper.png`, supplied by the user; the earlier generated bloom remains unused. The application, Start and search share one diffuse window shadow (0 20px 70px -14px rgb(3 17 43 / .48)). Taskbar glass uses a pale translucent fill and backdrop blur (24px); working and reading surfaces remain light and solid. Small app-icon, welcome-action and tooltip shadows support separation without a second shadow system.

Window entry lasts 220ms with the shared easing cubic-bezier(.16, 1, .3, 1), moving from a small downward offset and slight scale. Desktop hover changes are brief (140ms); welcome fades use 180ms. The reduced-motion media rule disables animation, transitions and smooth scrolling.

**The Material Boundary Rule.** Keep translucency on desktop chrome and preserve solid reading surfaces inside the application, Start and search.

## Shapes

Compact controls and sidebar links use small corners; windows and dialogs use the window radius. Search fields keep compact control corners; the taskbar has no search bar. The four approved category icons use original transparent PNG artwork inspired by layered Windows Fluent app icons. Each image retains its own silhouette, shading and transparent margins without an added tile or CSS shadow. Containers are 58px on the desktop, 46px in Start, 28px in the taskbar and 36px in search; responsive rules adjust their size. About retains its existing code badge. Maximized application windows have square corners.

## Components

### Application icons and welcome

Desktop app shortcuts are canonical category anchors with visible names and counts. The shared _ConceptIcon partial loads the category PNG by its canonical slug, with a versioned asset URL. The approved terminal, branching paths, repeat arrows and data grid use simple layered forms and restrained shading. Exact generation prompts and asset paths are recorded in docs/icon-preview-prompts.json. Hover and the active category use a subtle pale wash and border. Light focus outlines remain visible on the dark wallpaper. The welcome action opens Input/Output directly.

### Taskbar and Start

The taskbar groups Start, four categories and About, with active blue indicators and hover/keyboard tooltips. The clock displays local time and date when enhanced by JavaScript. Start uses native details/summary so its canonical links remain usable without JavaScript; enhancement dismisses it on outside click, Escape or link selection. Escape returns focus to Start.

When space permits, Start's outer width and height enlarge proportionally to fill the available height, leaving 12px at the top and 10px above the taskbar. Text, icons, controls and padding retain their smaller natural CSS sizes; the footer stays at the bottom, with extra space above it. Viewport width caps the enlargement with 12px side margins. On small or short screens, the existing responsive layout stays at its natural size and scrolls as needed, keeping controls readable. Without JavaScript, the native panel retains its natural size.

### Application shell and example navigation

A brand titlebar and right-aligned minimize/maximize/close controls remain at the very top. A full-width category header sits beneath it, followed by example navigation and one main content area. The sidebar contains the five example names and About/Desktop utilities; it has no search field. About retains category links because it has no current category. The category header spans both columns and stays outside the scrolling example workspace. The selected example uses a pale fill and blue leading mark. The workspace groups honest Description, Interface and Output sections without file-browser chrome or filename suffixes. Minimize retains the current example, and its active taskbar shortcut restores that exact view. Close returns to the desktop. Maximize retains access to the taskbar.

### Search

Native dialog supplies modal focus containment. A visible label, blue focus treatment, local filtering, canonical result links, explicit close control and empty state support the task. Ctrl/Cmd+K opens it; Escape closes it. Dismissal returns focus to Start. Search remains available in Start and Ctrl/Cmd+K.

### Temperature converter

Input/Output Example 3 uses the shared two-panel example layout. Amount, From and To stack in the Input panel above Confirm, Clear and Swap units. The Result panel shows a 24–32px conversion with unit symbols and a 12px formula below it. Celsius, Fahrenheit and Kelvin are selectable in either direction. Swap reuses the full-precision confirmed result; editing clears stale output and formula. Without JavaScript, Clear is a canonical GET link and Swap is a server form action. Shared fields and mobile touch targets match the other examples.

### Empty examples

Use the neutral status pill and dashed interface container to explain that the slot awaits implementation. Description, Interface and Output regions share the same detail typography. Preserve all five numbered anchors in every category; status changes only when the owner implements the example.

## Do's and Don'ts

### Do:
- **Do** read PRODUCT.md, DESIGN.md and the project showcase-ui skill before changing the interface.
- **Do** preserve the approved Windows desktop materials and shared application template on this branch.
- **Do** keep canonical navigation, visible focus and the exact-example minimize/restore behavior.
- **Do** check desktop, narrow phone, short landscape, keyboard and reduced-motion behavior after UI changes.

### Don't:
- **Don't** import React or introduce a frontend build pipeline into this MVC template.
- **Don't** replace the approved user-supplied wallpaper with CSS sculpture.
- **Don't** cover the persistent taskbar when maximizing the application.
- **Don't** replace honest empty examples with invented execution or persistence.

### Shared Input/Output forms

All implemented examples reuse the approved Example 4 layout: a shared heading and description, white Input panel on the left and pale-blue Result panel on the right. Shared CSS classes own the full-width form grid, 12px pane inset, 16px panel gap, 20px panel padding (16px on phones), 16px panel headings, vertical fields and 24–32px primary result text. Panels fill the available pane height and stack at viewport widths up to 1000px. Longer forms and results scroll within the workspace. Result panels remain present in empty, invalid and edited states; calculations and validation remain in C#. Unimplemented slots retain their honest empty templates.

Input/Output Example 4 fills the available pane height with a two-panel bento layout. A shared heading sits above a white Input panel on the left and a pale-blue Result panel on the right. Amount, From and To stack vertically above Convert and Clear. The result panel stays visible before conversion, after invalid input and after edits; successful output emphasizes the complete conversion with a directional arrow (for example, “23 hours → 82,800 seconds”), with precision guidance at the bottom. Panels share the available height and use 20px inner padding, reduced to 16px on phones. A 12px pane inset and 16px panel gap keep the workspace compact. At viewport widths up to 1000px the result stacks below the inputs to accommodate the sidebar; content scrolls when needed. Calculations and validation remain server-rendered.

Panel headings use 16px type. The complete conversion scales from 24px to 32px, with tabular numerals and wrapping for large conversions. A unit-specific formula sits below it at 12px, matching the Converted time label; both hide when inputs change. The workspace has no additional outer padding beyond the pane inset.

### ID preview

The ID generator uses the shared Input/Result panels with a slightly wider Result column. Its centered portrait card keeps a fixed 4:5 ratio and uses the available panel width up to 420px. The Student ID header, larger portrait photo and student number are centered; the number sits directly below the photo. Full name, course and year form a left-aligned list with values below their labels. All three values share the same 14–18px responsive size and 550 weight. Year values use ordinal labels such as “2nd year” in both native and enhanced previews. Compact padding and a small photo-to-number gap keep the content close; detail groups fill the remaining height without a blank footer area. It has no subtitle or footer. Typography and spacing respond to the card's own container; unusually long names/courses wrap and scroll inside a keyboard-focusable details region so the ratio remains fixed without overlap. The photo and details occupy separate bounded rows. Below 760px of actual workspace width the panels stack; student number and year share a row only when the Input container exceeds 340px. Generate has a busy state and validated text feedback. The photo is optional and local-only; no download or upload control is provided. Clear restores the empty Result state.

### Nested decision examples

The five decision examples use the shared two-panel layout, vertical labeled Paper inputs, blue actions and canonical Clear links. Sample accounts, hints and native decision-tree disclosures sit beneath the relevant inputs. Each Result panel shows a 24–32px decision result followed by 12px detail and an ordered Decision path. The panel remains present before submission and after edits. Decision trees scroll horizontally inside their disclosure on narrow phones. Inline validation is supplemented by a linked error summary. After native POST, keyboard focus moves to errors or the result; editing hides outdated feedback. All mobile fields, actions and disclosure targets are at least 44px high. No motion or palette changes are introduced.

### Loop examples

All five loop examples reuse the same Input/Result panels and responsive breakpoints. Inputs, reset links and a native loop-structure disclosure appear on the left. The right panel shows a 24–32px summary, a 12px explanation and ordered loop steps. The pyramid is an actual text artifact in 14px Consolas/monospace with preserved whitespace; wide pyramids scroll locally. Quiz prompts use the existing 13px body size. Guessing and quiz rounds show remaining attempts, keep inputs ready for retry and disable the submit action when complete, leaving New game or New quiz available. The disabled action uses reduced opacity and a disabled cursor. Linked errors and focused results match the decision examples. No new color or animation is introduced.

### Array examples

All five array examples share the existing panel grid, controls and 24–32px result summaries. One-dimensional arrays display indexed cells with 13px values and 10px index labels; duplicate counts and two-dimensional data use 12px tables. Flat-array entry uses a resizable textarea with a 96px minimum height. Numeric grids have three equal columns; member fields use three columns on desktop and stack on phones. Tables scroll locally if needed, and long names wrap inside cells. Search matches use the shared blue fill plus a visible Match label. The Tic-Tac-Toe input board is capped at 240px and uses native square submit buttons of at least 44px, with readable X/O marks and winning-square emphasis. Completed boards disable further moves and retain New game. Array results receive keyboard focus without pulling long output past the workspace heading; validation summaries still scroll into view. No new palette, shadow or motion is introduced.

### Rectangle area and perimeter

Input/Output Example 2 uses the shared white Input and pale-blue Result panels. Length and width stack above Calculate and a canonical Clear link. Both dimensions use the same unspecified unit. The result shows area in square units and perimeter in units, each at the shared 24–32px result size with a 12px formula. Decimal inputs from 0.01 to 1,000,000 are accepted; results display up to eight decimal places. The existing responsive grid, focus styling and reduced-motion rules apply without new visual tokens.

### Compact decision trees

All five Nested If…Else examples use the shared native disclosure beneath their inputs. Their diagrams are plain preserved text using ├──, └── and │ branch characters, four-space nesting and no vertical-only spacer rows. The panel uses the shared white Paper surface, dark Ink text and Line border, with 12px Consolas text at weight 400 and line height 1.7; only Yes and No branch labels use bold weight 700. Scrollbars and keyboard focus use the existing app tokens. Wider diagrams scroll locally and remain keyboard-focusable. The login flowchart pilot and its highlighting are removed; the Result panel retains its ordered Decision path.

### Latin Honors Evaluator

Nested If…Else Example 2 uses one General weighted average (GWA) field, Evaluate honors and canonical Clear. Valid decimal GWA is 1.00–5.00. Nested checks use upper limits 1.25, 1.50 and 1.75 to display Summa Cum Laude, Magna Cum Laude, Cum Laude or No Latin Honors. The result detail states Based on GWA only and preserves the entered numeric precision. The shared white decision-tree disclosure illustrates each nested threshold; only Yes/No labels are bold. Existing Input/Result panels, navigation, validation, focus and responsive behavior are reused.

### Focused examples and desktop credit

All category entry links on the desktop, Start, taskbar, welcome and search open that category's Example 1 directly. Legacy category-only URLs redirect to the canonical Example 1 route. The category intro screen and About page are removed, along with their links and styles. Reopening the active category still restores its current example, including after minimize. Start and all four category shortcuts remain visible on narrow phones.

All 20 examples omit the bottom app statusbar, category description, example subtitle and static input helper paragraphs. Titles lead directly into the shared Input/Result grid with a 16px gap. Input labels, placeholders, constraints, validation, results, formulas and teaching disclosures remain functional; live game/attempt state and photo feedback remain available. Removed helper IDs are removed from aria-describedby while error associations remain.

The desktop welcome description reads “Developed by: Arlan Martin N. Jalop” and “Section: SBIT3C” on separate lines, using the existing welcome typography. The taskbar brand returns to the desktop. These explicit user refinements supersede earlier references to About, category overview screens and example helper copy in this document.

### Category navigation beside the workspace

The category icon and title sit at the top of the left navigation column, directly below the shared brand titlebar. The example title starts at the top of the adjacent workspace, with the Input/Result panels immediately below it. The sidebar has no visible Examples caption. All 20 examples share this shell. Category headings use 20px, weight 600, a 30px icon and compact 12px padding; long category names wrap within the sidebar. The sidebar links scroll independently while Back to desktop stays at the bottom when space permits. On phones the category heading and horizontal example links stack above the workspace. This user-approved arrangement supersedes the earlier full-width category header layout.

The entire category navigation column, including its icon/title header, uses the shared Sidebar background. This consistent pale-blue surface distinguishes navigation from the Surface-colored example workspace; the selected example retains its stronger existing blue fill. The same grouping applies on desktop and phones.

### Replacement 2D array examples

Arrays Example 4 is Multiplication Table: two vertical dimension inputs and a result table with factor headers. Large products stay on one line and scroll locally in a keyboard-focusable region. Example 5 is House Member Finder: four vertical comma-separated house rosters, then Member to find and Find house/Clear. Roster textareas use a compact 64px minimum height and remain resizable; matching houses use the existing result heading and table. Both reuse the shared light Input/Result panels, error summary, navigation, mobile stacking and focus behavior without new colors or animation. These replace Number Search in a Grid and Group Member Organizer.
