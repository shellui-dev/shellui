# ShellUI Component Dependencies

`ComponentRegistry` metadata is the authoritative source for component names, availability, source dependencies, NuGet dependencies, and install paths. This document is a readable guide to that metadata; if it ever differs from [`ComponentRegistry.cs`](../src/ShellUI.Templates/ComponentRegistry.cs), the registry wins.

## Registry snapshot

- **176** total registry entries
- **76** direct CLI targets (`IsAvailable = true`)
- **100** hidden entries (`IsAvailable = false`)
- `shellui list` displays the 76 direct targets; hidden sub-components, variants, models, services, and support assets are omitted from the public list.

`Dependencies` contains registry-declared source/template dependencies. `NuGetDependencies` is separate and is used by the installer to add package references. A relationship visible in rendered markup is not automatically a registry dependency, so do not infer a dependency graph from component names or visual composition.

## Direct-target dependency map

The following table summarizes the non-empty `Dependencies` declared by the current direct targets. Empty dependency lists are intentionally not called “standalone” here; consult the registry for the current value.

| Direct target | Declared source dependencies | Declared NuGet dependencies |
|---|---|---|
| `accordion` | `accordion-type`, `accordion-item` | — |
| `alert` | `alert-variants` | — |
| `alert-dialog` | `dialog`, `button` | — |
| `area-chart` | `chart` | — |
| `avatar` | `avatar-variants` | — |
| `badge` | `badge-variants` | — |
| `bar-chart` | `chart` | — |
| `breadcrumb` | `breadcrumb-item` | — |
| `button` | `button-variants` | — |
| `callout` | `callout-variants` | — |
| `card` | `card-header`, `card-title`, `card-description`, `card-content`, `card-footer` | — |
| `carousel` | `carousel-item`, `carousel-content`, `carousel-previous`, `carousel-next`, `carousel-dots` | — |
| `chart` | `chart-variants`, `chart-styles` | `Blazor-ApexCharts` `6.0.2` |
| `collapsible` | `collapsible-trigger`, `collapsible-content` | — |
| `command` | `dialog` | — |
| `command-palette` | `command`, `shellui-js` | — |
| `context-menu` | `context-menu-models` | — |
| `copy-button` | `shellui-js` | — |
| `dashboard-01` | `sidebar`, `breadcrumb`, `separator`, `theme-toggle`, `app-sidebar` | — |
| `dashboard-02` | `sidebar`, `breadcrumb`, `separator`, `theme-toggle`, `app-sidebar` | — |
| `data-table` | `data-table-models` | `System.Linq.Dynamic.Core` `1.7.1` |
| `dialog` | `dialog-trigger`, `dialog-content`, `dialog-header`, `dialog-footer`, `dialog-title`, `dialog-description`, `dialog-close` | — |
| `donut-chart` | `chart` | — |
| `drawer` | `drawer-variants`, `drawer-trigger`, `drawer-content` | — |
| `empty-state` | `button` | — |
| `file-upload` | `shellui-js` | — |
| `form` | `label`, `input`, `button` | — |
| `input-otp` | `shellui-js` | — |
| `line-chart` | `chart` | — |
| `menubar` | `menubar-item` | — |
| `multi-series-chart` | `chart`, `chart-series` | — |
| `navigation-menu` | `navigation-menu-item` | — |
| `pie-chart` | `chart` | — |
| `radar-chart` | `chart` | — |
| `radial-chart` | `chart` | — |
| `radio-group` | `radio-group-item` | — |
| `sheet` | `sheet-variants`, `sheet-trigger`, `sheet-content` | — |
| `sidebar` | `shell`, `sidebar-models`, `sidebar-provider`, `sidebar-header`, `sidebar-content`, `sidebar-footer`, `sidebar-group`, `sidebar-group-label`, `sidebar-group-content`, `sidebar-menu`, `sidebar-menu-item`, `sidebar-menu-button`, `sidebar-menu-sub`, `sidebar-menu-sub-item`, `sidebar-menu-sub-button`, `sidebar-menu-action`, `sidebar-menu-badge`, `sidebar-separator`, `sidebar-trigger`, `sidebar-inset`, `sidebar-rail` | — |
| `sonner` | `sonner-variants`, `sonner-service` | — |
| `stepper` | `stepper-list`, `stepper-step`, `stepper-content` | — |
| `table` | `table-header`, `table-body`, `table-row`, `table-cell`, `table-head` | — |
| `tabs` | `tabs-list`, `tabs-trigger`, `tabs-content` | — |
| `theme-toggle` | `shellui-js` | — |
| `toggle` | `toggle-variants` | — |

The table is intentionally limited to the current direct targets. It does not replace the registry, and it should be regenerated when metadata changes. NuGet dependencies are shown where metadata declares them; chart-family targets inherit `chart`'s `Blazor-ApexCharts` dependency through recursive resolution.

## Hidden entries are support entries, not standalone claims

Several entries are deliberately hidden from `shellui list` while remaining part of the dependency graph:

- `button-variants` is hidden and is required by `button`.
- `card-header`, `card-title`, `card-description`, `card-content`, and `card-footer` are hidden card sub-components, not independent public card targets.
- `shell` is a hidden utility installed during `shellui init` and used by the sidebar graph.
- `shellui-js` is a hidden JavaScript support asset. It is used by `copy-button`, `file-upload`, `input-otp`, `theme-toggle`, `command-palette`, and the hidden `sidebar-provider`; it is not a standalone public component.
- `data-table-models`, `chart-variants`, `chart-styles`, `context-menu-models`, and similar entries are hidden support templates used by their registered parents.

A hidden entry can have an empty `Dependencies` list while still being required by a parent. For example, the metadata for `shellui-js` describes the asset itself; it does not turn every component that uses the asset into a dependency of the asset.

## Installation and resolution

When `shellui add` installs a target, the CLI:

1. Looks up the target in `ComponentRegistry`.
2. Installs each declared `Dependencies` entry recursively before the target.
3. Avoids duplicate work in the current batch and skips existing files unless `--force` is supplied.
4. Collects declared `NuGetDependencies` and adds each unique package after source files are written.
5. Wires registered `wwwroot` assets into the host when required by the installer.

For example:

```text
shellui add alert-dialog
```

`alert-dialog` resolves through its declared `dialog` and `button` dependencies, and each dependency is resolved recursively from the same registry. The exact installed set is therefore a registry behavior, not a hand-maintained standalone list.

## Checking the current graph

Use the CLI to inspect the public inventory:

```text
shellui list
```

For dependency changes, update `ComponentMetadata` in the registry and its template metadata together, then update this guide if a human-readable summary is useful. Do not add a dependency claim here unless it is represented by the registry metadata.
