<p align="center">
  <picture>
    <source media="(prefers-color-scheme: dark)" srcset="images/Shellui-dark.png">
    <source media="(prefers-color-scheme: light)" srcset="images/Shellui-light.png">
    <img alt="ShellUI Logo" src="images/Shellui-light.png" width="120">
  </picture>
</p>

<h1 align="center">ShellUI</h1>

<p align="center">
  A CLI-first Blazor component library inspired by <a href="https://ui.shadcn.com/">shadcn/ui</a>.<br/>
  Copy component source into your project or reference the components package.
</p>

<p align="center">
  <a href="https://github.com/shellui-dev/shellui"><img src="https://img.shields.io/github/stars/shellui-dev/shellui?style=flat-square" alt="GitHub stars"></a>
  <a href="https://www.nuget.org/packages/ShellUI.Components"><img src="https://img.shields.io/nuget/v/ShellUI.Components?style=flat-square&logo=nuget&color=004880" alt="ShellUI.Components on NuGet"></a>
  <a href="https://www.nuget.org/packages/ShellUI.CLI"><img src="https://img.shields.io/nuget/v/ShellUI.CLI?style=flat-square&logo=nuget&label=CLI&color=004880" alt="ShellUI.CLI on NuGet"></a>
  <a href="LICENSE.txt"><img src="https://img.shields.io/badge/license-MIT-blue?style=flat-square" alt="MIT license"></a>
</p>

## Status

| Channel | Version | Notes |
|---|---|---|
| Repository source | `0.4.0-alpha.1` | Current source; targets .NET 10 and Tailwind CSS `4.3.2` |
| Latest published stable packages | `0.2.1` | Stable CLI and components packages on NuGet |
| Latest published prerelease packages | `0.3.0-rc.1` | Published release candidate; older than this source checkout |

The repository is ahead of NuGet. Features described as **current source** require a local build or package and are not present in the published `0.2.1` or `0.3.0-rc.1` packages. The published prerelease targets .NET 9; the current source targets .NET 10.

ShellUI is alpha software. Validate it in your target Blazor and hosting environments before relying on it.

## Current source capabilities

- The CLI commands are `init`, `add`, `list`, `remove`, and `update`, plus `theme init`, `theme apply`, and `theme update`.
- The component registry has **173 entries**: **73 direct install targets** and **100 hidden dependency entries**. `list` shows direct targets; `add` resolves hidden dependencies.
- Current additions include `typed-select`, `command-palette`, `data-picker`, `multi-select`, and `tag-input`.
- `ShellUI.Components` supports a release-generated precompiled CSS bundle and a generated safelist for existing Tailwind builds.
- The CLI can install source with Tailwind's standalone executable or an npm-based build. The current Tailwind baseline is `4.3.2`.
- The repository and demo have migrated to .NET 10. The demo is `NET10/BlazorInteractiveServer`.

## Requirements

- .NET 10 SDK for the current source
- A .NET 10 Blazor project
- Tailwind CSS `4.3.2` via either:
  - the standalone CLI, which does not require Node.js; or
  - npm, which requires Node.js and npm

## CLI quick start

The published global tool is named `shellui`:

```bash
dotnet tool install --global ShellUI.CLI --version 0.2.1
shellui --help
```

To select the published prerelease instead:

```bash
dotnet tool install --global ShellUI.CLI --version 0.3.0-rc.1
```

A local .NET tool is invoked as `dotnet shellui`:

```bash
dotnet new tool-manifest
dotnet tool install --local ShellUI.CLI --version 0.3.0-rc.1
dotnet shellui --help
```

After building or installing the current source tool, its workflow is:

```bash
shellui init
shellui add button card dialog
shellui list
shellui remove button
shellui update
```

Use the same commands with a `dotnet ` prefix when the tool is installed locally. Do not use `dotnet shellui` for a global installation.

New CLI sidebar installs use the host-loaded `shellui.js`; the legacy `sidebar-js` module remains available only for older generated providers.

### Command reference

