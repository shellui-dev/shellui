# ShellUI Components

Beautiful, accessible Blazor components inspired by shadcn/ui. A CLI-first component library with Tailwind CSS styling.

## Features

- 🎨 **68 Production-ready installable components** - Button, Input, Card, Dialog, Sonner, Table, Charts, and more (sub-components and variants ship as auto-installed dependencies)
- 🎯 **CLI-first approach** - Install components individually with `dotnet shellui add`
- 🎨 **Tailwind CSS styling** - Utility-first CSS with dark mode support
- ♿ **Accessible by default** - Built with accessibility in mind
- 📱 **Responsive design** - Mobile-first approach
- 🔧 **Fully customizable** - Copy components to your project for full control

## Two install paths

Pick one based on whether you have Tailwind set up in your project.

### Path A — NuGet + your existing Tailwind setup (new in 0.4.x)

If your project already uses Tailwind, install the NuGet package and add one `@source` directive to your input.css. The package ships a `shellui-classes.txt` safelist that Tailwind scans at build time so utility classes used inside ShellUI components end up in your compiled CSS — tree-shaken, only what's used:

```bash
dotnet add package ShellUI.Components
```

After restore, the safelist appears at `wwwroot/shellui-classes.txt` in your project. In your `wwwroot/input.css`:

```css
@import "tailwindcss";
@source "./shellui-classes.txt";

/* ... your theme variables, custom layers, etc. ... */
```

Then in your `App.razor` or `_Host.cshtml`:
```html
@using ShellUI.Components
```

That's it. `<Button>`, `<Card>`, `<Dialog>` etc. work with full styling — Tailwind has seen every utility class ShellUI emits.

### Path B — CLI tool (best DX, full source control)

If you don't already use Tailwind, or want full source-level control over every component, use the CLI:

```bash
dotnet tool install -g ShellUI.CLI
shellui init                          # sets up Tailwind, theme CSS, patches App.razor
shellui add button card dialog        # copies component source so Tailwind can scan it
```

The CLI copies the `.razor` files into your project, where Tailwind picks up the classes. You can edit any component freely — it's your code.

`shellui add` copies the `.razor` files into your project, where Tailwind picks up the classes. From then on, components render styled.

A future release will ship a pre-compiled stylesheet (`shellui-all.css`) for users who don't want Tailwind set up at all — `<link>` tag, no build step. Tracked under the v0.5 milestone.

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
