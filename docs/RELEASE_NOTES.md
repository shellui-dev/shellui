# ShellUI v0.3.0-rc.1 🚦

> Release candidate for v0.3.0. Five branches of integration-tested fixes against the alpha series, surfaced from real-world Blazor Server consumer use. If no critical reports come in during the soak window, this code ships as `v0.3.0` stable with the suffix dropped — no further code changes. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## TL;DR

`shellui init` + `shellui add` now produce a project that compiles and runs end-to-end without manual host patching. Every alpha.3 install-time papercut documented in the integration notes is fixed and guarded by tests + CI:

- Three chart/dashboard templates no longer ship uncompilable C# verbatim strings
- `SidebarTrigger` mobile hamburger renders (was an invisible FontAwesome class)
- `ThemeToggle` no longer uses `eval` and no longer crashes during Blazor Server prerender
- `shellui init` patches `App.razor` with `@rendermode`, theme bootstrap script, and `shellui.js` link tag — and writes the full default theme to `input.css` instead of just `@import "tailwindcss";`
- `shellui add data-table` actually installs `DataTableModels.cs` and auto-runs `dotnet add package System.Linq.Dynamic.Core`
- `shellui add chart` auto-runs `dotnet add package Blazor-ApexCharts` and the chart tooltip CSS finally renders readable text instead of invisible white-on-white
- New `did you mean …?` typo suggestion (`shellui add datatable` → `Did you mean 'data-table'?`)

Component count corrected across docs: **68 installable** top-level components (not the 100 previously claimed; sub-components and variants ship as auto-installed dependencies).

## 🐛 Critical fixes

### Templates that shipped uncompilable C#
- **`ChartVariants`** — every JS-attribute quote inside the tooltip `Custom = @"..."` block now escapes as `""x""` (the verbatim-string form) instead of bare `"x"` (which terminated the outer string)
- **`PieChart`** — same class of bug but using `\"x\"` (which decoded to literal backslashes in rendered HTML); fixed to `""x""`
- **`DashboardLayout02`** — unterminated empty-string literal in `BuildBreadcrumb` (`segments[0] == ""))` → `segments[0] == """"))`)
- Live `PieChart.razor` in the library had the same backslash bug; fixed to remove visible `\"` from rendered tooltip HTML

### SidebarTrigger + ThemeToggle runtime
- **`SidebarTrigger`** — `<i class="fa-solid fa-bars-staggered">` swapped for inline SVG hamburger. FontAwesome is not a ShellUI dependency, so mobile users on a fresh install previously saw an invisible toggle button
- **`ThemeToggle`** — `JSRuntime.InvokeVoidAsync("eval", …)` dropped in favor of the `ShellUI.addClassToDocument` / `ShellUI.removeClassFromDocument` helpers already shipped in `shellui.js`. No more CSP-blockable `eval`, no new JS surface area
- **`ThemeToggle`** — localStorage read moved from `OnInitializedAsync` (where `IJSRuntime` is unavailable during Blazor Server prerender) to `OnAfterRenderAsync(firstRender)`
- **`InputOTP`** — same `eval` removal applied; now uses `ShellUI.focusElement` for digit-to-digit focus
- **`ThemeService`** — same `eval` cleanup as `ThemeToggle`

