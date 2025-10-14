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
        Version = "0.1.0",
        FilePath = "Card.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@CssClass"" @attributes=""AdditionalAttributes"">
    @if (Header != null)
    {
        <div class=""flex flex-col space-y-1.5 p-6"">
            @Header
        </div>
    }

    <div class=""p-6 pt-0"">
        @ChildContent
    </div>

    @if (Footer != null)
    {
        <div class=""flex items-center p-6 pt-0"">
            @Footer
        </div>
    }
</div>

@code {
    [Parameter] public RenderFragment? Header { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string CssClass => BuildCssClass();

    private string BuildCssClass()
    {
        var classes = new List<string>
        {
            ""rounded-lg border bg-card text-card-foreground shadow-sm""
        };

        return string.Join("" "", classes);
    }
}
";
}

