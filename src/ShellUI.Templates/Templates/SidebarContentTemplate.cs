using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-content",
        DisplayName = "Sidebar Content",
        Description = "Scrollable content area of the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarContent.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div data-sidebar=""content""
     class=""@Shell.Cn(""flex min-h-0 flex-1 flex-col gap-2 overflow-auto group-data-[collapsible=icon]:overflow-hidden"", Class)""
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
