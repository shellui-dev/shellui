using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-item",
        DisplayName = "Sidebar Menu Item",
        Description = "Individual menu item in the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuItem.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<li data-sidebar=""menu-item""
    class=""@Shell.Cn(""group/menu-item relative"", Class)""
    @attributes=""AdditionalAttributes"">
    @ChildContent
</li>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
