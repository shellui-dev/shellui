using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NavbarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "navbar",
        DisplayName = "Navbar",
        Description = "Top navigation bar with backdrop blur",
        Category = ComponentCategory.Layout,

        FilePath = "Navbar.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<nav class=""sticky top-0 z-50 w-full border-b border-border bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60 @ClassName"" @attributes=""AdditionalAttributes"">
    <div class=""container flex h-14 items-center"">
        @ChildContent
    </div>
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


