using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-footer",
        DisplayName = "Sidebar Footer",
        Description = "Footer section of the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarFooter.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div data-sidebar=""footer""
     class=""@Shell.Cn(""flex flex-col gap-2 p-2"", Class)""
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
