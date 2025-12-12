using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card-description",
        DisplayName = "Card Description",
        Description = "Description for the Card component",
        Category = ComponentCategory.Layout,
        Version = "0.1.0",
        FilePath = "CardDescription.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace ShellUI.Components

<p class=""@Shell.Cn(""text-sm text-muted-foreground"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</p>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

