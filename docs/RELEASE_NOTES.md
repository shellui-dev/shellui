# ShellUI v0.3.0-alpha.1 🚧

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

## 📦 Installation

```bash
# Install CLI (alpha)
dotnet tool install -g ShellUI.CLI --version 0.3.0-alpha.1

# Or via NuGet
dotnet add package ShellUI.Components --version 0.3.0-alpha.1
```

Initialize and add components:
```bash
shellui init
shellui add button card chart
shellui add sonner stepper callout copy-button file-upload
```

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.2.0...v0.3.0-alpha.1

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
