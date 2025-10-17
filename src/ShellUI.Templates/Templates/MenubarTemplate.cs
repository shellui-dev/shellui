using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar",
        DisplayName = "Menubar",
        Description = "Application menubar component",
        Category = ComponentCategory.Navigation,
        FilePath = "Menubar.razor",
        Version = "0.1.0",
        Tags = new List<string> { "navigation", "menu", "menubar", "app" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@(""flex h-10 items-center space-x-1 rounded-md border bg-background p-1 "" + ClassName)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

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

