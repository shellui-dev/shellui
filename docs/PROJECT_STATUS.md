# ShellUI Project Status

## Overview

| Area | Current fact |
|---|---|
| Version | See `Directory.Build.props` |
| Framework | .NET 10 (`net10.0`) |
| Tailwind CSS | `4.3.2` |
| Solution | `ShellUI.slnx` with six projects |
| Demo | `NET10/BlazorInteractiveServer/BlazorInteractiveServer.csproj`, outside the solution |
| Packable artifacts | `ShellUI.CLI` and `ShellUI.Components` only |

An unversioned install only selects stable releases, so pin prereleases with `--version`.

## Implemented

### Solution and distribution

- `ShellUI.slnx` contains the CLI, Components, Core, Templates, SafelistGenerator, and Tests projects.
- `ShellUI.CLI` references Core and Templates and is packed as a .NET global tool.
- `ShellUI.Components` is an independent packable Razor class library; it has no project reference to the CLI, Core, or Templates.
- Core, Templates, SafelistGenerator, and Tests are non-packable repository projects.
- The demo is a separate consumer application and is not built as part of `ShellUI.slnx`.

### CLI

The implemented commands are:

```text
init
add
list
remove
update
theme init
theme apply
theme update
```

`init` configures a Blazor project, Tailwind, host bootstrap assets, `shellui.json`, and MSBuild integration. `add` installs direct targets and their dependencies. `list` exposes the public direct targets. `remove` and `update` operate on installed source. The theme commands fetch themes from tweakcn, update a managed CSS region or an override file, and record `shellui.theme.lock` for `theme update`.

### Registry and components

`ComponentRegistry` has 176 entries:

- 76 direct targets with `IsAvailable = true`.
- 100 hidden entries with `IsAvailable = false`, generally installed as dependencies or assets.

The direct-target count is the number shown by the normal public list. The hidden count is not a second public library; it represents the sub-components, variants, models, services, and assets needed to make the direct targets work.

### Tailwind and theming

- The current Tailwind constant is `4.3.2`.
- Both standalone and npm Tailwind workflows are implemented.
- The generated MSBuild integration rebuilds the configured CSS during a consumer build.
- `theme init`, `theme apply`, and `theme update` are current commands, not planned features.
- The CLI's npm setup currently invokes npm through `cmd`; standalone mode is the portable path for non-Windows contributors.

### Tests and CI

`ShellUI.Tests` uses xUnit, `Microsoft.NET.Test.Sdk`, Roslyn through `Microsoft.CodeAnalysis.CSharp`, and `coverlet.collector`. The current suite covers registry/dependency checks, template parsing, template synchronization, initialization, chart assets, safelist drift, and related CLI behavior.

There is currently no bUnit, FluentAssertions, rendered-component, or browser-automation suite. Accessibility behavior should be reviewed in the relevant component and consumer context, but the project does not claim an automated accessibility-conformance level.

CI restores `ShellUI.slnx`, regenerates the precompiled CSS bundle, builds the solution, runs tests, and performs CLI scaffolding and NuGet-only smoke checks. The demo remains outside that solution workflow.

## Current Boundaries

- ShellUI is prerelease software and its APIs, templates, and generated output may change.
- Only `ShellUI.CLI` and `ShellUI.Components` are packable in the current project configuration.
- `ShellUI.Core` and `ShellUI.Templates` are internal; they are not consumer installation targets.
- No stable release date, adoption target, or guaranteed delivery schedule is stated here.
- The current test stack does not provide a browser or rendered-component test harness.

## Unscheduled Proposals

The following are discussion items, not current features or commitments:

- Additional components and design variants beyond the current registry.
- A larger documentation site, visual preview, or authoring experience.
- Browser, end-to-end, and accessibility-focused test automation.
- Further performance, packaging, migration, and distribution work.

Proposals should be filed as issues with their scope and trade-offs. They must not be copied into current-status claims until implemented and verified.

## Development Snapshot

From the repository root:

```bash
dotnet restore ShellUI.slnx
dotnet build ShellUI.slnx --configuration Release
dotnet test ShellUI.slnx --no-restore --no-build --configuration Release --verbosity normal
dotnet pack ShellUI.slnx --no-build --configuration Release
```

Run the safelist generator when component CSS, variant helpers, or safelist inputs change, then build the bundle:

```bash
dotnet run --project tools/ShellUI.SafelistGenerator -- src/ShellUI.Components/Components src/ShellUI.Components/wwwroot/shellui-classes.txt src/ShellUI.Components/build/ShellUI.Components.targets
bash scripts/rebuild-precompiled-css.sh
```

The CSS script consumes the generated safelist; it does not regenerate the safelist itself.

The pack command produces the two packable artifacts, `ShellUI.CLI` and `ShellUI.Components`; it does not turn Core or Templates into consumer packages.

Run the demo explicitly because it is outside the solution:

```bash
dotnet run --project NET10/BlazorInteractiveServer/BlazorInteractiveServer.csproj
```

## Source and Published Usage

For a published tool or package, select the version available in the intended channel and pin it:

```bash
dotnet tool install -g ShellUI.CLI --version <published-version>
dotnet add package ShellUI.Components --version <published-version> --prerelease
```

To test a checkout, build the CLI locally and install that package with the version from `Directory.Build.props`:

```bash
dotnet pack ShellUI.slnx --configuration Release
dotnet tool install -g ShellUI.CLI --add-source "./src/ShellUI.CLI/bin/Release" --version <version>
```

## Documentation and History

The [release notes](RELEASE_NOTES.md) have one section per release.

- [Project overview](../README.md)
- [Architecture](ARCHITECTURE.md)
- [Contributing](CONTRIBUTING.md)
- [Quick start](QUICKSTART.md)
- [CLI syntax](CLI_SYNTAX.md)
- [CLI installation](CLI_INSTALLATION.md)
- [Component dependencies](COMPONENT_DEPENDENCIES.md)
- [Tailwind setup](tailwind-setup.md)
- [Versioning strategy](../VERSIONING_STRATEGY.md)
- [GitHub issues](https://github.com/shellui-dev/shellui/issues)
