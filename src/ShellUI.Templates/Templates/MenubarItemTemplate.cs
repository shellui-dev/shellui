using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-item",
        DisplayName = "Menubar Item",
        Description = "Individual menubar item",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarItem.razor",
        Version = "0.1.0",
        Tags = new List<string> { "navigation", "menu", "menubar", "item" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<button type=""button""
        @onclick=""OnClick""
        disabled=""@Disabled""
        class=""@(""inline-flex items-center rounded-sm px-3 py-1.5 text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring "" + (Disabled ? ""opacity-50 cursor-not-allowed"" : ""hover:bg-accent hover:text-accent-foreground"") + "" "" + ClassName)""
        @attributes=""AdditionalAttributes"">
    @ChildContent
</button>

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }
    
    [Parameter]
    public bool Disabled { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

