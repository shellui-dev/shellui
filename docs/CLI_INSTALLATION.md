# ShellUI CLI Installation

This guide covers the current source and the packages that are currently published.

## Version guide

| Context | Version |
|---|---|
| Current source | `0.4.0-alpha.1`, targeting .NET 10 |
| Published stable package | `0.2.1` |
| Published prerelease package | `0.3.0-rc.1` |
| Tailwind used by current source | `4.3.2` |

The current source is not available as a `0.4.0-alpha.1` NuGet tool. A plain tool install resolves the published stable `0.2.1`; the prerelease must be selected explicitly. The published `0.3.0-rc.1` tool targets .NET 9, while the current source targets .NET 10.

## Global installation

A global tool is available as `shellui`:

```bash
dotnet tool install -g ShellUI.CLI
shellui --version
```

To pin the published stable version:

```bash
dotnet tool install -g ShellUI.CLI --version 0.2.1
shellui --version
```

To install the published prerelease:

```bash
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.1 --prerelease
shellui --version
```

If a global tool is already installed, use `dotnet tool update -g ShellUI.CLI` for the stable channel or specify the published prerelease version with `--version 0.3.0-rc.1 --prerelease`.

## Local installation

A local tool is recorded in the repository and invoked as `dotnet shellui` after the manifest exists.

```bash
dotnet new tool-manifest
dotnet tool install ShellUI.CLI --version 0.2.1
dotnet shellui --version
```

The manifest is `.config/dotnet-tools.json` and uses the installed package version:

```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "shellui.cli": {
      "version": "0.2.1",
      "commands": ["shellui"]
    }
  }
}
```

To select the prerelease for a local tool, install `ShellUI.CLI` with `--version 0.3.0-rc.1 --prerelease` and commit the resulting manifest. Commit the manifest so every developer and CI job uses the same tool version.

## Global or local?

| Aspect | Global | Local manifest |
|---|---|---|
| Command | `shellui` | `dotnet shellui` |
| Version selection | User profile install | `.config/dotnet-tools.json` |
| Team consistency | Each developer chooses | Manifest and restore |
| CI setup | Install the tool in each job | `dotnet tool restore` |
| Best fit | Personal projects and quick trials | Teams and reproducible builds |

## Working with the current source

The repository source is `0.4.0-alpha.1` and targets `net10.0`. Build it from the repository with the .NET 10 SDK:

```bash
dotnet --version
dotnet build ShellUI.slnx -c Release
```

To install the source build as a local tool package, generate the precompiled bundle first when packaging `ShellUI.Components`, then pack and install the CLI explicitly:

```bash
bash scripts/rebuild-precompiled-css.sh
dotnet pack ShellUI.slnx --configuration Release
dotnet tool install -g ShellUI.CLI --add-source "./src/ShellUI.CLI/bin/Release" --version 0.4.0-alpha.1
```

Do not use `0.4.0-alpha.1` as a NuGet package version until it is published. The published `0.2.1` and `0.3.0-rc.1` packages are separate installations.

## Updating and removing tools

```bash
dotnet tool update -g ShellUI.CLI
dotnet tool uninstall -g ShellUI.CLI
```

For a local tool, run these commands from the directory containing the manifest:

```bash
dotnet tool update ShellUI.CLI
dotnet tool uninstall ShellUI.CLI
dotnet tool restore
```

## CI/CD

### Global tool

The following workflow uses the published stable package and .NET 10:

```yaml
name: shellui
on: [push]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 10.0.x
      - run: dotnet tool install -g ShellUI.CLI --version 0.2.1
      - run: dotnet new blazor -n App
      - working-directory: App
        run: shellui init --yes --tailwind standalone
      - working-directory: App
        run: shellui add button input card
      - working-directory: App
        run: dotnet build
```

### Local manifest

Commit `.config/dotnet-tools.json`, then restore it in CI:

```yaml
name: shellui
on: [push]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 10.0.x
      - run: dotnet tool restore
      - run: dotnet new blazor -n App
      - working-directory: App
        run: dotnet shellui init --yes --tailwind standalone
      - working-directory: App
        run: dotnet shellui add button input card
      - working-directory: App
        run: dotnet build
```

## Troubleshooting

### `shellui` is not recognized

Check the global tools directory on `PATH`.

PowerShell:

```powershell
$env:PATH += ";$env:USERPROFILE\.dotnet\tools"
```

macOS/Linux:

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
```

Restart the terminal after changing the environment.

### `dotnet shellui` is not recognized

Run `dotnet tool restore` from the directory containing `.config/dotnet-tools.json`. A local tool is not available until the manifest has been created and the tool has been restored.

### The installed version is not the expected version

```bash
dotnet tool list -g
shellui --version
```

A plain install selects stable `0.2.1`. Use an explicit `--version 0.3.0-rc.1 --prerelease` when the published prerelease is required.

## Related documentation

- [CLI syntax](CLI_SYNTAX.md)
- [Quick start](QUICKSTART.md)
- [FAQ](FAQ.md)
- [Versioning strategy](../VERSIONING_STRATEGY.md)
- [GitHub repository](https://github.com/shellui-dev/shellui)
- [CLI package](https://www.nuget.org/packages/ShellUI.CLI)
- [Component package](https://www.nuget.org/packages/ShellUI.Components)
- [MIT license](../LICENSE.txt)
