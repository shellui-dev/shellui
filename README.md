# ShellUI

A modern, CLI-first Blazor component library inspired by shadcn/ui. Copy components directly into your project and customize them to match your needs.

**New here? Start with [START_HERE.md](START_HERE.md) for a complete guided tour!**

## Vision

ShellUI transforms Blazor component development with a hybrid approach:
- **CLI-First**: Copy components to YOUR codebase for full control (`dotnet shellui add button`)
- **NuGet Option**: Traditional package install for quick starts (`dotnet add package ShellUI`)
- **Choose your workflow**: Use CLI for customization, NuGet for speed, or mix both
- Powered by Tailwind CSS v4.x (standalone CLI - no Node.js required!)
- Best of both worlds: flexibility when you need it, convenience when you want it

## Current Status

This project is currently in development. The original Sysinfocus simple/ui library is being transformed into a modern hybrid (CLI + NuGet) design system with Tailwind CSS v4.

## Roadmap to v1.0

### MILESTONE 1: CLI Tool + NuGet Package Foundation
**Goal:** Create both CLI tool and traditional NuGet package for hybrid workflow

**Tasks:**
1. Create new .NET Tool project (ShellUI.CLI)
2. Create new Class Library project (ShellUI.Components)
3. Implement command parser with System.CommandLine
4. Implement `dotnet shellui init` command
   - Initialize shellui.json configuration file
   - Detect project type (Blazor Server, WASM, SSR)
   - Set up folder structure (Components/UI)
   - Download Tailwind CSS standalone CLI (no Node.js!)
   - Add Tailwind CSS v4 configuration
5. Implement `dotnet shellui add [components...]` command
   - Support multiple components: `add button card alert`
   - Support comma-separated: `add button,card,alert`
   - Download components from registry
   - Copy to user's Components/UI folder
   - Update necessary imports
   - Show installation success message for each
6. Implement `dotnet shellui list` command
   - Show all available components
   - Show installed components (marked differently)
7. Implement `dotnet shellui update [component]` command
   - Update specific component or all components
8. Create NuGet package (ShellUI.Components)
   - Traditional Razor Class Library
   - Same components, different distribution
   - Can work alongside CLI
9. Create both packages and test installation
   - CLI: `dotnet tool install -g ShellUI.CLI`
   - NuGet: `dotnet add package ShellUI.Components`
10. Add version checking and auto-update notifications

**Deliverables:**
- Working CLI tool installable via dotnet tool
- Working NuGet package installable traditionally
- Basic commands functional
- Configuration file system
- Hybrid workflow supported

---

### MILESTONE 2: Tailwind v4 Integration (No Node.js Required!)
**Goal:** Seamless Tailwind CSS v4.x.x integration using standalone CLI

**Tasks:**
1. Research Tailwind CSS v4 changes and standalone CLI
2. Implement Tailwind standalone CLI downloader
   - Auto-detect OS (Windows, Mac, Linux)
   - Download appropriate binary from GitHub releases
   - Cache binary in project/.shellui/ folder
   - No Node.js or npm required!
3. Create Tailwind v4 configuration templates
   - Create tailwind.config.js template
   - Create input.css with Tailwind directives
   - No package.json needed!
4. Set up CSS processing pipeline
   - Use standalone CLI for compilation
   - Configure build process for CSS compilation
   - Set up watch mode for development
5. Create design tokens system
   - Define color palette (primary, secondary, accent, etc.)
   - Define spacing scale
   - Define typography scale
   - Define border radius values
   - Define shadow values
6. Implement dark mode support
   - CSS variable-based theming
   - Dark mode toggle component
   - Persistent theme storage
7. Create utility classes for common patterns
8. Create MSBuild targets to run Tailwind CLI automatically
9. Document Tailwind standalone CLI approach

**Deliverables:**
- Complete Tailwind v4 setup (no Node.js!)
- Standalone CLI auto-download and caching
- Design token system
- Dark mode support
- Automated build integration

---

### MILESTONE 3: Component Architecture
**Goal:** Convert existing components to standalone, Tailwind-styled Razor components

**Tasks:**
1. Analyze current Sysinfocus component structure
2. Design new component architecture
   - Single-file Razor components
   - Tailwind utility classes (no CSS isolation)
   - Composition over configuration
   - Accessible by default (ARIA attributes)
3. Create component template structure
   - Standard parameter patterns
   - Event callback patterns
   - Slot/RenderFragment patterns
4. Convert essential components (Priority 1):
   - Button (with variants: default, outline, ghost, link, destructive)
   - Input (text, email, password, number, etc.)
   - Label
   - Card (with sub-components: CardHeader, CardTitle, CardDescription, CardContent, CardFooter)
   - Badge
   - Alert (with variants: default, destructive, warning, success)
