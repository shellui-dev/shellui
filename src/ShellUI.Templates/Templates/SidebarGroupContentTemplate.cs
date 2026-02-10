using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarGroupContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-group-content",
        DisplayName = "Sidebar Group Content",
        Description = "Content wrapper for a sidebar navigation group",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarGroupContent.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div data-sidebar=""group-content""
     class=""@Shell.Cn(""w-full text-sm"", Class)""
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
