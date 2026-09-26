# ShellUI + ShellIcons integration notes

> [!NOTE]
> These are dated consumer-project notes captured against published `0.3.0-rc.1`. They are not the current product documentation. Current source facts and workflows are maintained in `README.md` and `docs/`.

Notes captured while wiring the `SageEvolutionApi.Dashboard` Blazor Server project on `feat/dashboard`. Written for whoever picks this up next — either to reproduce, or to hand upstream to `shellui-dev`.

Ground truth used:
- Local ShellUI source: `C:\Users\Shewatipa\source\repos\Shell\shellui\shellui\shellui` (`0.4.0-alpha.1` in `Directory.Build.props`)
- Published, installed, and used in this project:
  - **`ShellUI.CLI` 0.3.0-rc.1** (`dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.1`)
  - **Component templates 0.3.0-rc.1** (delivered by the CLI as `.razor` sources into `Components/UI/`)
  - **`ShellIcons.Blazor` 0.1.0-alpha** (NuGet)

---

## Install-channel gotcha (this bit us first)

`dotnet tool install -g ShellUI.CLI` — no version — grabs stable-latest, which is **0.2.1**. All the fixes in `docs/RELEASE_NOTES.md` under the 0.3.0-rc.1 heading are trapped behind that: `App.razor` isn't patched with `@rendermode` / theme bootstrap / `shellui.js`, `input.css` is written as a single `@import "tailwindcss";` line so every `bg-background` class resolves to nothing, `data-table-models` fails to install, `ThemeToggle` uses `eval` and crashes during prerender.

**Do this instead:**
```powershell
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.1
# or upgrade an existing 0.2.1
dotnet tool update -g ShellUI.CLI --version 0.3.0-rc.1
```
Then `shellui --version` should report `0.3.0-rc.1+…`.

**Upstream ask.** Either (a) mark 0.3.0-rc.1 as the recommended install in the README quickstart (right now it says `dotnet tool install -g ShellUI.CLI` with no channel), or (b) publish 0.3.0 stable so plain-install lands on the fixes. Also worth: an `--allow-prerelease` reminder printed by `shellui --version` when a newer prerelease exists on NuGet.

---

## Bugs still shipping in 0.3.0-rc.1 CLI

After re-installing at 0.3.0-rc.1 and re-running `shellui init --yes --tailwind npm` + `shellui add <big list>`, three real bugs remained.

### 1. `accordion-type` sub-dep not registered (same class as the 0.2.1 `data-table-models` bug)

**Symptom.** `shellui add accordion` reports `Failed: accordion-type`. `Accordion.razor` compiles-in `[Parameter] public AccordionType Type { get; set; } = AccordionType.Single;` but `AccordionType.cs` never lands, so the project doesn't compile.

**Verified in source.** `src/ShellUI.Templates/Templates/AccordionTypeTemplate.cs` exists with `Name = "accordion-type"`, `FilePath = "AccordionType.cs"`, `IsAvailable = false`, and `AccordionTemplate.cs:16` declares `Dependencies = new List<string> { "accordion-type", "accordion-item" }`. So the template is there and the dep is declared — the wiring inside `ComponentRegistry.cs` is presumably what's missing (same shape as `data-table-models` and `chart-styles` were before). The RELEASE_NOTES fix that landed `data-table-models` didn't cover this case.

**Workaround.** Hand-write `Components/UI/AccordionType.cs`:
```csharp
namespace SageEvolutionApi.Dashboard.Components.UI;
public enum AccordionType { Single, Multiple }
```

**Upstream ask.** Add a test in `NuGetDepsAndSuggestionsTests.cs` that walks *every* component's `Dependencies` list and asserts each name resolves via `ComponentRegistry.GetMetadata(...)`. That catches this bug class once and for all.

### 2. `Tabs.razor` template ships with a mangled string literal

**Symptom.** `Components/UI/Tabs.razor(18,38): error RZ1000: Unterminated string literal.`

**Cause.** The installed Tabs.razor line 18 reads `private string _effectiveValue = ";` — three chars where four (`"";`) should be. The template source (`src/ShellUI.Templates/Templates/TabsTemplate.cs:35`) declares it as `private string _effectiveValue = "";` inside a `@"...""..."` verbatim block, so one of the two escape-doubled `""` gets clipped by the CLI's placeholder substitution or by the verbatim-decoder before write-out. `TemplateCompileTests` (per RELEASE_NOTES) should have caught this — it seems the compile pass is running against the raw template source, not against the CLI's *output* after placeholder substitution.

