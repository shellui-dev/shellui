# ShellUI

A modern, CLI-first Blazor component library inspired by shadcn/ui. Copy components directly into your project and customize them to match your needs.

**New here? Start with [START_HERE.md](START_HERE.md) for a complete guided tour!**

## Vision

ShellUI transforms Blazor component development with a hybrid approach:
- **CLI-First**: Copy components to YOUR codebase for full control (`dotnet shellui add button`)
- **NuGet Option**: Traditional package install for quick starts (`dotnet add package ShellUI`)
- **Choose your workflow**: Use CLI for customization, NuGet for speed, or mix both
- Powered by Tailwind CSS v4.1.14 (standalone CLI - no Node.js required!)
- Best of both worlds: flexibility when you need it, convenience when you want it

## Current Status: 69 Components Complete! 🎉

**ShellUI is now fully functional!** We've completed:
- ✅ **CLI Tool** (`dotnet tool install -g ShellUI.CLI`)
- ✅ **NuGet Package** (`dotnet add package ShellUI.Components`)
- ✅ **69 Production-Ready Components** with Tailwind v4.1.14
- ✅ **Hybrid Workflow** (CLI + NuGet)
- ✅ **No Node.js Required** (Standalone Tailwind CLI)
- ✅ **Comprehensive Documentation**
- ✅ **Working Demos & Examples**

**Ready to use today!** 🚀

## What's Working Today 🚀

### ✅ CLI Tool + NuGet Package
```bash
# Install CLI globally
dotnet tool install -g ShellUI.CLI

# Or install locally for testing
dotnet tool install -g ShellUI.CLI --add-source ./src/ShellUI.CLI/bin/Release

# Initialize in your Blazor project (choose npm or standalone)
dotnet shellui init

# Add components
dotnet shellui add button input card dialog
dotnet shellui list  # See all available components
```

### ✅ 69 Production-Ready Components

**Form Components (12):**
Button, Input, Textarea, Select, Checkbox, RadioGroup, RadioGroupItem, Switch, Toggle, Label, Slider, Form, InputOTP

**Layout Components (13):**
Card, Dialog, Sheet, Drawer, Popover, Tooltip, Separator, ScrollArea, Resizable, Collapsible, Accordion, AccordionItem, Breadcrumb, BreadcrumbItem

**Navigation Components (9):**
Navbar, Sidebar, NavigationMenu, NavigationMenuItem, Menubar, MenubarItem, Pagination, Tabs, Stepper

**Data Display (12):**
Table, TableHeader, TableBody, TableRow, TableHead, TableCell, DataTable, Badge, Avatar, Alert, Toast, Skeleton, Progress, Loading

**Interactive Components (7):**
Dropdown, Command, ContextMenu, HoverCard, ThemeToggle, EmptyState, FileUpload

**Advanced Components (16):**
Calendar, DatePicker, DateRangePicker, TimePicker, Combobox, AlertDialog, Carousel, CarouselItem, CarouselContent, CarouselPrevious, CarouselNext, CarouselDots

### ✅ Tailwind CSS v4.1.14 Integration

**Two Setup Methods:**

**Method 1: Standalone CLI (No Node.js!)**
```bash
dotnet shellui init  # Choose "standalone"
```
- Downloads Tailwind CLI binary automatically
- No Node.js or npm required
- Auto-builds on project compile

**Method 2: npm (If you prefer)**
```bash
dotnet shellui init  # Choose "npm"
```
- Installs `tailwindcss@^4.1.14` + `@tailwindcss/cli@^4.1.14`
- Uses `npx @tailwindcss/cli` for builds
- Requires Node.js

### 🎨 Easy Theme Customization

