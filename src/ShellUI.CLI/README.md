# ShellUI CLI

Command-line tool for managing ShellUI Blazor components. Install components as `.razor` source files you own, styled with Tailwind CSS.

## Is this what I want?

This is the **CLI install path (Path C)** — best for **owning your component source and editing freely**. If that's not you, there are three other paths worth considering:

- **[Path A — Precompiled bundle](https://www.nuget.org/packages/ShellUI.Components)** — one `<link>` tag, zero Tailwind setup. Best for quick starts.
- **[Path B — Safelist](https://www.nuget.org/packages/ShellUI.Components)** — for projects that already run Tailwind. Tree-shaken CSS.
- **[Path D — CDN](https://github.com/shellui-dev/shellui#path-d--cdn-no-nuget-static-html)** — one `<link>` tag from jsdelivr. Best for static HTML and prototypes.

**Use this CLI (Path C) when you want:**
- ✅ Source-level ownership of every component
- ✅ To restyle by editing `.razor` files directly
- ✅ Automatic host wiring (`shellui init` patches App.razor)
- ✅ tweakcn output to paste straight into your `wwwroot/input.css`
- ✅ Zero runtime CSS dependency on any package or CDN

## Installation

```bash
dotnet tool install -g ShellUI.CLI
```

## Quick Start

### Initialize ShellUI in your project

```bash
dotnet shellui init
```

Or in one shot with a tweakcn theme baked in:

```bash
dotnet shellui theme init https://tweakcn.com/themes/<id> --yes
```

`init` automatically:
- ✅ Downloads Tailwind CSS CLI (standalone, no Node.js required) — or uses your npm install if you prefer
- ✅ Writes the full default theme (`:root`, `.dark`, `@theme inline`) to `wwwroot/input.css`
- ✅ Patches `Components/App.razor` with `@rendermode="InteractiveServer"`, a theme-bootstrap `<script>` (no light-flash on dark pages), and the `<script src="shellui.js">` tag
- ✅ Patches `wwwroot/index.html` instead for Blazor WebAssembly standalone projects
- ✅ Sets up MSBuild integration so Tailwind rebuilds on every `dotnet build`
- ✅ Creates the `Components/UI/` folder structure
- ✅ Idempotent — running it twice is a no-op

### Add components

```bash
# Add a single component
dotnet shellui add button

# Add multiple components
dotnet shellui add input card dialog table

# List all available components
dotnet shellui list

# List installed components
dotnet shellui list --installed
```

## Commands

### `init`
Initialize ShellUI in your Blazor project.

```bash
dotnet shellui init
```

**Options:**
- `--force` - Overwrite existing files
- `--style <style>` - Choose CSS style (default: default)
- `--tailwind <method>` - Choose Tailwind method (standalone, npm) (default: standalone)
- `--yes` - Run in non-interactive mode with default options

### `add <components>`
Add one or more components to your project.

```bash
dotnet shellui add button input card
```

**Options:**
- `--force` - Overwrite existing components

### `list`
List available or installed components.

```bash
# List all available components
dotnet shellui list

# List installed components
dotnet shellui list --installed

# List available components
dotnet shellui list --available
```

### `remove <components>`
Remove components from your project.

```bash
dotnet shellui remove button input
```

**Options:**
- `--all` - Remove all installed components

### `theme` — bake tweakcn themes at build time

Fetch a theme from [tweakcn.com](https://tweakcn.com) and write it into your project. No runtime fetch, works offline, exactly-what-you-see-is-what-ships.

```bash
# Fresh project — init + apply theme in one step
dotnet shellui theme init https://tweakcn.com/themes/<id>

# Existing project — apply theme to wwwroot/input.css
dotnet shellui theme apply https://tweakcn.com/themes/<id>

# Emit standalone override CSS (for Path A/D consumers who use the precompiled bundle)
dotnet shellui theme apply https://tweakcn.com/themes/<id> --emit-override wwwroot/theme.css

# Re-fetch the theme recorded in shellui.theme.lock
dotnet shellui theme update
```

Each `apply` writes a `shellui.theme.lock` file (source URL + SHA-256) so `update` can refresh from the same source without you having to remember the URL. Re-applies are idempotent — user content outside the sentinel-marked region survives verbatim.

## Available Components

### Form Components
- `button`, `input`, `textarea`, `select`, `checkbox`, `switch`, `radio-group`, `slider`, `combobox`, `date-picker`, `time-picker`, `date-range-picker`, `input-otp`, `form`

### Layout Components
- `card`, `dialog`, `sheet`, `drawer`, `popover`, `tooltip`, `separator`, `scroll-area`, `resizable`, `collapsible`

### Navigation Components
- `navbar`, `sidebar`, `app-sidebar`, `navigation-menu`, `menubar`, `breadcrumb`, `pagination`, `tabs`

### Dashboard / Layout Blocks (shadcn-style)
- `dashboard-01` - Sidebar + content layout. Header scrolls with page.
- `dashboard-02` - Same layout with **sticky header** (breadcrumb bar stays fixed on scroll)

```bash
dotnet shellui add dashboard-01   # or dashboard-02
# Installs: sidebar, breadcrumb, separator, theme-toggle, app-sidebar + layout to Components/Layout/
```

### Data Display
- `table`, `data-table`, `badge`, `avatar`, `alert`, `toast`, `sonner`, `skeleton`, `progress`, `loading`

**68 installable components total** (run `dotnet shellui list` for full list; sub-components, variants, models, and services auto-install as dependencies)

### Interactive Components
- `dropdown`, `accordion`, `toggle`, `theme-toggle`, `command`, `context-menu`, `hover-card`

## Component Dependencies

When you install a component, its dependencies are automatically installed:

```bash
dotnet shellui add dialog
# Automatically installs: button (dependency)
```

## Project Structure

After running `init`, your project structure:

| Path | Purpose |
|------|---------|
| `Components/UI/` | Components are installed here |
| `wwwroot/input.css` | Tailwind input file |
| `wwwroot/app.css` | Compiled CSS (auto-generated) |
| `tailwind.config.js` | Tailwind configuration |
| `shellui.json` | ShellUI configuration |
| `Build/ShellUI.targets` | MSBuild integration |

## Updating Components

To update a component to the latest version:

```bash
dotnet shellui add button --force
```

## Troubleshooting

### Component not found
Make sure you're using the correct component name. Use `dotnet shellui list` to see all available components.

### Build errors
Ensure Tailwind CSS is properly configured:
1. Run `dotnet shellui init` if you haven't already
2. Check that `tailwind.config.js` exists
3. Verify `wwwroot/input.css` contains `@import "tailwindcss";`

### CLI not found
Reinstall the CLI tool:
```bash
dotnet tool uninstall -g ShellUI.CLI
dotnet tool install -g ShellUI.CLI
```

## Documentation

- [Full Documentation](https://shellui.dev/docs/cli)
- [Component Reference](https://shellui.dev/components)
- [GitHub Repository](https://github.com/shellui-dev/shellui)

## License

MIT License - see [LICENSE](https://github.com/shellui-dev/shellui/blob/main/LICENSE) for details.