**Workaround.**
```razor
private string _effectiveValue = "";
```

**Upstream ask.** Add a `TemplateOutputCompileTest` variant that (a) runs each template through the placeholder substitution the CLI uses, then (b) `CSharpSyntaxTree.ParseText` on the substituted result. That's the surface consumers actually see.

### 3. `Select.razor` renders "expand_more" as literal text

**Symptom.** The Select dropdown shows the raw string `expand_more` in the corner where a chevron should be.

**Cause.** Line 12: `<span class="material-symbols-outlined absolute …">expand_more</span>`. This depends on Google Material Symbols being loaded via `<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" />` — which ShellUI never adds to `App.razor` and doesn't list as a required host resource. Every other ShellUI component uses inline SVG for its chrome (see DataTable's sort arrow, ThemeToggle's sun/moon, Sheet's close X, Accordion's chevron), so this is the odd one out.

**Workaround.**
```razor
<svg class="pointer-events-none absolute right-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground"
     xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
</svg>
```

**Upstream ask.** Replace the Material Symbols span with the inline chevron SVG — matches every other component in the library and drops a hidden network dependency.

---

## 0.2.1 branch — fix candidates (backport from 0.3.0-rc.1)

Each of these was hit on a real `dotnet tool install -g ShellUI.CLI` (stable-latest = 0.2.1) + `dotnet new blazor --interactivity Server --empty` + `shellui init --yes --tailwind npm` + `shellui add …` run in this project on 2026-09-04. All are fixed in `main` / 0.3.0-rc.1. If you want to cut a `hotfix/v0.2.1.1` branch off the `v0.2.1` tag, this section is the punch list — each item lists the symptom I saw, a minimal repro, the file(s) to touch in the source, and the pattern the fix in `main` took so you can cherry-forward it.

Reference for every "the fix pattern in main is…" line below: `docs/RELEASE_NOTES.md` §"🐛 Critical fixes" under the 0.3.0-rc.1 heading.

### B1. `input.css` written as one-line `@import "tailwindcss";`

**Symptom.** Every ShellUI component renders unstyled. `bg-background`, `text-foreground`, `text-muted-foreground`, `border-border`, `bg-card` etc. all resolve to nothing because Tailwind v4 needs the tokens declared under `@theme inline`, and none of them are there.

**Repro.** After `shellui init` on 0.2.1, `wc -l wwwroot/input.css` → `1`.

**Files.** `src/ShellUI.CLI/Services/ThemeService.cs` (writes `input.css`).

**Fix pattern.** `main` writes ~193 lines: `:root {…light HSL vars…}`, `.dark {…dark HSL vars…}`, `@theme inline {…Tailwind color-token mapping…}`, `@custom-variant dark`, `@layer base {…}`, and Loading-component keyframes. Read the current `input.css` template out of `main`'s `ThemeService` and drop it verbatim.

**Consumer-side test to pin the fix.** After init, assert the file contains at minimum: `@theme inline`, `--background`, and `.dark { --background`. Anything less is a broken install.

### B2. `App.razor` not patched with `@rendermode`, theme bootstrap, or `shellui.js`

**Symptom.** No JS interop reaches any component (ThemeToggle silently no-ops, InputOTP focus-forward doesn't work, Sonner toasts don't appear). `<HeadOutlet />` renders SSR-only markup so the head never hydrates. On dark-preference machines, the page flashes light before the theme applies (FOUC).

**Repro.** After `shellui init` on 0.2.1, open `Components/App.razor` — the file the template produced is identical to `dotnet new blazor`'s default: no `@rendermode`, no `<script src="shellui.js">`, no theme bootstrap `<script>`, and CSS references `_framework/blazor.web.js` only.

**Files.** `src/ShellUI.CLI/Services/InitService.cs` — the App.razor rewriter.

