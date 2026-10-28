---
name: C# Programming Showcase
description: A familiar desktop for exploring C# fundamentals.
colors:
  primary: "#1768be"
  primary-soft: "#deebfb"
  ink: "#24354a"
  muted: "#566b83"
  paper: "#f8fafc"
  surface: "#eef3f9"
  line: "#dce4ee"
  desktop-text: "#f0f6ff"
  sidebar: "#eaf0f7"
  list: "#f2f6fa"
  sidebar-selected: "#cddff5"
  sidebar-selected-ink: "#184d83"
  row-selected: "#dceafb"
  status: "#eaf0f6"
  focus: "#1979df"
typography:
  display-emphasis:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "clamp(68px, 8.3vw, 120px)"
    fontWeight: 350
    lineHeight: 1.2
    letterSpacing: "-0.04em"
  headline:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "44px"
    fontWeight: 500
    lineHeight: 1.12
    letterSpacing: "-0.035em"
  toolbar:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "14px"
    fontWeight: 650
    letterSpacing: "-0.02em"
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
  display:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "clamp(32px, 4.3vw, 64px)"
    fontWeight: 500
    lineHeight: 1.15
    letterSpacing: "-0.04em"
  body:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "13px"
    fontWeight: 400
    lineHeight: 1.85
  title:
    fontFamily: "Geist, Segoe UI, sans-serif"
    fontSize: "30px"
    fontWeight: 600
    lineHeight: 1.3
    letterSpacing: "-0.035em"
rounded:
  small: "5px"
  row: "7px"
  result: "8px"
  folder: "9px"
  tile-mobile: "11px"
  tile: "13px"
  search: "16px"
  dock-mobile: "18px"
  window: "14px"
  dock: "20px"
  control: "6px"
spacing:
  xs: "4px"
  compact: "6px"
  sm: "8px"
  control-gap: "10px"
  inset: "12px"
  md: "16px"
  panel: "20px"
  lg: "24px"
  xl: "32px"
components:
  primary-link:
    textColor: "{colors.primary}"
    typography: "{typography.label}"
    padding: "8px 0"
  plain-button:
    textColor: "{colors.muted}"
    rounded: "{rounded.small}"
    padding: "6px"
  sidebar-link:
    textColor: "{colors.ink}"
    rounded: "{rounded.control}"
    padding: "10px 9px"
  sidebar-link-selected:
    backgroundColor: "{colors.sidebar-selected}"
    textColor: "{colors.sidebar-selected-ink}"
    rounded: "{rounded.control}"
  example-row:
    rounded: "{rounded.row}"
    padding: "12px 9px"
  example-row-selected:
    backgroundColor: "{colors.row-selected}"
    rounded: "{rounded.row}"
  status-pill:
    backgroundColor: "{colors.status}"
    textColor: "{colors.muted}"
    rounded: "{rounded.small}"
    padding: "5px 8px"
  empty-interface:
    backgroundColor: "#f2f6fb"
    rounded: "{rounded.row}"
    padding: "17px 13px"
  search-input:
    textColor: "{colors.ink}"
    padding: "8px 2px"
  desktop-folder:
    textColor: "{colors.desktop-text}"
    rounded: "{rounded.folder}"
    padding: "8px 4px"
  dock:
    rounded: "{rounded.dock}"
    padding: "10px 13px 15px"
    height: "83px"
  finder-window:
    backgroundColor: "{colors.paper}"
    textColor: "{colors.ink}"
    rounded: "{rounded.window}"
---

# Design System: C# Programming Showcase

## Overview
**Creative North Star: "A desktop for curious minds."**

Preserve the user's reference: a photographic blue mountain desktop, dimensional blue folders, a translucent menu bar, an expressive light welcome title and a glass dock. Opening a folder reveals one precise Finder-style workspace. Familiar desktop materials provide warmth; restrained application typography provides clarity.

The user's visual reference overrides generic skill bans against centered compositions, glass and dimensional icons. MVC is the required implementation, regardless of React/Tailwind defaults in supporting skills. This file owns the shared visual decisions; surface composition and modes live in docs/design-brief.md.

**Key Characteristics:**

- Cobalt mountain photography with readable light desktop text.
- Dimensional blue folders and softly tinted glass chrome.
- One opaque, precise Finder workspace with clear blue selection.
- Geist typography, compact navigation and visible keyboard focus.

## Colors
Cobalt and navy photography frame cool light application surfaces. `--blue` is the selection/action accent; `--ink`, `--muted`, `--paper`, `--surface` and `--line` are defined in wwwroot/css/site.css. `--desktop-text` is reserved for text over the wallpaper.

Dock tiles use fixed cyan, amber, violet and mint identities. Traffic lights use red, amber and green for familiar window actions; these are semantic exceptions to the primary blue accent. Sidebar, list and selected-state colors are separate observed cool tints. Supporting application text, including search placeholders and toolbar metadata, uses the muted token. Exact component gradients live in the sidecar.

**The Readable Desktop Rule.** Check light labels against the wallpaper beneath them; an average image color cannot establish text contrast.

## Typography
Self-hosted variable **Geist**, weights 100–900, from wwwroot/fonts/geist-latin.ttf; SIL Open Font License accompanies it. Fallback: Segoe UI, sans-serif. The main title uses 500 weight, with a 350-weight italic second line. Synthetic italic is explicitly allowed for that line because the shipped Geist font has no italic face.

The frontmatter records the reusable ramp. Example titles use the category title treatment at 29px; both become 26px on mobile. Toolbar titles become 12px. Navigation and metadata generally occupy the 10–13px range. Category copy is limited to 36ch and About copy to 55ch. Use sentence case and direct labels.

