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

<div class=""rounded-lg border border-border bg-card text-card-foreground shadow-sm"">
    @if (Header != null)
    {
        <div class=""flex flex-col space-y-1.5 p-6"">
            @Header
        </div>
    }
    
    @if (ChildContent != null)
    {
        <div class=""p-6 pt-0"">
            @ChildContent
        </div>
    }
    
    @if (Footer != null)
    {
        <div class=""flex items-center p-6 pt-0"">
            @Footer
        </div>
    }
</div>

@code {
    [Parameter]
    public RenderFragment? Header { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public RenderFragment? Footer { get; set; }
}
";
}