5. Convert layout components (Priority 2):
   - Separator
   - Skeleton
   - Avatar
   - Container
   - AspectRatio
6. Convert form components (Priority 3):
   - Checkbox
   - Radio / RadioGroup
   - Select
   - Textarea
   - Switch
   - Slider
   - Form (with validation)
7. Convert overlay components (Priority 4):
   - Dialog / Modal
   - Sheet / Drawer
   - Popover
   - Tooltip
   - DropdownMenu
   - Toast / Notification
8. Convert data display components (Priority 5):
   - Table / DataTable
   - Tabs
   - Accordion
   - Collapsible
   - Calendar
   - DatePicker
9. Convert navigation components (Priority 6):
   - NavigationMenu
   - Breadcrumb
   - Pagination
   - Sidebar
10. Add JSInterop utilities where needed
    - Click outside detection
    - Focus trap
    - Portal/Teleport for overlays

**Deliverables:**
- 40+ production-ready components
- Consistent API across all components
- Full accessibility support
- Comprehensive component documentation

---

### MILESTONE 4: Component Registry
**Goal:** Create a centralized component registry and distribution system

**Tasks:**
1. Design component registry structure
   - Component metadata (name, description, dependencies)
   - Component categories
   - Version tracking
   - Dependency graph
2. Create component template files
   - Component .razor file
   - Component code-behind (if needed)
   - Component dependencies list
   - Usage examples
3. Set up component storage
   - GitHub repository structure
   - Version control for components
   - Component versioning strategy
4. Implement component resolver in CLI
   - Fetch component from registry
   - Resolve dependencies
   - Handle version conflicts
5. Create component dependency system
   - Auto-install dependent components
   - Handle shared utilities
   - Manage JSInterop dependencies
6. Implement component templates with variations
   - Multiple style variants per component
   - Configuration options
7. Create hooks/utilities library
   - Common hooks (useMediaQuery, useLocalStorage, etc.)
   - Validation utilities
   - Form helpers
8. Add component examples and demos
   - Usage examples for each component
   - Composition examples
   - Best practices guide

**Deliverables:**
- Complete component registry
- Dependency resolution system
- Template variations
- Shared utilities library

---

### MILESTONE 5: Documentation & Polish
**Goal:** Create world-class documentation and developer experience

**Tasks:**
1. Create documentation website
   - Built with Blazor
   - Component showcase with live examples
   - Copy-paste ready code snippets
   - Search functionality
   - Responsive design
2. Write comprehensive guides
   - Getting Started guide
   - Installation guide
   - Customization guide
   - Theming guide
   - Accessibility guide
   - Best practices guide
3. Create component documentation
   - Component API reference
   - Props/Parameters documentation
   - Events documentation
   - Examples for each component
   - Composition patterns
4. Create video tutorials
   - Quick start video
   - Component usage videos
   - Customization videos
5. Set up community infrastructure
   - GitHub Discussions
   - Issue templates
   - Contributing guidelines
   - Code of conduct
   - Component request template
6. Create starter templates
   - Blazor Server starter
   - Blazor WASM starter
   - Blazor SSR starter
   - Full-stack template with auth
7. Add advanced features
   - Component preview in VS Code
   - Intellisense improvements
   - Code snippets for popular IDEs
8. Performance optimization
   - Bundle size optimization
   - Lazy loading patterns
   - Virtualization examples
9. Testing infrastructure
   - Unit test examples
   - Integration test examples
   - E2E test examples
10. Create migration guide from Sysinfocus simple/ui
11. Create comparison guide (vs MudBlazor, Radzen, etc.)
12. Add telemetry (opt-in) to understand usage patterns
13. Prepare for 1.0 release
    - Version all components
    - Create changelog
    - Release notes
    - Marketing materials

**Deliverables:**
- Complete documentation website
- Video tutorials
- Starter templates
- Migration guides
- v1.0 release

---

## Essential Components (First Implementation)

These components will be implemented first as they form the foundation:

1. Button - The most fundamental interactive element
2. Input - Essential for forms
3. Label - Form accessibility
4. Card - Common layout pattern
5. Badge - Status indicators
6. Alert - User feedback
7. Separator - Layout divider
8. Skeleton - Loading states

## Design Principles

1. **Copy, Don't Install**: Components are copied to your project, not imported from a package
2. **Tailwind-First**: All styling uses Tailwind utility classes
3. **Accessible by Default**: WCAG 2.1 AA compliant out of the box
4. **Composable**: Build complex components from simple ones
5. **Customizable**: Modify any component to fit your needs
6. **Type-Safe**: Leverage C# type system for better DX
7. **Performance**: Optimized for both Server and WASM scenarios

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

### Why Tailwind v4?
- Latest features and optimizations
- Better performance than v3
- Improved dark mode support
- Native CSS variable support
- Smaller output CSS

