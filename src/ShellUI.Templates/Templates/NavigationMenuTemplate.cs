using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class NavigationMenuTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "navigation-menu",
        DisplayName = "Navigation Menu",
        Description = "Navigation menu component",
        Category = ComponentCategory.Navigation,
        FilePath = "NavigationMenu.razor",
        Version = "0.1.0",
        Tags = new List<string> { "navigation", "menu", "nav" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<nav class=""@(""flex items-center space-x-1 "" + ClassName)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</nav>

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

