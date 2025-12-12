# ShellUI Architecture

This document describes the technical architecture, design decisions, and implementation details of ShellUI.

## Overview

ShellUI is a CLI-first Blazor component library that copies components directly into user projects rather than distributing them as NuGet packages. This architectural choice enables full customization and ownership of component code.

## Core Principles

### 1. Copy, Don't Install
Unlike traditional component libraries, ShellUI copies component source code directly into the user's project. This provides:
- Full control over component code
- Easy customization without forking
- No package version conflicts
- Transparent implementation
- Better debugging experience

### 2. Tailwind-First Styling
All components use Tailwind CSS v4 utility classes exclusively:
- No CSS modules or isolation
- No custom CSS files per component
- Design tokens via CSS variables
- Dark mode via class-based approach
- Consistent styling across all components

### 3. Accessible by Default
Every component is built with accessibility as a first-class concern:
- Semantic HTML elements
- Proper ARIA attributes
- Keyboard navigation support
- Screen reader compatibility
- Focus management
- High contrast support

### 4. Composability
Components are designed to be composed together:
- Small, focused components
- Composition over configuration
- RenderFragment slots for flexibility
- Consistent API patterns
- Predictable behavior

## System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                         ShellUI CLI                          │
│  (Global .NET Tool - Command Line Interface)                 │
│                                                              │
│  Commands: init, add, remove, list, update, diff            │
└─────────────────┬───────────────────────────────────────────┘
                  │
                  │ HTTP
                  ↓
┌─────────────────────────────────────────────────────────────┐
│                    Component Registry                        │
│  (GitHub Repository - Component Storage)                     │
│                                                              │
│  - Component metadata                                        │
│  - Component source files                                    │
│  - Dependency information                                    │
│  - Version history                                           │
└─────────────────┬───────────────────────────────────────────┘
                  │
                  │ Download & Copy
                  ↓
┌─────────────────────────────────────────────────────────────┐
│                      User's Project                          │
│  (Blazor Server/WASM/SSR Application)                        │
│                                                              │
│  Components/UI/      - Copied components                     │
│  wwwroot/styles/     - Compiled Tailwind CSS                 │
│  shellui.json        - Configuration                         │
│  tailwind.config.js  - Tailwind configuration                │
└─────────────────────────────────────────────────────────────┘
```

## Project Structure

### CLI Tool Structure
```
ShellUI.CLI/
├── Commands/
│   ├── InitCommand.cs        # Initialize project
│   ├── AddCommand.cs         # Add component(s)
│   ├── RemoveCommand.cs      # Remove component(s)
│   ├── ListCommand.cs        # List components
│   ├── UpdateCommand.cs      # Update component(s)
│   └── DiffCommand.cs        # Show differences
├── Services/
│   ├── ComponentRegistry.cs  # Component registry access
│   ├── ProjectDetector.cs    # Detect project type
│   ├── FileService.cs        # File operations
│   ├── ConfigService.cs      # Configuration management
│   └── TailwindService.cs    # Tailwind integration
├── Models/
│   ├── ComponentMetadata.cs  # Component info
│   ├── ShellUIConfig.cs      # Project config
│   └── ProjectType.cs        # Project type enum
└── Program.cs                # Entry point
```

### Component Registry Structure
```
registry/
├── components/
│   ├── button/
│   │   ├── button.razor              # Component code
│   │   ├── button.razor.cs           # Code-behind (optional)
│   │   ├── metadata.json             # Component metadata
│   │   ├── README.md                 # Documentation
│   │   └── examples/
│   │       ├── basic.razor           # Basic example
│   │       ├── variants.razor        # Variants example
│   │       └── with-icons.razor      # With icons example
│   ├── card/
│   │   └── ...
│   └── .../
├── utilities/
│   ├── cn.cs                         # Class name utility
│   ├── VariantBuilder.cs             # Variant composition
│   └── .../
├── hooks/
│   ├── UseMediaQuery.cs              # Media query hook
│   ├── UseLocalStorage.cs            # Local storage hook
│   └── .../
└── registry.json                     # Master registry
```

### User Project Structure (After Init)
```
UserBlazorApp/
├── Components/
│   └── UI/                           # ShellUI components
│       ├── Button.razor
│       ├── Card.razor
│       ├── Input.razor
│       └── ...
├── Lib/                              # Utilities & hooks
│   ├── cn.cs
│   └── ...
├── wwwroot/
│   └── styles/
│       ├── input.css                 # Tailwind input
│       └── output.css                # Compiled CSS
├── Pages/
├── Program.cs
├── _Imports.razor
├── shellui.json                      # ShellUI config
├── tailwind.config.js                # Tailwind config
├── postcss.config.js                 # PostCSS config
└── package.json                      # npm packages
```

## Component Architecture

### Component Template
```razor
@* Component: Button
   Description: Interactive button component with variants
   Accessibility: Full keyboard support, ARIA compliant *@

