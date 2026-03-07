using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card",
        DisplayName = "Card",
        Description = "Container component for grouping related content",
        Category = ComponentCategory.Layout,

        FilePath = "Card.razor",
        Dependencies = new List<string> { "card-header", "card-title", "card-description", "card-content", "card-footer" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""rounded-lg border border-border bg-card text-card-foreground shadow-sm"", Class)"" @attributes=""AdditionalAttributes"">
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

