<p align="center">
  <picture>
    <source media="(prefers-color-scheme: dark)" srcset="images/Shellui-dark.png">
    <source media="(prefers-color-scheme: light)" srcset="images/Shellui-light.png">
    <img alt="ShellUI Logo" src="images/Shellui-light.png" width="120">
  </picture>
</p>

<h1 align="center">ShellUI</h1>

<p align="center">
  A modern, CLI-first Blazor component library inspired by <a href="https://ui.shadcn.com/">shadcn/ui</a>.<br/>
  Copy components directly into your project and customize them to match your needs.
</p>

<p align="center">
  <a href="https://github.com/shellui-dev/shellui"><img src="https://img.shields.io/github/stars/shellui-dev/shellui?style=flat-square" alt="GitHub stars"></a>
  <a href="https://www.nuget.org/packages/ShellUI.Components"><img src="https://img.shields.io/nuget/v/ShellUI.Components?style=flat-square&logo=nuget&color=004880" alt="NuGet"></a>
  <a href="https://www.nuget.org/packages/ShellUI.CLI"><img src="https://img.shields.io/nuget/v/ShellUI.CLI?style=flat-square&logo=nuget&label=CLI&color=004880" alt="CLI"></a>
  <a href="https://github.com/shellui-dev/shellui/blob/main/LICENSE.txt"><img src="https://img.shields.io/badge/license-MIT-blue?style=flat-square" alt="License"></a>
</p>

<p align="center">
  <a href="https://star-history.com/#shellui-dev/shellui&Date">
    <img height="150" alt="Star History Chart" src="https://api.star-history.com/svg?repos=shellui-dev/shellui&type=Date" />
  </a>
</p>

---

