using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class BadgeTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "badge",
        DisplayName = "Badge",
        Description = "Small status indicator component",
        Category = ComponentCategory.DataDisplay,
        Version = "0.1.0",
        FilePath = "Badge.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@($""inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 {(Variant.ToLower() == ""secondary"" ? ""border-transparent bg-secondary text-secondary-foreground hover:bg-secondary/80"" : Variant.ToLower() == ""destructive"" ? ""border-transparent bg-destructive text-destructive-foreground hover:bg-destructive/80"" : Variant.ToLower() == ""outline"" ? ""text-foreground"" : Variant.ToLower() == ""success"" ? ""border-transparent bg-green-500 text-white hover:bg-green-600"" : Variant.ToLower() == ""warning"" ? ""border-transparent bg-yellow-500 text-white hover:bg-yellow-600"" : Variant.ToLower() == ""info"" ? ""border-transparent bg-blue-500 text-white hover:bg-blue-600"" : ""border-transparent bg-primary text-primary-foreground hover:bg-primary/80"")} {Class}"")"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [Parameter]
    public string Variant { get; set; } = ""default"";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string Class { get; set; } = string.Empty;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

