# Tailwind CSS Setup Guide for Blazor

This guide targets **Tailwind CSS 4.3.2**, the version ShellUI uses.

## Installation methods

| Method | Setup | Build integration | Node.js | Use for |
|---|---|---|---|---|
| **Play CDN** | One browser package | No local build | No | Prototypes and demos only |
| **Standalone** | One platform-specific CLI binary | MSBuild target | No | A .NET/Blazor project without Node.js |
| **npm** | `tailwindcss` plus `@tailwindcss/cli` | npm or MSBuild | Yes | Projects that already use Node.js or plugins |

The standalone CLI is cached inside the project at `.shellui/bin`. On Windows the executable is `.shellui/bin/tailwindcss.exe`; on macOS and Linux it is `.shellui/bin/tailwindcss`. The project-local `.shellui/bin` directory is the cache location.

For a production Blazor application, use either the standalone or npm workflow and build the CSS as part of the project. The Play CDN is a prototyping convenience and does not provide the same production build and caching behavior.

## Prerequisites

- .NET 10.0 SDK
- A Blazor project
- Node.js and npm only when using the npm method

## Method 1: Use the ShellUI CLI

### Install the CLI

```text
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.2
```

A plain install without `--version` selects the older stable `0.2.1`.

### Initialize a project

Run this from the project directory:

```text
shellui init
```

The default standalone method downloads the Tailwind 4.3.2 standalone executable into `.shellui/bin`, creates the CSS entry point and output file, wires an MSBuild target, and creates the ShellUI component folders. To choose npm explicitly:

```text
shellui init --tailwind npm --yes
```

Other initialization options include `--style`, `--force`, and `--yes`. The current CLI's npm setup invokes npm through `cmd`; use standalone mode on non-Windows systems or run the npm commands manually.

### Add and manage components

The current CLI command surface is:

```text
shellui init
shellui add <components>
shellui list
shellui remove <components>
shellui update [components]
shellui theme init <url>
shellui theme apply <url>
shellui theme update
```

Examples:

```text
shellui add button input card
shellui list
shellui remove card
shellui update
```

`list` shows the direct targets in the registry. Dependencies are resolved recursively from `ComponentRegistry`; hidden support entries are not separate public choices in the list.

## Method 2: Manual standalone setup

### Create the CSS entry point

Create `wwwroot/input.css`:

```css
@import "tailwindcss";
@custom-variant dark (&:is(.dark *));
```

Tailwind v4 uses the CSS entry point above. The older v3 layer-import sequence is not part of this setup.

Create `wwwroot/app.css` as the generated output placeholder. Edit `input.css`, not the generated output.

### Download the version-pinned standalone CLI

Create the project-local cache directory first.

Windows PowerShell:

```powershell
New-Item -ItemType Directory -Force ".shellui/bin" | Out-Null
curl.exe -fL -o ".shellui/bin/tailwindcss.exe" "https://github.com/tailwindlabs/tailwindcss/releases/download/v4.3.2/tailwindcss-windows-x64.exe"
```

Linux:

```text
mkdir -p .shellui/bin
curl -fL -o .shellui/bin/tailwindcss https://github.com/tailwindlabs/tailwindcss/releases/download/v4.3.2/tailwindcss-linux-x64
chmod +x .shellui/bin/tailwindcss
```

macOS:

```text
mkdir -p .shellui/bin
curl -fL -o .shellui/bin/tailwindcss https://github.com/tailwindlabs/tailwindcss/releases/download/v4.3.2/tailwindcss-macos-x64
chmod +x .shellui/bin/tailwindcss
```

The correct project-local cache paths are:

- Windows: `.shellui/bin/tailwindcss.exe`
- Linux: `.shellui/bin/tailwindcss`
- macOS: `.shellui/bin/tailwindcss`

### Add the MSBuild target

