using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabsListTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tabs-list",
        DisplayName = "Tabs List",
        Description = "Container for tab triggers",
        Category = ComponentCategory.Navigation,

        FilePath = "TabsList.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""inline-flex h-9 w-fit items-center justify-center rounded-lg bg-muted p-[3px] text-muted-foreground"", Class)"" @attributes=""AdditionalAttributes"">
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
