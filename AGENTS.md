# Project instructions

Read PRODUCT.md and DESIGN.md before UI changes, then .agents/skills/showcase-ui/SKILL.md. User instructions and the approved Windows 11 inspired direction on this branch override generic skill presets.

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
