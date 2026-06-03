using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-header",
        DisplayName = "Sidebar Header",
        Description = "Header section of the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarHeader.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div data-sidebar=""header""
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
