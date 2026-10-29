# Asset sources

## Active Windows wallpaper
`241611JalopFinalAssignment1/wwwroot/images/windows-bloom.webp` is an original dark cobalt sculptural bloom generated with the built-in image-generation tool for the user-approved Windows 11 inspired direction. It is a standalone wallpaper, not a UI screenshot, operating-system asset or approved pixel comp. The raster supplies its folded, flowing material; CSS supplies a left-side contrast wash and layout.

The source output was 1672×941, encoded as WebP at quality 90 (about 92 KB). Its exact generation prompt and creation timestamp are preserved in the adjacent `windows-bloom.webp.json`; the prompt is also embedded in the raster. The record describes broad cobalt silk ribbons, a bloom occupying the right/lower area, quiet navy space for text, and no text, logos, icons or UI. Keep this provenance with the asset when replacing or re-encoding it.

## Historical wallpaper
`mountain-wallpaper.webp` and its adjacent provenance JSON are retained historical source from the macOS design. The Windows stylesheet does not reference it. It was also generated with the built-in image-generation tool. The latest provenance scan found two raster assets and zero missing prompt records.

## Font
Geist variable font, self-hosted at `wwwroot/fonts/geist-latin.ttf`. Source: the official Google Fonts `ofl/geist/Geist[wght].ttf`, weights 100–900. License: `wwwroot/fonts/OFL.txt` (SIL Open Font License, supplied by the same repository). Segoe UI and sans-serif are fallbacks; the chosen local font remains Geist.

## Icons
The yellow folder and favicon are project-authored SVG assets in `wwwroot/images/`. The folder has a tab, paper insert and warm front gradient. Interface symbols are project-authored SVG in `Views/Shared/_Icons.cshtml`, with 24-unit view boxes; CSS sets default strokes to 1.6 units and varies individual icons for legibility. No purchased or proprietary operating-system application artwork is shipped.
