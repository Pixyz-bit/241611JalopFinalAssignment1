# C# Programming Showcase

A macOS-inspired ASP.NET Core MVC template with four concept folders, five empty example slots per folder, a Finder-style window, search and a dock.

## Run
Install the .NET 8 SDK (or a newer compatible SDK with the .NET 8 runtime), open this folder in Visual Studio or a terminal, and run:

```powershell
dotnet build 241611JalopFinalAssignment1.sln
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http
```

Open http://localhost:5196. No npm install or database setup is needed. The UI's font, wallpaper and icons are local assets.

## Navigation regression checks
With Node.js available, run `node --test tests/navigation.test.cjs` from the solution root. These optional development tests need no npm packages; the application itself still runs with .NET only.

## Project guidance
- [PRODUCT.md](PRODUCT.md): scope and confirmed decisions.
- [DESIGN.md](DESIGN.md): visual rules and reusable tokens.
- [AGENTS.md](AGENTS.md): coding-agent instructions.
- [.agents/skills/showcase-ui/SKILL.md](.agents/skills/showcase-ui/SKILL.md): project skill and installed design-skill coordination.
- [docs/architecture.md](docs/architecture.md): folder structure, routes and adding examples.
- [docs/assets.md](docs/assets.md): wallpaper provenance and font licensing.
- [docs/validation.md](docs/validation.md): verification results.

Input/Output, Nested If…Else, Loops and Arrays each have Example 01–05. The example programming logic is intentionally left to the owner.