<button 
    type="@Type"
    disabled="@Disabled"
    class="@CssClass"
    @onclick="HandleClick"
    @attributes="AdditionalAttributes">
    @if (IsLoading)
    {
        <span class="loading-spinner" aria-hidden="true"></span>
    }
    @ChildContent
</button>

@code {
    [Parameter] public string Variant { get; set; } = "default";
    [Parameter] public string Size { get; set; } = "md";
    [Parameter] public string Type { get; set; } = "button";
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private string CssClass => BuildCssClass();
    
    private string BuildCssClass()
    {
        var classes = new List<string>
        {
            "inline-flex items-center justify-center rounded-md text-sm font-medium",
            "transition-colors focus-visible:outline-none focus-visible:ring-2",
            "focus-visible:ring-ring focus-visible:ring-offset-2",
            "disabled:pointer-events-none disabled:opacity-50"
        };
        
        // Variant classes
        classes.Add(Variant switch
        {
            "default" => "bg-primary text-primary-foreground hover:bg-primary/90",
            "destructive" => "bg-destructive text-destructive-foreground hover:bg-destructive/90",
            "outline" => "border border-input bg-background hover:bg-accent hover:text-accent-foreground",
            "ghost" => "hover:bg-accent hover:text-accent-foreground",
            "link" => "text-primary underline-offset-4 hover:underline",
            _ => "bg-primary text-primary-foreground hover:bg-primary/90"
        });
        
        // Size classes
        classes.Add(Size switch
        {
            "sm" => "h-9 px-3",
            "md" => "h-10 px-4 py-2",
            "lg" => "h-11 px-8",
            _ => "h-10 px-4 py-2"
        });
        
        return string.Join(" ", classes);
    }
    
    private async Task HandleClick(MouseEventArgs args)
    {
        if (!Disabled && !IsLoading)
        {
            await OnClick.InvokeAsync(args);
        }
    }
}
```

### Component Patterns

#### 1. Parameter Pattern
All components follow consistent parameter patterns:

```csharp
// Style parameters
[Parameter] public string Variant { get; set; } = "default";
[Parameter] public string Size { get; set; } = "md";

// State parameters
[Parameter] public bool Disabled { get; set; }
[Parameter] public bool IsLoading { get; set; }

// Content parameters
[Parameter] public RenderFragment? ChildContent { get; set; }
[Parameter] public string? Label { get; set; }

// Event parameters
[Parameter] public EventCallback<TArgs> OnEvent { get; set; }

// Extensibility
[Parameter(CaptureUnmatchedValues = true)]
public Dictionary<string, object>? AdditionalAttributes { get; set; }
```

#### 2. Composition Pattern
Components support composition through slots:

```razor
<Card>
    <CardHeader>
        <CardTitle>Title</CardTitle>
        <CardDescription>Description</CardDescription>
    </CardHeader>
    <CardContent>
        Content
    </CardContent>
    <CardFooter>
        Footer
    </CardFooter>
</Card>
```

#### 3. Variant Pattern
Variants control visual appearance:

```csharp
private string GetVariantClasses(string variant) => variant switch
{
    "default" => "default-classes",
    "outline" => "outline-classes",
    "ghost" => "ghost-classes",
    _ => "default-classes"
};
```

## Tailwind Integration

### Configuration
Tailwind v4.1.17 uses CSS-first configuration with design tokens:

```javascript
// tailwind.config.js
module.exports = {
  content: ['./**/*.{razor,html,cs}'],
  // Theme is defined via CSS variables in input.css
}
```

### CSS Variables
Design tokens are defined as CSS variables in `wwwroot/input.css`:

```css
/* input.css */
@import "tailwindcss";

@theme inline {
  /* Design tokens via CSS variables */
  --font-sans: 'Kode Mono', monospace; /* Custom fonts */
  --default-font-family: var(--font-sans);
}

:root {
  --background: 0 0% 100%;
  --foreground: 222.2 84% 4.9%;
  --primary: 222.2 47.4% 11.2%;
  --primary-foreground: 210 40% 98%;
  /* ... more tokens */
}

.dark {
  --background: 222.2 84% 4.9%;
  --foreground: 210 40% 98%;
  /* ... dark theme tokens */
}
```

### Build Process
Tailwind is built via MSBuild using standalone CLI (no Node.js!):

```xml
<Target Name="BuildTailwindCSS" BeforeTargets="BeforeBuild">
  <Exec Command="&quot;$(TailwindExecutable)&quot; -i &quot;$(TailwindInputCss)&quot; -o &quot;$(TailwindOutputCss)&quot; --config &quot;$(TailwindConfig)&quot; $(TailwindMinify)" />
