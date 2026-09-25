# ShellUI CLI

The ShellUI command-line tool initializes a .NET 10 Blazor project and copies ShellUI component templates into source files that the application owns.

## Version scope

This README documents the command surface in repository source `0.4.0-alpha.1`.

- Latest published stable CLI: `0.2.1`
- Latest published prerelease CLI: `0.3.0-rc.1`
- Current source: `0.4.0-alpha.1`, not currently published

The theme commands, current registry inventory, and other source-only features are not implied by the published packages. The published prerelease targets .NET 9; the current source targets .NET 10. Run `shellui --help` with an installed package to inspect that package's command surface.

## Install a published tool

Install the latest published stable tool globally:

```bash
dotnet tool install --global ShellUI.CLI --version 0.2.1
```

Or install the latest published prerelease:

```bash
dotnet tool install --global ShellUI.CLI --version 0.3.0-rc.1
```

A global tool uses the `shellui` command:

```bash
shellui --help
```

A local .NET tool uses `dotnet shellui`:

```bash
dotnet new tool-manifest
dotnet tool install --local ShellUI.CLI --version 0.3.0-rc.1
dotnet shellui --help
```

`ShellUI.CLI` and `ShellUI.Components` are the only packable ShellUI projects. The current source CLI can be packed locally, but there is no published `0.4.0-alpha.1` tool package to install.

## Current source quick start

Run these commands from the root of the target Blazor project:

```bash
shellui init
shellui add button card dialog
shellui list
shellui list --installed
```

For a local tool, prefix each command with `dotnet `, for example `dotnet shellui init`.

`init` detects the Blazor project, creates the ShellUI folder and configuration structure, selects the Tailwind method, writes the default theme, wires the host, and adds the Tailwind MSBuild integration. It can use the standalone Tailwind executable or npm.

## Commands

### `init`

Initialize ShellUI in a Blazor project.

```bash
shellui init
```

Options:

- `--force` reinitialize a project that is already configured
- `--style <default|new-york|minimal>` select a component style
- `--tailwind <standalone|npm>` select the Tailwind build method
- `--yes` accept the defaults without prompts

### `add <components...>`

Install one or more direct component targets and their dependencies.

```bash
shellui add button
shellui add input card dialog table
shellui add button,input,card
```

Options:

- `--force` overwrite existing files

Dependency-only registry entries are hidden from `list` but are installed recursively. Some templates also add required NuGet packages and stylesheet links.

### `list`

List direct component targets.

```bash
shellui list
shellui list --installed
shellui list --available
```

### `remove <components...>`

Remove one or more named installed components.

```bash
shellui remove button input
```

The command accepts one or more component names. Review local changes before removing source files. Layout blocks such as `dashboard-01` and `dashboard-02` currently require manual removal from `Components/Layout` because remove is not yet layout-aware.

### `update [components...]`

Reinstall named components from the current CLI templates. With no names, or with `--all`, it updates every installed component.

```bash
shellui update button
shellui update card input
shellui update --all
```

`update` overwrites template files. Commit or otherwise preserve local customizations before running it.

### `theme init <url-or-id>`

Initialize a project and apply a public [tweakcn](https://tweakcn.com) theme in one operation.

```bash
shellui theme init https://tweakcn.com/themes/THEME_ID
shellui theme init THEME_ID --yes
```

The URL argument also accepts a bare theme ID or a public `https://tweakcn.com/r/themes/THEME_ID` URL. Options match `init`: `--force`, `--style`, `--tailwind`, and `--yes`.

### `theme apply <url-or-id>`

Apply a theme to an initialized project.

```bash
shellui theme apply https://tweakcn.com/themes/THEME_ID
shellui theme apply THEME_ID --emit-override wwwroot/theme.css
```

Without `--emit-override`, the command replaces the sentinel-marked region in `wwwroot/input.css` and leaves surrounding CSS intact. With `--emit-override`, it writes a standalone theme file that can be loaded after `shellui-all.css`.

### `theme update`

Re-fetch the theme recorded in `shellui.theme.lock` and apply it again.

```bash
shellui theme update
```

The lock file stores the original source URL, theme name, timestamp, and SHA-256 of the fetched theme JSON.

## Component inventory

The current source registry contains **173 entries**:

- **73 direct install targets**, shown by `list`
- **100 hidden dependency entries**, resolved by `add` but omitted from the direct list

The five current source additions are:

```bash
shellui add typed-select command-palette data-picker multi-select tag-input
```

Use `shellui list` for the complete direct-target inventory and descriptions.

## Generated project structure

A typical initialized project contains:

| Path | Purpose |
|---|---|
| `Components/UI/` | Installed component source |
| `Components/Layout/` | Dashboard layout blocks, when installed |
| `wwwroot/input.css` | Tailwind input and ShellUI theme variables |
| `wwwroot/app.css` | Compiled project CSS |
| `wwwroot/shellui.js` | JavaScript interop used by ShellUI components |
| `Build/ShellUI.targets` | MSBuild Tailwind integration |
| `shellui.json` | Installed component and Tailwind configuration |
| `shellui.theme.lock` | Theme source metadata, when a theme is applied |

## Tailwind setup

Current source uses Tailwind CSS `4.3.2`.

- `shellui init --tailwind standalone` downloads the standalone executable and does not require Node.js.
- `shellui init --tailwind npm --yes` installs `tailwindcss@^4.3.2` and `@tailwindcss/cli@^4.3.2` and requires Node.js and npm.

Both methods compile `wwwroot/input.css` to `wwwroot/app.css` through the generated MSBuild target. The current CLI invokes npm through `cmd`; use standalone mode on non-Windows systems or run npm manually.

## Development

From the repository root:

```bash
dotnet restore ShellUI.slnx
dotnet build ShellUI.slnx
dotnet test ShellUI.slnx
```

The CLI project is `src/ShellUI.CLI/ShellUI.CLI.csproj`. `ShellUI.Core` and `ShellUI.Templates` are internal dependencies and are not packable.

## Documentation

- [Repository README](https://github.com/shellui-dev/shellui/blob/main/README.md)
- [Contributing guide](https://github.com/shellui-dev/shellui/blob/main/docs/CONTRIBUTING.md)
- [Release notes](https://github.com/shellui-dev/shellui/blob/main/docs/RELEASE_NOTES.md)

The release notes are historical records for published versions and do not replace this source-version command reference.

## License

[MIT](https://github.com/shellui-dev/shellui/blob/main/LICENSE.txt)
