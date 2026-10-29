# C# Programming Showcase

A Windows 11 inspired ASP.NET Core MVC template with four concept folders, five unimplemented example slots per folder, a light Explorer window, Start navigation, search and a persistent taskbar. The `windows-design` branch contains this design; `main` retains the earlier macOS version.

## Run
Install the .NET 8 SDK (or a newer compatible SDK with the .NET 8 runtime), open this folder in Visual Studio or a terminal, and run:

```powershell
dotnet build 241611JalopFinalAssignment1.sln
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http
```

Open http://localhost:5196. No npm install or database setup is needed. The UI's font, wallpaper and icons are local assets. Stop a running app with Ctrl+C in its terminal (or Stop Debugging in Visual Studio) before building again if Windows reports locked binaries.

## Compare the designs
Stop the running app before switching branches. Run `git status` first; commit or stash your own pending changes so Git can switch safely. From the solution root:

```powershell
git switch main
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http
```

View the macOS version, stop the app, then switch back and run the same command:

```powershell
git switch windows-design
dotnet run --project 241611JalopFinalAssignment1 --launch-profile http
```

These commands select a local version; they do not merge or push either branch.

## Navigation regression checks
With Node.js available, run `node --test tests/navigation.test.cjs` from the solution root. These development checks need no npm packages; the application itself runs with .NET only.

## Project guidance
- [PRODUCT.md](PRODUCT.md): scope and confirmed decisions.
- [DESIGN.md](DESIGN.md): visual rules and reusable tokens.
- [AGENTS.md](AGENTS.md): coding-agent instructions.
- [.agents/skills/showcase-ui/SKILL.md](.agents/skills/showcase-ui/SKILL.md): project skill and installed design-skill coordination.
- [docs/architecture.md](docs/architecture.md): folder structure, routes and adding examples.
- [docs/assets.md](docs/assets.md): wallpaper provenance and font licensing.
- [docs/validation.md](docs/validation.md): verification results and limits.

Input/Output, Nested If…Else, Loops and Arrays each have Example 01–05. The example programming logic is intentionally left to the owner.
