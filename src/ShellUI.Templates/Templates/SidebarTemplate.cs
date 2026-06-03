using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar",
        DisplayName = "Sidebar",
        Description = "Dashboard sidebar with collapsible icon mode, mobile support, and inset variant (shadcn sidebar-08)",
        Category = ComponentCategory.Layout,

        FilePath = "Sidebar.razor",
        Dependencies = new List<string>
        {
            "shell", "sidebar-models", "sidebar-js", "sidebar-provider",
            "sidebar-header", "sidebar-content", "sidebar-footer",
            "sidebar-group", "sidebar-group-label", "sidebar-group-content",
            "sidebar-menu", "sidebar-menu-item", "sidebar-menu-button",
            "sidebar-menu-sub", "sidebar-menu-sub-item", "sidebar-menu-sub-button",
            "sidebar-menu-action", "sidebar-menu-badge",
            "sidebar-separator", "sidebar-trigger", "sidebar-inset", "sidebar-rail"
        },
        Tags = new List<string> { "sidebar", "navigation", "layout", "dashboard" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@{
    var variantValue = Variant.ToString().ToLowerInvariant();
    var sideValue = Side.ToString().ToLowerInvariant();
    var state = SidebarProvider?.State ?? ""expanded"";
    var isCollapsed = state == ""collapsed"";
    var collapsibleAttr = isCollapsed ? Collapsible.ToString().ToLowerInvariant() : """";
}

@if (Collapsible == SidebarCollapsible.None)
{
    <div class=""@Shell.Cn(""flex h-full flex-col bg-sidebar text-sidebar-foreground"", Class)""
         style=""width: var(--sidebar-width);""
         data-sidebar=""sidebar""
         @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}
else if (SidebarProvider?.IsMobile == true)
{
    @if (SidebarProvider.MobileOpen)
    {
        <!-- Mobile overlay backdrop -->
        <div class=""fixed inset-0 z-50 bg-black/80 animate-in fade-in-0""
             @onclick=""SidebarProvider.CloseMobileSidebar""></div>

        <!-- Mobile sidebar sheet -->
        <div class=""@Shell.Cn(
                 ""fixed inset-y-0 z-50 flex h-svh flex-col bg-sidebar text-sidebar-foreground shadow-lg transition-transform duration-200"",
                 Side == SidebarSide.Left ? ""left-0"" : ""right-0"",
                 Class)""
             style=""width: var(--sidebar-width);""
             data-sidebar=""sidebar""
             data-mobile=""true""
             @attributes=""AdditionalAttributes"">
            <div class=""flex h-full w-full flex-col"">
                @ChildContent
            </div>
        </div>
    }
}
else
{
    <!-- Desktop sidebar -->
    <div class=""group peer hidden text-sidebar-foreground md:block""
         data-state=""@state""
         data-collapsible=""@collapsibleAttr""
         data-variant=""@variantValue""
         data-side=""@sideValue"">

        <!-- Gap element that reserves space in flex layout -->
        <div class=""@GapClass"" style=""@GapStyle""></div>

        <!-- Fixed sidebar container -->
        <div class=""@FixedClass"" style=""@FixedStyle"" @attributes=""AdditionalAttributes"">
            <div data-sidebar=""sidebar""
                 class=""flex h-full w-full flex-col bg-sidebar group-data-[variant=floating]:rounded-lg group-data-[variant=floating]:border group-data-[variant=floating]:border-sidebar-border group-data-[variant=floating]:shadow"">
                @ChildContent
            </div>
        </div>
    </div>
}

@code {
    [CascadingParameter] public SidebarProvider? SidebarProvider { get; set; }

    [Parameter] public SidebarSide Side { get; set; } = SidebarSide.Left;
    [Parameter] public SidebarVariant Variant { get; set; } = SidebarVariant.Sidebar;
    [Parameter] public SidebarCollapsible Collapsible { get; set; } = SidebarCollapsible.Offcanvas;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool IsFloatingOrInset => Variant == SidebarVariant.Floating || Variant == SidebarVariant.Inset;

    // Inline styles for CSS variable widths (reliable across Tailwind versions)
    private string GapStyle
    {
        get
        {
            var state = SidebarProvider?.State ?? ""expanded"";
            var isCollapsed = state == ""collapsed"";

            if (isCollapsed && Collapsible == SidebarCollapsible.Offcanvas)
                return ""width: 0;"";
            if (isCollapsed && Collapsible == SidebarCollapsible.Icon)
                return IsFloatingOrInset
                    ? ""width: calc(var(--sidebar-width-icon) + 1rem);""
                    : ""width: var(--sidebar-width-icon);"";
            return ""width: var(--sidebar-width);"";
        }
    }

    private string FixedStyle
    {
        get
        {
            var state = SidebarProvider?.State ?? ""expanded"";
            var isCollapsed = state == ""collapsed"";
            var parts = new List<string>();

            if (isCollapsed && Collapsible == SidebarCollapsible.Icon)
                parts.Add(IsFloatingOrInset
                    ? ""width: calc(var(--sidebar-width-icon) + 1rem + 2px)""
                    : ""width: var(--sidebar-width-icon)"");
            else
                parts.Add(""width: var(--sidebar-width)"");

            if (Side == SidebarSide.Left)
            {
                parts.Add(isCollapsed && Collapsible == SidebarCollapsible.Offcanvas
                    ? ""left: calc(var(--sidebar-width) * -1)""
                    : ""left: 0"");
            }
            else
            {
                parts.Add(isCollapsed && Collapsible == SidebarCollapsible.Offcanvas
                    ? ""right: calc(var(--sidebar-width) * -1)""
                    : ""right: 0"");
            }

            return string.Join(""; "", parts) + "";"";
        }
    }

    private string GapClass => Shell.Cn(
        ""relative h-svh bg-transparent transition-[width] ease-linear duration-200"",
        Side == SidebarSide.Right ? ""rotate-180"" : null
    );

    private string FixedClass => Shell.Cn(
        ""fixed inset-y-0 z-10 hidden h-svh transition-[left,right,width] ease-linear duration-200 md:flex"",
        IsFloatingOrInset ? ""p-2"" : null,
        !IsFloatingOrInset && Side == SidebarSide.Left ? ""border-r"" : null,
        !IsFloatingOrInset && Side == SidebarSide.Right ? ""border-l"" : null,
        Class
    );
}
";
}


