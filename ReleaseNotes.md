# ShellUI Release Notes

## Project Transformation Announcement

ShellUI is a complete transformation of the Sysinfocus simple/ui library into a modern, CLI-first component library for Blazor, inspired by shadcn/ui.

### What's New?

ShellUI represents a fundamental shift in how Blazor developers work with UI components:

**From:** Installing NuGet packages with components locked away  
**To:** CLI-based component installation that copies code directly into your project

**From:** Limited customization within package constraints  
**To:** Full ownership and customization of every component

**From:** Custom CSS framework  
**To:** Tailwind CSS v4 for modern, utility-first styling

**From:** Complex setup process  
**To:** Simple `dotnet shellui init` and you're ready

### Key Features (In Development)

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
   - 40+ production-ready components
   - Fully accessible (WCAG 2.1 AA compliant)
   - Works with Blazor Server, WASM, and SSR
   - Customizable to your needs

4. **Developer Experience**
   - Copy-paste ready components
   - Full control over code
   - TypeScript-like DX with C#
   - Comprehensive documentation

### Current Status: In Development

This project is currently being developed according to the milestone plan:

- **Milestone 1:** CLI Tool Foundation (In Progress)
- **Milestone 2:** Tailwind v4 Integration (Planned)
- **Milestone 3:** Component Architecture (Planned)
- **Milestone 4:** Component Registry (Planned)
- **Milestone 5:** Documentation & Polish (Planned)

See [MILESTONES.md](MILESTONES.md) for detailed development roadmap.

### Target Release Timeline

- **Alpha Release:** Q1 2025 - CLI + Core Components
- **Beta Release:** Q2 2025 - Full Component Library
- **v1.0 Release:** Q3 2025 - Production Ready

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
