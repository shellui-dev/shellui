# ShellUI Architecture

This document describes the technical architecture, design decisions, and system structure of ShellUI.

## Overview

ShellUI is a CLI-first Blazor component library that copies components directly into user projects rather than distributing them as NuGet packages. This architectural choice enables full customization and ownership of component code.

## System Architecture

**User/Developer** runs `shellui init | add | list | remove | update`.

**ShellUI.CLI** (Global .NET Tool) handles commands via:
- InitService / TailwindDownloader (init)
- ComponentInstaller (add, update – resolves dependencies, writes files)
- ComponentManager (list, remove)

**ShellUI.Templates** (embedded in CLI) provides ComponentRegistry with 139 templates. `GetComponentContent(name)` returns Razor/C# source.

**Packages:** ShellUI.Core (models/config), ShellUI.Components (NuGet – components, variants, services, theme CSS).

**User's Blazor Project** receives copied source in Components/UI/, Variants/, Services/, plus shellui.json and Tailwind config.

## Package Dependencies

- CLI references: Templates, Core, Components
- Templates and Components both depend on Core
- CLI copies source to user project on `add`
- NuGet option: User adds ShellUI.Components package directly

## Component Counts (Actual Components Only)

When you run `shellui add button`, that counts as **1 component**. Dependencies (button-variants, etc.) are auto-installed and **not** counted.

| Category | Installable | Description |
|----------|-------------|-------------|
| Form | 14 | Button, Input, Select, Checkbox, Switch, etc. |
| Layout | 15 | Card, Dialog, Sheet, Drawer, Popover, etc. |
| Navigation | 12 | Navbar, Sidebar, Tabs, Breadcrumb, etc. |
| Data Display | 14 | Table, Badge, Avatar, Alert, Toast, Sonner, etc. |
| Feedback | 8 | Toast, Sonner, Loading, Skeleton, Progress, etc. |
| Overlay | 10 | Dialog, Sheet, Drawer, Tooltip, Popover, etc. |
| Charts | 9 | Chart, BarChart, LineChart, PieChart, etc. |
| Utility | 18 | ThemeToggle, CopyButton, ScrollArea, etc. |
| **Total** | **~100** | *Actual components only; *-variants, *-service excluded* |

## Project Structure

- **src/ShellUI.CLI/** – Global tool (init, add, list, remove, update)
- **src/ShellUI.Core/** – ComponentMetadata, ShellUIConfig
- **src/ShellUI.Templates/** – 139 component templates (embedded in CLI)
- **src/ShellUI.Components/** – NuGet package
- **NET9/** – Blazor demo app
- **docs/** – Documentation

## Core Principles

1. **Copy, Don't Install** – Components copied to your project
2. **Tailwind-First** – All styling via Tailwind CSS v4
3. **Composability** – Variants for styling, Services for shared state
4. **Dependency Resolution** – `shellui add dialog` installs dialog + sub-components automatically

## Data Flow (shellui add)

1. Look up component in ComponentRegistry
2. Resolve dependencies (e.g., button → button-variants)
3. Get content from Template.Content
4. Write files (e.g., Components/UI/Button.razor, Variants/ButtonVariants.cs)
5. Update shellui.json

## Related Documentation

- [QUICKSTART.md](QUICKSTART.md) - Get started
- [CLI_SYNTAX.md](CLI_SYNTAX.md) - Command reference
- [COMPONENT_DEPENDENCIES.md](COMPONENT_DEPENDENCIES.md) - Dependency details
- [COMPONENT_ROADMAP.md](COMPONENT_ROADMAP.md) - Roadmap
