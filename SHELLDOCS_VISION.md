# ShellDocs - Vision & Architecture

**"Fumadocs for .NET" - The documentation framework Blazor deserves**

## Overview

ShellDocs is a documentation framework for .NET projects that brings fumadocs-level polish and developer experience to the .NET ecosystem. Built with Blazor, styled with Tailwind CSS, and designed for component libraries, APIs, and any .NET project that needs beautiful documentation.

## The Problem

**.NET has NO modern documentation framework:**
- Docfx: Dated UI, feels like 2015
- Statiq: Static only, no live demos
- MkDocs/.NET: Requires Python
- Custom sites: Everyone rebuilds the same thing

**Meanwhile, JavaScript has:**
- fumadocs (Next.js) - Beautiful, modern, polished
- Nextra (Next.js) - Clean, fast
- Docusaurus (React) - Feature-rich
- VitePress (Vue) - Lightning fast

**The gap is massive.**

## The Solution

ShellDocs provides:
- ✅ Fumadocs-inspired UI (beautiful, modern, clean)
- ✅ Live component demos (Blazor components work live!)
- ✅ Interactive playground (edit and preview in real-time)
- ✅ Auto-generated navigation (from markdown files)
- ✅ Built-in search (Algolia or local)
- ✅ Dark mode (seamless)
- ✅ SEO optimized (Blazor SSR)
- ✅ Zero config (works out of the box)
- ✅ Fully customizable (themes, layouts, components)

## Fumadocs Structure Analysis

### What Makes Fumadocs Great

**1. Layout Structure**
```
┌─────────────────────────────────────────────────────┐
│  Header (Logo, Nav, Search, Theme Toggle)          │
├──────────┬──────────────────────────┬───────────────┤
│          │                          │               │
│ Sidebar  │   Main Content          │  TOC (right)  │
│ (left)   │                          │               │
│          │   • Breadcrumbs         │               │
│ • Docs   │   • Title               │  • Headings   │
│ • API    │   • Content             │  • Scroll spy │
│ • Guide  │   • Code blocks         │               │
│          │   • Component demos     │               │
│          │   • Prev/Next           │               │
│          │                          │               │
└──────────┴──────────────────────────┴───────────────┘
```

**2. Component Hierarchy**
```
App
├── DocsLayout
│   ├── Header
│   │   ├── Logo
│   │   ├── MainNav
│   │   ├── Search
│   │   └── ThemeToggle
│   ├── Sidebar
│   │   ├── SidebarNav
│   │   │   ├── NavSection
│   │   │   │   └── NavItem[]
│   │   │   └── NavSection[]
│   │   └── SidebarFooter
│   ├── MainContent
│   │   ├── Breadcrumb
│   │   ├── PageHeader
│   │   ├── MDXContent (or Razor content)
│   │   │   ├── Heading
│   │   │   ├── Paragraph
│   │   │   ├── CodeBlock
│   │   │   ├── ComponentDemo
│   │   │   └── ...
│   │   └── Pagination (Prev/Next)
│   └── TableOfContents
│       ├── TOCHeading[]
│       └── ScrollSpy
└── Footer
```

**3. Key Features**

**Search:**
- Algolia DocSearch integration
- Or local search with Flexsearch
- Keyboard shortcuts (Cmd+K)
- Instant results

**Navigation:**
- Auto-generated from file structure
- Collapsible sections
- Active state indication
- Breadcrumbs
- Prev/Next pagination

**Content:**
- Markdown/MDX support
- Syntax highlighting
- Copy buttons on code blocks
- Tabs for multiple examples
- Callouts (Note, Warning, Tip)
- Live component previews

**Theme:**
- Light/Dark mode
- CSS variables for customization
- Multiple color schemes
- Smooth transitions

## ShellDocs Architecture

### Technology Stack

```
ShellDocs
├── Backend: Blazor SSR (.NET 8+)
├── Styling: Tailwind CSS v4 (standalone CLI)
├── Content: Markdown → Razor components
├── Search: Algolia or local Flexsearch
├── Syntax: ColorCode.Blazor or Prism
└── Deployment: Docker or Static (Blazor WASM)
```

### Project Structure

