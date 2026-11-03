# Project instructions

Read PRODUCT.md and DESIGN.md before UI changes, then .agents/skills/showcase-ui/SKILL.md. User instructions and the approved Windows 11 inspired direction on this branch override generic skill presets.

## Standing workflow requirements

- Always use the skills related to the current task. For interface work, follow the showcase-ui skill and its applicable design-skill dependencies; do not invoke unrelated skills merely because they are installed.
- Commit each meaningful completed progress step to Git with a descriptive message. Keep implementation, follow-up changes and verification/documentation commits reviewable. Do not push unless requested, and do not include unrelated user changes.
- Keep every change consistent with the existing approved design: reuse the shared layout, components, typography, colors, spacing and interaction patterns. Treat user references as guidance for the requested element; preserve the surrounding design unless the user explicitly changes it.
- Rebuild and rerun the app after code changes, complete the relevant checks, and leave the latest version available locally for review.

- Preserve ASP.NET Core MVC, net8.0, Razor, CSS and plain JavaScript.
- Controllers handle routes; Models holds presentation data; Views holds Razor pages/partials; wwwroot holds public assets.
- Preserve the four categories and five example slots each. The user authors example logic.
- Canonical links must work without JavaScript. Enhance, rather than replace, navigation.
- Detailed documentation belongs in docs/. Project skills belong in .agents/skills/<name>/SKILL.md; do not duplicate globally installed skills.
- For new or changed code, add brief beginner-friendly comments beside meaningful lines explaining their purpose or unfamiliar syntax. Use the language's comment syntax; place a note immediately above when an inline comment would impair readability. Keep comments accurate as code changes, and put longer explanations in docs/architecture.md.
- Validate build, routes, browser Back/Forward, controls, desktop/mobile, keyboard and reduced motion.
- Update DESIGN.md deliberately when shared visual decisions change; do not canonize accidental drift.

## Commands from solution root
```
dotnet build 241611JalopFinalAssignment1.sln
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http
node --test tests/navigation.test.cjs
```
