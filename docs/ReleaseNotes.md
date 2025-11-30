# ShellUI Release Notes

## Version 0.0.3 (Current) - November 2025

### 🎉 Release v0.0.3 - Fixed v0.0.2 Issues

ShellUI v0.0.3 is now available on NuGet! This release fixes issues from v0.0.2 and includes improved documentation and package metadata.

### What's Included

- ✅ **CLI Tool** (`ShellUI.CLI` v0.0.3) - Install globally: `dotnet tool install -g ShellUI.CLI`
- ✅ **Components Package** (`ShellUI.Components` v0.0.3) - Install: `dotnet add package ShellUI.Components`
- ✅ **Templates Package** (`ShellUI.Templates` v0.0.3) - Internal dependency for CLI

### Features

1. **69 Production-Ready Components**
   - Form: Button, Input, Textarea, Select, Checkbox, Switch, RadioGroup, Slider, etc.
   - Layout: Card, Dialog, Sheet, Drawer, Popover, Tooltip, Separator, etc.
   - Navigation: Navbar, Sidebar, NavigationMenu, Menubar, Breadcrumb, Pagination, Tabs
   - Data Display: Table, DataTable, Badge, Avatar, Alert, Toast, Skeleton, Progress, Loading
   - Interactive: Dropdown, Accordion, Toggle, ThemeToggle, Command, ContextMenu, HoverCard
   - Advanced: Carousel, Stepper, EmptyState, FileUpload, Calendar, DatePicker, etc.

2. **Hybrid Distribution Model**
   - **CLI-First**: Copy components to your project for full customization
   - **NuGet Option**: Traditional package install for quick starts
   - Use both together - NuGet for speed, CLI for customization

3. **Tailwind CSS v4.1.14**
   - Standalone CLI (no Node.js required!)
   - Automatic dark mode support
   - Theme-aware components
   - Customizable design tokens

4. **Developer Experience**
   - Simple setup: `dotnet shellui init`
   - Add components: `dotnet shellui add button card dialog`
   - Full source code ownership
   - Easy customization

### Installation

**CLI Tool:**
```bash
dotnet tool install -g ShellUI.CLI
```

**Components Package:**
```bash
dotnet add package ShellUI.Components
```

### Quick Start

```bash
# Initialize ShellUI in your project
dotnet shellui init

# Add components
dotnet shellui add button input card dialog

# Or use NuGet package
dotnet add package ShellUI.Components
```

### Documentation