```
ShellDocs/
├── src/
│   ├── ShellDocs.CLI/                    # CLI tool
│   │   ├── Commands/
│   │   │   ├── InitCommand.cs           # shelldocs init
│   │   │   ├── DevCommand.cs            # shelldocs dev
│   │   │   ├── BuildCommand.cs          # shelldocs build
│   │   │   └── ScaffoldCommand.cs       # shelldocs scaffold
│   │   ├── Templates/                    # Project templates
│   │   └── Program.cs
│   │
│   ├── ShellDocs.Core/                   # Core library
│   │   ├── Models/
│   │   │   ├── DocsConfig.cs            # Configuration model
│   │   │   ├── NavItem.cs               # Navigation structure
│   │   │   └── PageMetadata.cs          # Page frontmatter
│   │   ├── Services/
│   │   │   ├── MarkdownService.cs       # MD → HTML
│   │   │   ├── NavigationService.cs     # Auto-gen nav
│   │   │   ├── SearchService.cs         # Search indexing
│   │   │   └── ThemeService.cs          # Theme management
│   │   └── Parsers/
│   │       ├── FrontmatterParser.cs     # YAML frontmatter
│   │       └── MarkdownParser.cs        # Markdown parsing
│   │
│   └── ShellDocs.Blazor/                 # Blazor components
│       ├── Components/
│       │   ├── Layout/
│       │   │   ├── DocsLayout.razor     # Main layout
│       │   │   ├── Header.razor         # Top header
│       │   │   ├── Sidebar.razor        # Left sidebar
│       │   │   ├── MainContent.razor    # Content area
│       │   │   └── TableOfContents.razor # Right TOC
│       │   ├── Navigation/
│       │   │   ├── NavSection.razor     # Nav section
│       │   │   ├── NavItem.razor        # Nav item
│       │   │   ├── Breadcrumb.razor     # Breadcrumb
│       │   │   └── Pagination.razor     # Prev/Next
│       │   ├── Content/
│       │   │   ├── CodeBlock.razor      # Code with copy
│       │   │   ├── Callout.razor        # Note/Warning
│       │   │   ├── Tabs.razor           # Tab container
│       │   │   └── ComponentDemo.razor  # Live preview
│       │   ├── Search/
│       │   │   ├── SearchDialog.razor   # Search modal
│       │   │   └── SearchResults.razor  # Results list
│       │   └── Theme/
│       │       ├── ThemeToggle.razor    # Light/Dark toggle
│       │       └── ThemeProvider.razor  # Theme context
│       ├── wwwroot/
│       │   ├── shelldocs.css            # Fumadocs-inspired styles
│       │   └── shelldocs.js             # Minimal JS
│       └── ShellDocs.Blazor.csproj
│
├── templates/                            # dotnet new templates
│   ├── docs-site/                       # Full docs site
│   └── component-lib-docs/              # Component library docs
│
├── examples/                             # Example projects
│   ├── api-docs/                        # API documentation
│   ├── component-docs/                  # Component library docs
│   └── guide-docs/                      # Tutorial/guide docs
│
└── docs/                                 # ShellDocs own docs (dogfooding!)
    ├── getting-started.md
    ├── configuration.md
    └── components/
```

### Configuration File

```json
// shelldocs.json
{
  "$schema": "https://shelldocs.dev/schema.json",
  "name": "ShellUI",
  "description": "CLI-first Blazor component library",
  "logo": {
    "light": "/logo-light.svg",
    "dark": "/logo-dark.svg"
  },
  "links": {
    "github": "https://github.com/yourorg/shellui",
    "discord": "https://discord.gg/shellui"
  },
  "navigation": [
    {
      "title": "Documentation",
      "items": [
        { "title": "Getting Started", "href": "/docs/getting-started" },
        { "title": "Installation", "href": "/docs/installation" }
      ]
    },
    {
      "title": "Components",
      "items": "auto", // Auto-generates from /docs/components/
      "directory": "/docs/components"
    },
    {
      "title": "API Reference",
      "items": "auto-generated", // From XML docs
      "source": "api"
    }
  ],
  "theme": {
    "primaryColor": "blue",
    "darkMode": {
      "enabled": true,
      "defaultTheme": "system"
    },
    "font": {
      "sans": "Inter, system-ui",
      "mono": "JetBrains Mono, monospace"
    }
  },
  "features": {
    "search": {
      "provider": "algolia", // or "local"
      "algolia": {
        "appId": "YOUR_APP_ID",
        "apiKey": "YOUR_API_KEY",
        "indexName": "shellui"
      }
    },
    "analytics": {
      "provider": "none" // or "google", "plausible"
    },
    "editLinks": {
      "enabled": true,
      "pattern": "https://github.com/yourorg/shellui/edit/main/docs/{path}"
    },
    "feedback": {
      "enabled": true
    }
  },
  "componentDemo": {
    "enabled": true,
    "defaultTab": "preview", // or "code"
    "showCopyButton": true,
    "playground": true // Interactive code editor
  },
  "build": {
    "output": "wwwroot",
    "baseUrl": "https://docs.shellui.dev"
  }
}
```

