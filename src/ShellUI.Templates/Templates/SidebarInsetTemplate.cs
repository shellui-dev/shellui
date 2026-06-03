using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarInsetTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-inset",
        DisplayName = "Sidebar Inset",
        Description = "Main content wrapper that adjusts layout based on sidebar variant",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarInset.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<main class=""@Shell.Cn(
         ""relative flex min-h-svh flex-1 flex-col bg-background"",
         ""peer-data-[variant=inset]:min-h-[calc(100svh-1rem)] md:peer-data-[variant=inset]:m-2 md:peer-data-[variant=inset]:ml-0 md:peer-data-[variant=inset]:rounded-xl md:peer-data-[variant=inset]:shadow"",
         Class)""
     @attributes=""AdditionalAttributes"">
    @ChildContent
</main>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
