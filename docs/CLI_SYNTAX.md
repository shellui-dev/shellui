# ShellUI CLI Syntax Guide

Complete reference for the ShellUI CLI commands.

## Installation

```bash
# Install globally
dotnet tool install -g ShellUI.CLI

# Update to latest version
dotnet tool update -g ShellUI.CLI

# Uninstall
dotnet tool uninstall -g ShellUI.CLI

# Check version
dotnet shellui --version
```

## Commands Overview

```bash
dotnet shellui [command] [options]

Commands:
  init              Initialize ShellUI in your project
  add               Add component(s) to your project
  remove            Remove component(s) from your project
  list              List available and installed components
  update            Update component(s) to latest version
  diff              Show differences between versions
  --version         Show CLI version
  --help            Show help information
```

## init - Initialize Project

Initialize ShellUI in your Blazor project.

```bash
dotnet shellui init [options]

Options:
  --force           Reinitialize even if already initialized
  --no-tailwind     Skip Tailwind CSS setup
  --style <style>   Choose component style (default, new-york, minimal)

Examples:
  dotnet shellui init
  dotnet shellui init --force
  dotnet shellui init --style new-york
```

**What it does:**
1. Detects your project type (Server/WASM/SSR)
2. Creates `Components/UI/` directory
3. Downloads Tailwind standalone CLI (no Node.js!)
4. Creates `shellui.json` configuration
5. Sets up Tailwind configuration
6. Updates `_Imports.razor`

## add - Add Components

Add one or more components to your project.

```bash
dotnet shellui add <components...> [options]

Arguments:
  components        Component name(s) to add

Options:
  --force          Overwrite existing components
  --style <style>  Choose component style
  --no-deps        Don't install dependencies
  
Syntax:
  # Single component
  dotnet shellui add button
  
  # Multiple components (space-separated)
  dotnet shellui add button card alert
  
  # Multiple components (comma-separated)
  dotnet shellui add button,card,alert
  
  # Mix both
  dotnet shellui add button,card alert dialog
  
  # Add with dependencies
  dotnet shellui add dialog  # Also installs button (dependency)
  
  # Add without dependencies
  dotnet shellui add dialog --no-deps
  
  # Force overwrite existing
  dotnet shellui add button --force
  
  # Choose style variant
  dotnet shellui add button --style new-york

Examples:
  # Essential components
  dotnet shellui add button,input,label,card,alert
  
  # Form components
  dotnet shellui add button,input,label,checkbox,radio,select
  
  # Layout components
  dotnet shellui add card,separator,skeleton,avatar
  
  # Add everything (why not!)
  dotnet shellui add button,card,alert,dialog,input,label,badge,skeleton,separator,avatar,checkbox,radio,select,textarea,switch,slider
```

## remove - Remove Components

Remove one or more components from your project.

```bash
dotnet shellui remove <components...> [options]

Arguments:
  components        Component name(s) to remove

Options:
  --force          Don't ask for confirmation
  --keep-deps      Keep dependencies

Syntax:
  # Single component
  dotnet shellui remove button
  
  # Multiple components (space-separated)
  dotnet shellui remove button card alert
  
  # Multiple components (comma-separated)
  dotnet shellui remove button,card,alert
  
  # Remove without confirmation
  dotnet shellui remove button --force

Examples:
  dotnet shellui remove button
  dotnet shellui remove button,card,alert
```

**Warning:** Will check if other components depend on the one you're removing.

## list - List Components

List all available components and show which are installed.

```bash
dotnet shellui list [options]

Options:
  --installed      Show only installed components
  --available      Show only available components
  --category <cat> Filter by category (form, layout, overlay, etc.)
  --json           Output as JSON

Examples:
  # List all components
  dotnet shellui list
  
  # Show only installed
  dotnet shellui list --installed
  
  # Show only available (not installed)
  dotnet shellui list --available
  
  # Filter by category
  dotnet shellui list --category form
  dotnet shellui list --category overlay
  
  # JSON output (for scripts)
  dotnet shellui list --json
```

**Output Example:**
```
Component      Status       Version    Category    Description
─────────────────────────────────────────────────────────────────────
button         installed    1.0.0      form        Interactive button
card           installed    1.0.0      layout      Content container
alert          available    1.0.0      feedback    Alert messages
dialog         available    1.0.0      overlay     Modal dialogs
input          available    1.0.0      form        Text input field
...
```

## update - Update Components

Update one or more components to the latest version.

```bash
dotnet shellui update [components...] [options]

Arguments:
  components        Component name(s) to update (empty = all)

Options:
  --force          Update without showing diff
  --all            Update all installed components

Syntax:
  # Update specific component
  dotnet shellui update button
  
  # Update multiple components
  dotnet shellui update button card alert
  dotnet shellui update button,card,alert
  
  # Update all installed components
  dotnet shellui update --all
  
  # Update without confirmation
  dotnet shellui update button --force

Examples:
  # Update single component (shows diff first)
  dotnet shellui update button
  
  # Update multiple
  dotnet shellui update button,card,alert
  
  # Update everything
  dotnet shellui update --all
```

**Note:** If you've customized a component, you'll see a diff and be asked to confirm.

## diff - Show Differences

Show differences between your version and the latest version.

