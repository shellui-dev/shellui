using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NavListTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "nav-list",
        DisplayName = "Nav List",
        Description = "List container for NavigationMenu items",
        Category = ComponentCategory.Navigation,
        FilePath = "NavList.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "nav", "list" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""group flex flex-1 list-none items-center justify-center space-x-1"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [CascadingParameter] public NavigationMenu? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
