using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuBadgeTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-badge",
        DisplayName = "Sidebar Menu Badge",
        Description = "Notification badge for sidebar menu items",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuBadge.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div data-sidebar=""menu-badge""
     class=""@Shell.Cn(
         ""pointer-events-none absolute right-1 flex min-w-5 select-none items-center justify-center rounded-md px-1 text-xs font-medium tabular-nums text-sidebar-foreground"",
         ""peer-hover/menu-button:text-sidebar-accent-foreground peer-data-[active=true]/menu-button:text-sidebar-accent-foreground"",
         ""group-data-[collapsible=icon]:hidden"",
         Class)""
     @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