### Markdown File Structure

```markdown
---
title: Button Component
description: Interactive button with multiple variants
category: Components
order: 1
---

# Button

Interactive button component with variants, sizes, and states.

<Callout type="tip">
  Button works in all Blazor render modes: Server, WASM, and SSR.
</Callout>

## Installation

<Tabs items={["CLI", "NuGet"]}>
  <Tab value="CLI">
    ```bash
    dotnet shellui add button
    ```
  </Tab>
  <Tab value="NuGet">
    ```bash
    dotnet add package ShellUI.Components
    ```
  </Tab>
</Tabs>

## Preview

<ComponentDemo>
  <Preview>
    <Button Variant="primary">Click Me</Button>
    <Button Variant="secondary">Secondary</Button>
    <Button Variant="outline">Outline</Button>
  </Preview>
  <Code language="razor">
    ```razor
    <Button Variant="primary">Click Me</Button>
    <Button Variant="secondary">Secondary</Button>
    <Button Variant="outline">Outline</Button>
    ```
  </Code>
</ComponentDemo>

## Variants

<ComponentDemo>
  <Preview>
    <Button Variant="default">Default</Button>
    <Button Variant="destructive">Destructive</Button>
    <Button Variant="ghost">Ghost</Button>
    <Button Variant="link">Link</Button>
  </Preview>
</ComponentDemo>

## API Reference

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| Variant | string | "default" | Button style variant |
| Size | string | "md" | Button size (sm, md, lg) |
| Disabled | bool | false | Disable button |
| OnClick | EventCallback | - | Click event handler |

### Events

| Event | Type | Description |
|-------|------|-------------|
| OnClick | EventCallback\<MouseEventArgs\> | Fires when button is clicked |

## Examples

### With Loading State

<ComponentDemo>
  <Preview>
    <Button IsLoading="true">Loading...</Button>
  </Preview>
  <Code>
    ```razor
    <Button IsLoading="@isLoading" OnClick="@HandleClick">
      Submit
    </Button>
    ```
  </Code>
</ComponentDemo>

## Accessibility

- ✅ Keyboard accessible (Enter/Space)
- ✅ ARIA attributes included
- ✅ Focus visible indicator
- ✅ Screen reader friendly
```

## CLI Commands

### init - Initialize ShellDocs

```bash
dotnet shelldocs init [options]

Options:
  --name <name>           Project name
  --type <type>           Project type (components, api, guide)
  --template <template>   Template to use (default, minimal, full)
  --no-examples          Skip example content
  --blazor-mode <mode>   Blazor render mode (ssr, server, wasm)

Examples:
  # Basic initialization
  dotnet shelldocs init
  
  # Component library docs
  dotnet shelldocs init --type components --name "MyUI"
  
  # API documentation
  dotnet shelldocs init --type api --name "MyAPI"
  
  # Minimal template
  dotnet shelldocs init --template minimal
```

**What it creates:**
```
YourProject.Docs/
├── docs/
│   ├── index.md
│   ├── getting-started.md
│   └── components/ (if --type components)
├── shelldocs.json
├── Program.cs
├── Components/
├── Pages/
└── wwwroot/
```

### dev - Development Server

```bash
dotnet shelldocs dev [options]

Options:
  --port <port>           Port number (default: 5000)
  --open                  Open browser automatically
  --https                 Use HTTPS

Examples:
  dotnet shelldocs dev
  dotnet shelldocs dev --port 3000 --open
```

**Features:**
- Hot reload for markdown changes
- Live component preview
- Fast refresh

### build - Production Build

```bash
dotnet shelldocs build [options]

Options:
  --output <path>         Output directory
  --mode <mode>           Build mode (static, server)
  --no-minify            Skip minification

Examples:
  # Build as Blazor SSR app
  dotnet shelldocs build
  
  # Build as static site (Blazor WASM)
  dotnet shelldocs build --mode static
  
  # Custom output
  dotnet shelldocs build --output ./dist
```

### scaffold - Generate Pages