```bash
dotnet shellui diff <component> [options]

Arguments:
  component         Component name

Options:
  --version <ver>  Compare with specific version

Examples:
  # Show diff with latest
  dotnet shellui diff button
  
  # Show diff with specific version
  dotnet shellui diff button --version 1.2.0
```

## Common Workflows

### Quick Start with CLI

```bash
# 1. Initialize
cd MyBlazorApp
dotnet shellui init

# 2. Add essential components
dotnet shellui add button,input,label,card,alert,badge,skeleton

# 3. Start building
# Components are in Components/UI/
```

### Quick Start with NuGet

```bash
# Just install package - all 40+ components available
dotnet add package ShellUI.Components

# No CLI needed, use immediately:
<Button>Click me</Button>
<Card>Content</Card>
```

### Hybrid Workflow (Recommended)

```bash
# 1. Install NuGet package (all components)
dotnet add package ShellUI.Components

# 2. Use everything
# All 40+ components work immediately

# 3. Later, customize specific ones
dotnet shellui add button,card

# 4. Edit Components/UI/Button.razor
# Your customized button is used
# Other components still from NuGet
```

### Building a Form

```bash
# Add all form-related components at once
dotnet shellui add button,input,label,textarea,checkbox,radio,select,switch,slider,form
```

### Building a Dashboard

```bash
# Add dashboard components
dotnet shellui add card,badge,avatar,table,tabs,separator,skeleton
```

### Building with Overlays

```bash
# Add overlay components
dotnet shellui add dialog,sheet,popover,tooltip,dropdown-menu,toast
```

## Tips & Tricks

### Batch Operations

```bash
# Install many components in one command
dotnet shellui add button,card,alert,badge,input,label,checkbox,radio,select,textarea,switch,slider,dialog,sheet,popover,tooltip,table,tabs,accordion,separator,skeleton,avatar

# Or use multiple commands
dotnet shellui add button,card,alert,badge
dotnet shellui add input,label,checkbox,radio,select
dotnet shellui add dialog,sheet,popover,tooltip
```

### Component Aliases

Some components have aliases for convenience:

```bash
dotnet shellui add btn       # Same as: button
dotnet shellui add dlg       # Same as: dialog
dotnet shellui add dd        # Same as: dropdown-menu
dotnet shellui add txt       # Same as: textarea
dotnet shellui add chk       # Same as: checkbox
```

### Wildcards (Future Feature)

```bash
# Add all form components (coming soon)
dotnet shellui add form.*

# Add all overlay components (coming soon)
dotnet shellui add overlay.*
```

## Configuration File

The `shellui.json` file stores your ShellUI configuration:

```json
{
  "version": "1.0.0",
  "projectType": "blazor-server",
  "componentsPath": "Components/UI",
  "tailwind": {
    "enabled": true,
    "version": "4.x",
    "binaryPath": ".shellui/tailwindcss.exe"
  },
  "components": [
    {
      "name": "button",
      "version": "1.0.0",
      "installedAt": "2026-01-15T10:30:00Z",
      "customized": true
    },
    {
      "name": "card",
      "version": "1.0.0",
      "installedAt": "2026-01-15T10:30:00Z",
      "customized": false
    }
  ]
}
```

## Exit Codes

- `0` - Success
- `1` - General error
- `2` - Component not found
- `3` - Already initialized
- `4` - Not initialized (need to run init first)
- `5` - Network error (can't download components)
- `6` - File conflict (component already exists, use --force)

## Environment Variables

```bash
# Change component registry URL
export SHELLUI_REGISTRY_URL="https://custom-registry.com"

# Change cache directory
export SHELLUI_CACHE_DIR="~/.shellui-cache"

# Disable telemetry
export SHELLUI_TELEMETRY=false
```

## Troubleshooting

### CLI not found after installation

```bash
# Make sure .NET tools path is in PATH
export PATH="$PATH:~/.dotnet/tools"

# Or reinstall
dotnet tool uninstall -g ShellUI.CLI
dotnet tool install -g ShellUI.CLI
```

### Component not found

```bash
# Update component list
dotnet shellui list --refresh

# Check spelling
dotnet shellui list | grep button
```

### Tailwind not compiling

```bash
# Check Tailwind binary
ls .shellui/

# Re-download Tailwind
dotnet shellui init --force
```

## Getting Help

```bash
# General help
dotnet shellui --help

# Command-specific help
dotnet shellui add --help
dotnet shellui init --help
dotnet shellui list --help
```

## Examples by Use Case

### Starter Template

```bash
# Everything you need to start
dotnet shellui add button,input,label,card,alert,badge,separator,skeleton
```

### E-commerce Site

```bash
dotnet shellui add button,card,badge,avatar,dialog,dropdown-menu,input,select,radio,checkbox,alert,toast
```

### Admin Dashboard

```bash
dotnet shellui add card,table,tabs,dialog,dropdown-menu,avatar,badge,button,input,select,sidebar,breadcrumb,pagination
```

### Landing Page

```bash
dotnet shellui add button,card,badge,separator,avatar,accordion
```

### Form-Heavy App

```bash
dotnet shellui add button,input,label,textarea,checkbox,radio,select,switch,slider,form,alert
```

---

**CLI Version:** 1.0.0  
**Last Updated:** October 2025

For more information, visit the [documentation](https://shellui.dev/docs) or run `dotnet shellui --help`.

