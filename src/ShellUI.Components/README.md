# ShellUI Components

Beautiful, accessible Blazor components inspired by shadcn/ui. A CLI-first component library with Tailwind CSS styling.

## Features

- 🎨 **53+ Production-ready components** - Button, Input, Card, Dialog, Table, and more
- 🎯 **CLI-first approach** - Install components individually with `dotnet shellui add`
- 🎨 **Tailwind CSS styling** - Utility-first CSS with dark mode support
- ♿ **Accessible by default** - Built with accessibility in mind
- 📱 **Responsive design** - Mobile-first approach
- 🔧 **Fully customizable** - Copy components to your project for full control

## Quick Start

### 1. Install the CLI tool

```bash
dotnet tool install -g ShellUI.CLI
```

### 2. Initialize ShellUI in your project

```bash
dotnet shellui init
```

### 3. Add components

```bash
# Add a button component
dotnet shellui add button

# Add multiple components
dotnet shellui add input card dialog

# List available components
dotnet shellui list
```

## Available Components

### Form Components
- **Button** - Various variants and sizes
- **Input** - Text input with validation
- **Textarea** - Multi-line text input
- **Select** - Dropdown selection
- **Checkbox** - Checkbox input
- **Switch** - Toggle switch
- **RadioGroup** - Radio button groups
- **Slider** - Range slider
- **Combobox** - Searchable dropdown
- **DatePicker** - Date selection
- **TimePicker** - Time selection
- **DateRangePicker** - Date range selection
- **InputOTP** - One-time password input
- **Form** - Form wrapper with validation

### Layout Components
- **Card** - Content container
- **Dialog** - Modal dialog
- **Sheet** - Side panel
- **Drawer** - Mobile drawer
- **Popover** - Floating content
- **Tooltip** - Hover tooltip
- **Separator** - Visual divider
- **ScrollArea** - Custom scrollable area
- **Resizable** - Resizable panels
- **Collapsible** - Collapsible content

### Navigation Components
- **Navbar** - Top navigation bar
- **Sidebar** - Side navigation
- **NavigationMenu** - Navigation menu
- **Menubar** - Menu bar
- **Breadcrumb** - Breadcrumb navigation
- **Pagination** - Page navigation
- **Tabs** - Tab navigation

### Data Display
- **Table** - Data table
- **Badge** - Status badges
- **Avatar** - User avatars
- **Alert** - Alert messages
- **Toast** - Toast notifications
- **Skeleton** - Loading placeholders
- **Progress** - Progress indicators

### Interactive Components
- **Dropdown** - Dropdown menu
- **Accordion** - Collapsible sections
- **Toggle** - Toggle button
- **ThemeToggle** - Dark/light mode toggle

## Styling

ShellUI components are styled with Tailwind CSS. Make sure you have Tailwind CSS configured in your project:

```bash
# Install Tailwind CSS
npm install -D tailwindcss
npx tailwindcss init
```

## Customization

Components are copied to your project, giving you full control:

```bash
# Components are installed to Components/UI/
Components/
  UI/
    Button.razor
    Input.razor
    Card.razor
    # ... other components
```

## Documentation

- [Component Gallery](https://shellui.dev/components)
- [Installation Guide](https://shellui.dev/docs/installation)
- [Customization](https://shellui.dev/docs/customization)

## Contributing

We welcome contributions! Please see our [Contributing Guide](https://github.com/shelltechlabs/shellui/blob/main/CONTRIBUTING.md).

## License

MIT License - see [LICENSE](https://github.com/shelltechlabs/shellui/blob/main/LICENSE) for details.

## Support

- 📖 [Documentation](https://shellui.dev)
- 🐛 [Issues](https://github.com/shelltechlabs/shellui/issues)
- 💬 [Discussions](https://github.com/shelltechlabs/shellui/discussions)