Create `Build/ShellUI.targets`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <PropertyGroup>
    <ShellUIBinPath Condition="'$(ShellUIBinPath)' == ''">$(MSBuildProjectDirectory)\.shellui\bin</ShellUIBinPath>
    <TailwindExecutable Condition="'$(OS)' == 'Windows_NT'">$(ShellUIBinPath)\tailwindcss.exe</TailwindExecutable>
    <TailwindExecutable Condition="'$(OS)' != 'Windows_NT'">$(ShellUIBinPath)/tailwindcss</TailwindExecutable>
    <TailwindInputCss Condition="'$(TailwindInputCss)' == ''">$(MSBuildProjectDirectory)\wwwroot\input.css</TailwindInputCss>
    <TailwindOutputCss Condition="'$(TailwindOutputCss)' == ''">$(MSBuildProjectDirectory)\wwwroot\app.css</TailwindOutputCss>
    <TailwindMinify Condition="'$(Configuration)' == 'Release'">--minify</TailwindMinify>
    <TailwindMinify Condition="'$(Configuration)' != 'Release'"></TailwindMinify>
  </PropertyGroup>

  <Target Name="BuildTailwindCSS" BeforeTargets="BeforeBuild" Condition="Exists('$(TailwindExecutable)') AND Exists('$(TailwindInputCss)')">
    <Message Importance="high" Text="Building Tailwind CSS..." />
    <Exec Command="&quot;$(TailwindExecutable)&quot; -i &quot;$(TailwindInputCss)&quot; -o &quot;$(TailwindOutputCss)&quot; $(TailwindMinify)" />
    <Message Importance="high" Text="Tailwind CSS built successfully!" />
  </Target>

  <Target Name="CleanTailwindCSS" AfterTargets="Clean" Condition="Exists('$(TailwindOutputCss)')">
    <Message Importance="high" Text="Cleaning Tailwind CSS output..." />
    <Delete Files="$(TailwindOutputCss)" />
  </Target>
</Project>
```

Import it from the project file:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <Import Project="Build\ShellUI.targets" />
</Project>
```

The target points at `.shellui/bin`, matching the download location above. Keep the executable in that project-local directory so the target and the downloaded binary agree.

### Build and link the output

Build manually with the project-local executable:

```text
.shellui/bin/tailwindcss -i wwwroot/input.css -o wwwroot/app.css
```

On Windows PowerShell:

```powershell
& ".\.shellui\bin\tailwindcss.exe" -i "wwwroot/input.css" -o "wwwroot/app.css"
```

`dotnet build` runs the MSBuild target. Link the generated stylesheet from the Blazor host:

```html
<link href="~/app.css" rel="stylesheet" />
```

## Method 3: npm and `@tailwindcss/cli`

Use npm when the project already has a Node.js toolchain or needs npm-based Tailwind integrations.

### Install the packages

```text
npm install -D tailwindcss@4.3.2 @tailwindcss/cli@4.3.2
```

Tailwind v4 does not require a separate initialization command. Create or edit `wwwroot/input.css` directly:

```css
@import "tailwindcss";
@custom-variant dark (&:is(.dark *));
```

Tailwind v4 can discover source files from the project. If a JavaScript config is required for compatibility or plugins, load it explicitly from the CSS entry point:

```css
@import "tailwindcss";
@config "../tailwind.config.js";
```

Place a JavaScript config at the project root when using that example. Keep the theme variables and `@theme inline` mappings in the CSS entry point.

### Build with `@tailwindcss/cli`

```text
npx @tailwindcss/cli -i wwwroot/input.css -o wwwroot/app.css
```

Minify release output with:

```text
npx @tailwindcss/cli -i wwwroot/input.css -o wwwroot/app.css --minify
```

For MSBuild, create a target that invokes the package:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <PropertyGroup>
    <NpmExecutable>npx</NpmExecutable>
    <TailwindInputCss Condition="'$(TailwindInputCss)' == ''">$(MSBuildProjectDirectory)\wwwroot\input.css</TailwindInputCss>
    <TailwindOutputCss Condition="'$(TailwindOutputCss)' == ''">$(MSBuildProjectDirectory)\wwwroot\app.css</TailwindOutputCss>
    <TailwindMinify Condition="'$(Configuration)' == 'Release'">--minify</TailwindMinify>
    <TailwindMinify Condition="'$(Configuration)' != 'Release'"></TailwindMinify>
  </PropertyGroup>

  <Target Name="BuildTailwindCSS" BeforeTargets="BeforeBuild" Condition="Exists('$(TailwindInputCss)')">
    <Message Importance="high" Text="Building Tailwind CSS with @tailwindcss/cli..." />
    <Exec Command="$(NpmExecutable) @tailwindcss/cli -i &quot;$(TailwindInputCss)&quot; -o &quot;$(TailwindOutputCss)&quot; $(TailwindMinify)" />
    <Message Importance="high" Text="Tailwind CSS built successfully!" />
  </Target>
