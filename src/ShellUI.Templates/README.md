# ShellUI Templates

`ShellUI.Templates` is the internal component-template catalog used by `ShellUI.CLI`. It supplies the source content and metadata that the CLI copies into a consumer's Blazor project.

## Packaging status

This project is part of the current `0.4.0-alpha.1` source and targets .NET 10.

`ShellUI.Templates` sets `IsPackable` to `false` and is not published to NuGet. Do not run `dotnet add package ShellUI.Templates`. The only packable ShellUI projects are:

- `ShellUI.CLI`
- `ShellUI.Components`

`ShellUI.Templates` and `ShellUI.Core` are compiled project dependencies of the CLI tool; consumers do not reference them directly.

## How the CLI uses the templates

After the CLI is installed as a global or local tool, it resolves a component from its registry and writes the selected templates into the target project:

```bash
shellui add button
shellui add typed-select command-palette data-picker multi-select tag-input
```

A local tool uses:

```bash
dotnet shellui list
```

The global tool command is `shellui`; a local .NET tool is invoked as `dotnet shellui`.

## Template inventory

The current source registry contains **173 entries**:

- **73 direct install targets** displayed by `shellui list`
- **100 hidden dependency entries** installed recursively and omitted from the direct list

Each entry provides content plus metadata used by the CLI, including its display name, category, description, target path, version, dependencies, and optional NuGet dependencies. The catalog also includes hidden subcomponents, variants, models, services, JavaScript, and stylesheet assets required by direct targets.

The current source additions include the direct targets `typed-select`, `command-palette`, `data-picker`, `multi-select`, and `tag-input`.

## Development

Build and test the repository solution from the repository root:

```bash
dotnet restore ShellUI.slnx
dotnet build ShellUI.slnx
dotnet test ShellUI.slnx
```

Template changes should keep the catalog, generated files, and component runtime in sync. The repository's test suite covers template compilation, synchronization, safelist drift, and CLI integration behavior.

## Documentation

- [Repository README](https://github.com/shellui-dev/shellui/blob/main/README.md)
- [CLI reference](https://github.com/shellui-dev/shellui/blob/main/docs/CLI_SYNTAX.md)
- [Contributing guide](https://github.com/shellui-dev/shellui/blob/main/docs/CONTRIBUTING.md)
- [Historical release notes](https://github.com/shellui-dev/shellui/blob/main/docs/RELEASE_NOTES.md)

Historical component counts in old release sections describe those releases, not the current registry.

## License

[MIT](https://github.com/shellui-dev/shellui/blob/main/LICENSE.txt)
