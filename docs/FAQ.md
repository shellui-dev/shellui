# ShellUI Frequently Asked Questions

Quick answers to common questions about ShellUI.

## NuGet vs CLI

### Q: What's the difference between NuGet package and CLI?

**NuGet Package:**
- **Contains:** ALL 40+ components in a compiled DLL
- **Installation:** `dotnet add package ShellUI.Components`
- **Format:** Compiled Razor Class Library (traditional)
- **Usage:** Import and use immediately
- **Customization:** Limited (CSS classes and parameters only)
- **Bundle:** Includes all components (tree-shaking reduces final size)
- **Updates:** `dotnet restore` updates entire package
- **Best for:** Quick starts, prototyping, standard usage

**CLI:**
- **Contains:** Only components you explicitly add
- **Installation:** `dotnet shellui add button card alert`
- **Format:** Source `.razor` files copied to YOUR project
- **Usage:** Edit the source code directly
- **Customization:** Full control (edit component logic and markup)
- **Bundle:** Only components you add
- **Updates:** `dotnet shellui update button` per component
- **Best for:** Custom design systems, heavy customization

### Q: Can I use both?

**YES! This is actually recommended!**

```bash
# 1. Install NuGet package (all 40+ components)
dotnet add package ShellUI.Components

# 2. Use NuGet components everywhere
<Alert>From NuGet</Alert>
<Badge>From NuGet</Badge>
<Card>From NuGet</Card>

# 3. Customize only what you need
dotnet shellui add button  # Copies source to Components/UI/

# 4. Edit Components/UI/Button.razor
# Add your custom logic, styles, variants

# 5. Your app now uses:
# - Custom Button from Components/UI/Button.razor
# - All other components from NuGet DLL
```

**Component resolution order:**
1. Check `Components/UI/` first (CLI-added components)
2. Fall back to NuGet package

This means CLI components override NuGet ones!

## Installing Multiple Components

### Q: Can I install multiple components at once?

**YES! Multiple syntaxes supported:**

```bash
# Space-separated
dotnet shellui add button card alert

# Comma-separated
dotnet shellui add button,card,alert

# Mix both (why not!)
dotnet shellui add button,card alert dialog

# Install many at once
dotnet shellui add button,input,label,card,alert,badge,skeleton,separator,avatar,checkbox,radio,select,textarea

# No limit!
dotnet shellui add button,card,alert,badge,input,label,checkbox,radio,select,textarea,switch,slider,dialog,sheet,popover,tooltip,dropdown-menu,toast,table,tabs,accordion,separator,skeleton,avatar,calendar,date-picker
```

**Common patterns:**

```bash
# Essential starter pack
dotnet shellui add button,input,label,card,alert,badge,skeleton,separator

# Form components
dotnet shellui add button,input,label,textarea,checkbox,radio,select,switch,slider,form

# Overlay components
dotnet shellui add dialog,sheet,popover,tooltip,dropdown-menu,toast

# Data components
dotnet shellui add table,tabs,accordion,calendar,date-picker

# Layout components
dotnet shellui add card,separator,skeleton,avatar,container,aspect-ratio
```

## Node.js and Tailwind

### Q: Do I need Node.js?

**NO! Zero Node.js required!**

ShellUI uses **Tailwind standalone CLI** - a single binary with no dependencies.

**How it works:**
```bash
dotnet shellui init
# ↓
# Detects your OS (Windows/Mac/Linux)
# ↓
# Downloads appropriate Tailwind binary
# ↓
# Caches in .shellui/tailwindcss.exe
# ↓
# Uses it to compile CSS
# ↓
# NO npm, NO node_modules, NO package.json!
```

### Q: What if I already have Node.js?

**That's fine!** You can keep using it for other projects. ShellUI won't touch it or require it.

### Q: How does CSS compilation work without Node.js?

**Tailwind standalone CLI:**
- Single executable binary (~15-20 MB)
- Supports all Tailwind v4 features
- Fast compilation
- Cross-platform (Windows, Mac, Linux)
- Zero dependencies

**MSBuild integration:**
```xml
<Target Name="BuildTailwind" BeforeTargets="Build">
  <Exec Command=".shellui/tailwindcss -i input.css -o output.css" />
</Target>
```

Just works!

### Q: Can I still use npm if I want?

**Yes**, but it's optional. If you prefer npm workflows, you can:
1. Skip the standalone CLI
2. Use npm to install Tailwind
3. ShellUI will detect and use your npm setup

But most users won't need this complexity!

## Workflow Questions

### Q: I'm new to Blazor. Which should I use?

**Start with NuGet:**
```bash
dotnet add package ShellUI.Components
```

- All components available immediately
- Familiar workflow
- Just import and use
- Learn the components first

