# ShellUI CLI

`shellui` sets up Tailwind CSS in a Blazor project and copies ShellUI components into it as source you own, in the spirit of shadcn/ui.

## Install

The tool needs the .NET 10 runtime. The projects it sets up can target .NET 8, 9 or 10:

```bash
dotnet tool install --global ShellUI.CLI
```

Or as a local tool, invoked as `dotnet shellui`:

```bash
dotnet new tool-manifest
dotnet tool install --local ShellUI.CLI
```

## Quick start

From the root of a Blazor project:

```bash
shellui init
shellui add button card dialog
dotnet watch
```

`init` sets up Tailwind CSS `4.3.2` (the standalone executable, or npm), writes the default theme to `wwwroot/input.css`, adds a build step that compiles it to `wwwroot/app.css`, and patches `App.razor` with an interactive render mode, a theme script, and `shellui.js`. It removes the template's local Bootstrap copy and its `<link>`.

`add` writes each component and everything it depends on to `Components/UI/`. It also adds the NuGet packages, stylesheet links, and `_Imports.razor` usings the components need.

## Commands

| Command | Purpose |
|---|---|
| `init` | Set up ShellUI and Tailwind in the current project |
| `add <names...>` | Install components and their dependencies |
| `list` | List the 76 components, with `--installed` or `--available` filters |
| `remove <names...>` | Delete installed component files |
| `update [names...]` | Rewrite installed components from the current templates (`--all` for every one) |
| `theme init <url-or-id>` | Run `init`, then apply a [tweakcn](https://tweakcn.com) theme |
| `theme apply <url-or-id>` | Apply a tweakcn theme to `wwwroot/input.css`, or write an override file with `--emit-override <path>` |
| `theme update` | Re-apply the theme recorded in `shellui.theme.lock` |

`init` options:

- `--tailwind standalone|npm` selects how Tailwind is installed. `standalone` needs no Node.js. npm mode runs npm through `cmd`, so it only works on Windows.
- `--yes` runs without prompts; Tailwind defaults to `standalone`.
- `--force` re-initializes a project that already has `shellui.json`.
- `--style` is recorded in `shellui.json`. All styles currently install the same templates.

`add` accepts names separated by spaces or commas, and `--force` overwrites existing files. `update` and `add --force` overwrite your copies, so commit customized components first. `remove` does not handle dashboard layouts; delete those from `Components/Layout/` yourself.

## Generated files

| Path | Purpose |
|---|---|
| `Components/UI/` | Installed components |
| `Components/Layout/` | Dashboard layouts, when installed |
| `wwwroot/input.css` | Tailwind input and theme variables |
| `wwwroot/app.css` | Compiled CSS |
| `wwwroot/shellui.js` | JavaScript used by some components |
| `Build/ShellUI.targets` | Tailwind build step |
| `shellui.json` | ShellUI settings and installed components |
| `shellui.theme.lock` | Theme source, when a theme is applied |

## Documentation

- [README](https://github.com/shellui-dev/shellui/blob/v0.3.1/README.md)
- [CLI reference](https://github.com/shellui-dev/shellui/blob/v0.3.1/docs/CLI_SYNTAX.md)
- [Release notes](https://github.com/shellui-dev/shellui/blob/v0.3.1/docs/RELEASE_NOTES.md)

## License

[MIT](https://github.com/shellui-dev/shellui/blob/v0.3.1/LICENSE.txt)
