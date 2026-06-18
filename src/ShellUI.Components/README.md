# ShellUI Components

Beautiful, accessible Blazor components inspired by shadcn/ui. A CLI-first component library with Tailwind CSS styling.

## Features

- 🎨 **68 Production-ready installable components** - Button, Input, Card, Dialog, Sonner, Table, Charts, and more (sub-components and variants ship as auto-installed dependencies)
- 🎯 **CLI-first approach** - Install components individually with `dotnet shellui add`
- 🎨 **Tailwind CSS styling** - Utility-first CSS with dark mode support
- ♿ **Accessible by default** - Built with accessibility in mind
- 📱 **Responsive design** - Mobile-first approach
- 🔧 **Fully customizable** - Copy components to your project for full control

## ⚠️ Read this first

This package ships the component DLLs, JS interop, and helpers (`Shell.Cn`). **It does not produce styled components on its own.** Tailwind v4 builds CSS by scanning `.razor` source files at compile time — the component source lives inside this DLL, so Tailwind never sees the utility classes used inside ShellUI components.

The supported way to get styled components is the **`ShellUI.CLI` global tool**:

```bash
dotnet tool install -g ShellUI.CLI
shellui init                          # sets up Tailwind, theme CSS, patches App.razor
shellui add button card dialog        # copies component source so Tailwind can scan it
```

`shellui add` copies the `.razor` files into your project, where Tailwind picks up the classes. From then on, components render styled.

A future release (`v0.4.x`) will support installing this NuGet package by itself — by shipping a pre-compiled stylesheet — without needing the CLI for setup. Until then, **install the CLI**.

## When is this NuGet package useful on its own?

- You're building a library that re-exports ShellUI components or extends them with custom variants
- You want to reference `Shell.Cn` (the class-name combiner) from your own helpers
- You want the JS interop registration (`shellui.js`) provided as a `_content/ShellUI.Components/` static asset

Otherwise: start with the CLI.

## CLI quick start

```bash
dotnet tool install -g ShellUI.CLI
shellui init           # one-time
shellui add button     # any time you want more components
shellui list           # see what's available
```

`shellui init` automatically:
- Downloads Tailwind CSS CLI (standalone, no Node.js required) or uses your existing npm install
- Writes the full default theme to `wwwroot/input.css` (`:root` / `.dark` / `@theme inline` blocks)
- Patches `Components/App.razor` (or `wwwroot/index.html` for WASM) with `@rendermode`, a theme-bootstrap `<script>` to avoid FOUC, and a `<script src="shellui.js">` tag for the JS interop
- Sets up MSBuild integration so Tailwind rebuilds on every `dotnet build`

`shellui add <name>` automatically:
- Resolves and installs all sub-component dependencies
- Runs `dotnet add package` for any required NuGet packages (e.g. `Blazor-ApexCharts` for charts, `System.Linq.Dynamic.Core` for `DataTable`)
- Auto-links any CSS assets (e.g. `wwwroot/css/charts.css`) into your `App.razor`
- Suggests close matches if you mistype a component name

## Using components

After `shellui init` + `shellui add`, components are in your project source:

```razor
@using YourProject.Components.UI

<Button Variant="ButtonVariant.Default">Click me</Button>
<Card>
    <CardHeader>
        <CardTitle>Hello World</CardTitle>
    </CardHeader>
    <CardContent>
        <p>This is a card component.</p>
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