**Fix pattern.** Idempotent rewriter that inserts four things:
1. `@rendermode="InteractiveServer"` on `<HeadOutlet />` and `<Routes />` (preserve any existing value — don't stomp `InteractiveAuto` / `InteractiveWebAssembly`).
2. Theme bootstrap `<script>` in `<head>` that reads `localStorage.theme` and adds `dark` class before Blazor mounts.
3. `<script src="shellui.js">` in `<body>`, ordered *before* `_framework/blazor.web.js`.
4. Same treatment for `wwwroot/index.html` on Blazor WASM projects (only the bootstrap + shellui.js — no render mode).

Test scaffolding for this exists in `main` as `InitBootstrapTests` (8 tests covering render-mode injection, theme bootstrap placement, script ordering, idempotency, preserved render modes, and both the modern `@Assets[…]` and bare `<script>` script-tag forms). Backport those tests alongside the rewriter.

### B3. `data-table-models` sub-dep not registered → half-install

**Symptom.** `shellui add data-table` writes `DataTable.razor` but reports `Failed: data-table-models` / `Component 'data-table-models' not found`. Project won't compile — `DataTableColumn<T>`, `DataTableAction<T>`, `SortDirection` are unresolved.

**Repro.** `shellui add data-table`; `dotnet build` → `CS0246: The type or namespace name 'DataTableColumn<>' could not be found`.

**Files.** `src/ShellUI.Templates/ComponentRegistry.cs` — the registry map, and `GetComponentContent` switch.

**Fix pattern in main.** `ComponentRegistry.cs:137` registers `{ "data-table-models", DataTableModelsTemplate.Metadata }` and `:314` routes `"data-table-models" => DataTableModelsTemplate.Content`. `DataTableModelsTemplate.cs` already exists in 0.2.1 source (that's the whole reason the error message says "not found" and not something worse); the fix is literally a two-line addition to the registry.

**Regression test to add.** In `NuGetDepsAndSuggestionsTests.cs`: enumerate every registered component, walk its `Dependencies`, assert each named dep resolves via `ComponentRegistry.GetMetadata(...)`. This is the test I asked for in current-CLI bug §1 (accordion-type) — same shape.

### B4. `chart-styles` sub-dep not registered → invisible chart tooltips

**Symptom.** Hovering a chart data point shows an invisible tooltip (white text on white background, because the `.custom-tooltip-*` classes the chart HTML emits are unstyled).

**Files.** Same registry file as B3. The template `ChartStylesTemplate.cs` exists — just not wired in.

**Fix pattern in main.** Register `chart-styles`, add it as a dep of every `chart-family` component (`bar-chart`, `line-chart`, `area-chart`, `pie-chart`, `chart-variants`), and fix its `FilePath` from a `Components/UI/wwwroot/...` path (which lands inside the components directory and is unreachable from `<link>`) to `../../wwwroot/css/charts.css` (project root). Then teach the CLI to inject `<link rel="stylesheet" href="css/charts.css" />` into `App.razor` on install (same idempotent rewriter pattern as B2).

### B5. NuGet packages not auto-installed on `shellui add`

**Symptom.** After `shellui add data-table area-chart bar-chart line-chart pie-chart`, `dotnet build` fails with `The type or namespace name 'ApexCharts' could not be found` and `The type or namespace name 'Dynamic' does not exist in the namespace 'System.Linq'`.

**Files.**
- `src/ShellUI.Core/Models/ComponentMetadata.cs` — add `NuGetDependencies` (list of `{PackageId, Version}`).
- `src/ShellUI.CLI/Services/AddService.cs` (or wherever install-time walk lives) — after all `.razor` sources are written, dedupe the transitively-collected `NuGetDependencies` and `dotnet add package` each one.
- The chart templates and the data-table template — populate their `NuGetDependencies`.

**Fix pattern in main.** `data-table` declares `System.Linq.Dynamic.Core 1.7.1`. Chart family declares `Blazor-ApexCharts 6.0.2`. CLI walks the graph and runs `dotnet add package <id> --version <v>` once per unique package.

**Consumer caveat that's worth documenting either way.** If the consumer already has a *different* version of `Blazor-ApexCharts` pinned (I had 7.0.0 from an earlier manual add), `dotnet add package` with a lower version silently downgrades — the consumer ends up on 6.0.2 without warning. Consider `dotnet list package | grep` before adding and skip / warn if a higher-version pin exists.

### B6. `PieChart.razor` template ships an unterminated string literal

**Symptom.** `Components/UI/PieChart.razor(49,53): error RZ1000: Unterminated string literal.` Cascading spurious errors around lines 22–25 because the Razor parser desynchronizes.

**Cause in 0.2.1.** The `Custom = @"function(…) { … return '<div class=\"custom-tooltip\">' … }"` block uses `\"` inside a verbatim `@"..."` string. In verbatim strings `\"` is not an escape — it's a literal `\` followed by `"`, so the `"` closes the string mid-expression.

**Files.** `src/ShellUI.Templates/Templates/PieChartTemplate.cs`.

**Fix pattern in main.** The entire `Custom` tooltip block was deleted from the live component — the current `PieChart.razor` in `main` is 35 lines with no `Custom =` at all. Simplest backport: delete the block. If keeping the JS-tooltip is required, escape as `""` (verbatim-string doubled-quote form).

**Regression test to add.** `TemplateCompileTests` (8 tests) in `main` runs `CSharpSyntaxTree.ParseText` over each fixed template's generated content. Backport `PieChart` into that suite and it stays fixed.

### B7. `ChartVariants` and `DashboardLayout02` templates share the same class of bug

Not a bug I hit personally (didn't install `ChartVariants` or the earlier `dashboard-layout-02` on 0.2.1), but `RELEASE_NOTES.md` lists them alongside PieChart:
- `ChartVariants`: same `\"` → `""` fix
- `DashboardLayout02`: unterminated empty-string literal in `BuildBreadcrumb` (`segments[0] == "")` needed → `segments[0] == "")`)

Same regression-test coverage as B6 catches both.

### B8. `ThemeToggle` crashes during Blazor Server prerender

**Symptom.** `System.InvalidOperationException: JavaScript interop calls cannot be issued at this time. This is because the component is being statically rendered.` on first page load. In 0.2.1's ThemeToggle the crash was caught silently by a try/catch, but the toggle state never initialized correctly and clicking did nothing until the second render.

**Files.** `src/ShellUI.Templates/Templates/ThemeToggleTemplate.cs`.

**Fix pattern in main.**
1. Move the `localStorage.getItem("theme")` read from `OnInitializedAsync` (where `IJSRuntime` is unavailable during Blazor Server SSR) to `OnAfterRenderAsync(bool firstRender)` gated on `firstRender`.
2. Delete the try/catch that was silently swallowing the SSR error — with the read moved, it can't fire.

### B9. `ThemeToggle` uses `JSRuntime.InvokeVoidAsync("eval", "…")`

**Symptom.** CSP-locked environments block `eval`, ThemeToggle silently no-ops. Also a code-review red flag on any downstream consumer.

**Files.** Same as B8. Also `InputOTP.razor` (uses `eval` for digit-to-digit focus) and `ThemeService.cs` template (same cleanup).

**Fix pattern in main.**
1. Add `ShellUI.addClassToDocument(className)` / `ShellUI.removeClassFromDocument(className)` / `ShellUI.focusElement(id)` helpers to `wwwroot/shellui.js`.
2. Replace `JSRuntime.InvokeVoidAsync("eval", "document.documentElement.classList.add('dark')")` with `JSRuntime.InvokeVoidAsync("ShellUI.addClassToDocument", "dark")`. Same for `remove`.
3. `InputOTP` calls `ShellUI.focusElement` instead of an `eval` string.

### B10. `SidebarTrigger` renders an invisible FontAwesome hamburger

**Symptom.** On mobile viewports the sidebar toggle button is completely invisible — clicking the right spot works, but there's no glyph. Cause: `<i class="fa-solid fa-bars-staggered">` without FontAwesome loaded (which ShellUI doesn't ship or require).

**Files.** `src/ShellUI.Templates/Templates/SidebarTriggerTemplate.cs`.

**Fix pattern in main.** Swap the `<i>` for an inline SVG hamburger. Any 4-line stroke SVG works — the current one in `main` is Lucide's `menu` shape.

### B11. `Tabs.razor` (0.2.1) uses items-based API; 0.3.0-rc.1 (and main) use shadcn composition — but 0.2.1 also ships a `@using ShellUI.Components.Models` line that only resolves if the consumer keeps the (deprecated) NuGet package referenced

Not strictly a fix candidate for the 0.2.1 branch since the API refactor is intentional — but if you're patching 0.2.1, the one-line surgical fix is: change `@using ShellUI.Components.Models` at the top of the installed `Tabs.razor` to `@using YourProjectNamespace.Components.Models` and let the CLI's placeholder substitution do its normal thing. Also make sure `TabItem` (the model class the 0.2.1 items-based API needs) ships as a `models` sub-dep, same shape as B3's `data-table-models`.

### Ordering guidance for the hotfix branch

If you're doing a 0.2.1 hotfix cut, the cheapest / highest-impact ordering is:
1. **B1 + B2 together** — without a real theme and a patched App.razor, nothing else *looks* right and every subsequent test-visually step lies to you.
2. **B3 + B4 + B5** — dep-graph correctness. These three unblock every DataTable/chart consumer.
3. **B6 + B7** — template escaping. Backport the `TemplateCompileTests` at the same time.
4. **B8 + B9** — ThemeToggle correctness + `eval` removal. Ship the `shellui.js` helper additions in the same PR.
5. **B10** — visual cleanup, no compile impact.
6. **B11** — only if you care about the 0.2.1 Tabs API long-term.

Every fix above has a corresponding test in `main` (per RELEASE_NOTES: `InitBootstrapTests`, `TemplateCompileTests`, `TemplateSyncTests`, `NuGetDepsAndSuggestionsTests`, `ChartStylesTests`, 53 tests total, was 0). Backport the tests alongside the fixes so 0.2.1.1 doesn't drift back.

---

## Bug I hit that is neither of the above

### 4. Namespace collision between ShellUI components and ShellIcons typed icons

If both `SageEvolutionApi.Dashboard.Components.UI` (or its equivalent) and `ShellIcons.Icons` are in scope, `<Badge />`, `<Alert />`, `<Table />` and similar become ambiguous — ShellUI has a component with the name, ShellIcons has an icon.

**Convention adopted here.**
1. Default to the dispatcher `<ShellIcon Name="badge" />` (namespace `ShellIcons`) — no clash with ShellUI component names.
2. Never put `@using ShellIcons.Icons` in `_Imports.razor` globally.
3. When a page really needs the tree-shakeable typed icon, alias per-page: `@using Icons = ShellIcons.Icons` then `<Icons.Badge />`.

The `shellicons.md` design proposal already flags this collision class ("common names like `Router` or `Activity` don't collide with Blazor/system types unless you opt in"). Cross-linking to ShellUI's component names in the ShellIcons README would help.

---

## Papercuts (survivable, but noisy)

### 5. `shellui add` result summary hides transitive-dep failures
The final line reads `Installed 90 component(s) successfully! Skipped 2 component(s) (already exists…). Failed: accordion-type`. The `Failed:` list is easy to miss because it doesn't wrap, isn't colored red, and comes after the green success count. Either add red styling to the failed row or return a non-zero exit code when any dep fails to install.

### 6. `shellui init --yes` picks standalone Tailwind silently unless you also pass `--tailwind npm`
Non-interactive mode defaults to `--tailwind standalone` (downloads a Tailwind binary). Most Blazor projects that already have Node prefer npm; `--yes` should either fail with "pick a Tailwind mode explicitly" or `standalone` should be spelled out prominently in the output so consumers know a binary was pulled.

### 7. The old README reported "68 installable components", while the registry has more entries
`shellui list` shows the 73 direct targets in current source and hides dependency-only entries; the summary may still show the full registry total. The old 68-count was a release-era documentation issue, not the current target count.

### 8. NuGet package docs are misleading
`ShellUI.Components` README opens with **"⚠️ Read this first."** but the package is still the natural install target when someone types `dotnet add package Shell…` in an IDE. Consider either publishing a `ShellUI.Meta` no-op package whose description says "install `ShellUI.CLI` instead", or emitting a build warning from the package's props file when it detects it's the sole install path.

`ShellIcons.Blazor`'s published README (0.1.0-alpha) says "Not yet published" and points at `<ProjectReference>`. It **is** published — the message is stale.

### 9. `<ShellIcon>` dispatcher isn't obvious from the ShellIcons README
The README opens with tree-shakeable typed icons and only later mentions `<ShellIcon Name="…" />`. For Blazor Server consumers who don't care about tree-shaking, the dispatcher is the correct default (no collisions, no per-page `@using`), and it should be presented first. A one-line "for most projects, start with `<ShellIcon Name="…" />`" would save the collision debugging in item 4.

---

## Suggestions (features, not bugs)

### F1. `shellui doctor`
Check consumer state, print a diagnosis:
- CLI version vs latest on NuGet (with a nudge if the newest available is a `-rc`/`-alpha` the user hasn't opted into)
- `Build/ShellUI.targets` present and imported
- `wwwroot/input.css` matches the current default theme (diff against template)
- `App.razor` patched with `@rendermode`, theme bootstrap, `shellui.js`
- Every referenced component in the consumer's `.razor` files exists in `Components/UI/`
- Every NuGet package listed as `NuGetDependencies` in installed components is actually referenced

### F2. `shellui migrate <component>` for API-breaking template refactors
Tabs went from items-based (`<Tabs Items="…" ActiveTab="…"/>`) to shadcn composition (`<Tabs Value="…"><TabsList><TabsTrigger>…</Tabs>`) between installed versions. When the CLI knows a component's API changed, `shellui migrate tabs` could rewrite consumer call sites — or at least print a diff.

### F3. Starter templates for common shapes
`shellui add starter dashboard` scaffolds `Layout/MainLayout.razor` with Sidebar + NavigationMenu + ThemeToggle, plus a stub `Overview.razor` with Card grid + charts placeholder. Every adopter reinvents this — a starter would exercise 80% of the components in one command and give consumers a working reference.

### F4. Component preview page in the doc site
`docs/COMPONENT_DEPENDENCIES.md` is a great text reference. A `/preview/<component>` route in the ShellUI docs site (or a `shellui preview button` command that opens a local Blazor page) would make visual review of a variant/prop change trivial.

### F5. ShellIcons: publish typed icon list as a JSON asset
Consumers building admin UIs frequently want "give me all icons whose name contains `chart`" for an icon picker. A small JSON asset (`_content/ShellIcons.Blazor/icons.json`) enumerating names + categories lets consumers build a picker in ~30 lines. The `/icons` route on the ShellIcons docs site already computes this; expose it.

### F6. ShellIcons: React alias table
The React ecosystem knows Lucide icon names in `kebab-case` and PascalCase. A tiny reference table in the README — "Lucide `chevron-right` → `<ChevronRight />` or `<ShellIcon Name="chevron-right" />`" — closes the gap for people arriving from a JS background.

### F7. Rendering perf: source-generator path for `<ShellIcon Name="literal" />`
The dispatcher is trimmer-hostile because it holds all 1555 icons. A source generator that scans `.razor` files for `<ShellIcon Name="…" />` with literal strings and rewrites them to the typed component would preserve the dispatcher DX and unlock tree-shaking.

---

## What I built on top

- **`Components/Routes.razor`** — `DefaultLayout="typeof(Layout.DashboardLayout02)"`, so every page inherits the sidebar-inset composition without needing per-page `@layout`.
- **`Components/Layout/MainLayout.razor`** — reduced to a no-op `@Body` pass-through, kept as a fallback for any page that ever wants `@layout MainLayout` explicitly. The Blazor error-UI div lives in `Components/App.razor`'s body now.
- **`Components/Layout/DashboardLayout02.razor`** — CLI-installed; provides the shell (SidebarProvider + AppSidebar + SidebarInset + sticky header with SidebarTrigger, breadcrumb, ThemeToggle).
- **`Components/UI/AppSidebar.razor`** — CLI-installed, then customized. Logo tile uses `<ShellIcon Name="activity" />`; menu group labeled "Observability" with Overview / Requests / Errors, each rendered with a `<ShellIcon>` glyph; footer link to Swagger. `Collapsible = SidebarCollapsible.Icon` (Ctrl+B → collapses to an icon rail; matches the reference's `sidebar-08` pattern).
- **`Components/Pages/Home.razor`** — 4× Kpi Card + AreaChart RPM + PieChart status classes + BarChart top endpoints.
- **`Components/Pages/Requests.razor`** — filter row (Select × 3, Input, Switch) + DataTable + Sheet detail with shadcn-style Tabs → Request / Response / Exception → BodyView.
- **`Components/Pages/Errors.razor`** — DataTable grouped by exception + endpoint.
- **`Components/Shared/Kpi.razor`** — tiny custom (ShellUI has no dedicated stat tile; Card is the right primitive to wrap).
- **`Components/Shared/BodyView.razor`** — pretty-JSON + copy button; the only custom piece — ShellUI has no JSON/code viewer, and Prism.js felt like too much for now.
- **Icon convention** enforced via `_Imports.razor`: `@using ShellIcons` (dispatcher only). Per-page alias `@using Icons = ShellIcons.Icons` when a page genuinely needs typed icons.
- **API side.** `Core/Audit/` — `ApiAuditEntry`, `AuditOptions`, `AuditSchema` (SQL Server, `IF NOT EXISTS` table + indexes), `SqlServerApiAuditSink` (bounded channel + background batched inserts), `AuditRedaction` (JSON field + header redaction), `AuditServiceCollectionExtensions.AddApiAudit()`. `Core/Middleware/RequestAuditMiddleware.cs` captures request/response body + status + duration + correlation ID; registered in `Program.cs` before `ApiKeyMiddleware` so 401s are logged. Sink stays idle when `Audit:ConnectionString` is empty.

`run-dev.ps1` / `run-dev.sh` spawn three processes side by side: `npx @tailwindcss/cli --watch` (rebuilds `wwwroot/app.css` from `wwwroot/input.css` on every `.razor` save), the API on `:5090`, and the dashboard on `:5047`.
