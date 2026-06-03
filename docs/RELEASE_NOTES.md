# ShellUI v0.3.0-alpha.3 🚧

> Third alpha of v0.3.0 — dashboard sidebar blocks, JS install fixes, and NuGet Core packaging. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## What's in this release

v0.3.0-alpha.3 adds shadcn-style dashboard layout blocks, completes the sidebar component install chain (including JavaScript interop), and fixes NuGet so `ShellUI.Components` can resolve `ShellUI.Core`.

### ✨ New

- **Dashboard layout blocks** — `dotnet shellui add dashboard-01` (header scrolls, shadcn sidebar-01) and `dashboard-02` (sticky header, shadcn sidebar-02)
- **`shellui-js` template** — installs `wwwroot/shellui.js` at init and as a dependency of `copy-button` / `file-upload`
- **Blocks demo** — `/blocks` page with live previews, install commands, and `CopyButton` per block
- **`DashboardDemoSidebar`** — isolated demo sidebar content (separate from main app sidebar)

### 🐛 Fixes

- **NuGet: `ShellUI.Core` now published** — `ShellUI.Core` is packable; release workflow pushes Core + Components so `dotnet add package ShellUI.Components` restores successfully (was NU1102 when only Core 0.1.0 existed on NuGet)
- **Dependency auto-install** — variant/service templates (`alert-variants`, `badge-variants`, `avatar-variants`, `sonner-variants`, `sonner-service`, `toggle-variants`) marked `IsAvailable = false` so they install only with their parent
- **Breadcrumb** — single `dotnet shellui add breadcrumb`; `breadcrumb-item` auto-installed
- **Blazor error UI** — hidden until an error occurs (`.show` class)
- **Init** — installs `shellui.js` and reminds you to add `<script src="shellui.js"></script>` in App.razor or index.html

### 🔧 Sidebar / JS

| Asset | Installed when | CLI usage |
|-------|----------------|-----------|
| `shellui-sidebar.js` | `sidebar-provider` → `sidebar-js` | Dynamic import in `SidebarProvider` |
| `shellui.js` | `init`, `copy-button`, `file-upload` | Global `ShellUI.*`; requires script tag in HTML |

NuGet consumers: both JS files ship as static web assets under `_content/ShellUI.Components/`.

### 📦 Installation

```bash
# CLI
dotnet tool install -g ShellUI.CLI --version 0.3.0-alpha.3
# or
dotnet tool update -g ShellUI.CLI --version 0.3.0-alpha.3

shellui init
shellui add dashboard-01   # or dashboard-02
```

```bash
# NuGet (requires ShellUI.Core on NuGet at same version)
dotnet add package ShellUI.Core --version 0.3.0-alpha.3 --prerelease
dotnet add package ShellUI.Components --version 0.3.0-alpha.3 --prerelease
```

After `shellui init`, add before the Blazor script:

```html
<script src="shellui.js"></script>
```

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.3.0-alpha.2...v0.3.0-alpha.3

---

# ShellUI v0.3.0-alpha.2 🚧 (Historical)

> Second alpha of v0.3.0 — fixes CLI registry for Drawer/Sheet compositional subcomponents. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## What's in this release

v0.3.0-alpha.2 fixes the CLI template registry so that `Drawer` and `Sheet` correctly ship with their compositional subcomponent pattern introduced in alpha.1.

### 🐛 Fixes (from alpha.1)

- **Drawer/Sheet templates upgraded to v0.3.0** — `Open`/`OpenChanged` replaces `IsOpen`/`IsOpenChanged`, `DrawerSide`/`SheetSide` enums replace string-based side props, explicit `Compositional` parameter enables subcomponent mode
- **Missing subcomponent templates added** — `DrawerTrigger`, `DrawerContent`, `SheetTrigger`, `SheetContent` were missing from the registry entirely
- **Missing variant templates added** — `DrawerVariants` (with `DrawerSide` enum) and `SheetVariants` (with `SheetSide` enum) now installable
- **Dependency direction fixed** — `shellui add drawer` now auto-installs all 4 files (Drawer, DrawerVariants, DrawerTrigger, DrawerContent); same for `shellui add sheet`

### ✨ What changed

| Command | alpha.1 | alpha.2 |
|---|---|---|
| `shellui add drawer` | Installed old v0.2.x Drawer only | Installs Drawer + DrawerVariants + DrawerTrigger + DrawerContent |
| `shellui add sheet` | Installed old v0.2.x Sheet only | Installs Sheet + SheetVariants + SheetTrigger + SheetContent |

### 🔧 Improvements
- **Version bump** — 0.3.0-alpha.1 → 0.3.0-alpha.2
- **Subcomponent dependency pattern** — Follows parent-owns-children convention (same as Dialog, Collapsible, etc.)

