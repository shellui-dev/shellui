# ShellUI Quick Start

This quick start follows the current source: `0.4.0-alpha.1`, .NET 10, Tailwind CSS `4.3.2`, and 73 direct component targets. The source is not published as `0.4.0-alpha.1`; the published stable package is `0.2.1` and the published prerelease is `0.3.0-rc.1`. The published prerelease targets .NET 9; use the matching SDK/runtime when following a published package instead.

## Prerequisites

- .NET 10 SDK
- A Blazor project
- The `ShellUI.CLI` tool
- Node.js and npm only when choosing `--tailwind npm`

Check the SDK with:

```bash
dotnet --version
```

## Install the CLI

A global tool is invoked as `shellui`:

```bash
dotnet tool install -g ShellUI.CLI
shellui --version
```

The plain install selects the published stable `0.2.1`. To use the published prerelease, select it explicitly:

```bash
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.1 --prerelease
```

The workflow below follows the current source. Published `0.2.1` and `0.3.0-rc.1` tools are older packages and may not expose source-only commands; check `shellui --help` or build the current source when using the syntax in this guide.

If the project has a .NET tool manifest, use `dotnet shellui` instead. See [CLI installation](CLI_INSTALLATION.md).

## Create and initialize a project

```bash
dotnet new blazor -n MyBlazorApp
cd MyBlazorApp
shellui init --yes --tailwind standalone
```

The current source detects the project type and sets up:

- `Components/UI/`
- `wwwroot/input.css` and `wwwroot/app.css`
- `tailwind.config.js`
- `Build/ShellUI.targets`
- `shellui.json`
- `Components/_Imports.razor` when it is present
- The host file used by the project

Standalone mode downloads Tailwind `4.3.2` into `.shellui/bin/` and does not require Node.js. To use npm instead:

```bash
shellui init --yes --tailwind npm
```

npm mode requires Node.js and npm. The current CLI invokes npm through `cmd`, so use standalone mode on non-Windows systems or run the npm commands manually.

## Add components

Add one target, several space-separated targets, or comma-separated targets:

```bash
shellui add button
shellui add button input card
shellui add button,input,card
```

Dependencies are copied automatically. Use `--force` to overwrite an existing component source file:

```bash
shellui add button --force
```

Use the exact names shown by `shellui list`. The current source exposes 73 direct targets; dependency-only registry entries are not direct targets.

## Use a component

After `init`, the project imports `YourProject.Components.UI`. For a project named `MyBlazorApp`:

```razor
@using MyBlazorApp.Components.UI

<Card Class="max-w-md">
    <h2 class="text-lg font-semibold">Welcome</h2>
    <Input Placeholder="Enter your email" Type="email" />
    <Button>Click me</Button>
</Card>
```

`card` installs its component parts automatically. `button` and `input` are separate targets, so add them explicitly when using them.

## Tailwind CSS v4

Edit `wwwroot/input.css`, not the generated `wwwroot/app.css`. The current v4 input begins with:

```css
@import "tailwindcss";
@custom-variant dark (&:is(.dark *));

:root {
  --primary: oklch(0.55 0.22 264.53);
  --primary-foreground: oklch(1 0 0);
}

@theme inline {
  --color-primary: var(--primary);
  --color-primary-foreground: var(--primary-foreground);
}
```

For an npm-based Tailwind workflow, use the v4 CLI package:

```bash
npm install -D tailwindcss@^4.3.2 @tailwindcss/cli@^4.3.2
npx @tailwindcss/cli -i ./wwwroot/input.css -o ./wwwroot/app.css --watch
```

`shellui init --tailwind npm --yes` performs the package installation and creates the build integration. A normal build also runs Tailwind:

```bash
dotnet build
```

## Manage installed components

```bash
shellui list
shellui list --installed
shellui list --available
shellui update button
shellui update button card
shellui update --all
shellui remove button card
```

`update` overwrites the selected source files directly. It has no comparison, merge, or force step. Commit or back up custom changes before running it.

`remove` accepts component names separated by spaces and has no options. It removes the named files, but it does not perform reverse-dependency cleanup. Layout blocks such as `dashboard-01` and `dashboard-02` currently require manual removal from `Components/Layout`. Check references before removing a component that another component may use.

## Apply a theme

The current source supports tweakcn themes:

```bash
shellui theme init <url-or-id> --yes --tailwind standalone
shellui theme apply <url-or-id>
shellui theme apply <url-or-id> --emit-override wwwroot/theme.css
shellui theme update
```

`theme apply` updates `wwwroot/input.css` by default and writes `shellui.theme.lock`. Use `--emit-override` for a separate CSS file. Run `theme update` from the project root to re-fetch the recorded theme.

## Troubleshooting

- Use `shellui` for a global tool and `dotnet shellui` for a local manifest.
- Run `shellui list` to verify a target name.
- Check that `wwwroot/input.css` contains `@import "tailwindcss";`.
- Check `.shellui/bin/` for standalone mode or run `npm install` for npm mode.
- Run `dotnet build` to invoke the generated Tailwind target.

## Related documentation

- [CLI syntax](CLI_SYNTAX.md)
- [CLI installation](CLI_INSTALLATION.md)
- [FAQ](FAQ.md)
- [GitHub repository](https://github.com/shellui-dev/shellui)
- [CLI package](https://www.nuget.org/packages/ShellUI.CLI)
- [Component package](https://www.nuget.org/packages/ShellUI.Components)
