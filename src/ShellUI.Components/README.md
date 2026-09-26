# ShellUI Components

`ShellUI.Components` is the .NET 10 Razor class library packaged as `ShellUI.Components`. It provides the component runtime, Tailwind classes, theme variables, JavaScript assets, and package build integration used by ShellUI.

## Version scope

| Channel | Version | Notes |
|---|---|---|
| Repository source | `0.4.0-alpha.1` | Targets .NET 10 and Tailwind CSS `4.3.2` |
| Latest published stable package | `0.2.1` | Available on NuGet |
| Latest published prerelease package | `0.3.0-rc.1` | Available on NuGet; older than this source checkout |

The precompiled CSS and safelist workflows below are current-source features. They are not implied by the published `0.2.1` or `0.3.0-rc.1` packages. The published prerelease targets .NET 9; the current source targets .NET 10.

## Install a published package

Pin the latest published stable version:

```bash
dotnet add package ShellUI.Components --version 0.2.1
```

To select the published prerelease instead:

```bash
dotnet add package ShellUI.Components --version 0.3.0-rc.1 --prerelease
```

The current `0.4.0-alpha.1` package is not published. To consume it, build `src/ShellUI.Components/ShellUI.Components.csproj` and use the resulting package from a local feed. Do not try to install `ShellUI.Core` or `ShellUI.Templates`; both are internal, non-packable projects.

## Current-source CSS workflows

After building the current package, consumers can choose one of two CSS workflows.

### Precompiled bundle

The release pipeline runs `scripts/rebuild-precompiled-css.sh` before packing. For a local current-source pack, run that script first; the generated `shellui-all.css` contains the ShellUI theme and safelisted utilities, so the consuming app does not need a Tailwind build for ShellUI styles.

```razor
<link href="_content/ShellUI.Components/shellui-all.css" rel="stylesheet" />
```

```razor
@using ShellUI.Components
```

`shellui-all.css` is generated and gitignored; it is not a checked-in source file.

### Safelist with an existing Tailwind build

For an app that already compiles Tailwind, the package build target writes `wwwroot/shellui-classes.txt` in the consuming project. Reference that file from the app's input stylesheet:

```css
@import "tailwindcss";
@source "./shellui-classes.txt";
```

Keep the theme variables used by ShellUI in the same Tailwind build. The current source baseline is Tailwind CSS `4.3.2`.

## Use a component

Add the package namespace to `Components/_Imports.razor`:

```razor
@using ShellUI.Components
```

Then use the components in a Razor file:

```razor
<Button Variant="ButtonVariant.Default">Save</Button>

<Card>
    <CardHeader>
        <CardTitle>ShellUI</CardTitle>
    </CardHeader>
    <CardContent>
        <p>Component content</p>
    </CardContent>
</Card>
```

The package also serves static assets, including `shellui.js`, from `_content/ShellUI.Components/` for components that use JavaScript interop. The package's own `SidebarProvider` retains `shellui-sidebar.js` for its runtime module; CLI-copied providers use the host-loaded `shellui.js` contract instead.

## Component inventory

The CLI registry backing the current source contains **173 entries**: **73 direct install targets** and **100 hidden dependency entries**. The CLI displays the 73 direct targets and resolves hidden entries recursively.

Current source additions include `typed-select`, `command-palette`, `data-picker`, `multi-select`, and `tag-input`. Use the current-source CLI's `list` command for the complete direct-target inventory.

## Theming with the current-source CLI

The current-source CLI can apply a public [tweakcn](https://tweakcn.com) theme:

```bash
shellui theme apply https://tweakcn.com/themes/THEME_ID
shellui theme apply https://tweakcn.com/themes/THEME_ID --emit-override wwwroot/theme.css
shellui theme update
```

The first form updates the managed region in `wwwroot/input.css` for a Tailwind build. The override form writes standalone variables for an app using `shellui-all.css`; load that file after the precompiled stylesheet. The source URL and hash are stored in `shellui.theme.lock` for `theme update`.

These commands require a build of the current CLI. They are not present in the published `0.2.1` or `0.3.0-rc.1` tools.

## Accessibility

ShellUI components use Blazor and Tailwind patterns for semantics, focus, and keyboard interaction where implemented. Accessibility still depends on the component API, configuration, content, and host application; test each consuming app rather than assuming a blanket conformance level.

## Documentation

- [Repository README](https://github.com/shellui-dev/shellui/blob/main/README.md)
- [Tailwind setup](https://github.com/shellui-dev/shellui/blob/main/docs/tailwind-setup.md)
- [Contributing guide](https://github.com/shellui-dev/shellui/blob/main/docs/CONTRIBUTING.md)
- [Historical release notes](https://github.com/shellui-dev/shellui/blob/main/docs/RELEASE_NOTES.md)

## License

[MIT](https://github.com/shellui-dev/shellui/blob/main/LICENSE.txt)
