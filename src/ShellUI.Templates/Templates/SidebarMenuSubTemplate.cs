using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuSubTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-sub",
        DisplayName = "Sidebar Menu Sub",
        Description = "Nested sub-menu container for sidebar items",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuSub.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<ul data-sidebar=""menu-sub""
    class=""@Shell.Cn(
        ""mx-3.5 flex min-w-0 translate-x-px flex-col gap-1 border-l border-sidebar-border px-2.5 py-0.5"",
        ""group-data-[collapsible=icon]:hidden"",
        Class)""
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