**Later, try CLI:**
```bash
dotnet shellui add button
```

- Copy one component
- Look at the source code
- Learn how it works
- Customize it

### Q: I'm building a custom design system. Which should I use?

**Use CLI from the start:**
```bash
dotnet shellui init
dotnet shellui add button,card,input,label,...
```

- Full control from day one
- Edit all component source
- Create custom variants
- Build your own design language

### Q: I'm prototyping quickly. Which should I use?

**Use NuGet:**
```bash
dotnet add package ShellUI.Components
```

- Fastest setup
- All components ready
- No customization needed
- Perfect for MVPs

### Q: My team has mixed skill levels. What do we use?

**Use both (hybrid):**

**For junior devs:**
- Install NuGet package
- Use components as-is
- Don't worry about internals

**For senior devs:**
- Use CLI to customize
- Edit component source
- Create team-specific variants

**Result:**
- Everyone productive
- Gradual skill progression
- Customization when needed

## Technical Questions

### Q: What's the bundle size difference?

**NuGet Package:**
- Initial: ~200-300 KB (all 40+ components)
- After tree-shaking: Only used components included
- Reasonable for most apps

**CLI:**
- Only components you add
- ~5-10 KB per component
- Minimal overhead

**Example:**
- App using 10 components:
  - NuGet: ~150 KB (after tree-shaking)
  - CLI: ~80 KB (only 10 components)

**Hybrid:**
- Best of both worlds
- Use NuGet for 30 components: ~150 KB
- Customize 3 with CLI: +30 KB
- Total: ~180 KB

### Q: Does NuGet package work with Blazor Server, WASM, and SSR?

**YES!** All three:
- Blazor Server ✓
- Blazor WebAssembly ✓
- Blazor SSR ✓
- Blazor Hybrid (MAUI) ✓

Same package works everywhere.

### Q: Does CLI work with all Blazor types?

**YES!** CLI detects your project type and configures accordingly:
- Blazor Server ✓
- Blazor WebAssembly ✓
- Blazor SSR ✓
- Blazor Hybrid (MAUI) ✓

### Q: Can I use ShellUI with my existing Blazor app?

**YES!**

**Option 1: NuGet (easiest)**
```bash
cd YourExistingApp
dotnet add package ShellUI.Components
```

**Option 2: CLI**
```bash
cd YourExistingApp
dotnet shellui init
dotnet shellui add button,card,alert
```

**Option 3: Both**
```bash
dotnet add package ShellUI.Components
dotnet shellui add button  # Customize just this one
```

### Q: What if I'm already using another UI library?

**ShellUI can coexist!**

```razor
@* Mix with MudBlazor *@
<MudButton>MudBlazor Button</MudButton>
<ShellUI.Button>ShellUI Button</ShellUI.Button>

@* Mix with Radzen *@
<RadzenButton>Radzen Button</RadzenButton>
<ShellUI.Button>ShellUI Button</ShellUI.Button>
```

Gradually migrate component by component.

## Customization Questions

### Q: How do I customize a NuGet component?

**You can't directly edit NuGet components (they're compiled).**

**Options:**
1. **CSS customization:**
   ```razor
   <Button class="my-custom-styles">Button</Button>
   ```

2. **Parameter customization:**
   ```razor
   <Button Variant="custom-variant">Button</Button>
   ```

3. **Copy with CLI:**
   ```bash
   dotnet shellui add button --force
   # Now edit Components/UI/Button.razor
   ```

### Q: How do I customize a CLI component?

**Just edit the file!**

```bash
# Add component
dotnet shellui add button

# Edit the source
code Components/UI/Button.razor

# Add your custom logic
@code {
    [Parameter] public string MyCustomProp { get; set; }
    
    // Add new variants
    private string GetCustomVariant() => Variant switch
    {
        "neon" => "bg-gradient-to-r from-pink-500 to-purple-500",
        "glass" => "backdrop-blur-md bg-white/10",
        _ => "bg-primary"
    };
}
```

It's YOUR code!

### Q: If I customize a CLI component, can I still update it?

**YES, but with caution:**

```bash
dotnet shellui update button
# ↓
# Shows diff between your version and latest
# ↓
# You decide:
# - Accept update (lose customizations)
# - Reject update (keep customizations)
# - Merge manually
```

**shellui.json tracks if you've customized:**
```json
{
  "components": [
    {
      "name": "button",
      "customized": true,  // ← CLI knows you edited it
      "version": "1.0.0"
    }
  ]
}
```

## Update and Maintenance

### Q: How do I update NuGet components?

**Standard NuGet workflow:**
```bash
dotnet add package ShellUI.Components
# Or
dotnet restore
```

All 40+ components update together.

### Q: How do I update CLI components?

