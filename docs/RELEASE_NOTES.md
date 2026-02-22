# ShellUI v0.2.1 🛠️

> Hotfix release - Chart fixes & CSS auto-install

## 🐛 Bug Fixes

- **Fixed chart tooltip escaping** - Custom tooltip HTML in `ChartVariantsTemplate.cs` used incorrect verbatim string escaping (`\""` → `""""`) causing broken `@"..."` blocks in installed files
- **Fixed ChartVariants namespace** - `ShellUI.Components.Variants.ChartVariants` was incorrectly under `ShellUI.Components` namespace
- **Fixed Chart.razor missing using** - Added `@using ShellUI.Components.Variants` to the Components library `Chart.razor`
- **Fixed ChartVariantsTemplate FilePath** - Changed from `"ChartVariants.cs"` to `"Variants/ChartVariants.cs"` to install in correct subdirectory
- **Fixed ChartVariantsTemplate IsAvailable** - Set to `false` since it's a dependency, not a standalone component

## ✨ New Features

### Chart Styles CSS (`chart-styles`)
- **New `charts.css` file** auto-installs to `wwwroot/css/` as a dependency when you run `shellui add chart`
- Contains all ApexCharts theme CSS: tooltips, custom tooltips, legends, toolbars, grid/axis styling
- Uses ShellUI CSS variables with hardcoded fallbacks for portability
- Replaces the previous approach of requiring manual CSS setup

### CLI: wwwroot File Support
- Components with `FilePath` starting with `wwwroot/` now correctly install relative to project root instead of inside `Components/UI/`

### CLI: Automatic CSS Link Injection
- After installing `chart-styles`, the CLI automatically injects `<link rel="stylesheet" href="css/charts.css" />` into your `App.razor` (or `index.html` for WASM)
- Idempotent — re-running `shellui add chart` won't duplicate the link
- Falls back to printing manual instructions if no layout file is found

## 📦 What `shellui add chart` now installs

```
shellui add chart
→ Installed 'chart-variants' (Variants/ChartVariants.cs)
→ Installed 'chart-styles' (wwwroot/css/charts.css)
→ Installed 'chart' (Chart.razor)
→ Added <link rel="stylesheet" href="css/charts.css" /> to Components\App.razor

Installed 3 component(s) successfully!
```

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

---

# ShellUI v0.2.0 📊

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
- **Component count: 80** (73 existing + 7 new chart components)
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
