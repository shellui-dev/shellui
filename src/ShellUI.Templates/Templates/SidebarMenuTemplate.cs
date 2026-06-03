using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu",
        DisplayName = "Sidebar Menu",
        Description = "Menu list container for sidebar navigation items",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenu.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<ul data-sidebar=""menu""
    class=""@Shell.Cn(""flex w-full min-w-0 flex-col gap-1"", Class)""
    @attributes=""AdditionalAttributes"">
    @ChildContent
</ul>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