```bash
dotnet shelldocs scaffold [options]

Options:
  --component <name>      Component name
  --api <dll>            Generate from XML docs
  --example              Include example usage

Examples:
  # Scaffold component documentation
  dotnet shelldocs scaffold --component Button
  
  # Generate API docs from XML
  dotnet shelldocs scaffold --api MyLibrary.dll
```

## Design Tokens (Fumadocs-inspired)

```css
/* shelldocs.css */
@tailwind base;
@tailwind components;
@tailwind utilities;

@layer base {
  :root {
    /* fumadocs colors */
    --fd-background: 0 0% 100%;
    --fd-foreground: 0 0% 9%;
    --fd-muted: 0 0% 96%;
    --fd-muted-foreground: 0 0% 45%;
    --fd-border: 0 0% 91%;
    --fd-accent: 0 0% 96%;
    --fd-accent-foreground: 0 0% 9%;
    --fd-primary: 220 100% 50%;
    --fd-primary-foreground: 0 0% 100%;
    --fd-card: 0 0% 100%;
    --fd-card-foreground: 0 0% 9%;
    --fd-popover: 0 0% 100%;
    --fd-popover-foreground: 0 0% 9%;
    
    /* Radius */
    --fd-radius: 0.5rem;
    
    /* Sidebar */
    --fd-sidebar-width: 256px;
    
    /* TOC */
    --fd-toc-width: 256px;
  }

  .dark {
    --fd-background: 0 0% 7%;
    --fd-foreground: 0 0% 98%;
    --fd-muted: 0 0% 15%;
    --fd-muted-foreground: 0 0% 65%;
    --fd-border: 0 0% 15%;
    --fd-accent: 0 0% 15%;
    --fd-accent-foreground: 0 0% 98%;
    --fd-primary: 220 100% 60%;
    --fd-primary-foreground: 0 0% 100%;
    /* ... dark mode colors ... */
  }
}
```

## Component Examples

### DocsLayout.razor

```razor
@inherits LayoutComponentBase
@inject NavigationService NavService
@inject ThemeService ThemeService

<div class="docs-layout">
    <Header />
    
    <div class="docs-container">
        <Sidebar Navigation="@NavService.GetNavigation()" />
        
        <main class="docs-main">
            <Breadcrumb />
            
            <article class="docs-content prose dark:prose-invert">
                @Body
            </article>
            
            <Pagination />
        </main>
        
        <TableOfContents Headings="@CurrentPageHeadings" />
    </div>
</div>
```

### ComponentDemo.razor

```razor
<div class="component-demo">
    <div class="demo-tabs">
        <button class="@(activeTab == "preview" ? "active" : "")" 
                @onclick="() => activeTab = "preview"">
            Preview
        </button>
        <button class="@(activeTab == "code" ? "active" : "")" 
                @onclick="() => activeTab = "code"">
            Code
        </button>
        @if (Playground)
        {
            <button class="@(activeTab == "playground" ? "active" : "")" 
                    @onclick="() => activeTab = "playground"">
                Playground
            </button>
        }
    </div>
    
    <div class="demo-content">
        @if (activeTab == "preview")
        {
            <div class="demo-preview">
                @Preview
            </div>
        }
        else if (activeTab == "code")
        {
            <div class="demo-code">
                <CopyButton Code="@Code" />
                <CodeBlock Language="@Language" Code="@Code" />
            </div>
        }
        else if (activeTab == "playground")
        {
            <InteractiveEditor Code="@Code" />
        }
    </div>
</div>

@code {
    [Parameter] public RenderFragment? Preview { get; set; }
    [Parameter] public string Code { get; set; } = "";
    [Parameter] public string Language { get; set; } = "razor";
    [Parameter] public bool Playground { get; set; }
    
    private string activeTab = "preview";
}
```

## Build Order & Strategy

### Phase 1: ShellDocs MVP (Q4 2026)
**Goal:** Working docs framework with basic features

**Build with:**
- Plain HTML/CSS (fumadocs-inspired)
- Basic Blazor components
- NO ShellUI dependency yet

**Features:**
1. ✅ Markdown parsing
2. ✅ Auto navigation
3. ✅ Syntax highlighting
4. ✅ Dark mode
5. ✅ Responsive layout
6. ✅ Search (basic)

**Duration:** 2 months

### Phase 2: ShellUI Integration (Q1 2027)
**Goal:** Replace HTML components with ShellUI components

**Replace:**
- Button → ShellUI.Button
- Card → ShellUI.Card
- Tabs → ShellUI.Tabs
- Input → ShellUI.Input (for search)
- Badge → ShellUI.Badge
- Separator → ShellUI.Separator

