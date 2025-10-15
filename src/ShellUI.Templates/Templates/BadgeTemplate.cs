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

<span class=""inline-flex items-center rounded-full border border-border px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 @(Variant == ""secondary"" ? ""border-transparent bg-secondary text-secondary-foreground hover:bg-secondary/80"" : Variant == ""destructive"" ? ""border-transparent bg-destructive text-destructive-foreground hover:bg-destructive/80"" : Variant == ""outline"" ? ""text-foreground"" : ""border-transparent bg-primary text-primary-foreground hover:bg-primary/80"")"">
    @ChildContent
</span>

@code {
    [Parameter]
    public string Variant { get; set; } = ""default"";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
";
}