</Target>
```

The standalone CLI is automatically downloaded during `dotnet shellui init`.

## CLI Implementation

### Command Structure
Uses `System.CommandLine` for parsing:

```csharp
var rootCommand = new RootCommand("ShellUI - CLI for Blazor components");

var initCommand = new Command("init", "Initialize ShellUI in your project");
initCommand.SetHandler(InitHandler);
rootCommand.AddCommand(initCommand);

var addCommand = new Command("add", "Add component(s)");
var componentArg = new Argument<string[]>("components", "Component name(s)");
addCommand.AddArgument(componentArg);
addCommand.SetHandler(AddHandler, componentArg);
rootCommand.AddCommand(addCommand);

await rootCommand.InvokeAsync(args);
```

### Component Resolution
Components are resolved from the embedded `ComponentRegistry`:

```csharp
// ComponentRegistry.cs - Embedded in ShellUI.Templates package
public static class ComponentRegistry
{
    public static readonly Dictionary<string, ComponentMetadata> Components = new()
    {
        { "button", ButtonTemplate.Metadata },
        { "input", InputTemplate.Metadata },
        { "card", CardTemplate.Metadata },
        // ... 53 components total
    };
    
    public static string? GetComponentContent(string componentName)
    {
        return componentName switch
        {
            "button" => ButtonTemplate.Content,
            "input" => InputTemplate.Content,
            // ... all components
            _ => null
        };
    }
}
```

The CLI tool uses this registry to copy component source code to user projects.

### Dependency Resolution
```csharp
public async Task<List<string>> ResolveDependenciesAsync(
    ComponentMetadata component,
    HashSet<string>? visited = null)
{
    visited ??= new HashSet<string>();
    var dependencies = new List<string>();
    
    if (visited.Contains(component.Name)) return dependencies;
    visited.Add(component.Name);
    
    foreach (var depName in component.Dependencies)
    {
        var dep = await GetComponentAsync(depName);
        if (dep == null) continue;
        
        // Recursive resolution
        var subDeps = await ResolveDependenciesAsync(dep, visited);
        dependencies.AddRange(subDeps);
        dependencies.Add(depName);
    }
    
    return dependencies.Distinct().ToList();
}
```

## Configuration System

### shellui.json Schema
```json
{
  "$schema": "https://shellui.dev/schema.json",
  "version": "1.0.0",
  "projectType": "blazor-server",
  "componentsPath": "Components/UI",
  "utilitiesPath": "Lib",
  "tailwind": {
    "enabled": true,
    "version": "4.x",
    "configPath": "tailwind.config.js",
    "inputCss": "wwwroot/styles/input.css",
    "outputCss": "wwwroot/styles/output.css"
  },
  "aliases": {
    "@ui": "./Components/UI",
    "@lib": "./Lib"
  },
  "components": [
    {
      "name": "button",
      "version": "1.0.0",
      "installedAt": "2025-01-15T10:30:00Z"
    }
  ]
}
```

### Project Type Detection
```csharp
public enum ProjectType
{
    Unknown,
    BlazorServer,
    BlazorWebAssembly,
    BlazorServerSideRendering,
    BlazorHybrid
}

public ProjectType DetectProjectType(string projectPath)
{
    var csprojContent = File.ReadAllText(projectPath);
    
    if (csprojContent.Contains("Microsoft.AspNetCore.Components.WebAssembly"))
        return ProjectType.BlazorWebAssembly;
    
    if (csprojContent.Contains("AddInteractiveServerComponents"))
        return ProjectType.BlazorServer;
    
    if (csprojContent.Contains("AddRazorComponents"))
        return ProjectType.BlazorServerSideRendering;
    
    return ProjectType.Unknown;
}
```

## Accessibility Implementation

### ARIA Attributes
Every component includes proper ARIA attributes:

```razor
<button
    role="button"
    aria-label="@AriaLabel"
    aria-pressed="@(IsPressed ? "true" : "false")"
    aria-disabled="@(Disabled ? "true" : "false")"
    tabindex="@(Disabled ? -1 : 0)">
    @ChildContent
