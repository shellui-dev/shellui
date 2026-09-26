# ShellUI Quick Start

This quick start uses ShellUI `0.3.0-rc.2`: .NET 10, Tailwind CSS `4.3.2`, and 76 direct component targets.

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
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.2
shellui --version
```

A plain install without `--version` selects the older stable `0.2.1`, which lacks several commands in this guide.

If the project has a .NET tool manifest, use `dotnet shellui` instead. See [CLI installation](CLI_INSTALLATION.md).

## Create and initialize a project

```bash
dotnet new blazor -n MyBlazorApp
cd MyBlazorApp
shellui init --yes --tailwind standalone
```

`init` detects the project type and sets up:

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

Use the exact names shown by `shellui list`. There are 76 direct targets; dependency-only registry entries are not direct targets.

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

The CLI supports tweakcn themes:

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
