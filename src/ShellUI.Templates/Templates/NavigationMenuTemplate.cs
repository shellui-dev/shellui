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

        Tags = new List<string> { "navigation", "menu", "nav" },
        Dependencies = new List<string> { "navigation-menu-item", "nav-list", "nav-item", "nav-trigger", "nav-content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<CascadingValue Value=""this"" IsFixed=""true"">
    <nav class=""@Shell.Cn(""relative z-10 flex max-w-max flex-1 items-center justify-center"", ClassName, Class)"" @attributes=""AdditionalAttributes"">
        @if (UseNavList)
        {
            @ChildContent
        }
        else
        {
            <div class=""group flex flex-1 list-none items-center justify-center space-x-1"">
                @ChildContent
            </div>
        }
    </nav>
</CascadingValue>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    // Deprecated: use Class.
    [Parameter] public string ClassName { get; set; } = """";
    [Parameter] public bool UseNavList { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}