**Per-component or all:**
```bash
# Update one
dotnet shellui update button

# Update multiple
dotnet shellui update button,card,alert

# Update all
dotnet shellui update --all
```

Each component updates independently.

### Q: What about dependencies?

**Automatically handled!**

```bash
dotnet shellui add dialog
# ↓
# Dialog depends on button
# ↓
# Installs both:
# - button
# - dialog
```

**Removing:**
```bash
dotnet shellui remove button
# ↓
# ⚠️ Warning: dialog depends on button
# ↓
# Remove anyway? (y/N)
```

## Migration Questions

### Q: Can I migrate from NuGet to CLI?

**YES!**

```bash
# 1. You're using NuGet
<Button>From NuGet</Button>

# 2. Add via CLI
dotnet shellui add button

# 3. Edit Components/UI/Button.razor
# Customize as needed

# 4. Your app now uses CLI version
<Button>From CLI (customized)</Button>

# 5. Keep NuGet for other components
<Alert>Still from NuGet</Alert>
```

No breaking changes!

### Q: Can I migrate from CLI to NuGet?

**YES!**

```bash
# 1. Delete CLI components
rm -rf Components/UI/

# 2. Install NuGet
dotnet add package ShellUI.Components

# 3. Everything works
<Button>Now from NuGet</Button>
```

### Q: I forked components from the original Sysinfocus library. Can I migrate?

**Migration guide coming at v1.0!**

Main changes:
- Styling: Custom CSS → Tailwind classes
- API: Some parameter names changed
- Distribution: NuGet only → NuGet + CLI

Detailed migration guide will be available.

## Troubleshooting

### Q: Component not found after CLI add

**Check namespace:**
```razor
@using YourProject.Components.UI

<Button>Should work now</Button>
```

**Or check _Imports.razor:**
```razor
@using YourProject.Components.UI
```

### Q: NuGet and CLI components conflicting

**Resolution order:**
1. CLI components (Components/UI/) take precedence
2. NuGet components (DLL) are fallback

**To force NuGet version:**
```razor
@using ShellUI.Components

<ShellUI.Components.Button>NuGet version</ShellUI.Components.Button>
```

**To force CLI version:**
```razor
<YourProject.Components.UI.Button>CLI version</YourProject.Components.UI.Button>
```

### Q: Tailwind not compiling

**Check binary:**
```bash
ls .shellui/
# Should see: tailwindcss.exe (Windows) or tailwindcss (Mac/Linux)
```

**Re-download:**
```bash
dotnet shellui init --force
```

**Check MSBuild:**
```bash
dotnet build -v detailed
# Look for Tailwind compilation
```

## Pricing and Licensing

### Q: How much does ShellUI cost?

**FREE!** Forever.
- CLI tool: Free
- NuGet package: Free
- All components: Free
- MIT License

### Q: Can I use it commercially?

**YES!** MIT License allows:
- Commercial use
- Modification
- Distribution
- Private use

No attribution required (but appreciated!).

### Q: What about enterprise support?

**v1.0 roadmap includes:**
- Community support (free)
- Documentation (free)
- GitHub Issues (free)
- Enterprise support (planned, paid)

## Getting Help

### Q: Where can I get help?

**Resources:**
- [Documentation](https://shellui.dev/docs) (coming at v1.0)
- [GitHub Issues](https://github.com/yourorg/shellui/issues)
- [GitHub Discussions](https://github.com/yourorg/shellui/discussions)
- [CLI_SYNTAX.md](CLI_SYNTAX.md) - Complete CLI reference

**Commands:**
```bash
dotnet shellui --help
dotnet shellui add --help
dotnet shellui init --help
```

### Q: How do I report bugs?

**GitHub Issues:**
1. Check existing issues
2. Create new issue
3. Include:
   - ShellUI version
   - .NET version
   - Blazor type (Server/WASM/SSR)
   - Steps to reproduce
   - Expected vs actual behavior

### Q: Can I contribute?

**After v1.0 alpha!**

Currently in development. Contributions will open after alpha release (Q1 2026).

See [CONTRIBUTING.md](CONTRIBUTING.md) for future guidelines.

---

## Quick Decision Guide

**Choose NuGet if you want:**
- Quick setup
- All components immediately
- Familiar workflow
- Automatic updates
- Standard usage

**Choose CLI if you want:**
- Full customization
- Minimal bundle
- Component source code
- Learn internals
- Custom design system

**Choose Both if you want:**
- Quick start (NuGet)
- Selective customization (CLI)
- Best of both worlds
- Team flexibility

---

**Still have questions?**

Open a [GitHub Discussion](https://github.com/yourorg/shellui/discussions) or check the [documentation](https://shellui.dev/docs).

**Version:** 1.0.0  
**Last Updated:** October 2025

