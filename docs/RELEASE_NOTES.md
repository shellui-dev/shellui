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
