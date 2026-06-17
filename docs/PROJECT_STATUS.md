# ShellUI Project Status

## Current Status: v0.3.0-alpha.2 (Pre-release) 🎉

**ShellUI alpha is available on NuGet!** Test before upgrading to stable.

- ✅ **Version 0.3.0-alpha.3** - 68 installable components (top-level only; sub-components, variants, models, services auto-installed)
- ✅ **68 Production-Ready Components** - Fully functional and tested
- ✅ **CLI Tool Published** - `dotnet tool install -g ShellUI.CLI`
- ✅ **NuGet Packages Published** - `ShellUI.Components`, `ShellUI.CLI`, `ShellUI.Templates`
- ✅ **Hybrid Distribution** - CLI + NuGet packages (best of both worlds!)
- ✅ **No Node.js Required** - Using Tailwind standalone CLI (zero JavaScript dependencies!)
- ✅ **Tailwind CSS v4.1.18** - Latest version with standalone CLI support

## What's Complete ✅

### Core Infrastructure
- [x] CLI Tool (`ShellUI.CLI`) - Published to NuGet
- [x] Components Package (`ShellUI.Components`) - Published to NuGet
- [x] Templates Package (`ShellUI.Templates`) - Embedded in CLI
- [x] Component Registry - 167 templates (68 installable; rest are sub-components/variants/models/services that auto-install)
- [x] Tailwind CSS Integration - v4.1.18 standalone CLI support
- [x] MSBuild Integration - Automatic CSS compilation

### Components (68 Installable)

Counts are top-level components users invoke directly via `shellui add <name>`. Anything that auto-installs as a dependency (e.g. `SidebarTrigger`, `DialogContent`, `ButtonVariants`, `SonnerService`) is not counted.

- [x] **Form (17)**: Button, Checkbox, Combobox, DatePicker, DateRangePicker, FileUpload, Form, Input, InputOTP, Label, RadioGroup, Select, Slider, Switch, Textarea, TimePicker, Toggle
- [x] **Layout (12)**: Accordion, Breadcrumb, Card, Collapsible, DashboardLayout01, DashboardLayout02, LinkCard, Navbar, Resizable, ScrollArea, Separator, Sidebar
- [x] **Navigation (7)**: ContextMenu, Menubar, NavigationMenu, Pagination, PrevNextNav, Stepper, Tabs
- [x] **Overlay (8)**: AlertDialog, Command, Dialog, Drawer, Dropdown, HoverCard, Popover, Sheet
- [x] **Data Display (13)**: AreaChart, Avatar, Badge, BarChart, Calendar, Carousel, Chart, ChartSeries, DataTable, LineChart, MultiSeriesChart, PieChart, Table
- [x] **Feedback (9)**: Alert, Callout, EmptyState, Loading, Progress, Skeleton, Sonner, Toast, Tooltip
- [x] **Utility (2)**: CopyButton, ThemeToggle

### Documentation
- [x] README.md - Project overview and quick start
- [x] ARCHITECTURE.md - Technical architecture and design
- [x] DEPLOYMENT.md - Release and deployment guide
- [x] ReleaseNotes.md - Version history
- [x] Component READMEs - Included in NuGet packages

### Features
- [x] CLI commands: `init`, `add`, `remove`, `list`
- [x] Component dependency resolution
- [x] Tailwind CSS standalone CLI integration
- [x] Theme-aware components (light/dark mode)
- [x] Responsive design
- [x] Accessibility support (ARIA attributes, keyboard navigation)

## What's Next 🚀

### Short Term (v0.0.3+)
- [ ] More component variants
- [ ] Enhanced documentation with examples
- [ ] Component playground/demo site
- [ ] Performance optimizations
- [ ] Additional loading animations
- [ ] More form validation helpers

### Medium Term (v0.3.0+)
- [ ] Component themes/presets
- [ ] Visual component editor
- [ ] Storybook-like playground
- [ ] Advanced composition patterns
- [ ] Additional components

### Long Term (v1.0.0+)
- [ ] Full component library (100+ components)
- [ ] Comprehensive documentation website
- [ ] Video tutorials
- [ ] Community contributions
- [ ] Performance monitoring tools

## Timeline

| Milestone | Status |
|-----------|--------|
| v0.3.0-alpha - 68 installable components, CLI, NuGet | ✅ Done |
| Tailwind v4.1.18 standalone | ✅ Done |
| Alpha testing & validation | 🔄 In progress |
| v0.3.0 stable (after testing) | 🎯 Next |
| v1.0 - Full release | 🎯 Target Q2-Q3 2026 |

*Note: Stepper active-state highlighting known issue; shipping as-is for alpha.*

## How to Get Started with Development

### Prerequisites
- .NET 9.0 SDK or higher (components target .NET 9.0)
- Git
- Visual Studio 2022 or VS Code
- Basic understanding of Blazor
- Familiarity with Tailwind CSS

**No Node.js required!** We use Tailwind standalone CLI.

### Installation

**Install CLI Tool:**
```bash
dotnet tool install -g ShellUI.CLI
```

**Install Components Package:**
```bash
dotnet add package ShellUI.Components
```

