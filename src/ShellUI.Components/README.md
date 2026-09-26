# ShellUI Components

`ShellUI.Components` is the .NET 10 Razor class library packaged as `ShellUI.Components`. It provides the component runtime, Tailwind classes, theme variables, JavaScript assets, and package build integration used by ShellUI.

## Install

Requires a .NET 10 project. Prereleases are not picked up by a plain install, so pass the version:

```bash
dotnet add package ShellUI.Components --version 0.3.0-rc.2
```

Projects still on .NET 9 can use `0.3.0-rc.1`. Do not install `ShellUI.Core` or `ShellUI.Templates`; both are internal, non-packable projects.

## CSS workflows

After building the current package, consumers can choose one of two CSS workflows.

### Precompiled bundle

The release pipeline runs `scripts/rebuild-precompiled-css.sh` before packing. For a local pack, run that script first; the generated `shellui-all.css` contains the ShellUI theme and safelisted utilities, so the consuming app does not need a Tailwind build for ShellUI styles.

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

Keep the theme variables used by ShellUI in the same Tailwind build. ShellUI uses Tailwind CSS `4.3.2`.

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

The CLI registry contains **176 entries**: **76 direct install targets** and **100 hidden dependency entries**. The CLI displays the 76 direct targets and resolves hidden entries recursively.

`0.3.0-rc.2` adds `typed-select`, `command-palette`, `data-picker`, `multi-select`, `tag-input`, `donut-chart`, `radar-chart`, and `radial-chart`. Use the CLI's `list` command for the complete direct-target inventory.

## Theming with the CLI

The CLI can apply a public [tweakcn](https://tweakcn.com) theme:

```bash
shellui theme apply https://tweakcn.com/themes/THEME_ID
shellui theme apply https://tweakcn.com/themes/THEME_ID --emit-override wwwroot/theme.css
shellui theme update
```

The first form updates the managed region in `wwwroot/input.css` for a Tailwind build. The override form writes standalone variables for an app using `shellui-all.css`; load that file after the precompiled stylesheet. The source URL and hash are stored in `shellui.theme.lock` for `theme update`.

These commands require CLI `0.3.0-rc.2` or later.

## Accessibility

ShellUI components use Blazor and Tailwind patterns for semantics, focus, and keyboard interaction where implemented. Accessibility still depends on the component API, configuration, content, and host application; test each consuming app rather than assuming a blanket conformance level.

## Documentation

- [Repository README](https://github.com/shellui-dev/shellui/blob/main/README.md)
- [Tailwind setup](https://github.com/shellui-dev/shellui/blob/main/docs/tailwind-setup.md)
- [Contributing guide](https://github.com/shellui-dev/shellui/blob/main/docs/CONTRIBUTING.md)
- [Historical release notes](https://github.com/shellui-dev/shellui/blob/main/docs/RELEASE_NOTES.md)

## License

[MIT](https://github.com/shellui-dev/shellui/blob/main/LICENSE.txt)