</button>
```

### Keyboard Navigation
Components handle keyboard events:

```csharp
private async Task HandleKeyDown(KeyboardEventArgs e)
{
    switch (e.Key)
    {
        case "Enter":
        case " ": // Space
            await OnClick.InvokeAsync();
            break;
        case "Escape":
            await OnClose.InvokeAsync();
            break;
        case "ArrowDown":
            FocusNext();
            break;
        case "ArrowUp":
            FocusPrevious();
            break;
    }
}
```

### Focus Management
Focus trap for modals and dialogs:

```csharp
public class FocusTrap : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private IJSObjectReference? _module;
    
    public async Task TrapFocusAsync(ElementReference element)
    {
        _module = await _js.InvokeAsync<IJSObjectReference>(
            "import", "./_content/ShellUI/focusTrap.js");
        
        await _module.InvokeVoidAsync("trapFocus", element);
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_module != null)
        {
            await _module.InvokeVoidAsync("releaseFocus");
            await _module.DisposeAsync();
        }
    }
}
```

## Performance Considerations

### Bundle Size Optimization
- Copy only needed components (not entire library)
- Tailwind purges unused CSS in production
- Lazy load heavy components
- Use Blazor's native code splitting

### Server vs WASM
Components work in both environments:

```csharp
// Detect environment
@inject IJSRuntime JS

@code {
    private bool IsServer => 
        JS.GetType().Name == "UnsupportedJavaScriptRuntime";
    
    protected override void OnInitialized()
    {
        if (IsServer)
        {
            // Server-specific logic
        }
        else
        {
            // WASM-specific logic
        }
    }
}
```

### Rendering Optimization
- Use `@key` directives for lists
- Implement `ShouldRender()` when appropriate
- Avoid unnecessary re-renders
- Use `EventCallback` instead of `Action`

## Testing Strategy

### Unit Tests (bUnit)
```csharp
public class ButtonTests : TestContext
{
    [Fact]
    public void Button_RendersCorrectly()
    {
        var cut = RenderComponent<Button>(parameters => parameters
            .Add(p => p.ChildContent, "Click Me"));
        
        cut.Find("button").TextContent.Should().Be("Click Me");
    }
    
    [Fact]
    public void Button_TriggersOnClick()
    {
        var clicked = false;
        var cut = RenderComponent<Button>(parameters => parameters
            .Add(p => p.OnClick, () => clicked = true));
        
        cut.Find("button").Click();
        
        clicked.Should().BeTrue();
    }
}
```

### Accessibility Tests
```csharp
[Fact]
public void Button_HasProperAriaAttributes()
{
    var cut = RenderComponent<Button>(parameters => parameters
        .Add(p => p.Disabled, true)
        .Add(p => p.AriaLabel, "Submit"));
    
    var button = cut.Find("button");
    button.GetAttribute("aria-label").Should().Be("Submit");
    button.GetAttribute("aria-disabled").Should().Be("true");
}
```

### Integration Tests
Test component interactions and composition:

```csharp
[Fact]
public void Form_SubmitsWithValidData()
{
    var submitted = false;
    var cut = RenderComponent<TestForm>(parameters => parameters
        .Add(p => p.OnSubmit, () => submitted = true));
    
    cut.Find("input[name='email']").Input("test@example.com");
    cut.Find("button[type='submit']").Click();
    
    submitted.Should().BeTrue();
}
```

## Security Considerations

### XSS Prevention
- All user input is automatically encoded by Razor
- Use `@bind` instead of manual value binding
- Sanitize HTML when using `MarkupString`
- Validate all input on server-side

### CSRF Protection
- Use Blazor's built-in anti-forgery
- Validate form tokens
- Use `[ValidateAntiForgeryToken]` on APIs

### Dependency Security
- Regular dependency updates
- Security scanning in CI/CD
- Minimal dependencies
- Audit all third-party code

## Deployment

### NuGet Package (CLI)
```xml
<PropertyGroup>
  <PackAsTool>true</PackAsTool>
  <ToolCommandName>shellui</ToolCommandName>
  <PackageId>ShellUI.CLI</PackageId>
  <Version>1.0.0</Version>
  <Authors>ShellUI Team</Authors>
  <Description>CLI tool for ShellUI components</Description>
</PropertyGroup>
```

### GitHub Repository (Registry)
- Components stored in `/registry` folder
- Versioned via git tags
- Automated releases via GitHub Actions
- CDN delivery via GitHub Pages

## Future Considerations

### v1.1+
- Component variants (multiple styles)
- Component themes (complete style sets)
- Visual component editor
- Storybook-like playground
- Advanced composition patterns
- Performance monitoring
- Usage analytics (opt-in)

### v2.0+
- AI-powered component generation
- Visual builder integration
- Real-time collaboration features
- Advanced animation system
- 3D components
- WebXR support

---

This architecture is designed to be:
- **Flexible** - Easy to extend and customize
- **Performant** - Optimized for both Server and WASM
- **Maintainable** - Clear patterns and conventions
- **Accessible** - WCAG 2.1 AA compliant
- **Developer-Friendly** - Great DX with minimal setup

For questions or suggestions about the architecture, open an issue on GitHub.