</Project>
```

Use one Tailwind build path per project: either the standalone target or the npm target. Running both can produce competing writes to `wwwroot/app.css`.

## Method 4: Play CDN for prototypes

The browser build is useful for a quick prototype only:

```html
<head>
  <script src="https://cdn.jsdelivr.net/npm/@tailwindcss/browser@4.3.2"></script>
</head>
```

It does not replace a version-pinned local build for production. Keep the application's theme variables and component CSS available when testing a prototype.

## Verification

After setup:

```text
dotnet build
```

Check that:

- `wwwroot/app.css` contains generated utilities.
- The host includes the generated stylesheet.
- A Razor component using a ShellUI utility is styled in the browser.
- The standalone executable is at `.shellui/bin/tailwindcss.exe` on Windows or `.shellui/bin/tailwindcss` on macOS/Linux.
- The npm project can run `npx @tailwindcss/cli -i wwwroot/input.css -o wwwroot/app.css`.

## Troubleshooting

### Tailwind CSS is not building

- Confirm `wwwroot/input.css` contains `@import "tailwindcss";`.
- Confirm the standalone executable is in the project-local `.shellui/bin` directory.
- Confirm `Build/ShellUI.targets` is imported by the project.
- Confirm the input and output paths exist.
- For npm, confirm `tailwindcss@4.3.2` and `@tailwindcss/cli@4.3.2` are installed and run `npx @tailwindcss/cli --help`.

### Styles are not applying

- Confirm `app.css` is linked from the Blazor host.
- Rebuild after changing `input.css` or Razor markup.
- Clear the browser cache while testing.
- Check the browser console for missing JavaScript assets separately from CSS generation.

### Version or syntax errors

- Use Tailwind `4.3.2` for both the standalone download and the npm packages.
- Use `@import "tailwindcss";`, not the v3 layer imports.
- Use `npx @tailwindcss/cli` with the version-pinned packages and CSS entry point.

## Custom themes and fonts

Tailwind v4 themes are CSS-first. Keep the color variables in `input.css`, then map them into Tailwind with `@theme inline`:

```css
@import "tailwindcss";

:root {
  --background: oklch(0.99 0 0);
  --foreground: oklch(0 0 0);
  --primary: oklch(0.55 0.22 264.53);
  --primary-foreground: oklch(1 0 0);
  --radius: 0.5rem;
}

.dark {
  --background: oklch(0 0 0);
  --foreground: oklch(1 0 0);
  --primary: oklch(0.81 0.17 75.35);
  --primary-foreground: oklch(0 0 0);
}

@theme inline {
  --color-background: var(--background);
  --color-foreground: var(--foreground);
  --color-primary: var(--primary);
  --color-primary-foreground: var(--primary-foreground);
  --radius-lg: var(--radius);
}
```

A theme tool such as tweakcn can provide a replacement `:root` and `.dark` block. Paste the generated variables into `input.css`, then rebuild Tailwind.

For fonts, use a project-local stylesheet or a font provider and retain fallbacks:

```css
:root {
  --font-sans: 'Custom Sans', ui-sans-serif, system-ui, sans-serif;
  --font-mono: 'Custom Mono', ui-monospace, monospace;
}
```

## Next steps

1. Initialize the project with `shellui init`.
2. Choose the standalone or npm Tailwind method.
3. Add components with `shellui add`.
4. Customize the variables in `wwwroot/input.css`.
5. Build the project and verify the generated stylesheet.

Useful references:

- [Tailwind CSS documentation](https://tailwindcss.com/docs)
- [Tailwind CSS v4 upgrade guide](https://tailwindcss.com/docs/upgrade-guide)
- [ShellUI documentation](https://shellui.dev)
- [ShellUI GitHub repository](https://github.com/shellui-dev/shellui)
