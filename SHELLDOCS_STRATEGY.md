# ShellDocs Build Strategy

**How to build ShellDocs by analyzing fumadocs**

## The Strategy

### Step 1: Analyze Fumadocs Structure ✅ (YOU ARE HERE)

**What to do:**
1. Clone fumadocs repository
2. Study the codebase structure
3. Document component hierarchy
4. Map features we need
5. Create this MD file for AI reference

**Commands:**
```bash
# Clone fumadocs
git clone https://github.com/fuma-nama/fumadocs
cd fumadocs

# Explore structure
tree -L 3
```

**What to document:**
- Component structure
- Layout patterns
- Styling approach
- Navigation logic
- Search implementation
- Theme system
- Build process

### Step 2: Create ShellDocs Project Structure

**What to do:**
1. Create .NET solution
2. Set up project structure
3. Add Tailwind CSS v4 (standalone CLI)
4. Create base Blazor components
5. DO NOT use ShellUI yet (comes later!)

**Commands:**
```bash
# Create solution
dotnet new sln -n ShellDocs

# Create projects
dotnet new classlib -n ShellDocs.Core -o src/ShellDocs.Core
dotnet new tool -n ShellDocs.CLI -o src/ShellDocs.CLI
dotnet new blazor -n ShellDocs.Blazor -o src/ShellDocs.Blazor

# Add to solution
dotnet sln add src/**/*.csproj
```

### Step 3: Build Core Components (NO ShellUI)

**Build with plain HTML/CSS:**
- Layout components
- Navigation components
- Content components
- Search components
- Theme components

**Why plain HTML first?**
- ShellDocs should work independently
- Can be used without ShellUI
- Easier to develop in parallel
- ShellUI integration is optional

### Step 4: Integrate ShellUI (After ShellUI v1.0)

**Replace HTML with ShellUI components:**
```
Plain HTML Button → ShellUI.Button
Plain HTML Card → ShellUI.Card
Plain HTML Tabs → ShellUI.Tabs
etc.
```

**Benefits:**
- Dogfooding ShellUI
- Better UX
- Showcases both products
- Optional dependency

### Step 5: Use ShellDocs for ShellUI Docs

**Final synergy:**
- ShellUI docs site built with ShellDocs
- ShellDocs uses ShellUI components
- Perfect showcase for both

## Build Order - THE ANSWER! 🎯

```
Timeline:

Q4 2025 - Q3 2026: ShellUI Development
├── Build ShellUI components
├── Test, polish, document
└── Release ShellUI v1.0 ✅

Q4 2026 - Q1 2027: ShellDocs Development
├── Analyze fumadocs ← START HERE
├── Build ShellDocs core
├── Build with plain HTML/Tailwind
├── Test with sample project
└── ShellDocs v1.0 MVP (no ShellUI) ✅

Q2 2027: Integration
├── Replace ShellDocs HTML with ShellUI components
├── Migrate ShellUI docs to ShellDocs
├── Polish both together
└── Launch ecosystem together 🚀
```

## What Comes First? (Answered!)

**Question:** Should we use ShellUI components in ShellDocs or what comes first?

**Answer:**

### Phase 1: Build ShellUI First
```
Time: Q4 2025 - Q3 2026

Why:
- ShellUI is the foundation
- Provides reusable components
- Needs to be production-ready
- ShellDocs will showcase it

Result:
✅ ShellUI v1.0 released
✅ 40+ components available
✅ NuGet + CLI working
✅ Ready to be used by ShellDocs
```

### Phase 2: Build ShellDocs (Plain HTML)
```
Time: Q4 2026 - Q1 2027

What to build:
- ShellDocs.Core (markdown parsing, nav generation)
- ShellDocs.CLI (init, dev, build commands)
- ShellDocs.Blazor (layout components with plain HTML)
- Tailwind CSS styling (fumadocs-inspired)

Why plain HTML:
- ShellDocs should work independently
- Faster initial development
- No dependency on ShellUI yet
- Can be used by projects without ShellUI

Result:
✅ ShellDocs v1.0 MVP
✅ Works with any .NET project
✅ No ShellUI dependency (yet)
```