### `shellui init` produces a working host
- **App.razor patching** — `<HeadOutlet />` → `<HeadOutlet @rendermode="InteractiveServer" />`, same for `<Routes />`. Existing `@rendermode` values are preserved (won't overwrite `InteractiveAuto`/`InteractiveWebAssembly`)
- **Theme bootstrap `<script>` injected into `<head>`** — reads `localStorage.theme` and applies the `dark` class before paint to avoid the light-flash on dark pages
- **`<script src="shellui.js">` injected** before `_framework/blazor.web.js` (handles both the modern `@Assets[...]` wrapper and the bare form)
- **`wwwroot/index.html` patched** for Blazor WebAssembly standalone projects (theme bootstrap + `shellui.js` only — no render-mode pattern in WASM)
- **`input.css` ships the full default theme** — `:root` light vars, `.dark` dark vars, `@theme inline` mapping for Tailwind v4, `@custom-variant dark`, `@layer base` defaults, and Loading-component animation keyframes. Previously emitted only `@import "tailwindcss";`, so any component using theme variables rendered unstyled
- **`tailwind.config.js` (npm)** — dropped the redundant `hsl(var(--x))` color block; Tailwind v4 reads the palette from `@theme inline` in `input.css`
- **All patching is idempotent** — running `shellui init` twice produces no duplicate scripts or tags

### `shellui add data-table` is no longer a half-install
- **`data-table-models` registered** — the template existed on disk but was missing from `ComponentRegistry`, so the CLI reported `Component 'data-table-models' not found` and left the consumer project unable to compile
- **NuGet auto-install** — new `NuGetDependencies` field on `ComponentMetadata`. `data-table` declares `System.Linq.Dynamic.Core 1.7.1`, `chart` declares `Blazor-ApexCharts 6.0.2`. The CLI walks the dep graph and runs `dotnet add package` once per unique package after all source files are written
- **Did-you-mean suggestions** — Levenshtein-based hint surfaces close matches when a user mistypes a component name. `shellui add datatable` → `Did you mean 'data-table'?` Hidden sub-components are excluded from suggestions

### `shellui add chart` tooltip is finally readable
- **`chart-styles` registered** — the CSS template existed but, like `data-table-models`, was never wired into the registry. Hovering a chart point previously showed an invisible white-on-white tooltip because nothing styled the `.custom-tooltip-*` classes the chart HTML emits
- **`chart` depends on `chart-styles`** — the recursive install walk transitively pulls the CSS in for every chart-family component
- **CSS lands in the right place** — `FilePath` corrected from a path that would have buried the file in `Components/UI/wwwroot/css/` (unreachable) to `../../wwwroot/css/charts.css` (project root)
- **`<link>` auto-injected into App.razor** — same idempotent rewriter pattern used for `shellui.js`. Generic by design: any future `wwwroot/`-targeting CSS asset gets the same treatment

## 🔧 Improvements

- **`docs/RELEASE_NOTES.md`** is now the source of truth that `release.yml` uses as the GitHub Release body — no more drift between the tag description and the file
- **Component count audit** — README, ARCHITECTURE, COMPONENT_ROADMAP, PROJECT_STATUS, COMPARISON, FAQ, QUICKSTART, CLI_SYNTAX, VERSIONING_STRATEGY, and the three package READMEs all now report **68 installable components**, with category breakdowns rederived from `ComponentRegistry` (Form 17, Layout 12, Navigation 7, Overlay 8, Data Display 13, Feedback 9, Utility 2)
- **Preview pipeline restored** — `actions/configure-pages@v4` now passes `enablement: true` so the GitHub Pages site is created on first run instead of erroring with `HttpError: Not Found`

## 🧪 Tests + CI

This release adds three independent layers of regression coverage. The previous alpha shipped with zero of these:

- **`TemplateCompileTests`** (8 tests) — Roslyn `CSharpSyntaxTree.ParseText` over every fixed template's generated content. Pure-C# templates parse whole content; Razor templates extract the `@code` block via a quote/comment-aware brace-balancing tokenizer. When extraction fails (unterminated string), falls back to a class-wrapped parse filtered to literal-related diagnostic IDs (`CS1010`, etc.) so the failure message points at the offending line
- **`TemplateSyncTests`** (3 tests) — for every component that has both a live `.razor` in `src/ShellUI.Components/Components/` and a CLI template, compares the `@code` blocks after normalization (strip comments, blank lines, whitespace). Catches the class of bug that shipped in alpha.3 where someone added a parameter to the live `ThemeToggle.razor` but forgot to mirror it in the template. `AllowedDrift` exception list exists but is empty
- **`InitBootstrapTests`** (8 tests) — direct tests of the App.razor / index.html rewriter. Covers render-mode injection, theme bootstrap placement, script-tag ordering, idempotency, existing-render-mode preservation, and both the modern `@Assets[...]` and bare `<script src="…">` forms
- **`NuGetDepsAndSuggestionsTests`** (15 tests) — Levenshtein matcher, `did-you-mean` exclusions, NuGet metadata, chart-family transitive dependency proofs, namespace convention
- **`ChartStylesTests`** (10 tests) — registry wiring, `FilePath` targets wwwroot, CSS content covers both `.custom-tooltip-*` and `.apexcharts-*` classes with theme variables, link injector + `ResolveHostStylesheetHref` semantics, idempotency
- **CI smoke step** — packs the CLI, installs as a global tool, `dotnet new blazor` → `shellui init` → `shellui add chart pie-chart dashboard-02 data-table` → `dotnet build`. Asserts the auto-injected pieces (`@rendermode`, theme bootstrap, `shellui.js` script, `charts.css` link, NuGet refs, `DataTableModels.cs` presence, full theme in `input.css`) all appear in the produced files. The previous CI did neither tests-on-PR nor end-to-end scaffolding

**Test total:** 53 unit tests (was 0). All pass in Release. Drift canary verified by stashing a fix, observing the precise line-level diff in the test output, and restoring.

## 📦 Installation

```bash
# CLI (prerelease channel — the suffix is intentional)
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.1

# or upgrade from any alpha
dotnet tool update -g ShellUI.CLI --version 0.3.0-rc.1

shellui init
shellui add button card dialog
```

```bash
# NuGet packages (prerelease channel)
dotnet add package ShellUI.Core --version 0.3.0-rc.1 --prerelease
dotnet add package ShellUI.Components --version 0.3.0-rc.1 --prerelease
```

If `shellui init` runs cleanly and `dotnet build` succeeds on a fresh `dotnet new blazor`, you have everything. There are no manual `<script>` / `<link>` / `@rendermode` patches to make.

## ⏭ What's next

- **`v0.3.0`** stable — same code as rc.1 with the suffix dropped, after a soak window. No further alphas planned for the 0.3 line
- **`v0.4.0-alpha.1`** — .NET 10 upgrade. Begins after 0.3.0 ships. TFM bump, package updates, CI matrix
- **`v0.4.0` and beyond** — generic `DataPicker<TItem, TKey>` (Fix 11 from integration notes), Preview app rewrite, ApexCharts chrome restyle (default legend / axis text Tailwind-styled to match shadcn polish)

See [docs/COMPONENT_ROADMAP.md](COMPONENT_ROADMAP.md) for the longer view.

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.3.0-alpha.3...v0.3.0-rc.1

---

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
