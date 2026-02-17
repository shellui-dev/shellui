using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabsContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tabs-content",
        DisplayName = "Tabs Content",
        Description = "Tab content panel",
        Category = ComponentCategory.Navigation,

        FilePath = "TabsContent.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (ParentTabs?.CurrentValue == Value)
{
    <div class=""@Shell.Cn(""mt-2 ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 focus-visible:ring-offset-background"", Class)"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] private Tabs? ParentTabs { get; set; }
    [Parameter] public string Value { get; set; } = """";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
