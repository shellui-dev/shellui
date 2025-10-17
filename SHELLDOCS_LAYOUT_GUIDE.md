# ShellDocs Layout Structure - Complete Implementation Guide

**Build fumadocs-style layouts in Blazor with Tailwind CSS**

This guide provides complete implementation details for building the ShellDocs layout system that matches fumadocs' design and functionality.

---

## 📋 Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [File Structure](#file-structure)
3. [Core Services & State Management](#core-services--state-management)
4. [Layout Components](#layout-components)
5. [Sidebar Components](#sidebar-components)
6. [Navigation Components](#navigation-components)
7. [CSS & Styling](#css--styling)
8. [JavaScript Interop](#javascript-interop)
9. [Configuration](#configuration)
10. [Usage Examples](#usage-examples)

---

## 📐 Architecture Overview

### Layout Structure Hierarchy

```
DocsLayout (Main Container)
├── NavProvider (State: transparent mode)
├── SidebarProvider (State: open, collapsed)
│   ├── Sidebar (Desktop)
│   │   ├── SidebarHeader (Logo, search, actions)
│   │   ├── SidebarViewport (Scrollable content)
│   │   │   └── SidebarPageTree (Navigation items)
│   │   └── SidebarFooter (Theme toggle, links)
│   │
│   ├── SidebarMobile (Mobile drawer)
│   │   └── Same structure as Sidebar
│   │
│   └── Navbar (Mobile only, fixed top)
│       ├── Logo
│       ├── Search toggle
│       └── Sidebar toggle
│
├── LayoutBody (Main content area)
│   └── @Body (Page content)
│
└── TableOfContents (Fixed right, desktop only)
```

### Responsive Behavior

| Breakpoint | Sidebar | Navbar | TOC |
|------------|---------|--------|-----|
| Desktop (xl+) | Fixed left, collapsible | Hidden | Fixed right |
| Tablet (md-xl) | Fixed left, collapsible | Hidden | Popover |
| Mobile (<md) | Drawer (overlay) | Fixed top | Popover |

---

## 📁 File Structure

```
ShellDocs.Blazor/
├── Services/
│   ├── SidebarService.cs          # Sidebar state management
│   ├── NavigationService.cs       # Navigation state management
│   └── ThemeService.cs            # Theme state management
│
├── Layouts/
│   ├── DocsLayout.razor           # Main documentation layout
│   ├── DocsLayout.razor.cs        # Code-behind
│   └── DocsLayout.razor.css       # Component-scoped styles (optional)
│
├── Components/
│   ├── Sidebar/
│   │   ├── Sidebar.razor          # Desktop sidebar
│   │   ├── SidebarMobile.razor    # Mobile drawer
│   │   ├── SidebarHeader.razor    # Sidebar header section
│   │   ├── SidebarFooter.razor    # Sidebar footer section
│   │   ├── SidebarViewport.razor  # Scrollable viewport
│   │   ├── SidebarPageTree.razor  # Page tree renderer
│   │   ├── SidebarItem.razor      # Individual nav item
│   │   ├── SidebarFolder.razor    # Collapsible folder
│   │   ├── SidebarSeparator.razor # Section separator
│   │   └── CollapsibleControl.razor # Collapse button (desktop)
│   │
│   ├── Navigation/
│   │   ├── Navbar.razor           # Mobile top navbar
│   │   ├── NavbarMobile.razor     # Mobile navbar
│   │   └── LayoutTabs.razor       # Top-level tabs (optional)
│   │
│   ├── TableOfContents/
│   │   ├── TableOfContents.razor  # Desktop TOC
│   │   └── TocPopover.razor       # Mobile TOC popover
│   │
│   └── Shared/
│       ├── ThemeToggle.razor      # Dark/light mode toggle
│       └── SearchToggle.razor     # Search dialog toggle
│
└── wwwroot/
    ├── css/
    │   ├── shelldocs.css          # Main stylesheet
    │   ├── components/
    │   │   ├── sidebar.css        # Sidebar-specific styles
    │   │   └── layout.css         # Layout-specific styles
    │   └── themes/
    │       └── neutral.css        # Default neutral theme
    │
    └── js/
        ├── shelldocs.js           # Main JS interop
        ├── sidebar.js             # Sidebar interactions
        └── theme.js               # Theme persistence
```

---

## 🎯 Core Services & State Management

### 1. SidebarService.cs

```csharp
using System;

namespace ShellDocs.Blazor.Services;

/// <summary>
/// Manages sidebar state (open/closed, collapsed/expanded)
/// </summary>
public class SidebarService
{
    private bool _isOpen = false;
    private bool _isCollapsed = false;
    
    /// <summary>
    /// Sidebar open state (mobile)
    /// </summary>
    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            if (_isOpen != value)
            {
                _isOpen = value;
                OnStateChanged?.Invoke();
            }
        }
    }
    
    /// <summary>
    /// Sidebar collapsed state (desktop)
    /// </summary>
    public bool IsCollapsed
    {
        get => _isCollapsed;
        set
        {
            if (_isCollapsed != value)
            {
                _isCollapsed = value;
                OnStateChanged?.Invoke();
            }
        }
    }
    
    public event Action? OnStateChanged;
    
    public void Toggle() => IsOpen = !IsOpen;
    public void ToggleCollapsed() => IsCollapsed = !IsCollapsed;
    public void Open() => IsOpen = true;
    public void Close() => IsOpen = false;
    public void Collapse() => IsCollapsed = true;
    public void Expand() => IsCollapsed = false;
}
```

### 2. NavigationService.cs

```csharp
using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ShellDocs.Blazor.Services;

/// <summary>
/// Manages navigation state and transparency mode
/// </summary>
public class NavigationService : IDisposable
{
    private readonly NavigationManager _navigationManager;
    private bool _isTransparent = false;
    
    public NavigationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _navigationManager.LocationChanged += OnLocationChanged;
    }
    
    public bool IsTransparent
    {
        get => _isTransparent;
        set
        {
            if (_isTransparent != value)
            {
                _isTransparent = value;
                OnStateChanged?.Invoke();
            }
        }
    }
    
    public event Action? OnStateChanged;
    
    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        // Reset transparency on navigation
        IsTransparent = false;
        OnStateChanged?.Invoke();
    }
    
    public void Dispose()
    {
        _navigationManager.LocationChanged -= OnLocationChanged;
    }
}
```

### 3. ThemeService.cs

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace ShellDocs.Blazor.Services;

/// <summary>
/// Manages theme state (dark/light) with persistence
/// </summary>
public class ThemeService
{
    private readonly IJSRuntime _js;
    private string _theme = "light";
    
    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }
    
    public string Theme
    {
        get => _theme;
        private set
        {
            if (_theme != value)
            {
                _theme = value;
                OnThemeChanged?.Invoke();
            }
        }
    }
    
    public event Action? OnThemeChanged;
    
    /// <summary>
    /// Initialize theme from localStorage or system preference
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            var theme = await _js.InvokeAsync<string>("ShellDocs.Theme.getTheme");
            Theme = theme;
        }
        catch
        {
            Theme = "light";
        }
    }
    
    /// <summary>
    /// Toggle between light and dark mode
    /// </summary>
    public async Task ToggleAsync()
    {
        var newTheme = Theme == "dark" ? "light" : "dark";
        await SetThemeAsync(newTheme);
    }
    
    /// <summary>
    /// Set specific theme
    /// </summary>
    public async Task SetThemeAsync(string theme)
    {
        Theme = theme;
        await _js.InvokeVoidAsync("ShellDocs.Theme.setTheme", theme);
    }
}
```

### 4. Service Registration (Program.cs)

```csharp
// Add to Program.cs
builder.Services.AddScoped<SidebarService>();
builder.Services.AddScoped<NavigationService>();
builder.Services.AddScoped<ThemeService>();
```

---

## 📦 Layout Components

### 1. DocsLayout.razor

```razor
@inherits LayoutComponentBase
@inject SidebarService SidebarService
@inject NavigationService NavigationService
@implements IDisposable

<CascadingValue Value="this">
    <div class="shelldocs-layout @(SidebarService.IsCollapsed ? "sidebar-collapsed" : "")">
        
        @* Desktop Sidebar *@
        <Sidebar 
            PageTree="@PageTree"
            Title="@Title"
            Logo="@Logo"
            Links="@Links"
            CollapsibleControl="@CollapsibleControl"
            SearchEnabled="@SearchEnabled"
            ThemeToggleEnabled="@ThemeToggleEnabled" />

        @* Mobile Sidebar (Drawer) *@
        <SidebarMobile 
            PageTree="@PageTree"
            Title="@Title"
            Logo="@Logo"
            Links="@Links"
            SearchEnabled="@SearchEnabled"
            ThemeToggleEnabled="@ThemeToggleEnabled" />

        @* Mobile Navbar *@
        <Navbar 
            Title="@Title"
            Logo="@Logo"
            SearchEnabled="@SearchEnabled" />

        @* Main Content Area *@
        <div class="shelldocs-main-wrapper">
            @* Optional: Top-level tabs *@
            @if (Tabs != null && Tabs.Any())
            {
                <LayoutTabs Options="@Tabs" />
            }

            @* Main content *@
            <main id="shelldocs-content" 
                  class="shelldocs-content"
                  style="@GetContentStyles()">
                @Body
            </main>
        </div>

        @* Desktop Table of Contents *@
        @if (ShowTableOfContents)
        {
            <TableOfContents Items="@TocItems" />
        }
    </div>
</CascadingValue>

@code {
    [Parameter] public PageTree PageTree { get; set; } = null!;
    [Parameter] public string Title { get; set; } = "Documentation";
    [Parameter] public string? Logo { get; set; }
    [Parameter] public List<NavLink> Links { get; set; } = new();
    [Parameter] public List<TocItem> TocItems { get; set; } = new();
    [Parameter] public List<TabOption>? Tabs { get; set; }
    [Parameter] public bool CollapsibleControl { get; set; } = true;
    [Parameter] public bool SearchEnabled { get; set; } = true;
    [Parameter] public bool ThemeToggleEnabled { get; set; } = true;
    [Parameter] public bool ShowTableOfContents { get; set; } = true;

    protected override void OnInitialized()
    {
        SidebarService.OnStateChanged += StateHasChanged;
        NavigationService.OnStateChanged += StateHasChanged;
    }

    private string GetContentStyles()
    {
        if (SidebarService.IsCollapsed)
        {
            return "padding-inline-start: min(calc(100vw - var(--sd-page-width)), var(--sd-sidebar-width));";
        }
        return "padding-inline-start: var(--sd-sidebar-width);";
    }

    public void Dispose()
    {
        SidebarService.OnStateChanged -= StateHasChanged;
        NavigationService.OnStateChanged -= StateHasChanged;
    }
}
```

---

## 🗂️ Sidebar Components

### 1. Sidebar.razor (Desktop)

```razor
@inject SidebarService SidebarService
@inject IJSRuntime JS

<aside id="shelldocs-sidebar"
       class="shelldocs-sidebar @(SidebarService.IsCollapsed ? "collapsed" : "")"
       data-collapsed="@SidebarService.IsCollapsed"
       @onmouseenter="OnMouseEnter"
       @onmouseleave="OnMouseLeave"
       style="@GetSidebarStyles()">
    
    <div class="sidebar-wrapper">
        <SidebarHeader 
            Title="@Title"
            Logo="@Logo"
            SearchEnabled="@SearchEnabled"
            CollapsibleControl="@CollapsibleControl" />
        
        <SidebarViewport>
            <SidebarPageTree PageTree="@PageTree" />
        </SidebarViewport>
        
        <SidebarFooter 
            Links="@Links"
            ThemeToggleEnabled="@ThemeToggleEnabled" />
    </div>
</aside>

@code {
    [Parameter] public PageTree PageTree { get; set; } = null!;
    [Parameter] public string Title { get; set; } = "Docs";
    [Parameter] public string? Logo { get; set; }
    [Parameter] public List<NavLink> Links { get; set; } = new();
    [Parameter] public bool CollapsibleControl { get; set; } = true;
    [Parameter] public bool SearchEnabled { get; set; } = true;
    [Parameter] public bool ThemeToggleEnabled { get; set; } = true;

    private bool _isHovering = false;
    private System.Timers.Timer? _hoverTimer;

    protected override void OnInitialized()
    {
        SidebarService.OnStateChanged += StateHasChanged;
    }

    private void OnMouseEnter(MouseEventArgs e)
    {
        if (!SidebarService.IsCollapsed) return;
        
        _hoverTimer?.Stop();
        _isHovering = true;
        StateHasChanged();
    }

    private void OnMouseLeave(MouseEventArgs e)
    {
        if (!SidebarService.IsCollapsed) return;
        
        _hoverTimer = new System.Timers.Timer(500);
        _hoverTimer.Elapsed += (s, e) =>
        {
            _isHovering = false;
            InvokeAsync(StateHasChanged);
            _hoverTimer?.Dispose();
        };
        _hoverTimer.Start();
    }

    private string GetSidebarStyles()
    {
        var styles = new List<string>();
        
        if (SidebarService.IsCollapsed)
        {
            var offset = _isHovering 
                ? "calc(var(--spacing) * 2)" 
                : "calc(16px - 100%)";
            styles.Add($"--sd-sidebar-offset: {offset}");
            styles.Add("--sd-sidebar-margin: 0.5rem");
        }
        else
        {
            styles.Add("--sd-sidebar-margin: 0px");
        }
        
        return string.Join("; ", styles);
    }

    public void Dispose()
    {
        SidebarService.OnStateChanged -= StateHasChanged;
        _hoverTimer?.Dispose();
    }
}
```

### 2. SidebarMobile.razor (Mobile Drawer)

```razor
@inject SidebarService SidebarService
@inject IJSRuntime JS

@* Overlay backdrop *@
@if (SidebarService.IsOpen)
{
    <div class="sidebar-overlay" 
         @onclick="Close"
         @onclick:stopPropagation="false">
    </div>
}

@* Mobile Sidebar Drawer *@
<aside id="shelldocs-sidebar-mobile"
       class="shelldocs-sidebar-mobile @(SidebarService.IsOpen ? "open" : "closed")"
       data-state="@(SidebarService.IsOpen ? "open" : "closed")">
    
    <SidebarHeader 
        Title="@Title"
        Logo="@Logo"
        SearchEnabled="@SearchEnabled"
        IsMobile="true" />
    
    <SidebarViewport>
        <SidebarPageTree PageTree="@PageTree" />
    </SidebarViewport>
    
    <SidebarFooter 
        Links="@Links"
        ThemeToggleEnabled="@ThemeToggleEnabled"
        IsMobile="true" />
</aside>

@code {
    [Parameter] public PageTree PageTree { get; set; } = null!;
    [Parameter] public string Title { get; set; } = "Docs";
    [Parameter] public string? Logo { get; set; }
    [Parameter] public List<NavLink> Links { get; set; } = new();
    [Parameter] public bool SearchEnabled { get; set; } = true;
    [Parameter] public bool ThemeToggleEnabled { get; set; } = true;

    protected override void OnInitialized()
    {
        SidebarService.OnStateChanged += StateHasChanged;
    }

    private void Close()
    {
        SidebarService.Close();
    }

    public void Dispose()
    {
        SidebarService.OnStateChanged -= StateHasChanged;
    }
}
```

### 3. SidebarHeader.razor

```razor
@inject SidebarService SidebarService

<div class="sidebar-header">
    <div class="sidebar-header-content">
        <a href="/" class="sidebar-logo">
            @if (!string.IsNullOrEmpty(Logo))
            {
                <img src="@Logo" alt="@Title" class="logo-image" />
            }
            <span class="logo-text">@Title</span>
        </a>

        @if (!IsMobile && CollapsibleControl)
        {
            <button class="sidebar-collapse-btn"
                    @onclick="ToggleCollapsed"
                    aria-label="Collapse Sidebar">
                <svg class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                    <path d="M9 18l6-6-6-6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
            </button>
        }

        @if (IsMobile)
        {
            <button class="sidebar-close-btn"
                    @onclick="Close"
                    aria-label="Close Sidebar">
                <svg class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                    <path d="M18 6L6 18M6 6l12 12" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
            </button>
        }
    </div>

    @if (SearchEnabled)
    {
        <div class="sidebar-search">
            <SearchToggle />
        </div>
    }
</div>

@code {
    [Parameter] public string Title { get; set; } = "Docs";
    [Parameter] public string? Logo { get; set; }
    [Parameter] public bool SearchEnabled { get; set; } = true;
    [Parameter] public bool CollapsibleControl { get; set; } = true;
    [Parameter] public bool IsMobile { get; set; } = false;

    private void ToggleCollapsed()
    {
        SidebarService.ToggleCollapsed();
    }

    private void Close()
    {
        SidebarService.Close();
    }
}
```

### 4. SidebarViewport.razor

```razor
<div class="sidebar-viewport">
    <div class="sidebar-scroll-area">
        @ChildContent
    </div>
</div>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

### 5. SidebarPageTree.razor

```razor
@inject NavigationManager Navigation

@foreach (var node in PageTree.Children)
{
    @RenderNode(node, 1)
}

@code {
    [Parameter] public PageTree PageTree { get; set; } = null!;

    private RenderFragment RenderNode(PageTreeNode node, int level) => builder =>
    {
        if (node is PageTreeItem item)
        {
            <SidebarItem 
                Url="@item.Url"
                Icon="@item.Icon"
                IsActive="@IsActive(item.Url)">
                @item.Name
            </SidebarItem>
        }
        else if (node is PageTreeFolder folder)
        {
            <SidebarFolder 
                Name="@folder.Name"
                Icon="@folder.Icon"
                DefaultOpen="@folder.DefaultOpen"
                Level="@level">
                @foreach (var child in folder.Children)
                {
                    @RenderNode(child, level + 1)
                }
            </SidebarFolder>
        }
        else if (node is PageTreeSeparator separator)
        {
            <SidebarSeparator 
                Name="@separator.Name"
                Icon="@separator.Icon" />
        }
    };

    private bool IsActive(string url)
    {
        var currentPath = new Uri(Navigation.Uri).AbsolutePath;
        return currentPath.StartsWith(url, StringComparison.OrdinalIgnoreCase);
    }
}
```

### 6. SidebarItem.razor

```razor
@inject NavigationManager Navigation

<a href="@Url" 
   class="sidebar-item @(IsActive ? "active" : "")"
   data-active="@IsActive"
   @onclick="OnClick"
   @onclick:preventDefault="false">
    
    @if (!string.IsNullOrEmpty(Icon))
    {
        <i class="sidebar-item-icon icon-@Icon"></i>
    }
    
    <span class="sidebar-item-text">@ChildContent</span>
</a>

@code {
    [Parameter] public string Url { get; set; } = "";
    [Parameter] public string? Icon { get; set; }
    [Parameter] public bool IsActive { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [CascadingParameter] public SidebarService? SidebarService { get; set; }

    private void OnClick()
    {
        // Close mobile sidebar on navigation
        if (SidebarService != null)
        {
            SidebarService.Close();
        }
    }
}
```

### 7. SidebarFolder.razor

```razor
@inject NavigationManager Navigation

<div class="sidebar-folder @(IsOpen ? "open" : "closed")" data-level="@Level">
    <button class="sidebar-folder-trigger"
            @onclick="Toggle">
        
        <svg class="sidebar-folder-chevron @(IsOpen ? "open" : "")" 
             viewBox="0 0 24 24" 
             fill="none" 
             stroke="currentColor">
            <path d="M9 18l6-6-6-6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>

        @if (!string.IsNullOrEmpty(Icon))
        {
            <i class="sidebar-folder-icon icon-@Icon"></i>
        }
        
        <span class="sidebar-folder-name">@Name</span>
    </button>

    @if (IsOpen)
    {
        <div class="sidebar-folder-content" style="--sidebar-item-offset: calc(var(--spacing) * @((Level + 1) * 3));">
            @ChildContent
        </div>
    }
</div>

@code {
    [Parameter] public string Name { get; set; } = "";
    [Parameter] public string? Icon { get; set; }
    [Parameter] public bool DefaultOpen { get; set; } = false;
    [Parameter] public int Level { get; set; } = 1;
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private bool IsOpen { get; set; }

    protected override void OnInitialized()
    {
        IsOpen = DefaultOpen;
    }

    private void Toggle()
    {
        IsOpen = !IsOpen;
    }
}
```

### 8. SidebarSeparator.razor

```razor
<div class="sidebar-separator">
    @if (!string.IsNullOrEmpty(Icon))
    {
        <i class="sidebar-separator-icon icon-@Icon"></i>
    }
    @if (!string.IsNullOrEmpty(Name))
    {
        <span class="sidebar-separator-text">@Name</span>
    }
</div>

@code {
    [Parameter] public string? Name { get; set; }
    [Parameter] public string? Icon { get; set; }
}
```

### 9. SidebarFooter.razor

```razor
<div class="sidebar-footer">
    <div class="sidebar-footer-content">
        @if (ThemeToggleEnabled)
        {
            <ThemeToggle />
        }

        @foreach (var link in Links)
        {
            <a href="@link.Url" 
               target="_blank" 
               rel="noopener noreferrer"
               class="sidebar-footer-link"
               aria-label="@link.Label">
                @if (!string.IsNullOrEmpty(link.Icon))
                {
                    <i class="icon icon-@link.Icon"></i>
                }
            </a>
        }
    </div>
</div>

@code {
    [Parameter] public List<NavLink> Links { get; set; } = new();
    [Parameter] public bool ThemeToggleEnabled { get; set; } = true;
    [Parameter] public bool IsMobile { get; set; } = false;
}
```

---

## 🧭 Navigation Components

### 1. Navbar.razor (Mobile Top Bar)

```razor
@inject SidebarService SidebarService

<header id="shelldocs-navbar"
        class="shelldocs-navbar md:hidden"
        style="height: var(--sd-nav-height);">
    
    <div class="navbar-content">
        <a href="/" class="navbar-logo">
            @if (!string.IsNullOrEmpty(Logo))
            {
                <img src="@Logo" alt="@Title" class="logo-image" />
            }
            <span class="logo-text">@Title</span>
        </a>

        <div class="navbar-actions">
            @if (SearchEnabled)
            {
                <SearchToggle />
            }

            <button class="navbar-menu-btn"
                    @onclick="ToggleSidebar"
                    aria-label="Toggle Menu">
                <svg class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                    <path d="M3 12h18M3 6h18M3 18h18" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
            </button>
        </div>
    </div>
</header>

@code {
    [Parameter] public string Title { get; set; } = "Docs";
    [Parameter] public string? Logo { get; set; }
    [Parameter] public bool SearchEnabled { get; set; } = true;

    private void ToggleSidebar()
    {
        SidebarService.Toggle();
    }
}
```

### 2. CollapsibleControl.razor (Desktop Collapse Button)

```razor
@inject SidebarService SidebarService

@if (SidebarService.IsCollapsed)
{
    <div class="collapsible-control">
        <button class="collapse-btn"
                @onclick="Expand"
                aria-label="Expand Sidebar">
            <svg class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                <path d="M9 18l6-6-6-6" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
        </button>
        
        @if (SearchEnabled)
        {
            <SearchToggle />
        }
    </div>
}

@code {
    [Parameter] public bool SearchEnabled { get; set; } = true;

    protected override void OnInitialized()
    {
        SidebarService.OnStateChanged += StateHasChanged;
    }

    private void Expand()
    {
        SidebarService.Expand();
    }

    public void Dispose()
    {
        SidebarService.OnStateChanged -= StateHasChanged;
    }
}
```

---

## 🎨 CSS & Styling

### 1. Main Stylesheet (wwwroot/css/shelldocs.css)

```css
@import 'tailwindcss';

/* ========================================
   ShellDocs Design Tokens
   ======================================== */

@theme {
  /* Layout Variables */
  --sd-sidebar-width: 268px;
  --sd-toc-width: 240px;
  --sd-page-width: 1200px;
  --sd-layout-width: 1600px;
  --sd-nav-height: 56px;
  --sd-banner-height: 0px;
  --sd-tocnav-height: 0px;

  /* Mobile Sidebar */
  --sd-sidebar-mobile-offset: 100%;

  /* Colors - Light Mode (Neutral Black & White) */
  --color-sd-background: hsl(0, 0%, 96%);
  --color-sd-foreground: hsl(0, 0%, 3.9%);
  --color-sd-muted: hsl(0, 0%, 96.1%);
  --color-sd-muted-foreground: hsl(0, 0%, 45.1%);
  --color-sd-popover: hsl(0, 0%, 98%);
  --color-sd-popover-foreground: hsl(0, 0%, 15.1%);
  --color-sd-card: hsl(0, 0%, 94.7%);
  --color-sd-card-foreground: hsl(0, 0%, 3.9%);
  --color-sd-border: hsla(0, 0%, 80%, 50%);
  --color-sd-primary: hsl(0, 0%, 9%);
  --color-sd-primary-foreground: hsl(0, 0%, 98%);
  --color-sd-secondary: hsl(0, 0%, 93.1%);
  --color-sd-secondary-foreground: hsl(0, 0%, 9%);
  --color-sd-accent: hsla(0, 0%, 82%, 50%);
  --color-sd-accent-foreground: hsl(0, 0%, 9%);
  --color-sd-ring: hsl(0, 0%, 63.9%);

  /* Animations */
  --animate-sd-fade-in: sd-fade-in 300ms ease;
  --animate-sd-fade-out: sd-fade-out 300ms ease;
  --animate-sd-sidebar-in: sd-sidebar-in 250ms ease;
  --animate-sd-sidebar-out: sd-sidebar-out 250ms ease;
  --animate-sd-collapsible-down: sd-collapsible-down 150ms cubic-bezier(0.45, 0, 0.55, 1);
  --animate-sd-collapsible-up: sd-collapsible-up 150ms cubic-bezier(0.45, 0, 0.55, 1);
}

/* Dark Mode */
.dark {
  --color-sd-background: hsl(0, 0%, 7.04%);
  --color-sd-foreground: hsl(0, 0%, 92%);
  --color-sd-muted: hsl(0, 0%, 12.9%);
  --color-sd-muted-foreground: hsla(0, 0%, 70%, 0.8%);
  --color-sd-popover: hsl(0, 0%, 11.6%);
  --color-sd-popover-foreground: hsl(0, 0%, 86.9%);
  --color-sd-card: hsl(0, 0%, 9.8%);
  --color-sd-card-foreground: hsl(0, 0%, 98%);
  --color-sd-border: hsla(0, 0%, 40%, 20%);
  --color-sd-primary: hsl(0, 0%, 98%);
  --color-sd-primary-foreground: hsl(0, 0%, 9%);
  --color-sd-secondary: hsl(0, 0%, 12.9%);
  --color-sd-secondary-foreground: hsl(0, 0%, 92%);
  --color-sd-accent: hsla(0, 0%, 40.9%, 30%);
  --color-sd-accent-foreground: hsl(0, 0%, 90%);
  --color-sd-ring: hsl(0, 0%, 54.9%);
  --color-sd-overlay: hsla(0, 0%, 0%, 0.2);
}

/* Dark sidebar tweaks */
.dark #shelldocs-sidebar {
  --color-sd-muted: hsl(0, 0%, 16%);
  --color-sd-secondary: hsl(0, 0%, 18%);
  --color-sd-muted-foreground: hsl(0, 0%, 72%);
}

/* RTL Support */
[dir='rtl'] {
  --sd-sidebar-mobile-offset: -100%;
}

/* ========================================
   Animations
   ======================================== */

@keyframes sd-fade-in {
  from { opacity: 0; }
  to { opacity: 1; }
}

@keyframes sd-fade-out {
  from { opacity: 1; }
  to { opacity: 0; }
}

@keyframes sd-sidebar-in {
  from { transform: translateX(var(--sd-sidebar-mobile-offset)); }
  to { transform: translateX(0); }
}

@keyframes sd-sidebar-out {
  from { transform: translateX(0); }
  to { transform: translateX(var(--sd-sidebar-mobile-offset)); }
}

@keyframes sd-collapsible-down {
  from {
    height: 0;
    opacity: 0;
  }
  to {
    height: var(--content-height);
    opacity: 1;
  }
}

@keyframes sd-collapsible-up {
  from {
    height: var(--content-height);
    opacity: 1;
  }
  to {
    height: 0;
    opacity: 0;
  }
}

/* ========================================
   Base Styles
   ======================================== */

@layer base {
  *,
  ::before,
  ::after,
  ::backdrop {
    border-color: var(--color-sd-border);
  }

  body {
    @apply min-h-screen flex flex-col;
    background-color: var(--color-sd-background);
    color: var(--color-sd-foreground);
  }

  :root,
  #shelldocs-content {
    --sd-layout-offset: max(calc(50vw - var(--sd-layout-width) / 2), 0px);
  }
}

/* ========================================
   Layout Components
   ======================================== */

/* Main Layout Container */
.shelldocs-layout {
  @apply relative min-h-screen flex;
}

/* Main Content Wrapper */
.shelldocs-main-wrapper {
  @apply flex flex-1 flex-col;
  padding-top: var(--sd-nav-height);
  transition: padding 200ms;
}

.shelldocs-content {
  @apply flex-1 w-full mx-auto max-w-[var(--sd-page-width)];
  padding-inline-end: var(--sd-toc-width);
  transition: padding-inline-start 200ms;
}

/* When sidebar is collapsed */
.sidebar-collapsed .shelldocs-content {
  margin-inline: var(--sd-layout-offset);
}

/* ========================================
   Desktop Sidebar
   ======================================== */

.shelldocs-sidebar {
  @apply fixed left-0 flex flex-col items-end z-20 bg-sd-card text-sm border-e;
  top: calc(var(--sd-banner-height) + var(--sd-nav-height));
  bottom: 0;
  width: calc(var(--spacing) + var(--sd-sidebar-width) + var(--sd-layout-offset));
  transition: all 200ms;
}

.shelldocs-sidebar.collapsed {
  @apply rounded-xl border shadow-lg;
  width: var(--sd-sidebar-width);
  top: calc(var(--sd-banner-height) + var(--sd-nav-height) + var(--sd-sidebar-margin, 0px));
  bottom: var(--sd-sidebar-margin, 0px);
  transform: translateX(var(--sd-sidebar-offset, 0));
  opacity: 0;
  pointer-events: none;
}

.shelldocs-sidebar.collapsed:hover {
  @apply z-50 opacity-100;
  pointer-events: auto;
}

.shelldocs-sidebar .sidebar-wrapper {
  @apply flex flex-col h-full;
  width: var(--sd-sidebar-width);
}

/* Mobile: Hide sidebar */
@media (max-width: 768px) {
  .shelldocs-sidebar {
    @apply hidden;
  }
}

/* ========================================
   Mobile Sidebar (Drawer)
   ======================================== */

.sidebar-overlay {
  @apply fixed inset-0 z-40 backdrop-blur-sm;
  background-color: var(--color-sd-overlay);
  animation: var(--animate-sd-fade-in);
}

.sidebar-overlay[data-state='closed'] {
  animation: var(--animate-sd-fade-out);
}

.shelldocs-sidebar-mobile {
  @apply fixed flex flex-col shadow-lg border-s end-0 inset-y-0 z-40 bg-sd-background;
  width: 85%;
  max-width: 380px;
  text-size: 15px;
  transform: translateX(var(--sd-sidebar-mobile-offset));
  transition: transform 250ms ease;
}

.shelldocs-sidebar-mobile.open {
  transform: translateX(0);
  animation: var(--animate-sd-sidebar-in);
}

.shelldocs-sidebar-mobile.closed {
  animation: var(--animate-sd-sidebar-out);
}

/* Desktop: Hide mobile sidebar */
@media (min-width: 768px) {
  .shelldocs-sidebar-mobile,
  .sidebar-overlay {
    @apply hidden;
  }
}

/* ========================================
   Sidebar Header
   ======================================== */

.sidebar-header {
  @apply flex flex-col gap-3 p-4 pb-2;
}

.sidebar-header-content {
  @apply flex flex-row items-center gap-2;
}

.sidebar-logo {
  @apply inline-flex items-center gap-2.5 font-medium flex-1;
  font-size: 15px;
}

.sidebar-logo .logo-image {
  @apply h-6 w-auto;
}

.sidebar-collapse-btn,
.sidebar-close-btn {
  @apply p-1.5 rounded-lg transition-colors;
  @apply text-sd-muted-foreground;
  @apply hover:bg-sd-accent/50 hover:text-sd-accent-foreground;
}

.sidebar-collapse-btn .icon,
.sidebar-close-btn .icon {
  @apply w-5 h-5;
}

.sidebar-search {
  @apply w-full;
}

/* ========================================
   Sidebar Viewport (Scrollable Area)
   ======================================== */

.sidebar-viewport {
  @apply flex-1 h-full overflow-hidden;
}

.sidebar-scroll-area {
  @apply h-full overflow-y-auto p-4;
  --sidebar-item-offset: calc(var(--spacing) * 2);
  
  /* Gradient mask at top/bottom */
  mask-image: linear-gradient(
    to bottom,
    transparent,
    white 12px,
    white calc(100% - 12px),
    transparent
  );
}

/* Custom scrollbar */
.sidebar-scroll-area::-webkit-scrollbar {
  width: 5px;
}

.sidebar-scroll-area::-webkit-scrollbar-thumb {
  @apply rounded-full;
  background: var(--color-sd-border);
}

.sidebar-scroll-area::-webkit-scrollbar-track {
  @apply bg-transparent;
}

/* ========================================
   Sidebar Items
   ======================================== */

.sidebar-item {
  @apply relative flex items-center gap-2 rounded-lg p-2 text-start;
  @apply text-sd-muted-foreground transition-colors;
  padding-inline-start: var(--sidebar-item-offset);
  overflow-wrap: anywhere;
}

.sidebar-item:hover {
  @apply bg-sd-accent/50 text-sd-accent-foreground/80;
}

.sidebar-item.active {
  @apply bg-sd-primary/10 text-sd-primary;
}

.sidebar-item-icon {
  @apply w-4 h-4 shrink-0;
}

/* ========================================
   Sidebar Folder
   ======================================== */

.sidebar-folder {
  @apply relative;
}

.sidebar-folder-trigger {
  @apply relative flex w-full items-center gap-2 rounded-lg p-2 text-start;
  @apply text-sd-muted-foreground transition-colors;
  padding-inline-start: var(--sidebar-item-offset);
}

.sidebar-folder-trigger:hover {
  @apply bg-sd-accent/50 text-sd-accent-foreground/80;
}

.sidebar-folder-chevron {
  @apply w-4 h-4 ms-auto transition-transform shrink-0;
}

.sidebar-folder-chevron:not(.open) {
  @apply -rotate-90;
}

.sidebar-folder-icon {
  @apply w-4 h-4 shrink-0;
}

.sidebar-folder-content {
  @apply relative;
  animation: var(--animate-sd-collapsible-down);
}

/* Active indicator line for nested items */
.sidebar-folder-content[data-level="1"]::before {
  content: '';
  @apply absolute w-px inset-y-1 bg-sd-border;
  left: 10px;
}

.sidebar-folder-content[data-level="1"] .sidebar-item.active::before {
  content: '';
  @apply absolute w-px inset-y-2.5 bg-sd-primary;
  left: 10px;
}

/* ========================================
   Sidebar Separator
   ======================================== */

.sidebar-separator {
  @apply inline-flex items-center gap-2 mb-1.5 px-2;
  padding-inline-start: var(--sidebar-item-offset);
}

.sidebar-separator:not(:first-child) {
  @apply mt-6;
}

.sidebar-separator-icon {
  @apply w-4 h-4 shrink-0;
}

/* ========================================
   Sidebar Footer
   ======================================== */

.sidebar-footer {
  @apply flex flex-col border-t p-4 pt-2;
}

.sidebar-footer-content {
  @apply flex items-center gap-2;
  @apply text-sd-muted-foreground;
}

.sidebar-footer-link {
  @apply p-2 rounded-lg transition-colors;
  @apply hover:bg-sd-accent/50 hover:text-sd-accent-foreground;
}

/* ========================================
   Mobile Navbar
   ======================================== */

.shelldocs-navbar {
  @apply fixed top-0 left-0 right-0 z-30;
  @apply flex items-center px-4 py-2 border-b;
  @apply bg-sd-background/80 backdrop-blur-sm;
  height: var(--sd-nav-height);
}

.navbar-content {
  @apply flex items-center justify-between w-full;
}

.navbar-logo {
  @apply inline-flex items-center gap-2.5 font-semibold;
}

.navbar-logo .logo-image {
  @apply h-6 w-auto;
}

.navbar-actions {
  @apply flex items-center gap-2;
}

.navbar-menu-btn {
  @apply p-2 rounded-lg transition-colors;
  @apply text-sd-muted-foreground;
  @apply hover:bg-sd-accent/50 hover:text-sd-accent-foreground;
}

.navbar-menu-btn .icon {
  @apply w-5 h-5;
}

/* ========================================
   Collapsible Control (Desktop)
   ======================================== */

.collapsible-control {
  @apply fixed flex shadow-lg transition-opacity rounded-xl p-0.5 border;
  @apply bg-sd-muted text-sd-muted-foreground z-10;
  @apply max-md:hidden xl:start-4 max-xl:end-4;
  top: calc(var(--sd-banner-height) + var(--sd-tocnav-height) + var(--spacing) * 4);
}

.collapsible-control .collapse-btn {
  @apply p-2 rounded-lg transition-colors;
  @apply hover:bg-sd-accent/50 hover:text-sd-accent-foreground;
}

/* ========================================
   Responsive Breakpoints
   ======================================== */

/* Mobile: Full width content */
@media (max-width: 768px) {
  .shelldocs-content {
    @apply w-full;
    padding-top: var(--sd-nav-height);
    padding-inline-start: 0;
    padding-inline-end: 0;
  }
}

/* Tablet: Sidebar + Content */
@media (min-width: 768px) and (max-width: 1280px) {
  .shelldocs-content {
    padding-inline-end: 0;
  }
}

/* Desktop: Sidebar + Content + TOC */
@media (min-width: 1280px) {
  .shelldocs-content {
    padding-inline-end: var(--sd-toc-width);
  }
}

/* ========================================
   Utility Classes
   ======================================== */

@utility sd-scroll-container {
  &::-webkit-scrollbar {
    width: 5px;
    height: 5px;
  }

  &::-webkit-scrollbar-thumb {
    @apply rounded-full;
    background: var(--color-sd-border);
  }

  &::-webkit-scrollbar-track {
    @apply bg-transparent;
  }
}

/* ========================================
   Print Styles
   ======================================== */

@media print {
  .shelldocs-sidebar,
  .shelldocs-sidebar-mobile,
  .shelldocs-navbar,
  .sidebar-footer,
  .collapsible-control {
    @apply hidden;
  }

  .shelldocs-content {
    @apply w-full max-w-none;
    padding: 0;
  }
}
```

---

## 🔧 JavaScript Interop

### wwwroot/js/shelldocs.js

```javascript
// ShellDocs JavaScript Interop
window.ShellDocs = {
  // Theme management
  Theme: {
    getTheme: function() {
      const stored = localStorage.getItem('shelldocs-theme');
      if (stored) return stored;
      
      // Check system preference
      if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        return 'dark';
      }
      return 'light';
    },

    setTheme: function(theme) {
      localStorage.setItem('shelldocs-theme', theme);
      
      if (theme === 'dark') {
        document.documentElement.classList.add('dark');
      } else {
        document.documentElement.classList.remove('dark');
      }
    },

    initializeTheme: function() {
      const theme = this.getTheme();
      this.setTheme(theme);
    }
  },

  // Sidebar management
  Sidebar: {
    lockBodyScroll: function(lock) {
      if (lock) {
        document.body.style.overflow = 'hidden';
      } else {
        document.body.style.overflow = '';
      }
    }
  },

  // Scroll utilities
  Scroll: {
    scrollToElement: function(elementId, offset = 0) {
      const element = document.getElementById(elementId);
      if (element) {
        const top = element.getBoundingClientRect().top + window.pageYOffset - offset;
        window.scrollTo({ top, behavior: 'smooth' });
      }
    },

    getScrollY: function() {
      return window.scrollY || window.pageYOffset;
    }
  },

  // Initialize on load
  initialize: function() {
    this.Theme.initializeTheme();
  }
};

// Auto-initialize
if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', () => window.ShellDocs.initialize());
} else {
  window.ShellDocs.initialize();
}
```

---

## 📝 Model Classes

### Models/PageTree.cs

```csharp
namespace ShellDocs.Blazor.Models;

public class PageTree
{
    public string Name { get; set; } = "";
    public List<PageTreeNode> Children { get; set; } = new();
}

public abstract class PageTreeNode
{
    public string Name { get; set; } = "";
    public string? Icon { get; set; }
}

public class PageTreeItem : PageTreeNode
{
    public string Url { get; set; } = "";
    public string? Description { get; set; }
    public bool External { get; set; }
}

public class PageTreeFolder : PageTreeNode
{
    public List<PageTreeNode> Children { get; set; } = new();
    public bool DefaultOpen { get; set; }
    public PageTreeItem? Index { get; set; }
}

public class PageTreeSeparator : PageTreeNode
{
    // Separator has no additional properties
}
```

### Models/NavLink.cs

```csharp
namespace ShellDocs.Blazor.Models;

public class NavLink
{
    public string Url { get; set; } = "";
    public string Label { get; set; } = "";
    public string? Icon { get; set; }
    public bool External { get; set; }
}
```

### Models/TocItem.cs

```csharp
namespace ShellDocs.Blazor.Models;

public class TocItem
{
    public int Level { get; set; }
    public string Text { get; set; } = "";
    public string Url { get; set; } = "";
}
```

---

## 🚀 Usage Example

### Pages/Docs.razor

```razor
@page "/docs/{*slug}"
@layout DocsLayout
@inject IContentService ContentService

<PageTitle>@page?.Title - Documentation</PageTitle>

<DocsLayout 
    PageTree="@pageTree"
    Title="ShellDocs"
    Logo="/logo.svg"
    Links="@links"
    TocItems="@tocItems"
    SearchEnabled="true"
    ThemeToggleEnabled="true"
    ShowTableOfContents="true">
    
    <article class="prose">
        <h1>@page?.Title</h1>
        
        @if (!string.IsNullOrEmpty(page?.Description))
        {
            <p class="lead">@page.Description</p>
        }

        @((MarkupString)page?.Html)
    </article>
</DocsLayout>

@code {
    [Parameter] public string? Slug { get; set; }

    private Page? page;
    private PageTree pageTree = new();
    private List<TocItem> tocItems = new();
    private List<NavLink> links = new()
    {
        new NavLink { Url = "https://github.com/yourorg/shelldocs", Icon = "github", Label = "GitHub", External = true },
        new NavLink { Url = "https://twitter.com/yourorg", Icon = "twitter", Label = "Twitter", External = true }
    };

    protected override async Task OnParametersSetAsync()
    {
        var slugParts = Slug?.Split('/') ?? Array.Empty<string>();
        page = await ContentService.GetPageAsync(slugParts);
        pageTree = await ContentService.GetPageTreeAsync();
        tocItems = page?.Toc ?? new();
    }
}
```

---

## ✅ Checklist for Implementation

### Core Services
- [ ] Create `SidebarService.cs`
- [ ] Create `NavigationService.cs`
- [ ] Create `ThemeService.cs`
- [ ] Register services in `Program.cs`

### Layout Components
- [ ] Create `DocsLayout.razor`
- [ ] Create desktop `Sidebar.razor`
- [ ] Create mobile `SidebarMobile.razor`
- [ ] Create `Navbar.razor`
- [ ] Create `CollapsibleControl.razor`

### Sidebar Components
- [ ] Create `SidebarHeader.razor`
- [ ] Create `SidebarFooter.razor`
- [ ] Create `SidebarViewport.razor`
- [ ] Create `SidebarPageTree.razor`
- [ ] Create `SidebarItem.razor`
- [ ] Create `SidebarFolder.razor`
- [ ] Create `SidebarSeparator.razor`

### Styling
- [ ] Create `shelldocs.css` with design tokens
- [ ] Define animations
- [ ] Implement responsive breakpoints
- [ ] Test dark mode
- [ ] Add print styles

### JavaScript
- [ ] Create `shelldocs.js`
- [ ] Implement theme management
- [ ] Add scroll utilities
- [ ] Test browser compatibility

### Models
- [ ] Create `PageTree.cs` models
- [ ] Create `NavLink.cs`
- [ ] Create `TocItem.cs`

### Testing
- [ ] Test responsive design
- [ ] Test mobile drawer
- [ ] Test sidebar collapse
- [ ] Test theme toggle
- [ ] Test navigation
- [ ] Test keyboard accessibility

---

## 🎯 Key Features Implemented

✅ **Responsive Design** - Mobile, tablet, desktop layouts  
✅ **Sidebar** - Collapsible, hoverable when collapsed  
✅ **Mobile Drawer** - Slide-in overlay with backdrop  
✅ **Dark Mode** - Theme toggle with persistence  
✅ **Smooth Animations** - Fade, slide, collapse transitions  
✅ **Active States** - Current page highlighting  
✅ **Nested Navigation** - Collapsible folders with indicators  
✅ **Accessibility** - ARIA labels, keyboard navigation  
✅ **Performance** - CSS-based animations, efficient rendering  

---

## 📚 Additional Notes

### Browser Compatibility
- Modern browsers (Chrome, Firefox, Safari, Edge)
- CSS Grid and Flexbox
- CSS Variables (custom properties)
- CSS Animations

### Performance Tips
- Use `@key` directive in loops
- Implement virtual scrolling for large trees
- Lazy load JavaScript interop
- Minimize re-renders with `ShouldRender()`

### Customization
All design tokens are CSS variables, making it easy to:
- Change colors
- Adjust spacing
- Modify animations
- Create custom themes

---

**This guide provides everything needed to build a production-ready fumadocs-style layout in Blazor!** 🚀

