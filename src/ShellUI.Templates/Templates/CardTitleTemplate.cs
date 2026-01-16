using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardTitleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card-title",
        DisplayName = "Card Title",
        Description = "Title for the Card component",
        Category = ComponentCategory.Layout,

        FilePath = "CardTitle.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<h3 class=""@Shell.Cn(""text-2xl font-semibold leading-none tracking-tight"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</h3>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}