### Phase 3: Integrate ShellUI
```
Time: Q2 2027

What to do:
1. Add ShellUI as optional dependency
2. Replace plain HTML components:
   - Button → ShellUI.Button
   - Card → ShellUI.Card
   - Tabs → ShellUI.Tabs
   - Input → ShellUI.Input
   - Badge → ShellUI.Badge
   - etc.
3. Keep plain HTML as fallback
4. Make integration toggleable

Configuration:
{
  "components": {
    "useShellUI": true  // Use ShellUI components
  }
}

Result:
✅ Beautiful UI (ShellUI components)
✅ Dogfooding ShellUI
✅ Optional for users
✅ ShellDocs still works standalone
```

### Phase 4: ShellUI Docs Migration
```
Time: Q2 2027

What to do:
1. Create ShellUI docs project with ShellDocs
2. Migrate all documentation markdown
3. Add live component demos
4. Add interactive playground
5. Deploy to docs.shellui.dev

Result:
✅ ShellUI docs using ShellDocs
✅ ShellDocs using ShellUI components
✅ Perfect synergy showcase
✅ Both products market each other
```

## Dependency Diagram

```
Build Order:

1. ShellUI (Component Library)
   ↓
   [ShellUI v1.0 Released - Q3 2026]
   ↓
2. ShellDocs MVP (Plain HTML)
   ↓
   [ShellDocs v1.0 Released - Q1 2027]
   ↓
3. ShellDocs + ShellUI Integration
   ↓
   [ShellDocs v1.1 with ShellUI - Q2 2027]
   ↓
4. ShellUI Docs on ShellDocs
   ↓
   [Ecosystem Complete - Q2 2027]
```

## Fumadocs Analysis Guide

### What to Study

**1. Project Structure**
```
fumadocs/
├── packages/
│   ├── core/           # Core functionality
│   ├── ui/             # UI components
│   ├── mdx/            # MDX processing
│   └── openapi/        # API docs
├── apps/
│   └── docs/           # Fumadocs own docs
└── examples/           # Example projects
```

**Key Questions:**
- How is navigation generated?
- How is markdown parsed?
- How are components structured?
- How is search implemented?
- How is theme handled?

**2. Component Analysis**

Study these components closely:
```
fumadocs-ui/
├── layout/
│   ├── docs.tsx          # Main docs layout
│   ├── nav.tsx           # Navigation
│   └── toc.tsx           # Table of contents
├── components/
│   ├── tabs.tsx          # Tab component
│   ├── callout.tsx       # Callout/alert
│   ├── code-block.tsx    # Code with copy
│   └── card.tsx          # Card component
└── provider/
    └── theme.tsx         # Theme provider
```

**For each component, document:**
- Props/parameters
- Styling approach
- Interactivity
- Accessibility features
- How to replicate in Blazor

**3. Feature Mapping**

Map fumadocs features to ShellDocs:

| Fumadocs | ShellDocs | Implementation |
|----------|-----------|----------------|
| MDX parsing | Markdown parsing | Markdig |
| React components | Razor components | Blazor |
| next.js routing | Blazor routing | Built-in |
| Tailwind CSS | Tailwind CSS | Same! |
| Algolia search | Algolia/local | Same options |
| Theme provider | Theme service | IJSRuntime |

**4. Styling Study**

Extract their CSS:
```css
/* Copy fumadocs design tokens */
:root {
  --fd-background: ...
  --fd-foreground: ...
  --fd-primary: ...
}
```

Create `shelldocs.css` with same tokens!

## Development Workflow

### Current Phase: Fumadocs Analysis

**Tasks:**
1. ✅ Create SHELLDOCS_VISION.md
2. ✅ Create SHELLDOCS_STRATEGY.md
3. ⬜ Clone fumadocs repository
4. ⬜ Analyze component structure
5. ⬜ Document navigation logic
6. ⬜ Document theme system
7. ⬜ Create component mapping table
8. ⬜ Extract design tokens
9. ⬜ Create Blazor component templates

**Output:**
- Comprehensive fumadocs analysis document
- Ready to build ShellDocs
- Clear component mapping
- Design system documented

### Next Phase: ShellDocs MVP

**After fumadocs analysis is complete:**

