using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CollapsibleContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "collapsible-content",
        DisplayName = "Collapsible Content",
        Description = "Content container for Collapsible component",
        Category = ComponentCategory.Layout,
        FilePath = "CollapsibleContent.razor",
        Dependencies = new List<string>(),
        IsAvailable = false,
        Tags = new List<string> { "layout", "collapsible", "content", "body" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (Parent?.IsOpen == true)
{
    <div class=""overflow-hidden transition-all"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] public Collapsible? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
";
}