- [Getting Started Guide](../README.md)
- [Architecture Documentation](ARCHITECTURE.md)
- [Component Reference](../README.md#available-components)
- [CLI Usage Guide](CLI_SYNTAX.md)

### Known Limitations

- This is a pre-release version (0.0.3)
- Some advanced features may be missing
- Documentation is being actively improved
- Breaking changes may occur before v1.0.0

### What's Next

- More components (targeting 70+)
- Enhanced documentation
- Component examples and demos
- Performance optimizations
- Community feedback integration

---

## Version 0.0.1 (November 2025)

### Initial Release

- First NuGet package release
- Basic CLI functionality
- Core component set
- Tailwind CSS integration

---

## Project Transformation Announcement

ShellUI is a complete transformation into a modern, CLI-first component library for Blazor, inspired by shadcn/ui.

### Key Features

1. **CLI-First Approach**
   - Install globally: `dotnet tool install -g ShellUI.CLI`
   - Initialize project: `dotnet shellui init`
   - Add components: `dotnet shellui add button card alert`
   - Components are copied to your project, not locked in packages

2. **Tailwind CSS v4 Integration**
   - Modern utility-first CSS framework
   - Automatic dark mode support
   - Customizable design tokens
   - Optimized for production

3. **Component Library**
   - 69 production-ready components
   - Fully accessible (WCAG 2.1 AA compliant)
   - Works with Blazor Server, WASM, and SSR
   - Customizable to your needs

4. **Developer Experience**
   - Copy-paste ready components
   - Full control over code
   - TypeScript-like DX with C#
   - Comprehensive documentation

### Breaking Changes from Sysinfocus simple/ui

If you're migrating from Sysinfocus simple/ui:

1. **Installation Method Changed**
   - Old: `dotnet add package Sysinfocus.AspNetCore.Components`
   - New: `dotnet shellui init` + `dotnet shellui add [component]`

2. **Styling Approach Changed**
   - Old: Custom CSS framework with built-in styles
   - New: Tailwind CSS utility classes

3. **Component Distribution Changed**
   - Old: NuGet package with compiled components
   - New: CLI that copies source code to your project

4. **Setup Process Changed**
   - Old: Manual configuration in Program.cs, _Imports.razor, etc.
   - New: Automated setup via `dotnet shellui init`

A comprehensive migration guide will be available before v1.0 release.

### For Existing Sysinfocus simple/ui Users

**Don't worry!** The original Sysinfocus simple/ui library is still available and functional. This repo has been forked to create ShellUI as a separate, modern alternative.

If you're using Sysinfocus simple/ui:
- Continue using it - it works great!
- Watch this repo for ShellUI updates
- A migration guide will be provided when ShellUI v1.0 is released

### Contributing

We're actively developing ShellUI and will welcome contributions once we reach alpha release. For now:

- Star the repo to follow progress
- Watch for release announcements
- Check milestone progress in [MILESTONES.md](MILESTONES.md)
- Open issues for suggestions (labeled as "Future Consideration")

### Stay Updated

- GitHub: Watch this repository for updates
- Issues: Track progress and report bugs
- Discussions: Join conversations about the project

---

## Previous Sysinfocus simple/ui Release Notes

Below are the historical release notes from the original Sysinfocus simple/ui library that this project was forked from. These versions are part of the old architecture and will not receive further updates in this repository.

For the original Sysinfocus simple/ui library, visit: [Original Repository](https://github.com/Sysinfocus/simple-ui)

---

### Version 0.0.3.3 (Legacy - Sysinfocus simple/ui)

**New / Updates / Bug fixes**
1. Fixed `EmptyTemplate` bug in DataTable component
2. Updated Checkbox component to highlight border when hovered/focused
3. Updated RadioGroupItem component to highlight border when hovered/focused
4. Added SafeAreaView component
5. Updated Calendar component smart year navigation
6. Fixed Select component placeholder styling
7. Replaced base font to Google Font "Geist"
8. Fixed Drawer component bottom padding

---

### Version 0.0.3.2 (Legacy - Sysinfocus simple/ui)

**New / Updates / Bug fixes**
1. Fixed Icon component OnClick bug when Disabled
2. Added ContainerStyle property to Input component
3. Fixed DatePicker component DateTime issue

---

### Version 0.0.3.1 (Legacy - Sysinfocus simple/ui)

**New / Updates / Bug fixes**
1. Added ServerSentEvents component
2. Fixed Input component UI
3. Fixed Select component submit action issue

---

### Version 0.0.3.0 (Legacy - Sysinfocus simple/ui)

**New / Updates / Bug fixes**
1. Fixed Input component UI and binding issue
2. Added DisableMove property to Pills component
3. Other minor UI fixes

---

### Version 0.0.2.9 (Legacy - Sysinfocus simple/ui)

**New / Updates / Bug fixes**
1. Updated Input component to support Format attribute
2. Updated Treeview component events
3. Added Prefix and Suffix attributes to Input
4. Added browser extension methods
5. Added MarkdownPreview component
6. Other minor UI fixes

---

### Version 0.0.2.8 (Legacy - Sysinfocus simple/ui)

**New / Updates / Bug fixes**
1. Fixed bug for Tabs component
2. Updated Scheduler component
3. Other minor UI fixes

---

### Version 0.0.2.7 (Legacy - Sysinfocus simple/ui)

**New / Updates / Bug fixes**
1. ResizeObserver component added
2. Fixed scrollIntoViewIfNeeded bug
3. Other minor UI fixes

---

### Version 0.0.2.6 (Legacy - Sysinfocus simple/ui)

**New / Updates / Bug fixes**
1. TabPages component added
2. Presenter component added
3. Pills component added
4. Updated Input component Format parameter
5. Fixed Tabs component event issue
6. Streamlined box-shadow
7. Other minor UI fixes

---

### Version 0.0.2.5 (Legacy - Sysinfocus simple/ui)

**New / Updates / Bug fixes**
1. Sidebar component added
2. Timeline component added
3. Updated BrowserExtensions
4. Other minor UI fixes

---

### Version 0.0.2.4 (Legacy - Sysinfocus simple/ui)

**New / Updates / Bug fixes**
1. ColorPicker component added
2. Notification component added
3. Updated Icon component with FluentUI and Lucide icons
4. Updated BrowserExtensions with clipboard and share methods
5. Other minor UI fixes

---

For complete legacy release history, see commit history before project transformation.

---

## Acknowledgments

ShellUI is built on the foundation of [Sysinfocus simple/ui](https://github.com/Sysinfocus/simple-ui) by [@sysinfocus](https://github.com/Sysinfocus). We're grateful for their work in creating the original component library.

This transformation takes the project in a new direction, inspired by the CLI-first approach of [shadcn/ui](https://ui.shadcn.com/).

## License

MIT License - See LICENSE.txt for details

---

**Have questions or suggestions?**  
Open an issue on GitHub or start a discussion!
