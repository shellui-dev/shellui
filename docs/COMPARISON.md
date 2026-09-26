# ShellUI vs Other Blazor UI Libraries

This is a qualitative comparison, not a benchmark report. Library features, package contents, licensing, and community activity change independently; verify the current project documentation before making a production decision.

## Current ShellUI snapshot

- **Target framework:** .NET 10
- **Tailwind CSS:** `4.3.2`
- **CLI inventory:** 76 direct targets, 176 registry entries, and 100 hidden entries
- **Distribution:** the CLI copies source into an application; the repository also contains the packable `ShellUI.Components` package
- **Packaging boundary:** only `ShellUI.CLI` and `ShellUI.Components` are packable. `ShellUI.Core` and `ShellUI.Templates` are internal/non-packable projects.

The direct-target count is the number users invoke with `shellui add`. It is not directly comparable to a package's total file or sub-component count.

## Quick comparison

| Area | ShellUI | MudBlazor | Radzen | Blazorise | Ant Design Blazor | Sysinfocus simple/ui |
|---|---|---|---|---|---|---|
| Distribution | CLI source-copy workflow; `ShellUI.Components` is packable | NuGet package | NuGet package | NuGet package | NuGet package | NuGet package |
| CSS model | Tailwind CSS `4.3.2` | Material-oriented component styling | Project-specific component styling | Provider-based CSS options | Ant Design styling | Project-specific styling |
| Direct component count | 76 CLI targets; 176 registry entries; 100 hidden | Not audited here | Not audited here | Not audited here | Not audited here | Not audited here |
| Source customization | Copied component source can be edited | Package/API customization | Package/API customization | Package/API customization | Package/API customization | Package/API customization |
| CLI workflow | Yes | No CLI in this comparison | No CLI in this comparison | No CLI in this comparison | No CLI in this comparison | No CLI in this comparison |
| Accessibility status | Semantic markup is used; no current WCAG conformance claim | Not audited here | Not audited here | Not audited here | Not audited here | Not audited here |
| Bundle/performance status | No current reproducible benchmark published | No current benchmark published here | No current benchmark published here | No current benchmark published here | No current benchmark published here | No current benchmark published here |
| Community status | Current project is an alpha snapshot; no comparable size claim | See the current project community | See the current project community | See the current project community | See the current project community | See the current project community |

The “not audited here” cells are intentional. They avoid presenting stale component counts, community metrics, or performance estimates as if they were current facts.

## ShellUI's differentiators

### CLI-first source ownership

`shellui init` creates the project integration and `shellui add` writes the selected component source into the application. That makes the generated code inspectable and editable. It also means the application owns the installed code and must manage updates to customized files.

### Tailwind-first composition

ShellUI templates use Tailwind utility classes and CSS variables. ShellUI pins Tailwind CSS `4.3.2`; the generated Tailwind setup uses the v4 CSS entry point rather than the older v3 `@import` layer sequence.

### Registry-driven dependencies

`ComponentRegistry` is authoritative for the 76 direct targets, 100 hidden support entries, source dependencies, and NuGet dependencies. A component's visible sub-components or support files do not imply that it is a standalone package.

### Compositional APIs

Dialog, Card, Tabs, Stepper, Select, Sidebar, Carousel, Dropdown, Popover, ContextMenu, NavigationMenu, Drawer, and Sheet have explicit registry-managed sub-components where supported. See the [component roadmap](COMPONENT_ROADMAP.md) and [dependency guide](COMPONENT_DEPENDENCIES.md) for the current scope.

## When ShellUI may fit

Choose ShellUI when the project benefits from:

- Direct ownership and editing of copied Blazor component source
- Tailwind CSS as the styling foundation
- A CLI workflow similar to copy-based component libraries
- A .NET 10 component source that can be adapted for a particular application
- Explicit control over generated code and dependencies

These are workflow and architecture characteristics, not a claim that ShellUI has more features, better performance, or broader compatibility than every packaged library.

## When another library may fit

Consider a packaged library when the project values a maintained package update path, a vendor-provided visual builder, or a complete pre-integrated design system. The right choice depends on the exact components, render mode, accessibility testing, support requirements, and release cadence required by the application.

| Library | Characteristic to evaluate in its current documentation |
|---|---|
| MudBlazor | Material-oriented component set and package-based delivery |
| Radzen | Data-heavy components, visual tooling, and commercial offerings |
| Blazorise | Provider choices and CSS-framework flexibility |
| Ant Design Blazor | Ant Design conventions and enterprise-oriented components |
| Sysinfocus simple/ui | The package-based foundation and its current feature set |

Descriptions here are orientation points, not a current feature audit.

## Accessibility

ShellUI components use semantic HTML and accessibility-oriented attributes where appropriate, but the repository does not publish a current WCAG 2.1 AA conformance audit. Do not treat the project as certified or universally accessible without testing the rendered application.

For an application using ShellUI or another library, test at least:

- Keyboard navigation and visible focus
- Screen-reader names, roles, and state changes
- Dialogs, menus, tabs, comboboxes, and other focus-managed overlays
- Color contrast and reduced-motion behavior
- Validation and error announcements
- The target browser, render mode, and assistive-technology combination

## Performance and bundle size

No reproducible ShellUI-versus-library benchmark is published in this repository, so the old estimated bundle-size and first-paint tables should not be used as evidence. Actual output depends on the selected components, Tailwind scanning, compression, hosting model, render mode, and browser cache state.

A useful comparison should use the same .NET 10 application, release configuration, component set, theme, and host. Measure the generated CSS and application payload, then record the exact build and runtime conditions.

The CLI's copy-based model can avoid installing unused component source, but it is not a guarantee of a smaller final application. Conversely, a NuGet package is not automatically larger or smaller for every consumer.

## Community and support

Community size is not a stable, comparable metric. Stars, forum activity, Discord membership, and commercial support change over time, and no current comparable dataset is maintained here. Check the official project repositories and support channels directly.

ShellUI is prerelease software. Validate the specific release you plan to use.

## Migration considerations

### From a packaged Blazor library

1. Inventory the components and behavior actually used.
2. Map package components to ShellUI direct targets where an equivalent exists.
3. Replace package imports and provider configuration with the ShellUI initialization and Tailwind setup.
4. Review event names, parameter types, validation, and JavaScript requirements.
5. Test accessibility, rendering mode, and browser behavior before shipping.

### From Sysinfocus simple/ui

1. Initialize the project with the ShellUI CLI.
2. Add the direct targets that replace the components in use.
3. Review the generated Tailwind variables and CSS.
4. Adjust API differences and test the resulting application.
5. Consult the [release notes](RELEASE_NOTES.md) for historical migration context.

## Decision checklist

- **Need source-level control?** ShellUI is designed around copied, editable source.
- **Prefer a package update path?** Evaluate the current support and release model of a packaged library.
- **Need Tailwind?** ShellUI's current templates target Tailwind CSS `4.3.2`.
- **Need a specific advanced component?** Confirm that the exact component is in the current registry before choosing a library.
- **Need accessibility or performance guarantees?** Run project-specific audits and benchmarks; this document does not provide either guarantee.

## Current ShellUI references

- [Component roadmap](COMPONENT_ROADMAP.md)
- [Component dependency metadata guide](COMPONENT_DEPENDENCIES.md)
- [Tailwind setup guide](tailwind-setup.md)
- [Release notes](RELEASE_NOTES.md)
- [GitHub repository](https://github.com/shellui-dev/shellui)
- [ShellUI.Components on NuGet](https://www.nuget.org/packages/ShellUI.Components)
- [ShellUI documentation](https://shellui.dev)