| Command | Purpose |
|---|---|
| `init` | Set up ShellUI, Tailwind, host wiring, and the generated project structure |
| `add <components...>` | Install one or more direct targets and their hidden dependencies |
| `list` | List direct targets, installed targets, or targets still available |
| `remove <components...>` | Remove named installed components |
| `update [components...]` | Reinstall named or all installed templates |
| `theme init <url-or-id>` | Initialize a project and apply a tweakcn theme |
| `theme apply <url-or-id>` | Apply a theme to `wwwroot/input.css` or emit override CSS |
| `theme update` | Re-fetch the source recorded in `shellui.theme.lock` |

The five current source targets can be installed together:

```bash
shellui add typed-select command-palette data-picker multi-select tag-input
```

## Components package

Published versions can be installed explicitly:

```bash
dotnet add package ShellUI.Components --version 0.2.1
```

To select the published prerelease instead:

```bash
dotnet add package ShellUI.Components --version 0.3.0-rc.1 --prerelease
```

`0.4.0-alpha.1` is not currently published. To consume that source version, pack `src/ShellUI.Components/ShellUI.Components.csproj` and use it from a local package feed. `ShellUI.Core` and `ShellUI.Templates` are internal projects and must not be installed by consumers.

The current-source package supports two CSS workflows.

### Precompiled bundle

Release packaging generates `shellui-all.css`, so the consuming project does not need its own Tailwind build:

```razor
<link href="_content/ShellUI.Components/shellui-all.css" rel="stylesheet" />
```

```razor
@using ShellUI.Components
```

The generated bundle is not checked into the repository. Before packing the current source locally, run `bash scripts/rebuild-precompiled-css.sh`; the release pipeline runs the same script automatically.

### Tailwind safelist

For a project that already builds Tailwind, the package build target writes `wwwroot/shellui-classes.txt`. Reference it from the input stylesheet:

```css
@import "tailwindcss";
@source "./shellui-classes.txt";
```

The safelist lets Tailwind emit the classes used by ShellUI components even when the component assembly is supplied as a compiled package.

## Theme commands

The current-source CLI can fetch public themes from [tweakcn](https://tweakcn.com):

```bash
shellui theme init https://tweakcn.com/themes/THEME_ID
shellui theme apply https://tweakcn.com/themes/THEME_ID
shellui theme apply https://tweakcn.com/themes/THEME_ID --emit-override wwwroot/theme.css
shellui theme update
```

`theme apply` updates the sentinel-marked theme region in `wwwroot/input.css` and preserves content outside that region. `--emit-override` writes standalone theme variables for use after a precompiled stylesheet. The source URL and content hash are recorded in `shellui.theme.lock` so `theme update` can fetch the same theme again.

## Repository development

The solution is `ShellUI.slnx`:

```bash
dotnet restore ShellUI.slnx
dotnet build ShellUI.slnx
dotnet test ShellUI.slnx
```

The .NET 10 Blazor Interactive Server demo is outside that solution:

```bash
dotnet run --project NET10/BlazorInteractiveServer/BlazorInteractiveServer.csproj
```

`Directory.Build.props` is the source of the centralized version, currently `0.4.0-alpha.1`. Only `ShellUI.CLI` and `ShellUI.Components` are packable; `ShellUI.Core`, `ShellUI.Templates`, the test project, and the safelist generator are not published packages.

ShellUI uses semantic Blazor and Tailwind patterns, but accessibility depends on the component, configuration, and host application. Test keyboard use, focus behavior, contrast, and assistive-technology output in the consuming application.

## Documentation

- [Quick start](docs/QUICKSTART.md)
- [CLI syntax](docs/CLI_SYNTAX.md)
- [Tailwind setup](docs/tailwind-setup.md)
- [Contributing](docs/CONTRIBUTING.md)
- [Release notes](docs/RELEASE_NOTES.md)

Release notes contain historical release records plus a separate current-source section for `0.4.0-alpha.1`; older version sections describe only the release they document.

## License

[MIT](LICENSE.txt)

## Acknowledgments

- Inspired by [shadcn/ui](https://ui.shadcn.com/) for the CLI-first approach
- Forked from [Sysinfocus simple/ui](https://github.com/Sysinfocus/simple-ui) by [@sysinfocus](https://github.com/Sysinfocus)
