# ShellUI Frequently Asked Questions

Answers for the current ShellUI source and the currently published packages.

## Versions and target frameworks

### Which version should I use?

Use `0.3.0-rc.2`. It targets `net10.0`, uses Tailwind CSS `4.3.2`, and exposes 76 direct component targets. A plain install selects the older stable `0.2.1`, so select the prerelease explicitly:

```bash
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.2
```

Projects still on .NET 9 can use `0.3.0-rc.1`, the last release targeting .NET 9.

### Which command prefix should I use?

Use `shellui` for a global installation. After `dotnet new tool-manifest` and a local tool install, use `dotnet shellui`.

```bash
shellui --version
dotnet shellui --version
```

## NuGet and CLI

### What is the difference?

`ShellUI.Components` is a NuGet package. It provides compiled Razor components in the `ShellUI.Components` namespace and is updated by changing the package version and restoring the project.

The CLI copies source templates into the application. Components go in `Components/UI/`, and layout blocks go in `Components/Layout/`. You own and edit those files.

```bash
dotnet add package ShellUI.Components
shellui init
shellui add button card
```

The package and CLI can be used in the same project, but avoid defining the same component in both namespaces. Choose the package or the local source for a given component.

### Can the CLI add several components at once?

Yes. `add` accepts space-separated names, comma-separated names, or both:

```bash
shellui add button input card
shellui add button,input,card
shellui add button,card dialog
```

Use the exact names printed by `shellui list`. Dependency-only entries are installed automatically and are not counted among the 76 direct targets.

### Which names should I use?

Use the registry names, such as `button`, `input`, `card`, `dialog`, `dropdown`, `date-picker`, `radio-group`, `file-upload`, `data-table`, and `chart`. Do not translate a target into a guessed component name.

## Tailwind and Node.js

### Do I need Node.js?

Not for standalone mode:

```bash
shellui init --yes --tailwind standalone
```

Standalone mode downloads the Tailwind `4.3.2` CLI into `.shellui/bin/` and integrates it with the generated build target.

npm mode is also supported. It requires Node.js and npm:

```bash
shellui init --yes --tailwind npm
npm install -D tailwindcss@^4.3.2 @tailwindcss/cli@^4.3.2
npx @tailwindcss/cli -i ./wwwroot/input.css -o ./wwwroot/app.css --watch
```

The npm commands are Tailwind v4 commands. The generated `wwwroot/input.css` uses `@import "tailwindcss";`; `wwwroot/app.css` is generated output. The current CLI invokes npm through `cmd`; on non-Windows systems use standalone mode or run the npm commands manually.

## Managing CLI components

### How do I list components?

```bash
shellui list
shellui list --installed
shellui list --available
```

There are 76 direct targets. Use the command output as the authoritative name list.

### What happens when I update a component?

`update` overwrites the selected source template without showing a diff, merging changes, or asking for a force flag:

```bash
shellui update button
shellui update button card
shellui update --all
```

Running `shellui update` without names also updates all installed components. Commit or back up local changes first.

### What happens when I remove a component?

`remove` deletes the named component files and updates `shellui.json`. It has no options and does not perform reverse-dependency cleanup. Layout blocks such as `dashboard-01` and `dashboard-02` currently require manual removal from `Components/Layout` because remove is not yet layout-aware:

```bash
shellui remove button
shellui remove button card
```

If another component still uses a removed file, update that usage or remove the dependent component yourself.

## Configuration

### What does `shellui.json` contain?

The serialized properties are PascalCase. The main fields are:

| Field | Meaning |
|---|---|
| `Schema` | Schema identifier |
| `Style` | `default`, `new-york`, or `minimal` |
| `ComponentsPath` | Usually `Components/UI` |
| `LayoutPath` | Usually `Components/Layout` |
| `Tailwind` | Tailwind settings |
| `InstalledComponents` | Installed component records |
| `ProjectType` | Detected Blazor project type |

A representative Tailwind and component record is:

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
      "Version": "0.3.0-rc.2",
      "InstalledAt": "2026-01-01T00:00:00Z",
      "IsCustomized": false
    }
  ],
  "ProjectType": 1
}
```

`IsCustomized` is metadata only. It does not provide a merge workflow, and `update` overwrites the file.

### Which files does `init` create or update?

- `Components/UI/`
- `wwwroot/input.css`
- `wwwroot/app.css`
- `tailwind.config.js`
- `Build/ShellUI.targets`
- `shellui.json`
- `Components/_Imports.razor` when it is present

The host file is `Components/App.razor` for a Blazor Web App or `wwwroot/index.html` for a standalone WebAssembly project.

## Theming

### Can I apply a tweakcn theme?

Yes:

```bash
shellui theme init <url-or-id> --yes --tailwind standalone
shellui theme apply <url-or-id>
shellui theme apply <url-or-id> --emit-override wwwroot/theme.css
shellui theme update
```

`theme apply` updates `wwwroot/input.css` unless `--emit-override` is used. The command records the source in `shellui.theme.lock`, and `theme update` re-fetches that source.

## Existing projects and troubleshooting

### Can I use ShellUI in an existing Blazor app?

Yes. Run the CLI from the project root:

```bash
cd MyExistingApp
shellui init --yes --tailwind standalone
shellui add button card
dotnet build
```

Review the changes to `Components/App.razor`, `Components/_Imports.razor`, the project file, and the generated Tailwind files before committing them.

### The command is not found

Use the prefix that matches the installation:

```bash
shellui --version
dotnet shellui --version
```

For a local tool, run `dotnet tool restore` from the directory containing `.config/dotnet-tools.json`.

### A component is not found

Run:

```bash
shellui list
```

Use the exact target name shown in the list.

### Tailwind output is stale

Check that `wwwroot/input.css` exists and starts with the Tailwind v4 import, then run:

```bash
dotnet build
```

For standalone mode, check `.shellui/bin/`. For npm mode, run `npm install`.

## Package updates and licensing

### How do I update the NuGet package?

Pass the version explicitly:

```bash
dotnet add package ShellUI.Components --version 0.3.0-rc.2
```

Then restore and build the project.

### What license does ShellUI use?

The repository uses the MIT license. See [LICENSE.txt](../LICENSE.txt).

## Related documentation

- [CLI syntax](CLI_SYNTAX.md)
- [CLI installation](CLI_INSTALLATION.md)
- [Quick start](QUICKSTART.md)
- [GitHub repository](https://github.com/shellui-dev/shellui)
- [GitHub issues](https://github.com/shellui-dev/shellui/issues)
- [GitHub discussions](https://github.com/shellui-dev/shellui/discussions)
- [CLI package](https://www.nuget.org/packages/ShellUI.CLI)
- [Component package](https://www.nuget.org/packages/ShellUI.Components)