**Benefits:**
- Dogfooding ShellUI
- Better UX
- Consistent design
- Showcases ShellUI

**Duration:** 1 month

### Phase 3: ShellUI Docs Migration (Q1 2027)
**Goal:** Migrate ShellUI docs to ShellDocs

**Result:**
- ShellUI docs built with ShellDocs
- ShellDocs uses ShellUI components
- Perfect synergy!
- Both products showcase each other

**Duration:** 2 weeks

### Phase 4: Polish & Launch (Q2 2027)
**Goal:** Release both products together

**Marketing:**
```
"Introducing the ShellTech Ecosystem:
├── ShellUI: Blazor components (like shadcn/ui)
└── ShellDocs: Documentation framework (like fumadocs)

Built for each other. Built with each other."
```

**Duration:** 1 month

## Success Metrics

### For ShellDocs

**Adoption:**
- 1,000+ GitHub stars in 6 months
- 100+ projects using it
- 10+ testimonials

**Features:**
- All fumadocs features parity
- Better Blazor-specific features
- Faster than fumadocs
- Better DX than existing .NET solutions

**Community:**
- Active Discord
- Weekly updates
- Good documentation (dogfooding!)
- Video tutorials

## Competitive Advantage

**ShellDocs vs Fumadocs:**

| Feature | ShellDocs | fumadocs |
|---------|-----------|----------|
| **Live Component Demos** | ✅ Blazor runs live! | ❌ React only |
| **API Documentation** | ✅ From XML docs | ❌ Manual |
| **.NET Integration** | ✅ Native | ❌ N/A |
| **Type Safety** | ✅ C# types | ✅ TypeScript |
| **Component Playground** | ✅ Edit Blazor live | ✅ Edit React live |
| **Modern UI** | ✅ Fumadocs-inspired | ✅ Original |
| **Dark Mode** | ✅ | ✅ |
| **Search** | ✅ | ✅ |
| **SEO** | ✅ SSR | ✅ SSR |

**ShellDocs vs Docfx:**

| Feature | ShellDocs | Docfx |
|---------|-----------|-------|
| **Modern UI** | ✅ 2026 design | ❌ 2015 design |
| **Live Components** | ✅ | ❌ |
| **Easy Setup** | ✅ One command | ❌ Complex |
| **Dark Mode** | ✅ | ❌ |
| **Component Playground** | ✅ | ❌ |
| **Beautiful by Default** | ✅ | ❌ |

## Potential Users

**Target Audience:**
1. Component library authors (like ShellUI, MudBlazor, Radzen)
2. .NET library authors (FluentValidation, AutoMapper, etc.)
3. Enterprise teams with internal libraries
4. Open source .NET projects
5. API documentation needs
6. Tutorial/guide websites

**Estimated Market:**
- 1,000+ .NET component libraries
- 10,000+ .NET libraries on NuGet
- Unlimited enterprise internal libraries

## Revenue Model (Optional)

**Option 1: Fully Free**
- MIT License
- Free for everyone
- GitHub Sponsors for support

**Option 2: Freemium**
- Free: Open source projects
- Paid: Enterprise features (SSO, analytics, etc.)

**Option 3: Free + Services**
- Free: Framework
- Paid: Custom themes, setup help, support

**Recommendation:** Start fully free, add paid services later if needed.

## Next Steps

1. ✅ Document fumadocs structure (this file!)
2. ⬜ Clone fumadocs repository
3. ⬜ Analyze fumadocs source code
4. ⬜ Create detailed component mapping
5. ⬜ Build ShellDocs MVP
6. ⬜ Test with sample project
7. ⬜ Integrate ShellUI components
8. ⬜ Migrate ShellUI docs
9. ⬜ Launch together

## Conclusion

ShellDocs fills a MASSIVE gap in the .NET ecosystem. No modern documentation framework exists. This is a real problem with a clear solution.

Combined with ShellUI, you're creating an ecosystem that could become the standard for Blazor development.

**Timeline:**
- ShellUI v1.0: Q3 2026
- ShellDocs v1.0: Q2 2027
- Combined launch: Q2 2027

**Impact:**
- Better documentation across .NET
- Showcase for ShellUI
- Community contribution
- Potential legacy project

Let's build this! 🚀

---

**Version:** 1.0.0  
**Date:** October 2025  
**Status:** Planning Phase

