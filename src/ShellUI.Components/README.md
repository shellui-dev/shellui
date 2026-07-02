# ShellUI Components

Beautiful, accessible Blazor components inspired by shadcn/ui. A CLI-first component library with Tailwind CSS styling.

## Features

- 🎨 **68 Production-ready installable components** - Button, Input, Card, Dialog, Sonner, Table, Charts, and more (sub-components and variants ship as auto-installed dependencies)
- 🎯 **CLI-first approach** - Install components individually with `dotnet shellui add`
- 🎨 **Tailwind CSS styling** - Utility-first CSS with dark mode support
- ♿ **Accessible by default** - Built with accessibility in mind
- 📱 **Responsive design** - Mobile-first approach
- 🔧 **Fully customizable** - Copy components to your project for full control

## Three install paths for this package

This NuGet package supports three install patterns. Pick the one that fits your project.

_(A fourth path — CDN via jsdelivr — is available for non-NuGet use cases like static pages and JSFiddle. See the main [ShellUI README](https://github.com/shellui-dev/shellui#path-d--cdn-no-nuget-static-html).)_

### 🚦 Quick decision matrix

| I want to… | Use |
|---|---|
| Ship the fastest — one `<link>` tag, no build config | **Path A** — precompiled bundle |
| Tree-shaken CSS, I already run Tailwind | **Path B** — safelist |
| Own the component source, restyle by editing `.razor` files | **Path C** — CLI |

---

### Path A — Precompiled CSS bundle (simplest)

**Who it's for:** you want components on screen with minimum ceremony. No Tailwind, no npm, no CLI.

**Setup:**

```bash
dotnet add package ShellUI.Components
```

```razor
@* App.razor <head> *@
<link href="_content/ShellUI.Components/shellui-all.css" rel="stylesheet" />

@* _Imports.razor *@
@using ShellUI.Components
```

Done. `<Button>`, `<Card>`, `<Dialog>` all render styled.

**Benefits:**
- ✅ Fastest to set up — 30 seconds from `dotnet new blazor` to styled components
- ✅ Zero build step for CSS — the package ships a pre-compiled ~77KB bundle
- ✅ No downloads at build time — no Tailwind binary, no npm install
- ✅ Deterministic — exact CSS shipped == exact CSS you see locally
- ✅ Works offline forever

**Trade-offs:**
- ❌ No tree-shaking — you pay ~77KB even if you use 3 components (negligible for most sites)
- ❌ Theme customization is via override only — theme vars are baked in. You add a `<style>` block *after* the link:
  ```html
  <link href="_content/ShellUI.Components/shellui-all.css" rel="stylesheet" />
  <style>
      :root { --primary: oklch(0.55 0.15 250); }  /* your color */
  </style>
  ```

**Best for:** new projects, prototypes, teams without existing Tailwind, "just get me components" scenarios.

---

### Path B — Safelist + your existing Tailwind (tree-shaken)

**Who it's for:** your project already runs Tailwind for your own utilities. You want ShellUI's classes compiled into the same output, tree-shaken.

**Setup:**

```bash
dotnet add package ShellUI.Components
```

The package copies `shellui-classes.txt` (safelist of 307 unique Tailwind classes ShellUI uses) into your `wwwroot/`. Point Tailwind at it:

```css
/* wwwroot/input.css */
@import "tailwindcss";
@source "./shellui-classes.txt";

/* Your theme — full source of truth, paste tweakcn output directly */
:root {
    --background: oklch(0.99 0 0);
    --foreground: oklch(0 0 0);
    /* ... */
}
```

```razor
@* _Imports.razor *@
@using ShellUI.Components
```

**Benefits:**
- ✅ Tree-shaken CSS — only classes your compiled app actually uses (~30-50KB typical)
- ✅ Single Tailwind build — ShellUI's classes + yours compile in one pass
- ✅ Your `input.css` is the theme source of truth — no cascade tricks
- ✅ Composable — mix with `@apply`, custom `@theme` blocks, your own utilities

**Trade-offs:**
- ❌ Requires an existing Tailwind pipeline
- ❌ Marginally more setup than Path A

**Best for:** existing Blazor projects that already run Tailwind, teams that care about CSS efficiency, projects with heavy custom styling.

---

### Path C — CLI (source ownership)

**Who it's for:** you want to **own the component source**. Copy `.razor` files into your project, edit freely, version them yourself. This is the shadcn philosophy.

**Setup:**

```bash
dotnet tool install -g ShellUI.CLI
shellui init                          # one-time
shellui add button card dialog        # any time you want more components
```

`shellui init` automatically:
- Downloads Tailwind standalone CLI (no Node.js needed) or uses your npm install
- Writes the full default theme to `wwwroot/input.css`
- Patches `App.razor` with render mode, theme bootstrap script, `shellui.js` tag
- Sets up MSBuild — Tailwind rebuilds on every `dotnet build`

`shellui add <name>` copies `.razor` files into `Components/UI/`. Edit them however you like.

**Benefits:**
- ✅ Source-level ownership — every component is your code
- ✅ Best theming experience — you own `wwwroot/input.css`, tweakcn pastes directly
- ✅ Zero external runtime deps — Tailwind rebuilds locally on `dotnet build`
- ✅ Automatic host wiring — `shellui init` patches App.razor for you
- ✅ Composable — mix with NuGet-installed components

**Trade-offs:**
- ❌ First `shellui init` downloads ~25MB Tailwind binary (cached in `.shellui/bin/`)
- ❌ Component updates are manual — you own the source, so you also merge upstream changes

**Best for:** custom design systems, projects heavily restyling ShellUI, teams that want everything in-repo, shadcn workflow followers.


## When is this NuGet package useful on its own?

- You're building a library that re-exports ShellUI components or extends them with custom variants
- You want to reference `Shell.Cn` (the class-name combiner) from your own helpers
- You want the JS interop registration (`shellui.js`) provided as a `_content/ShellUI.Components/` static asset

Otherwise: start with the CLI.

## CLI quick start

```bash
dotnet tool install -g ShellUI.CLI
shellui init           # one-time
shellui add button     # any time you want more components
shellui list           # see what's available
```

`shellui init` automatically:
- Downloads Tailwind CSS CLI (standalone, no Node.js required) or uses your existing npm install
- Writes the full default theme to `wwwroot/input.css` (`:root` / `.dark` / `@theme inline` blocks)
- Patches `Components/App.razor` (or `wwwroot/index.html` for WASM) with `@rendermode`, a theme-bootstrap `<script>` to avoid FOUC, and a `<script src="shellui.js">` tag for the JS interop
- Sets up MSBuild integration so Tailwind rebuilds on every `dotnet build`

`shellui add <name>` automatically:
- Resolves and installs all sub-component dependencies
- Runs `dotnet add package` for any required NuGet packages (e.g. `Blazor-ApexCharts` for charts, `System.Linq.Dynamic.Core` for `DataTable`)
- Auto-links any CSS assets (e.g. `wwwroot/css/charts.css`) into your `App.razor`
- Suggests close matches if you mistype a component name

## Using components

After `shellui init` + `shellui add`, components are in your project source:

```razor
@using YourProject.Components.UI

<Button Variant="ButtonVariant.Default">Click me</Button>
<Card>
    <CardHeader>
        <CardTitle>Hello World</CardTitle>
    </CardHeader>
    <CardContent>
        <p>This is a card component.</p>
    </CardContent>
</Card>
```

## Available Components

### Form Components
- **Button** - Various variants and sizes
- **Input** - Text input with validation
- **Textarea** - Multi-line text input
- **Select** - Dropdown selection
- **Checkbox** - Checkbox input
- **Switch** - Toggle switch
- **RadioGroup** - Radio button groups
- **Slider** - Range slider
- **Combobox** - Searchable dropdown
- **DatePicker** - Date selection
- **TimePicker** - Time selection
- **DateRangePicker** - Date range selection
- **InputOTP** - One-time password input
- **Form** - Form wrapper with validation

### Layout Components
- **Card** - Content container
- **Dialog** - Modal dialog
- **Sheet** - Side panel
- **Drawer** - Mobile drawer
- **Popover** - Floating content
- **Tooltip** - Hover tooltip
- **Separator** - Visual divider
- **ScrollArea** - Custom scrollable area
- **Resizable** - Resizable panels
- **Collapsible** - Collapsible content

### Navigation Components
- **Navbar** - Top navigation bar
- **Sidebar** - Side navigation
- **NavigationMenu** - Navigation menu
- **Menubar** - Menu bar
- **Breadcrumb** - Breadcrumb navigation
- **Pagination** - Page navigation
- **Tabs** - Tab navigation

### Data Display
- **Table** - Data table
- **Badge** - Status badges
- **Avatar** - User avatars
- **Alert** - Alert messages
- **Toast** - Toast notifications
- **Skeleton** - Loading placeholders
- **Progress** - Progress indicators

### Interactive Components
- **Dropdown** - Dropdown menu
- **Accordion** - Collapsible sections
- **Toggle** - Toggle button
- **ThemeToggle** - Dark/light mode toggle

## Bootstrap Compatibility

ShellUI components work alongside Bootstrap. You can:

- **Keep both** - ShellUI and Bootstrap can coexist
- **Remove Bootstrap** - Delete Bootstrap references if you prefer Tailwind-only
- **Gradual migration** - Use ShellUI for new components, keep Bootstrap for existing ones

## Customization

### 🎨 Theme Customization

**Easily customize themes with [tweakcn](https://tweakcn.com/):**

1. Design your perfect theme on tweakcn
2. Copy the generated CSS variables
3. Paste into `wwwroot/input.css`
4. All ShellUI components update automatically!

**Custom Fonts:** Add Google Fonts links and update `--font-*` variables in your CSS.

### Component Customization

Components are copied to your project, giving you full control:

```bash
# Components are installed to Components/UI/
Components/
  UI/
    Button.razor      # Edit directly!
    Input.razor       # Customize as needed
    Card.razor        # Full ownership
    # ... other components
```

Modify any component to match your design system perfectly!

## Documentation

- [Component Gallery](https://shellui.dev/components)
- [Installation Guide](https://shellui.dev/docs/installation)
- [Customization](https://shellui.dev/docs/customization)

## Contributing

We welcome contributions! Please see our [Contributing Guide](https://github.com/shellui-dev/shellui/blob/main/CONTRIBUTING.md).

## License

MIT License - see [LICENSE](https://github.com/shellui-dev/shellui/blob/main/LICENSE) for details.

## Support

- 📖 [Documentation](https://shellui.dev)
- 🐛 [Issues](https://github.com/shellui-dev/shellui/issues)
- 💬 [Discussions](https://github.com/shellui-dev/shellui/discussions)