### Component Structure
```
YourProject/
├── Components/
│   └── UI/              # ShellUI components live here
│       ├── Button.razor
│       ├── Input.razor
│       ├── Card.razor
│       └── ...
├── wwwroot/
│   └── styles/
│       ├── input.css    # Tailwind input
│       └── output.css   # Compiled CSS
├── package.json
├── tailwind.config.js
└── shellui.json         # ShellUI configuration
```

## Target Developer Experience

### Initialize a new project
```bash
# Install CLI globally
dotnet tool install -g ShellUI.CLI

# Initialize in your Blazor project
dotnet shellui init

# Add single component
dotnet shellui add button

# Add multiple components (space-separated)
dotnet shellui add button card alert

# Add multiple components (comma-separated)
dotnet shellui add button,card,alert,dialog,input

# Mix both
dotnet shellui add button,card alert dialog
```

### Use components
```razor
@page "/example"

<Card>
    <CardHeader>
        <CardTitle>Welcome to ShellUI</CardTitle>
        <CardDescription>Build beautiful Blazor apps</CardDescription>
    </CardHeader>
    <CardContent>
        <Input Placeholder="Enter your email" Type="email" />
    </CardContent>
    <CardFooter>
        <Button>Subscribe</Button>
    </CardFooter>
</Card>
```

### Customize components
Simply edit the component file in `Components/UI/` - it's yours!

## Technical Requirements

- .NET 8.0 or higher
- Tailwind CSS standalone CLI (automatically downloaded, no Node.js required!)
- Blazor Server, WASM, or SSR

**Note**: Node.js is optional. We use Tailwind's standalone CLI which has zero dependencies.

## Comparison with Existing Solutions

| Feature | ShellUI | MudBlazor | Radzen | Sysinfocus |
|---------|---------|-----------|--------|------------|
| CLI Installation | Yes | No | No | No |
| NuGet Package | Yes | Yes | Yes | Yes |
| Component Ownership (CLI) | Yes | No | No | No |
| Tailwind CSS | Yes (v4) | No | No | No |
| No Node.js Required | Yes | N/A | N/A | N/A |
| Hybrid Workflow | Yes | No | No | No |
| Free & Open Source | Yes | Yes | Partial | Yes |
| Customization | Full | Limited | Limited | Limited |

## Contributing

This project is in active development. Once we reach v1.0, we'll welcome contributions. For now, follow along with the milestones!

## License

MIT License - See LICENSE.txt for details

## Acknowledgments

- Inspired by [shadcn/ui](https://ui.shadcn.com/) for the CLI-first approach
- Forked from [Sysinfocus simple/ui](https://github.com/Sysinfocus/simple-ui) by [@sysinfocus](https://github.com/Sysinfocus)
- Built with love for the Blazor community

## Documentation

**Quick Links:**
- [INDEX.md](INDEX.md) - Complete documentation index and navigation guide
- [UPDATES.md](UPDATES.md) - Important updates: Hybrid approach + No Node.js!
- [FAQ.md](FAQ.md) - Frequently asked questions
- [CLI_SYNTAX.md](CLI_SYNTAX.md) - Complete CLI command reference
- [PROJECT_STATUS.md](PROJECT_STATUS.md) - Current status and immediate next steps

**Comprehensive Documentation:**
- [MILESTONES.md](MILESTONES.md) - All 5 milestones with 295+ detailed tasks
- [ROADMAP.md](ROADMAP.md) - Visual timeline from Q4 2025 to Q3 2026
- [ARCHITECTURE.md](ARCHITECTURE.md) - Technical architecture and design decisions
- [QUICKSTART.md](QUICKSTART.md) - Quick start guide (for when v1.0 is released)
- [COMPARISON.md](COMPARISON.md) - How ShellUI compares to other Blazor libraries
- [CONTRIBUTING.md](CONTRIBUTING.md) - Contribution guidelines (will open after alpha)

**See [INDEX.md](INDEX.md) for complete navigation guide.**

## Future: ShellDocs

After ShellUI v1.0, we'll build **ShellDocs** - a fumadocs-inspired documentation framework for .NET!

- [SHELLDOCS_VISION.md](SHELLDOCS_VISION.md) - Complete vision and architecture
- [SHELLDOCS_STRATEGY.md](SHELLDOCS_STRATEGY.md) - Build strategy (what comes first)

**ShellDocs = Fumadocs for .NET** - filling a massive gap in the .NET ecosystem!

## Status

**Current Phase:** Planning Complete ✓  
**Next Phase:** Milestone 1 - CLI Tool Foundation

Currently in development. Star this repo to follow progress!

See [PROJECT_STATUS.md](PROJECT_STATUS.md) for detailed status.

---

**Note:** This README represents the vision and roadmap. Implementation is in progress. Check the milestones above for current status.