**Initialize in Your Project:**
```bash
dotnet shellui init
dotnet shellui add button input card
```

### Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/shellui-dev/shellui.git
   cd shellui
   ```

2. **Build the solution**
   ```bash
   dotnet build ShellUI.sln
   ```

3. **Run the demo**
   ```bash
   cd NET9/BlazorInteractiveServer
   dotnet run
   ```

4. **Test CLI locally**
   ```bash
   cd src/ShellUI.CLI
   dotnet pack -c Release
   dotnet tool install -g ShellUI.CLI --add-source ./bin/Release
   ```

## Key Decisions Made

### 1. CLI-First Approach
**Decision:** Use CLI to copy components instead of NuGet packages  
**Rationale:** Gives users full control, better customization, transparent code  
**Inspired by:** shadcn/ui for React

### 2. Tailwind CSS v4
**Decision:** Use Tailwind v4 as the styling framework  
**Rationale:** Modern, popular, utility-first, great for component libraries  
**Alternative considered:** Custom CSS (like original Sysinfocus)

### 3. Component Ownership Model
**Decision:** Components live in user's codebase  
**Rationale:** Full customization, no version lock-in, better debugging  
**Trade-off:** Users manage updates manually

### 4. .NET 9.0 Target
**Decision:** Target .NET 9.0  
**Rationale:** Latest features, best performance, modern Blazor capabilities  
**Impact:** Requires .NET 9.0 SDK or higher

### 5. Accessibility First
**Decision:** WCAG 2.1 AA compliance required for all components  
**Rationale:** Accessibility is not optional, better for everyone  
**Implementation:** Built into component templates

## Success Criteria

### v0.3.0-alpha Success ✅
- [x] CLI tool published to NuGet
- [x] NuGet packages published (Components, CLI, Templates)
- [x] Can initialize projects (no Node.js!)
- [x] 68 installable components working (both CLI and NuGet)
- [x] Basic documentation live
- [ ] 50+ GitHub stars (in progress)

### Beta Success (Q2 2026)
- [x] 68 installable components available
- [ ] Component registry operational
- [ ] Hybrid workflow proven
- [ ] 500+ GitHub stars
- [ ] 50+ projects using ShellUI
- [ ] Active community feedback

### v1.0 Success (Q3 2026)
- [ ] All milestones complete
- [ ] 1000+ GitHub stars
- [ ] 500+ projects using ShellUI
- [ ] Documentation website live
- [ ] Featured on Awesome Blazor
- [ ] Active community (100+ members)

## Resources

### Documentation Files
- [README.md](README.md) - Start here
- [MILESTONES.md](MILESTONES.md) - Detailed tasks
- [ROADMAP.md](ROADMAP.md) - Timeline and progress
- [ARCHITECTURE.md](ARCHITECTURE.md) - Technical design
- [QUICKSTART.md](QUICKSTART.md) - Future user guide
- [COMPARISON.md](COMPARISON.md) - vs other libraries

### External Resources
- [shadcn/ui](https://ui.shadcn.com/) - Inspiration
- [Tailwind CSS v4](https://tailwindcss.com/) - CSS framework
- [Blazor Docs](https://learn.microsoft.com/aspnet/core/blazor/) - Framework docs
- [WCAG 2.1](https://www.w3.org/WAI/WCAG21/quickref/) - Accessibility guidelines

## Contributing

We welcome contributions! See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

**Ways to contribute:**
- Report bugs via GitHub Issues
- Suggest new components or features
- Improve documentation
- Submit pull requests
- Share feedback and ideas

## Questions?

- GitHub Issues: [Report bugs or ask questions](https://github.com/shellui-dev/shellui/issues)
- NuGet: [ShellUI.Components](https://www.nuget.org/packages/ShellUI.Components/)
- Documentation: See [docs/](../docs/) folder

## Contact

- GitHub: [@shellui-dev/shellui](https://github.com/shellui-dev/shellui)
- NuGet: [ShellUI Packages](https://www.nuget.org/packages?q=shellui)
- Issues: [GitHub Issues](https://github.com/shellui-dev/shellui/issues)

---

**Last Updated:** February 2026  
**Current Version:** v0.3.0-alpha.2  
**Status:** Alpha (pre-release; test and validate before stable release)

---

## Quick Reference: Using ShellUI Today

### Installation
```bash
# Install CLI globally
dotnet tool install -g ShellUI.CLI

# Or install components package
dotnet add package ShellUI.Components
```

### Initialize Project
```bash
dotnet shellui init
```

### Add Components
```bash
dotnet shellui add button card alert dialog
```

### Use Components
```razor
@using ShellUI.Components

<Card>
    <CardHeader>
        <CardTitle>Hello ShellUI</CardTitle>
    </CardHeader>
    <CardContent>
        <Input Placeholder="Enter text" />
    </CardContent>
    <CardFooter>
        <Button>Submit</Button>
    </CardFooter>
</Card>
```

### Customize
Just edit `Components/UI/Button.razor` - it's your code!

---

**Ready to use ShellUI?** Check the [README.md](../README.md) for complete getting started guide!

