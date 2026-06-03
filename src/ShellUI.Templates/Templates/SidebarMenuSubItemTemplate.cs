using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuSubItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-sub-item",
        DisplayName = "Sidebar Menu Sub Item",
        Description = "Individual item in a sidebar sub-menu",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuSubItem.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<li data-sidebar=""menu-sub-item""
    class=""@Class""
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