**New here? Check out the [Quick Start](#quick-start) section below!**

## Vision

ShellUI transforms Blazor component development with a hybrid approach:
- **CLI-First**: Copy components to YOUR codebase for full control (`shellui add button`)
- **NuGet Option**: Traditional package install for quick starts (`dotnet add package ShellUI.Components`)
- **Choose your workflow**: Use CLI for customization, NuGet for speed, or mix both
- Powered by Tailwind CSS v4.3.1 (standalone CLI - no Node.js required!)
- Best of both worlds: flexibility when you need it, convenience when you want it

## Current Status: 68 Components (Alpha) 🎉

**ShellUI is in alpha!** Test and provide feedback before we ship stable. We've completed:
- ✅ **CLI Tool** (`dotnet tool install -g ShellUI.CLI`)
- ✅ **NuGet Package** (`dotnet add package ShellUI.Components`)
- ✅ **68 Installable Components** with Tailwind v4.3.1 *(top-level components you `shellui add` — sub-components, variants, models, and services auto-install as dependencies)*
- ✅ **Hybrid Workflow** (CLI + NuGet)
- ✅ **No Node.js Required** (Standalone Tailwind CLI)
- ✅ **Comprehensive Documentation**
- ✅ **Working Demos & Examples**

**Ready to use today!** 🚀

## What's Working Today 🚀

### ✅ CLI Tool + NuGet Package
```bash
# Install CLI globally
dotnet tool install -g ShellUI.CLI

# Initialize in your Blazor project (choose npm or standalone)
shellui init

# For CI/CD or automated environments:
shellui init --yes  # Uses standalone Tailwind with default options

# Add components
shellui add button input card dialog

# List all available components
shellui list
```

**⚠️ Note:** The CLI tool must be installed first. Use `shellui` commands (not `dotnet shellui`).

## 📦 Unified Versioning System

ShellUI uses a **centralized versioning system** where all components, CLI tool, and packages share the same version number. This ensures consistency and simplifies dependency management.

### Version Update Process

To update ShellUI version across all components:

1. **Edit `Directory.Build.props`** in the repository root:
   ```xml
   <ShellUIVersion>0.3.0</ShellUIVersion>
   <ShellUIVersionSuffix></ShellUIVersionSuffix>  <!-- Leave empty for stable releases -->
   ```

2. **Clean and rebuild** all projects:
   ```bash
   dotnet clean
   dotnet build --configuration Release
   ```

This single file change updates:
- ✅ All NuGet packages (`ShellUI.CLI`, `ShellUI.Components`)
- ✅ All component templates (68 installable components)
- ✅ Build configurations and metadata

**Example for pre-release:**
```xml
<ShellUIVersion>0.3.0</ShellUIVersion>
<ShellUIVersionSuffix>alpha.2</ShellUIVersionSuffix>
```
Results in version: `0.3.0-alpha.2`

### Component Versioning Strategy

**By Design:** All components share the same version because they:
- Work together as a cohesive system
- Depend on shared utilities and theming
- Follow consistent design patterns
- Are tested together

**For Advanced Users:** Future versions may support component-specific versioning for power users who need granular control.

### ✅ 68 Production-Ready Components

Counts below are top-level components you can `shellui add` directly. Sub-components (e.g. `SidebarTrigger`, `DialogContent`, `TableRow`), variants (`ButtonVariants`, `AlertVariants`, …), models, and services auto-install as dependencies and are not counted.

**Form (17):**
Button, Checkbox, Combobox, DatePicker, DateRangePicker, FileUpload, Form, Input, InputOTP, Label, RadioGroup, Select, Slider, Switch, Textarea, TimePicker, Toggle

**Layout (12):**
Accordion, Breadcrumb, Card, Collapsible, DashboardLayout01, DashboardLayout02, LinkCard, Navbar, Resizable, ScrollArea, Separator, Sidebar

**Navigation (7):**
ContextMenu, Menubar, NavigationMenu, Pagination, PrevNextNav, Stepper, Tabs

**Overlay (8):**
AlertDialog, Command, Dialog, Drawer, Dropdown, HoverCard, Popover, Sheet

**Data Display (13):**
AreaChart, Avatar, Badge, BarChart, Calendar, Carousel, Chart, ChartSeries, DataTable, LineChart, MultiSeriesChart, PieChart, Table

**Feedback (9):**
Alert, Callout, EmptyState, Loading, Progress, Skeleton, Sonner, Toast, Tooltip

**Utility (2):**
CopyButton, ThemeToggle

### ✅ Tailwind CSS v4.3.1 Integration

**Two Setup Methods:**

**Method 1: Standalone CLI (No Node.js!)**
```bash
shellui init  # Choose "standalone"
# Or: dotnet shellui init
```
- Downloads Tailwind CLI binary automatically
- No Node.js or npm required
- Auto-builds on project compile

**Method 2: npm (If you prefer)**
```bash
shellui init  # Choose "npm"
# Or: dotnet shellui init
```
- Installs `tailwindcss@^4.3.1` + `@tailwindcss/cli@^4.3.1`
- Uses `npx @tailwindcss/cli` for builds
- Requires Node.js

### 🎨 Easy Theme Customization

**Customize themes instantly with [tweakcn](https://tweakcn.com/):**

1. Visit tweakcn and design your perfect theme
2. Copy the generated CSS variables
3. Paste into `wwwroot/input.css`
4. All ShellUI components update automatically!

**Custom fonts?** Add Google Fonts links and update your CSS variables - works seamlessly! 🔤

## What's Next

### Phase 1: Additional Components
**Target: Q1 2026**

- More advanced components
- Enhanced DataTable features (sorting/filtering)
- Charts and data visualization
- VirtualScroll for large lists
- PDFViewer component
- And more...

### Phase 2: Documentation & Polish
**Target: Q2 2026**

- Complete documentation website
- Video tutorials
- Migration guides
- Performance optimization
- Testing infrastructure

## Design Principles

1. **Copy, Don't Install**: Components are copied to your project, not imported from a package
2. **Tailwind-First**: All styling uses Tailwind CSS v4.3.1 utility classes
3. **Accessible by Default**: WCAG 2.1 AA compliant out of the box
4. **Composable**: Build complex components from simple ones
5. **Customizable**: Modify any component to fit your needs
6. **Type-Safe**: Leverage C# type system for better DX
7. **Performance**: Optimized for both Server and WASM scenarios
8. **No Node.js Required**: Standalone Tailwind CLI for maximum compatibility

## Architecture Decisions

### Why Hybrid Approach (CLI + NuGet)?
**CLI Benefits:**
- Full control over component code
- Customize without forking
- Only include what you use (smaller bundles)
- No version lock-in
- Better debugging experience

**NuGet Benefits:**
- Traditional workflow developers know
- Faster initial setup
- Automatic updates via package manager
- Good for prototyping
- Team familiarity

**Use both:** Start with NuGet, migrate to CLI for components you customize heavily!

### Why Tailwind v4.3.1?
- Latest stable version with v4 features
- Better performance than v3
- Improved dark mode support
- Native CSS variable support
- Smaller output CSS
- Standalone CLI (no Node.js required!)

### Component Structure

- `Components/UI/` - ShellUI components (Button.razor, Input.razor, Card.razor, ...)
- `Components/UI/Variants/` - Variant classes (*Variants.cs)
- `wwwroot/` - input.css, app.css (compiled)
- `.shellui/bin/` - Tailwind CLI binary (standalone)
- `tailwind.config.js`, `shellui.json`
- `Build/ShellUI.targets` - MSBuild integration

## Developer Experience Today

### Quick Start
```bash
# Install CLI globally
dotnet tool install -g ShellUI.CLI

# Initialize in your Blazor project (choose npm or standalone)
shellui init
# Or: dotnet shellui init

# Add components
shellui add button input card dialog
# Or: dotnet shellui add button input card dialog

shellui list  # See all 68 available components
# Or: dotnet shellui list
```

### Use Components
```razor
@page "/example"

<Card>
    <CardHeader>
        <CardTitle>Welcome to ShellUI</CardTitle>
        <CardDescription>Build beautiful Blazor apps</CardDescription>
    </CardHeader>
    <CardContent>
        <Input Placeholder="Enter your email" Type="email" />
        <Button Class="mt-4">Subscribe</Button>
    </CardContent>
</Card>
```

### Customize Components
Simply edit the component file in `Components/UI/` - it's yours to modify!

## Technical Requirements

- .NET 8.0 or higher
- **Choice of Tailwind setup:**
  - **Standalone CLI** (recommended): No Node.js required
  - **npm**: Requires Node.js, uses `tailwindcss@^4.3.1`

## Comparison with Existing Solutions

| Feature | ShellUI | MudBlazor | Radzen | Blazorise |
|---------|---------|-----------|--------|-----------|
| CLI Installation | ✅ | ❌ | ❌ | ❌ |
| NuGet Package | ✅ | ✅ | ✅ | ✅ |
| Component Ownership (CLI) | ✅ | ❌ | ❌ | ❌ |
| Tailwind CSS | ✅ (v4.3.1) | ❌ | ❌ | ❌ |
| No Node.js Required | ✅ | N/A | N/A | N/A |
| Hybrid Workflow | ✅ | ❌ | ❌ | ❌ |
| Free & Open Source | ✅ | ✅ | Partial | ✅ |
| Customization | Full | Limited | Limited | Limited |
| Components | 68 | 70+ | 50+ | 80+ |
| Current Status | Production Ready | Mature | Commercial | Mature |

## 📦 Package Overview

ShellUI ships two NuGet packages — the CLI is the primary install path; the runtime DLL is optional.

| Package | Type | Required? | Purpose |
|---------|------|-----------|---------|
| `ShellUI.CLI` | .NET global tool | ✅ Yes — primary install path | Sets up Tailwind, theme CSS, patches `App.razor`, and copies component source so Tailwind can scan it |
| `ShellUI.Components` | Razor class library | Optional | Runtime DLL with the same components + `shellui.js` interop + `Shell.Cn` helper. Useful when you want to reference component types from your own code or build a library on top of ShellUI |

`ShellUI.Core` and `ShellUI.Templates` are internal to the CLI and not published to NuGet — consumers never reference them directly.

## Installation

Two paths — pick based on whether you already have Tailwind set up.

### Path A — NuGet + your existing Tailwind setup (new in 0.4.x)

If your project already builds Tailwind, install the runtime DLL via NuGet and add one line to your `input.css`:

```bash
dotnet add package ShellUI.Components
```

The package ships a `shellui-classes.txt` safelist at `wwwroot/shellui-classes.txt` in your project. Wire Tailwind to scan it:

```css
/* wwwroot/input.css */
@import "tailwindcss";
@source "./shellui-classes.txt";
```

`@using ShellUI.Components` in your `_Imports.razor` and you're done. Tailwind tree-shakes — only classes that actually appear in your code (yours + ShellUI's) end up in the compiled CSS.

### Path B — CLI tool (full source control, no Tailwind setup required)

If you want zero Tailwind config of your own, or want to edit component source directly, use the CLI:

```bash
dotnet tool install -g ShellUI.CLI
shellui init                          # one-time setup — installs Tailwind, theme CSS, patches App.razor
shellui add button card dialog        # any time you want more components
```

The CLI copies the `.razor` files into your project. You own them — edit them however you like.

### Which one?

| You want… | Use |
|---|---|
| Full source control over every component | CLI |
| Restyle by editing the component source | CLI |
| Minimal install, you already use Tailwind | NuGet |
| Mix — NuGet for most, CLI for ones you customize | both (no conflict; NuGet's classes are tree-shaken via the safelist, CLI-copied components Tailwind scans directly)

### Configure Tailwind CSS manually (advanced)
Create/update `wwwroot/tailwind.config.js`:
```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./**/*.{razor,html,cshtml}",
    "./wwwroot/js/**/*.js"
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
```

Create `wwwroot/input.css`:
```css
@import "tailwindcss";

/* ShellUI Theme Variables - Light Mode */
:root {
  --background: oklch(0.9900 0 0);
  --foreground: oklch(0 0 0);
  --card: oklch(1 0 0);
  --card-foreground: oklch(0 0 0);
  --popover: oklch(0.9900 0 0);
  --popover-foreground: oklch(0 0 0);
  --primary: oklch(0 0 0);
  --primary-foreground: oklch(1 0 0);
  --secondary: oklch(0.9400 0 0);
  --secondary-foreground: oklch(0 0 0);
  --muted: oklch(0.9700 0 0);
  --muted-foreground: oklch(0.4400 0 0);
  --accent: oklch(0.9400 0 0);
  --accent-foreground: oklch(0 0 0);
  --destructive: oklch(0.6300 0.1900 23.0300);
  --destructive-foreground: oklch(1 0 0);
  --border: oklch(0.9200 0 0);
  --input: oklch(0.9400 0 0);
  --ring: oklch(0 0 0);
  --chart-1: oklch(0.8100 0.1700 75.3500);
  --chart-2: oklch(0.5500 0.2200 264.5300);
  --chart-3: oklch(0.7200 0 0);
  --chart-4: oklch(0.9200 0 0);
  --chart-5: oklch(0.5600 0 0);
  --sidebar: oklch(0.9900 0 0);
  --sidebar-foreground: oklch(0 0 0);
  --sidebar-primary: oklch(0 0 0);
  --sidebar-primary-foreground: oklch(1 0 0);
  --sidebar-accent: oklch(0.9400 0 0);
  --sidebar-accent-foreground: oklch(0 0 0);
  --sidebar-border: oklch(0.9400 0 0);
  --sidebar-ring: oklch(0 0 0);
  --font-sans: Geist, sans-serif;
  --font-serif: Georgia, serif;
  --font-mono: Geist Mono, monospace;
  --radius: 0.5rem;
  --shadow-x: 0px;
  --shadow-y: 1px;
  --shadow-blur: 2px;
  --shadow-spread: 0px;
  --shadow-opacity: 0.18;
  --shadow-color: hsl(0 0% 0%);
  --shadow-2xs: 0px 1px 2px 0px hsl(0 0% 0% / 0.09);
  --shadow-xs: 0px 1px 2px 0px hsl(0 0% 0% / 0.09);
  --shadow-sm: 0px 1px 2px 0px hsl(0 0% 0% / 0.18), 0px 1px 2px -1px hsl(0 0% 0% / 0.18);
  --shadow: 0px 1px 2px 0px hsl(0 0% 0% / 0.18), 0px 1px 2px -1px hsl(0 0% 0% / 0.18);
  --shadow-md: 0px 1px 2px 0px hsl(0 0% 0% / 0.18), 0px 2px 4px -1px hsl(0 0% 0% / 0.18);
  --shadow-lg: 0px 1px 2px 0px hsl(0 0% 0% / 0.18), 0px 4px 6px -1px hsl(0 0% 0% / 0.18);
  --shadow-xl: 0px 1px 2px 0px hsl(0 0% 0% / 0.18), 0px 8px 10px -1px hsl(0 0% 0% / 0.18);
  --shadow-2xl: 0px 1px 2px 0px hsl(0 0% 0% / 0.45);
  --tracking-normal: 0em;
  --spacing: 0.25rem;
}

/* Dark Mode Variables */
.dark {
  --background: oklch(0 0 0);
  --foreground: oklch(1 0 0);
  --card: oklch(0.1400 0 0);
  --card-foreground: oklch(1 0 0);
  --popover: oklch(0.1800 0 0);
  --popover-foreground: oklch(1 0 0);
  --primary: oklch(1 0 0);
  --primary-foreground: oklch(0 0 0);
  --secondary: oklch(0.2500 0 0);
  --secondary-foreground: oklch(1 0 0);
  --muted: oklch(0.2300 0 0);
  --muted-foreground: oklch(0.7200 0 0);
  --accent: oklch(0.3200 0 0);
  --accent-foreground: oklch(1 0 0);
  --destructive: oklch(0.6900 0.2000 23.9100);
  --destructive-foreground: oklch(0 0 0);
  --border: oklch(0.2600 0 0);
  --input: oklch(0.3200 0 0);
  --ring: oklch(0.7200 0 0);
  --chart-1: oklch(0.8100 0.1700 75.3500);
  --chart-2: oklch(0.5800 0.2100 260.8400);
  --chart-3: oklch(0.5600 0 0);
  --chart-4: oklch(0.4400 0 0);
  --chart-5: oklch(0.9200 0 0);
  --sidebar: oklch(0.1800 0 0);
  --sidebar-foreground: oklch(1 0 0);
  --sidebar-primary: oklch(1 0 0);
  --sidebar-primary-foreground: oklch(0 0 0);
  --sidebar-accent: oklch(0.3200 0 0);
  --sidebar-accent-foreground: oklch(1 0 0);
  --sidebar-border: oklch(0.3200 0 0);
  --sidebar-ring: oklch(0.7200 0 0);
  --font-sans: Geist, sans-serif;
  --font-serif: Georgia, serif;
  --font-mono: Geist Mono, monospace;
  --radius: 0.5rem;
  --shadow-x: 0px;
  --shadow-y: 1px;
  --shadow-blur: 2px;
  --shadow-spread: 0px;
  --shadow-opacity: 0.18;
  --shadow-color: hsl(0 0% 0%);
  --shadow-2xs: 0px 1px 2px 0px hsl(0 0% 0% / 0.09);
  --shadow-xs: 0px 1px 2px 0px hsl(0 0% 0% / 0.09);
  --shadow-sm: 0px 1px 2px 0px hsl(0 0% 0% / 0.18), 0px 1px 2px -1px hsl(0 0% 0% / 0.18);
  --shadow: 0px 1px 2px 0px hsl(0 0% 0% / 0.18), 0px 1px 2px -1px hsl(0 0% 0% / 0.18);
  --shadow-md: 0px 1px 2px 0px hsl(0 0% 0% / 0.18), 0px 2px 4px -1px hsl(0 0% 0% / 0.18);
  --shadow-lg: 0px 1px 2px 0px hsl(0 0% 0% / 0.18), 0px 4px 6px -1px hsl(0 0% 0% / 0.18);
  --shadow-xl: 0px 1px 2px 0px hsl(0 0% 0% / 0.18), 0px 8px 10px -1px hsl(0 0% 0% / 0.18);
  --shadow-2xl: 0px 1px 2px 0px hsl(0 0% 0% / 0.45);
}

/* Tailwind Theme Integration */
@theme inline {
  --color-background: var(--background);
  --color-foreground: var(--foreground);
  --color-card: var(--card);
  --color-card-foreground: var(--card-foreground);
  --color-popover: var(--popover);
  --color-popover-foreground: var(--popover-foreground);
  --color-primary: var(--primary);
  --color-primary-foreground: var(--primary-foreground);
  --color-secondary: var(--secondary);
  --color-secondary-foreground: var(--secondary-foreground);
  --color-muted: var(--muted);
  --color-muted-foreground: var(--muted-foreground);
  --color-accent: var(--accent);
  --color-accent-foreground: var(--accent-foreground);
  --color-destructive: var(--destructive);
  --color-destructive-foreground: var(--destructive-foreground);
  --color-border: var(--border);
  --color-input: var(--input);
  --color-ring: var(--ring);
  --color-chart-1: var(--chart-1);
  --color-chart-2: var(--chart-2);
  --color-chart-3: var(--chart-3);
  --color-chart-4: var(--chart-4);
  --color-chart-5: var(--chart-5);
  --color-sidebar: var(--sidebar);
  --color-sidebar-foreground: var(--sidebar-foreground);
  --color-sidebar-primary: var(--sidebar-primary);
  --color-sidebar-primary-foreground: var(--sidebar-primary-foreground);
  --color-sidebar-accent: var(--sidebar-accent);
  --color-sidebar-accent-foreground: var(--sidebar-accent-foreground);
  --color-sidebar-border: var(--sidebar-border);
  --color-sidebar-ring: var(--sidebar-ring);

  --font-sans: var(--font-sans);
  --font-mono: var(--font-mono);
  --font-serif: var(--font-serif);

  --radius-sm: calc(var(--radius) - 4px);
  --radius-md: calc(var(--radius) - 2px);
  --radius-lg: var(--radius);
  --radius-xl: calc(var(--radius) + 4px);

  --shadow-2xs: var(--shadow-2xs);
  --shadow-xs: var(--shadow-xs);
  --shadow-sm: var(--shadow-sm);
  --shadow: var(--shadow);
  --shadow-md: var(--shadow-md);
  --shadow-lg: var(--shadow-lg);
  --shadow-xl: var(--shadow-xl);
  --shadow-2xl: var(--shadow-2xl);
}
```

Update `wwwroot/app.css` (or create it):
```css
@import "./input.css";
```

#### 5. **Reference the compiled CSS**
Add the link to your layout (`_Layout.cshtml` or `MainLayout.razor`):
```html
<link href="app.css" rel="stylesheet" />
```

This is what `shellui init` does for you automatically; documented here for the manual path.

**⚠️ Reminder:** even with all the manual setup above, you still need to copy each component's `.razor` source into your project (`shellui add <name>`) so Tailwind sees the classes it uses. There's no way around this for Tailwind v4 short of pre-compiling a complete CSS bundle and shipping it with the package — which is what the [v0.4.x NuGet-only path](https://github.com/shellui-dev/shellui/issues) will deliver.

## Contributing

ShellUI is production-ready! We welcome contributions:

- 🐛 **Bug reports** via GitHub Issues
- 💡 **Feature requests** for new components
- 📝 **Documentation improvements**
- 🧪 **Testing and feedback**

## License

MIT License - See LICENSE.txt for details

## Acknowledgments

- Inspired by [shadcn/ui](https://ui.shadcn.com/) for the CLI-first approach
- Forked from [Sysinfocus simple/ui](https://github.com/Sysinfocus/simple-ui) by [@sysinfocus](https://github.com/Sysinfocus)
- Built with love for the Blazor community

## Documentation

**Quick Links:**
- [tailwind-setup.md](docs/tailwind-setup.md) - Tailwind CSS setup guide
- [COMPONENT_ROADMAP.md](docs/COMPONENT_ROADMAP.md) - Component development roadmap

**Installation & Usage:**
- [QUICKSTART.md](docs/QUICKSTART.md) - Quick start guide
- [FAQ.md](docs/FAQ.md) - Frequently asked questions
- [CLI_SYNTAX.md](docs/CLI_SYNTAX.md) - CLI command reference

**Architecture:**
- [ARCHITECTURE.md](docs/ARCHITECTURE.md) - System architecture, diagrams, and design decisions

## Status

**Alpha:** 68 components, CLI + NuGet, Tailwind v4.3.1. Test before stable.
**🚀 Ready to use today!**

---

**ShellUI is fully functional and ready for production use!** 🎉
