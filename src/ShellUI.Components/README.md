# ShellUI Components

Beautiful, accessible Blazor components inspired by shadcn/ui. A CLI-first component library with Tailwind CSS styling.

## Features

- 🎨 **100 Production-ready components** - Button, Input, Card, Dialog, Sonner, Table, Charts, and more
- 🎯 **CLI-first approach** - Install components individually with `dotnet shellui add`
- 🎨 **Tailwind CSS styling** - Utility-first CSS with dark mode support
- ♿ **Accessible by default** - Built with accessibility in mind
- 📱 **Responsive design** - Mobile-first approach
- 🔧 **Fully customizable** - Copy components to your project for full control

## Quick Start

### Option 1: CLI Tool (Recommended)

The CLI tool provides the best developer experience with automatic setup:

#### 1. Install the CLI tool

```bash
dotnet tool install -g ShellUI.CLI
```

#### 2. Initialize ShellUI in your project

```bash
dotnet shellui init
```

This automatically:
- ✅ Downloads Tailwind CSS CLI (standalone, no Node.js required)
- ✅ Creates CSS files and configuration
- ✅ Sets up MSBuild integration for auto-building
- ✅ Creates component folders

#### 3. Add components

```bash
# Add a button component
dotnet shellui add button

# Add multiple components
dotnet shellui add input card dialog

# List available components
dotnet shellui list
```

### Option 2: NuGet Package

For manual setup or existing projects:

#### 1. Install the package

```bash
dotnet add package ShellUI.Components
```

#### 2. Set up Tailwind CSS

Choose one of these methods:

**Method A: Tailwind CLI (Recommended)**
```bash
# Download Tailwind CLI (standalone)
curl -sLO https://github.com/tailwindlabs/tailwindcss/releases/latest/download/tailwindcss-windows-x64.exe
# Or for Linux/Mac: tailwindcss-linux-x64 or tailwindcss-macos-x64

# Create input.css
echo '@import "tailwindcss";' > wwwroot/input.css

# Create tailwind.config.js
echo 'module.exports = {
  content: ["./**/*.{razor,html,cs}"],
  theme: { extend: {} },
  plugins: []
}' > tailwind.config.js

# Build CSS
./tailwindcss -i wwwroot/input.css -o wwwroot/app.css
```

**Method B: npm (if you prefer Node.js)**
```bash
# Install Tailwind CSS
npm install -D tailwindcss
npx tailwindcss init

# Update tailwind.config.js
echo 'module.exports = {
  content: ["./**/*.{razor,html,cs}"],
  theme: { extend: {} },
  plugins: []
}' > tailwind.config.js

# Create input.css
echo '@import "tailwindcss/base";
@import "tailwindcss/components";
@import "tailwindcss/utilities";' > wwwroot/input.css

# Build CSS
npx tailwindcss -i wwwroot/input.css -o wwwroot/app.css
```

#### 3. Add to your layout

```html
<!-- In your MainLayout.razor or App.razor -->
<link href="~/app.css" rel="stylesheet" />
```

#### 4. Use components

```html
@using ShellUI.Components

<Button Variant="primary">Click me</Button>
<Input Placeholder="Enter text..." />
<Card>
    <CardHeader>
        <CardTitle>Hello World</CardTitle>
    </CardHeader>
    <CardContent>
        <p>This is a card component!</p>
    </CardContent>
</Card>
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

## Bootstrap Compatibility

ShellUI components work alongside Bootstrap. You can:

- **Keep both** - ShellUI and Bootstrap can coexist
- **Remove Bootstrap** - Delete Bootstrap references if you prefer Tailwind-only
- **Gradual migration** - Use ShellUI for new components, keep Bootstrap for existing ones

## Customization

### 🎨 Theme Customization

**Easily customize themes with [tweakcn](https://tweakcn.com/):**

1. Design your perfect theme on tweakcn
2. Copy the generated CSS variables
3. Paste into `wwwroot/input.css`
4. All ShellUI components update automatically!

**Custom Fonts:** Add Google Fonts links and update `--font-*` variables in your CSS.

### Component Customization

Components are copied to your project, giving you full control:

```bash
# Components are installed to Components/UI/
Components/
  UI/
    Button.razor      # Edit directly!
    Input.razor       # Customize as needed
    Card.razor        # Full ownership
    # ... other components
```

Modify any component to match your design system perfectly!

## Documentation

- [Component Gallery](https://shellui.dev/components)
- [Installation Guide](https://shellui.dev/docs/installation)
- [Customization](https://shellui.dev/docs/customization)

## Contributing

We welcome contributions! Please see our [Contributing Guide](https://github.com/shellui-dev/shellui/blob/main/CONTRIBUTING.md).

## License

MIT License - see [LICENSE](https://github.com/shellui-dev/shellui/blob/main/LICENSE) for details.

## Support

- 📖 [Documentation](https://shellui.dev)
- 🐛 [Issues](https://github.com/shellui-dev/shellui/issues)
- 💬 [Discussions](https://github.com/shellui-dev/shellui/discussions)