## 📦 Installation

```bash
# Install CLI (alpha)
dotnet tool install -g ShellUI.CLI --version 0.3.0-alpha.2

# Or upgrade from alpha.1
dotnet tool update -g ShellUI.CLI --version 0.3.0-alpha.2
```

Initialize and add components:
```bash
shellui init
shellui add drawer       # installs Drawer + DrawerVariants + DrawerTrigger + DrawerContent
shellui add sheet        # installs Sheet + SheetVariants + SheetTrigger + SheetContent
```

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.3.0-alpha.1...v0.3.0-alpha.2

---

# ShellUI v0.3.0-alpha.1 🚧 (Historical)

> First alpha of v0.3.0 — test thoroughly before stable. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## What's in this release

v0.3.0-alpha.1 includes everything from v0.2.0 (charts, Tailwind 4.x) plus new components and improvements.

### ✨ New Components

**Docs essentials:**
- **Callout** / **CalloutVariants** — Info, warning, tip, danger admonition boxes
- **CopyButton** — One-click copy to clipboard
- **LinkCard** — Card-style links for related pages
- **PrevNextNav** — Previous/Next page navigation for docs

**Feedback & Data:**
- **Sonner** / **SonnerService** / **SonnerVariants** — Modern toast notifications (shadcn-style)
- **Stepper** / **StepperList** / **StepperStep** / **StepperContent** — Step-by-step wizard with value-based API
- **ChartVariants** — Variant styling support for charts


### 🔧 Improvements
- **Version bump** — 0.2.0 → 0.3.0-alpha.1 across all packages
- **CI/CD** — NuGet caching, explicit solution paths, concurrency, pre-release tag support
- **Documentation** — Tailwind setup guide now generic for Blazor, versioning strategy updated for alpha workflow
- **Release workflow** — Tag pattern updated to match `v0.3.0-alpha.1`-style prereleases

### ⚠️ Known issues
- **Stepper** — Active-state highlighting may not always reflect current step; documented, shipping as-is

---

# ShellUI v0.2.0 📊 (Historical)

> Feature release - Charts & Data Visualization

## ✨ New Features

### Charts & Data Visualization (7 new components)
Built on **ApexCharts.Blazor** with full ShellUI theme integration:

- **Chart** - Base chart component with theme-aware styling
- **BarChart** - Vertical bar charts
- **LineChart** - Smooth line charts
- **AreaChart** - Filled area charts
- **PieChart** - Pie charts with custom tooltips
- **MultiSeriesChart** - Multiple data series on one chart
- **ChartSeries** - Flexible series composition

### Chart Themes
Three built-in color themes via `ChartTheme` enum:
- **Default** - Professional blue-based palette (blue, red, green, yellow, purple)
- **Colorful** - Vibrant multi-color palette with 7 colors
- **Monochrome** - Slate grays for minimal aesthetic

### Theme-Aware Chart Containers
Charts automatically use your theme's CSS variables:
- `var(--radius)` for border radius
- `var(--shadow)` for box shadow
- `var(--border)`, `var(--card)`, `var(--card-foreground)` for colors

### Custom Tooltips
Fully custom HTML tooltips replacing ApexCharts defaults:
- Compact, shadcn-inspired design
- Proper marker/text alignment via flexbox
- Multi-series support (shows all values at a data point)
- Light/dark mode support via CSS variables
- Separate pie chart tooltip using `seriesIndex`

## 🐛 Bug Fixes

- **Fixed version mismatch** - Component versions now correctly read from assembly metadata instead of hardcoded fallback
- **Fixed Tailwind version in config** - `shellui.json` now correctly shows Tailwind v4.1.18 for all install methods
- **Fixed CS1998 warnings** - Removed unnecessary `async` from synchronous methods in `ComponentInstaller`

## 🔧 Improvements

- **Tailwind CSS updated to v4.1.18**
- **Component count: 100** installable (dependencies like *-variants auto-installed)
- **X-axis labels** - Charts use `XAxisType.Category` for proper string label display
- **Version system** - Fallback version now reads `AssemblyInformationalVersion` baked at build time

## 📦 Installation

```bash
# Install CLI globally
dotnet tool install -g ShellUI.CLI

# Initialize your project
shellui init

# Add chart components
shellui add chart bar-chart line-chart pie-chart area-chart multi-series-chart
```

Or via NuGet:
```bash
dotnet add package ShellUI.Components
```

**Note:** Charts require the `Blazor-ApexCharts` NuGet package:
```bash
dotnet add package Blazor-ApexCharts
```

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

---

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.1.1...v0.2.0
