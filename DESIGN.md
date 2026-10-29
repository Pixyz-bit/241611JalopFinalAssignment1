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

The approved Windows 11 inspired world pairs an original cobalt bloom wallpaper with colored application badges, a full-width translucent taskbar and a light Windows application workspace. Desktop familiarity invites exploration; compact application typography keeps the examples easy to navigate.

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

Category headings use 28px type (25px on phones), weight 600 and line height 1.25. The header uses 16px vertical/24px horizontal padding and a 44px icon. Its description can span up to 100ch, avoiding unnecessary wrapping on desktops; template text remains bounded at 60ch. The example workspace uses 24px padding, reduced on phones. The taskbar clock uses tabular numerals and two lines.

**The Two Registers Rule.** Keep expressive welcome and About headings separate from compact application navigation and example detail.

## Layout

Desktop shortcuts form a left rail (108px wide); the welcome begins at 20% of viewport width. The taskbar spans the full bottom edge and uses three columns to center Start and category shortcuts independently of the brand and clock. Its desktop height is defined by the frontmatter taskbar component.

The ordinary application window reserves the shortcut rail and taskbar: top max(24px, 6vh), left max(146px, (100vw - 1150px) / 2), right max(32px, (100vw - 1280px) / 2), bottom taskbar height + max(28px, 5vh). Its sidebar/main columns are 218px / flexible, becoming 194px / flexible at 1100px. The main content scrolls within the bounded window. The brand titlebar remains the top row. A full-width category icon, heading and description form the next row, above both the example sidebar and the scrollable workspace. The sidebar selects Example 1 through Example 5.

At 720px and below, taskbar height becomes 64px plus the bottom safe-area inset, the tray/brand disappear, home app shortcuts form four columns, and the application sits 10px from the sides and 12px above the taskbar. The example sidebar becomes a row of five numbered targets; About retains a two-column category menu. Caption, taskbar, search and category controls use at least 44px mobile targets. At 360px and below, About stays available in Start while its taskbar shortcut hides to preserve control width. At desktop heights up to 760px the shortcut rail uses compact icons and hides counts while retaining About. The rail is bounded above the taskbar and scrolls when necessary. Rules at 500px further reduce application spacing.

**The Persistent Taskbar Rule.** The maximized application fills the viewport above the taskbar; the taskbar remains visible and never covers application content.

## Elevation & Depth

The bloom carries the sculptural material as a raster asset. Desktop wallpaper uses centered `contain` sizing over the cobalt ground so the complete landscape composition is visible without zoom cropping or distortion. Portrait layouts at 720px and below use `cover` with the focal position at 65% horizontally to keep the bloom visible. The application, Start and search share one diffuse window shadow (0 20px 70px -14px rgb(3 17 43 / .48)). Taskbar glass uses a pale translucent fill and backdrop blur (24px); working and reading surfaces remain light and solid. Small app-icon, welcome-action and tooltip shadows support separation without a second shadow system.

Window entry lasts 220ms with the shared easing cubic-bezier(.16, 1, .3, 1), moving from a small downward offset and slight scale. Desktop hover changes are brief (140ms); welcome fades use 180ms. The reduced-motion media rule disables animation, transitions and smooth scrolling.

**The Material Boundary Rule.** Keep translucency on desktop chrome and preserve solid reading surfaces inside the application, Start and search.

## Shapes

Compact controls and sidebar links use small corners; windows and dialogs use the window radius. Search fields keep compact control corners; the taskbar has no search bar. The four approved category icons use original transparent PNG artwork inspired by layered Windows Fluent app icons. Each image retains its own silhouette, shading and transparent margins without an added tile or CSS shadow. Containers are 58px on the desktop, 46px in Start, 28px in the taskbar and 36px in search; responsive rules adjust their size. About retains its existing code badge. Maximized application windows have square corners.

## Components

### Application icons and welcome

Desktop app shortcuts are canonical category anchors with visible names and counts. The shared _ConceptIcon partial loads the category PNG by its canonical slug, with a versioned asset URL. The approved terminal, branching paths, repeat arrows and data grid use simple layered forms and restrained shading. Exact generation prompts and asset paths are recorded in docs/icon-preview-prompts.json. Hover and the active category use a subtle pale wash and border. Light focus outlines remain visible on the dark wallpaper. The welcome action opens Input/Output directly.

### Taskbar and Start

The taskbar groups Start, four categories and About, with active blue indicators and hover/keyboard tooltips. The clock displays local time and date when enhanced by JavaScript. Start uses native details/summary so its canonical links remain usable without JavaScript; enhancement dismisses it on outside click, Escape or link selection. Escape returns focus to Start.

### Application shell and example navigation

A brand titlebar and right-aligned minimize/maximize/close controls remain at the very top. A full-width category header sits beneath it, followed by example navigation and one main content area. The sidebar contains Example 1 through Example 5 and About/Desktop utilities; it has no search field. About retains category links because it has no current category. The category header spans both columns and stays outside the scrolling example workspace. The selected example uses a pale fill and blue leading mark. The workspace groups honest Description, Interface and Output sections without file-browser chrome or filename suffixes. Minimize retains the current example, and its active taskbar shortcut restores that exact view. Close returns to the desktop. Maximize retains access to the taskbar.

### Search

Native dialog supplies modal focus containment. A visible label, blue focus treatment, local filtering, canonical result links, explicit close control and empty state support the task. Ctrl/Cmd+K opens it; Escape closes it. Dismissal returns focus to Start. Search remains available in Start and Ctrl/Cmd+K.

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
- **Don't** replace the authored bloom with CSS sculpture or use purchased operating-system assets.
- **Don't** cover the persistent taskbar when maximizing the application.
- **Don't** replace honest empty examples with invented execution or persistence.
