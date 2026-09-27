using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AspectRatioTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "aspect-ratio",
        DisplayName = "Aspect Ratio",
        Description = "Container that keeps its content at a fixed aspect ratio",
        Category = ComponentCategory.Layout,
        FilePath = "AspectRatio.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "layout", "media", "ratio" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""relative w-full overflow-hidden [&>*]:absolute [&>*]:inset-0 [&>*]:h-full [&>*]:w-full"", Class)"" style=""aspect-ratio: @RatioCss"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [Parameter] public double Ratio { get; set; } = 16d / 9d;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string RatioCss => Ratio.ToString(System.Globalization.CultureInfo.InvariantCulture);
}
";
}
