# ShellUI CLI

Command-line tool for managing ShellUI Blazor components. Install components individually with a simple command.

## Installation

```bash
dotnet tool install -g ShellUI.CLI
```

## Quick Start

### Initialize ShellUI in your project

```bash
dotnet shellui init
```

This automatically:
- ✅ Downloads Tailwind CSS CLI (standalone, no Node.js required)
- ✅ Creates CSS files and configuration
- ✅ Sets up MSBuild integration for auto-building
- ✅ Creates component folders

### Add components

```bash
# Add a single component
dotnet shellui add button

# Add multiple components
dotnet shellui add input card dialog table

# List all available components
dotnet shellui list

# List installed components
dotnet shellui list --installed
```

## Commands

### `init`
Initialize ShellUI in your Blazor project.

```bash
dotnet shellui init
```

**Options:**
- `--force` - Overwrite existing files
- `--style <style>` - Choose CSS style (default: default)
- `--tailwind <method>` - Choose Tailwind method (standalone, npm) (default: standalone)
- `--yes` - Run in non-interactive mode with default options

### `add <components>`
Add one or more components to your project.

```bash
dotnet shellui add button input card
```

**Options:**
- `--force` - Overwrite existing components

### `list`
List available or installed components.

```bash
# List all available components
dotnet shellui list

# List installed components
dotnet shellui list --installed

# List available components
dotnet shellui list --available
```

### `remove <components>`
Remove components from your project.

```bash
dotnet shellui remove button input
```

**Options:**
- `--all` - Remove all installed components

## Available Components

### Form Components
- `button`, `input`, `textarea`, `select`, `checkbox`, `switch`, `radio-group`, `slider`, `combobox`, `date-picker`, `time-picker`, `date-range-picker`, `input-otp`, `form`

### Layout Components
- `card`, `dialog`, `sheet`, `drawer`, `popover`, `tooltip`, `separator`, `scroll-area`, `resizable`, `collapsible`

### Navigation Components
- `navbar`, `sidebar`, `navigation-menu`, `menubar`, `breadcrumb`, `pagination`, `tabs`

### Data Display
- `table`, `data-table`, `badge`, `avatar`, `alert`, `toast`, `skeleton`, `progress`, `loading`

### Interactive Components
- `dropdown`, `accordion`, `toggle`, `theme-toggle`, `command`, `context-menu`, `hover-card`

## Component Dependencies

When you install a component, its dependencies are automatically installed:

```bash
dotnet shellui add dialog
# Automatically installs: button (dependency)
```

## Project Structure

After running `init`, your project structure will look like:

```
YourProject/
├── Components/
│   └── UI/          # Components are installed here
├── wwwroot/
│   ├── input.css    # Tailwind input file
│   └── app.css      # Compiled CSS (auto-generated)
├── tailwind.config.js
└── Build/
    └── ShellUI.targets  # MSBuild integration
```

## Updating Components

To update a component to the latest version:

```bash
dotnet shellui add button --force
```

## Troubleshooting

### Component not found
Make sure you're using the correct component name. Use `dotnet shellui list` to see all available components.

### Build errors
Ensure Tailwind CSS is properly configured:
1. Run `dotnet shellui init` if you haven't already
2. Check that `tailwind.config.js` exists
3. Verify `wwwroot/input.css` contains `@import "tailwindcss";`

### CLI not found
Reinstall the CLI tool:
```bash
dotnet tool uninstall -g ShellUI.CLI
dotnet tool install -g ShellUI.CLI
```

## Documentation

- [Full Documentation](https://shellui.dev/docs/cli)
- [Component Reference](https://shellui.dev/components)
- [GitHub Repository](https://github.com/shellui-dev/shellui)

## License

MIT License - see [LICENSE](https://github.com/shellui-dev/shellui/blob/main/LICENSE) for details.