**The Two Registers Rule.** Keep expressive display typography on the desktop welcome and About headline; use the compact application ramp inside navigation and example detail.

## Layout
The desktop fills min-height:100dvh. A 42px top bar, left folder rail and bottom centered dock frame the welcome composition. On ordinary desktop viewports, the welcome block has equal 18.5% side insets and its center is aligned with the viewport center; its vertical offset accounts for the 42px menu bar. Keep the entire block centered, including its introduction, title, description and action. The Finder is a single 1040px-wide, 582px-high maximum workspace at ordinary desktop sizes. Its columns are 184px sidebar, 235px example list and flexible detail. At 1100px, the fixed columns reduce to 160px/200px.

At 760px and below, the top bar is 48px, desktop folders become a four-column grid, the Finder fills the available area, categories become horizontal navigation and all five example numbers share a compact strip. Content scrolls within the detail region. Mobile traffic-light controls keep small colored dots inside 44px targets. At 390px and below, gaps, labels and detail padding tighten. At 1600px and above, the Finder grows to 1100px by 620px.

An active maximized Finder occupies the viewport below the menu bar with a 12px margin on desktop and an 8px margin on mobile. Its fixed host uses the menu-bar height token (42px desktop, 48px mobile) and reserves no dock space. Hide the dock and desktop signature while the maximized window is visible; restore them when the window is restored, minimized or closed. Keep the content panes independently scrollable.

Desktop heights at or below 800px reduce folder spacing; at or below 690px, counts disappear and dock/window sizing becomes compact. The final landscape rule at height 500px or less and width at least 761px takes precedence: folders form a horizontal rail above the dock, the welcome simplifies, Finder columns become 140px/170px/flexible, and the window fills its bounded host with scrollable work panes. The spacing scale records recurring values rather than forcing every inset onto a rigid grid.

**The Clear Workspace Rule.** Ordinary windows reserve the dock's footprint. Maximized windows hide the dock and use the full workspace below the menu bar; desktop chrome must never cover detail or status content.

## Elevation & Depth
The wallpaper is a real raster asset with a darkening overlay. Glass belongs only to desktop chrome. The Finder is an opaque light workspace. Structural shadow: `0 24px 80px -16px rgb(3 22 49 / .5)`. Icons have restrained inset highlights and directional blue-tinted shadows. No unrelated neon glow or decorative blur.

Motion uses `cubic-bezier(.16,1,.3,1)`: 350ms window appearance, 250ms dock/folder movement, 200ms link/folder interaction and 180ms chrome/tooltip transitions. Animate transform and opacity. Reduced motion disables transitions, animations and dock/folder hover translation. Keep content visible without depending on entrance motion. Exact shadows and motion values live in .impeccable/design.json.

**The Material Boundary Rule.** Keep glass on desktop chrome; preserve the opaque light Finder as the reading and working surface.

## Shapes
Windows: 14px radius. Dock: 20px (18px mobile). Dock tiles: 13px (11px mobile). Navigation selections: 6–7px. Search panel: 16px. Blue folders use the single local folder.svg asset with a consistent concept emblem; no emoji substitutes.

## Components
- **Desktop folder:** one icon, clear concept name and five-example count; hover/selected surface, keyboard focus, functional category link.
- **Finder:** one instance with shared toolbar, sidebar, example list, detail and status bar. Close returns to desktop; minimize restores from the active dock icon; maximize toggles available workspace size.
- **Example template:** title, honest unimplemented status, description/interface/output regions. Never fake execution or present dummy mutations.
- **Search:** labeled modal search with visible results, empty state, Escape dismissal and native focus containment. Ctrl/Cmd+K is an optional shortcut, not the only way to open it.
- **Navigation:** real MVC anchors with progressive enhancement, active states and browser history. All meaningful actions have accessible labels and visible focus.
- **Tab switching:** keep the Finder mounted while changing its contents. Reuse visited views without a new request or opening animation; reserve the opening animation for showing a window from the desktop. Keep the current view visible while an uncached route loads.
- **Controls and selection:** blue text links underline on hover; quiet buttons gain a pale fill. Sidebar and example rows distinguish hover from selected blue tints. Global keyboard focus uses a 3px outline with 4px offset, adapted over wallpaper.
- **Progressive state:** the server renders the open-window class and inert welcome. JavaScript-only controls are hidden without JavaScript; canonical navigation and the close link remain usable.
- **Representative previews:** .impeccable/design.json contains self-contained HTML/CSS for the existing primitives, with inline SVG, interaction states and reduced-motion treatments where applicable.

## Do's and Don'ts
- Do read PRODUCT.md and this file before UI changes. Use the project skill at .agents/skills/showcase-ui/ to coordinate the installed design skills.
- Do preserve the reference's desktop identity and reuse shared Razor partials.
- Do inspect desktop, phone and short-height layouts, including keyboard and reduced-motion behavior.
- Don't import React libraries or add a frontend build pipeline to this MVC template.
- Don't copy the reference's personal identity, Apple logo or proprietary dock icons.
- Don't replace clearly unimplemented examples with invented programs.

Last updated: 2026-10-28 — source-derived token record and final responsive behavior reconciled after implementation.

2026-10-28 — centered the desktop welcome; maximized Finder fills the viewport below the menu bar with small responsive margins and temporarily hides the dock.

2026-10-28 — tab switches reuse the mounted Finder and cached views; the opening animation is reserved for opening a window from the desktop.
