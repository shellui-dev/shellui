# ShellUI CLI Syntax

This reference follows the current source: `0.4.0-alpha.1`, `net10.0`, Tailwind CSS `4.3.2`, and 73 direct component targets. The current source is not a published package. The published CLI versions are stable `0.2.1` and prerelease `0.3.0-rc.1`; their command surface may differ from the source described here.

## Command prefix

A global tool is invoked as `shellui`. After creating a .NET tool manifest, a local tool is invoked as `dotnet shellui`.

The examples below use a global tool. Replace `shellui` with `dotnet shellui` when using a local tool manifest.

```bash
shellui --help
shellui --version
```

## Command overview

```text
shellui init [--force] [--style <style>] [--tailwind standalone|npm] [--yes]
shellui add <name...> [--force]
shellui list [--installed|--available]
shellui remove <name...>
shellui update [name...] [--all]
shellui theme init <url-or-id> [--force] [--style <style>] [--tailwind standalone|npm] [--yes]
shellui theme apply <url-or-id> [--emit-override <path>]
shellui theme update
```

`sidebar-js` is retained as a hidden legacy component for existing generated providers. New sidebar and provider templates depend on `shellui-js` instead.

## `init`

Initializes ShellUI in the current Blazor project.

```bash
shellui init
shellui init --force
shellui init --style new-york
shellui init --tailwind npm --yes
```

Options:

- `--force` reinitializes a project that already has `shellui.json`.
- `--style <style>` selects `default`, `new-york`, or `minimal`.
- `--tailwind standalone|npm` selects the Tailwind setup method.
- `--yes` runs without prompts and uses the selected defaults. Without an explicit method, the default is `standalone`.

Initialization creates or updates:

- `Components/UI/`
- `wwwroot/input.css` and `wwwroot/app.css`
- `tailwind.config.js` at the project root
- `Build/ShellUI.targets`
- `shellui.json`
- `Components/_Imports.razor` when it already exists
- the host file when applicable

`Components/Layout/` is created when a layout target is installed.

Standalone mode stores the Tailwind executable in `.shellui/bin/`. npm mode installs `tailwindcss@^4.3.2` and `@tailwindcss/cli@^4.3.2` and requires Node.js and npm. The current CLI invokes npm through `cmd`; use standalone mode on non-Windows systems or run npm manually.

## `add`

Copies one or more component targets and their source dependencies into the project.

```bash
shellui add button
shellui add button card dialog
shellui add button,card,dialog
shellui add button,card dialog
shellui add button --force
```

`add` accepts space-separated names, comma-separated names, and a mixture of both. Use the exact target names printed by `shellui list`.

`--force` overwrites an existing component file. Dependencies are installed automatically; there is no separate dependency option.

## `list`

Lists the direct targets in the current source and their installation status.

```bash
shellui list
shellui list --installed
shellui list --available
```

The current source has 73 direct targets. Registry entries used only as dependencies are not counted as direct targets. Choose either `--installed` or `--available` to filter the output.

## `remove`

Removes the named component files and their entries from `shellui.json`.

```bash
shellui remove button
shellui remove button card dialog
```

Pass names separated by spaces. `remove` has no force flag or other options, does not prompt, and does not perform reverse-dependency cleanup. If another component still references a removed file, update that usage or remove the dependent component yourself. Layout blocks such as `dashboard-01` and `dashboard-02` currently require manual removal from `Components/Layout` because remove is not yet layout-aware.

## `update`

Replaces installed component source with the current template source.

```bash
shellui update button
shellui update button card
shellui update --all
shellui update
```

Names are separated by spaces. With no names, or with `--all`, every installed component is updated. `update` overwrites files directly; there is no diff, merge, confirmation, or force option. Commit or back up local changes before updating.

## `theme`

The theme commands fetch a public tweakcn theme by URL or theme ID and write its CSS variables into the project.

### `theme init`

Initializes a project and applies a theme in one step.