**Customize themes instantly with [tweakcn](https://tweakcn.com/):**

1. Visit tweakcn and design your perfect theme
2. Copy the generated CSS variables
3. Paste into `wwwroot/input.css`
4. All ShellUI components update automatically!

**Custom fonts?** Add Google Fonts links and update your CSS variables - works seamlessly! 🔤

## What's Next

### Phase 1: Additional Components
**Target: Q1 2026**

- More advanced components
- Enhanced DataTable features (sorting/filtering)
- Charts and data visualization
- VirtualScroll for large lists
- PDFViewer component
- And more...

### Phase 2: Documentation & Polish
**Target: Q2 2026**

- Complete documentation website
- Video tutorials
- Migration guides
- Performance optimization
- Testing infrastructure

## Design Principles

1. **Copy, Don't Install**: Components are copied to your project, not imported from a package
2. **Tailwind-First**: All styling uses Tailwind CSS v4.1.14 utility classes
3. **Accessible by Default**: WCAG 2.1 AA compliant out of the box
4. **Composable**: Build complex components from simple ones
5. **Customizable**: Modify any component to fit your needs
6. **Type-Safe**: Leverage C# type system for better DX
7. **Performance**: Optimized for both Server and WASM scenarios
8. **No Node.js Required**: Standalone Tailwind CLI for maximum compatibility

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

### Why Tailwind v4.1.14?
- Latest stable version with v4 features
- Better performance than v3
- Improved dark mode support
- Native CSS variable support
- Smaller output CSS
- Standalone CLI (no Node.js required!)

### Component Structure
```
YourProject/
├── Components/
│   └── UI/                    # ShellUI components live here
│       ├── Button.razor
│       ├── Input.razor
│       ├── Card.razor
│       └── ... (69 components)
├── wwwroot/
│   ├── input.css              # Tailwind input (@import "tailwindcss";)
│   └── app.css                # Compiled CSS (auto-generated)
├── .shellui/
│   └── bin/                   # Tailwind CLI binary (standalone method)
├── tailwind.config.js         # Tailwind configuration
├── shellui.json              # ShellUI configuration
└── Build/
    └── ShellUI.targets       # MSBuild integration
```

## Developer Experience Today

### Quick Start
```bash
# Install CLI globally
dotnet tool install -g ShellUI.CLI

# Or test locally
dotnet tool install -g ShellUI.CLI --add-source ./src/ShellUI.CLI/bin/Release

# Initialize in your Blazor project (choose npm or standalone)
dotnet shellui init

# Add components
dotnet shellui add button input card dialog
dotnet shellui list  # See all 69 available components
```

### Use Components
```razor
@page "/example"

<Card>
    <CardHeader>
        <CardTitle>Welcome to ShellUI</CardTitle>
        <CardDescription>Build beautiful Blazor apps</CardDescription>
    </CardHeader>
    <CardContent>
        <Input Placeholder="Enter your email" Type="email" />
        <Button Class="mt-4">Subscribe</Button>
    </CardContent>
</Card>
```

### Customize Components
Simply edit the component file in `Components/UI/` - it's yours to modify!

## Technical Requirements

- .NET 8.0 or higher
- **Choice of Tailwind setup:**
  - **Standalone CLI** (recommended): No Node.js required
  - **npm**: Requires Node.js, uses `tailwindcss@^4.1.14`

## Comparison with Existing Solutions

| Feature | ShellUI | MudBlazor | Radzen | Blazorise |
|---------|---------|-----------|--------|-----------|
| CLI Installation | ✅ | ❌ | ❌ | ❌ |
| NuGet Package | ✅ | ✅ | ✅ | ✅ |
| Component Ownership (CLI) | ✅ | ❌ | ❌ | ❌ |
| Tailwind CSS | ✅ (v4.1.14) | ❌ | ❌ | ❌ |
| No Node.js Required | ✅ | N/A | N/A | N/A |
| Hybrid Workflow | ✅ | ❌ | ❌ | ❌ |
| Free & Open Source | ✅ | ✅ | Partial | ✅ |
| Customization | Full | Limited | Limited | Limited |
| Components | 69+ | 70+ | 50+ | 80+ |
| Current Status | Production Ready | Mature | Commercial | Mature |

## Installation Options

### Option 1: CLI Tool (Recommended)
```bash
dotnet tool install -g ShellUI.CLI
dotnet shellui init  # Choose your Tailwind method
dotnet shellui add button input card dialog
```

### Option 2: NuGet Package
```bash
dotnet add package ShellUI.Components
# Manual Tailwind setup required (see README.md)
```

## Contributing

ShellUI is production-ready! We welcome contributions:

- 🐛 **Bug reports** via GitHub Issues
- 💡 **Feature requests** for new components
- 📝 **Documentation improvements**
- 🧪 **Testing and feedback**

## License

MIT License - See LICENSE.txt for details

## Acknowledgments

- Inspired by [shadcn/ui](https://ui.shadcn.com/) for the CLI-first approach
- Forked from [Sysinfocus simple/ui](https://github.com/Sysinfocus/simple-ui) by [@sysinfocus](https://github.com/Sysinfocus)
- Built with love for the Blazor community

## Documentation

**Quick Links:**
- [INDEX.md](INDEX.md) - Complete documentation index
- [tailwind-setup.md](tailwind-setup.md) - Tailwind CSS setup guide
- [COMPONENT_ROADMAP.md](COMPONENT_ROADMAP.md) - Component development roadmap

**Installation & Usage:**
- [START_HERE.md](START_HERE.md) - Complete guided tour
- [FAQ.md](FAQ.md) - Frequently asked questions
- [CLI_SYNTAX.md](CLI_SYNTAX.md) - CLI command reference

**Architecture:**
- [ARCHITECTURE.md](docs/ARCHITECTURE.md) - Technical architecture and design decisions

## Status

**✅ Production Ready:** 69 components, CLI + NuGet, Tailwind v4.1.14 integration
**🚀 Ready to use today!**

---

**ShellUI is fully functional and ready for production use!** 🎉
