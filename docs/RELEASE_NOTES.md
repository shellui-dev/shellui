# ShellUI v0.1.0 🎉

> Beautiful, accessible Blazor components inspired by shadcn/ui

## ✨ Highlights

This is the **first official release** of ShellUI - a CLI-first Blazor component library that brings the shadcn/ui philosophy to .NET developers.

### 🚀 What's Included

#### CLI Tool (`ShellUI.CLI`)
- **`shellui init`** - Initialize your Blazor project with Tailwind CSS
- **`shellui add <component>`** - Add individual components to your project
- **`shellui list`** - List all available components
- **`shellui remove <component>`** - Remove installed components
- **`shellui update`** - Update components to the latest version

#### Components Library (`ShellUI.Components`)
Pre-built components for NuGet installation:
- **Button** - Interactive button with variants (default, destructive, outline, secondary, ghost, link)
- **Badge** - Status indicators with multiple variants
- **Alert** - Notification banners with icons
- **Card** - Content containers with header, content, and footer sections
- **Input** - Form input fields
- **Label** - Accessible form labels
- **Separator** - Visual dividers
- **Shell utilities** - `Shell.Cn()` for Tailwind class merging

## 📦 Installation

### Option 1: CLI (Recommended)
```bash
# Install the CLI globally
dotnet tool install -g ShellUI.CLI

# Initialize your project
shellui init

# Add components
shellui add button badge alert card
```

### Option 2: NuGet Package
```bash
dotnet add package ShellUI.Components
```

## 🎨 Styling

ShellUI uses **Tailwind CSS v4** with CSS variables for theming. The `shellui init` command automatically sets up:
- Tailwind CSS standalone CLI (no Node.js required)
- CSS variables for light/dark themes
- Component styling aligned with shadcn/ui

## 📋 Requirements

- .NET 9.0 or later
- Blazor Server, WebAssembly, or Interactive modes

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

## 🙏 Acknowledgments

Inspired by [shadcn/ui](https://ui.shadcn.com/) - the beautiful React component library.

---

**Full Changelog**: https://github.com/shellui-dev/shellui/commits/v0.1.0