```bash
shellui theme init <url-or-id>
shellui theme init <url-or-id> --force --style default --tailwind standalone --yes
```

`--style` accepts `default`, `new-york`, or `minimal`. The command uses the same initialization options as `init`.

### `theme apply`

Applies a theme to `wwwroot/input.css` by default.

```bash
shellui theme apply <url-or-id>
shellui theme apply <url-or-id> --emit-override wwwroot/theme.css
```

With `--emit-override`, the command writes a separate CSS file instead of modifying the input stylesheet. Link that file after the ShellUI stylesheet.

### `theme update`

Re-fetches the source recorded in `shellui.theme.lock`.

```bash
shellui theme update
```

Run it from the project root after a theme has been applied.

## Tailwind files

| Path | Purpose |
|---|---|
| `wwwroot/input.css` | Tailwind v4 input and theme variables |
| `wwwroot/app.css` | Generated Tailwind output |
| `tailwind.config.js` | Project content globs and dark-mode setting |
| `.shellui/bin/tailwindcss` | Standalone CLI on macOS/Linux |
| `.shellui/bin/tailwindcss.exe` | Standalone CLI on Windows |
| `Build/ShellUI.targets` | MSBuild integration |
| `shellui.json` | ShellUI project configuration |
| `shellui.theme.lock` | Theme source and integrity record |

The npm v4 command is:

```bash
npm install -D tailwindcss@^4.3.2 @tailwindcss/cli@^4.3.2
npx @tailwindcss/cli -i ./wwwroot/input.css -o ./wwwroot/app.css --watch
```

The generated `wwwroot/input.css` uses the Tailwind v4 import:

```css
@import "tailwindcss";
```

## Configuration fields

`ShellUIConfig` is serialized with these property names:

| Property | Meaning |
|---|---|
| `Schema` | Schema identifier |
| `Style` | `default`, `new-york`, or `minimal` |
| `ComponentsPath` | Usually `Components/UI` |
| `LayoutPath` | Usually `Components/Layout` |
| `Tailwind` | Tailwind settings |
| `InstalledComponents` | Installed component records |
| `ProjectType` | Detected Blazor project type |

Representative fields look like this:

```json
{
  "Style": "default",
  "ComponentsPath": "Components/UI",
  "LayoutPath": "Components/Layout",
  "Tailwind": {
    "Enabled": true,
    "Version": "4.3.2",
    "Method": "standalone",
    "ConfigPath": "tailwind.config.js",
    "CssPath": "wwwroot/app.css"
  },
  "InstalledComponents": [
    {
      "Name": "button",
      "Version": "0.4.0-alpha.1",
      "InstalledAt": "2026-01-01T00:00:00Z",
      "IsCustomized": false
    }
  ],
  "ProjectType": 1
}
```

`ProjectType` is serialized as the enum value; `1` is `BlazorServer` in the current source. `IsCustomized` is metadata, not a merge mechanism. `update` still overwrites the file.

## Common workflow

```bash
shellui init --yes --tailwind standalone
shellui add button,input,card
dotnet build
```

For a theme-aware setup:

```bash
shellui theme init <url-or-id> --yes --tailwind standalone
shellui add button,card
```

## Troubleshooting

- If the command is not found, use `shellui` for a global tool or `dotnet shellui` for a local manifest.
- Run `shellui list` to verify a target name.
- Confirm `wwwroot/input.css` exists and contains `@import "tailwindcss";`.
- Run `dotnet build` to invoke the generated Tailwind build target.
- In standalone mode, check `.shellui/bin/`; in npm mode, run `npm install`.

## Related documentation

- [CLI installation](CLI_INSTALLATION.md)
- [Quick start](QUICKSTART.md)
- [FAQ](FAQ.md)
- [GitHub repository](https://github.com/shellui-dev/shellui)
- [CLI package](https://www.nuget.org/packages/ShellUI.CLI)
- [Component package](https://www.nuget.org/packages/ShellUI.Components)
