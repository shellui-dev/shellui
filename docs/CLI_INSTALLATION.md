# ShellUI CLI Installation

## Version guide

| Context | Version |
|---|---|
| Latest prerelease (recommended) | `0.3.0-rc.2`, needs the .NET 10 runtime |
| Latest stable | `0.2.1` |
| Tailwind | `4.3.2` |

A plain tool install only selects stable releases, so pass `--version` for the prerelease. `0.3.0-rc.1` was the last release targeting .NET 9.

## Global installation

A global tool is available as `shellui`:

```bash
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.2
shellui --version
```

If a global tool is already installed, update it to the same version:

```bash
dotnet tool update -g ShellUI.CLI --version 0.3.0-rc.2
```

## Local installation

A local tool is recorded in the repository and invoked as `dotnet shellui` after the manifest exists.

```bash
dotnet new tool-manifest
dotnet tool install ShellUI.CLI --version 0.3.0-rc.2
dotnet shellui --version
```

The manifest is `.config/dotnet-tools.json` and uses the installed package version:

```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "shellui.cli": {
      "version": "0.3.0-rc.2",
      "commands": ["shellui"]
    }
  }
}
```

Commit the manifest so every developer and CI job uses the same tool version.

## Global or local?

| Aspect | Global | Local manifest |
|---|---|---|
| Command | `shellui` | `dotnet shellui` |
| Version selection | User profile install | `.config/dotnet-tools.json` |
| Team consistency | Each developer chooses | Manifest and restore |
| CI setup | Install the tool in each job | `dotnet tool restore` |
| Best fit | Personal projects and quick trials | Teams and reproducible builds |

## Building from source

The repository targets `net10.0`. Build it with the .NET 10 SDK:

```bash
dotnet --version
dotnet build ShellUI.slnx -c Release
```

To install the source build as a local tool package, generate the precompiled bundle first when packaging `ShellUI.Components`, then pack and install the CLI explicitly:

```bash
bash scripts/rebuild-precompiled-css.sh
dotnet pack ShellUI.slnx --configuration Release
dotnet tool install -g ShellUI.CLI --add-source "./src/ShellUI.CLI/bin/Release" --version <version>
```

Use the version from `Directory.Build.props`.

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
      - run: dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.2
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

A plain install selects the latest stable release (`0.2.1`). Pass `--version 0.3.0-rc.2` for the prerelease.

## Related documentation

- [CLI syntax](CLI_SYNTAX.md)
- [Quick start](QUICKSTART.md)
- [FAQ](FAQ.md)
- [Versioning strategy](../VERSIONING_STRATEGY.md)
- [GitHub repository](https://github.com/shellui-dev/shellui)
- [CLI package](https://www.nuget.org/packages/ShellUI.CLI)
- [Component package](https://www.nuget.org/packages/ShellUI.Components)
- [MIT license](../LICENSE.txt)