```bash
# 1. Create projects
dotnet new sln -n ShellDocs
cd ShellDocs

# 2. Add projects
dotnet new classlib -n ShellDocs.Core -o src/ShellDocs.Core
dotnet new tool -n ShellDocs.CLI -o src/ShellDocs.CLI
dotnet new blazor -n ShellDocs.Blazor -o src/ShellDocs.Blazor -int SSR

# 3. Add Tailwind standalone CLI
cd src/ShellDocs.Blazor
# Download Tailwind binary
# Set up tailwind.config.js
# Create shelldocs.css

# 4. Build core services
cd ../ShellDocs.Core
# Implement MarkdownService
# Implement NavigationService
# Implement SearchService

# 5. Build CLI commands
cd ../ShellDocs.CLI
# Implement InitCommand
# Implement DevCommand
# Implement BuildCommand

# 6. Build Blazor components
cd ../ShellDocs.Blazor
# Implement DocsLayout
# Implement Sidebar
# Implement CodeBlock
# etc.

# 7. Test
cd ../../
dotnet build
dotnet test

# 8. Package
dotnet pack
```

## Using This Document with AI

**Share this document with AI to:**

1. **Understand the structure**
   ```
   "Here's our analysis of fumadocs. 
    Help me build the equivalent in Blazor/C#."
   ```

2. **Generate components**
   ```
   "Based on fumadocs' tabs.tsx component,
    help me create Tabs.razor for ShellDocs."
   ```

3. **Map features**
   ```
   "fumadocs uses X for search.
    What's the best approach for ShellDocs in .NET?"
   ```

4. **Design tokens**
   ```
   "Here are fumadocs' CSS variables.
    Help me create shelldocs.css with the same design."
   ```

## Success Criteria

**ShellDocs is successful when:**

1. ✅ Looks as good as fumadocs
2. ✅ Works as smoothly as fumadocs
3. ✅ Has feature parity with fumadocs
4. ✅ Has better Blazor-specific features
5. ✅ Used by 100+ .NET projects
6. ✅ Becomes the standard for .NET docs

## Timeline Summary

```
October 2025:  Planning (Current) ✅
Q4 2025-Q3 2026: Build ShellUI
Q4 2026-Q1 2027: Build ShellDocs MVP
Q2 2027:         Integrate & Launch
```

## Key Decisions

### Decision 1: Build Order
**✅ Decided:** ShellUI first, then ShellDocs

**Rationale:**
- ShellUI provides reusable components
- ShellDocs can showcase ShellUI
- Each can work independently
- Better focus and quality

### Decision 2: Plain HTML First
**✅ Decided:** Build ShellDocs with plain HTML, integrate ShellUI later

**Rationale:**
- ShellDocs works standalone
- Faster initial development
- No circular dependency
- ShellUI integration is optional feature

### Decision 3: Fumadocs Analysis
**✅ Decided:** Clone and study fumadocs source code

**Rationale:**
- Learn from the best
- Understand their approach
- Map features accurately
- Steal the good ideas legally! (MIT licensed)

### Decision 4: Technology Stack
**✅ Decided:**
- Backend: Blazor SSR
- Styling: Tailwind CSS v4
- Markdown: Markdig
- Search: Algolia or Flexsearch
- Syntax: ColorCode.Blazor

**Rationale:**
- Modern, fast, maintainable
- Same as ShellUI stack
- Great developer experience
- Good performance

## Next Steps

**Immediate (This Week):**
1. Clone fumadocs repository
2. Study their structure
3. Document findings
4. Create component map

**Short Term (This Month):**
1. Complete fumadocs analysis
2. Create detailed spec
3. Start ShellDocs project structure
4. Build first prototype

**Long Term (After ShellUI v1.0):**
1. Complete ShellDocs MVP
2. Test with sample projects
3. Integrate ShellUI
4. Launch together

---

**The Answer to "What Comes First?"**

1. **ShellUI** (Q4 2025 - Q3 2026)
2. **ShellDocs** with plain HTML (Q4 2026 - Q1 2027)
3. **ShellDocs + ShellUI** integration (Q2 2027)
4. **ShellUI docs** on ShellDocs (Q2 2027)

Build them separately, integrate them later, launch them together! 🚀

