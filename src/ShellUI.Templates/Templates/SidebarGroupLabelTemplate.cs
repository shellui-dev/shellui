using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarGroupLabelTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-group-label",
        DisplayName = "Sidebar Group Label",
        Description = "Label for a sidebar navigation group",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarGroupLabel.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div data-sidebar=""group-label""
     class=""@Shell.Cn(
         ""flex h-8 shrink-0 items-center rounded-md px-2 text-xs font-medium text-sidebar-foreground/70 outline-none ring-sidebar-ring transition-[margin,opacity] duration-200 ease-linear focus-visible:ring-2 [&>svg]:size-4 [&>svg]:shrink-0"",
         ""group-data-[collapsible=icon]:-mt-8 group-data-[collapsible=icon]:opacity-0"",
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
