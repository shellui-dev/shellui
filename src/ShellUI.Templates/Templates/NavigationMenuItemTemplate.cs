using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class NavigationMenuItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "navigation-menu-item",
        DisplayName = "Navigation Menu Item",
        Description = "Individual navigation menu item",
        Category = ComponentCategory.Navigation,
        FilePath = "NavigationMenuItem.razor",
        Version = "0.1.0",
        Tags = new List<string> { "navigation", "menu", "nav", "item" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (!string.IsNullOrEmpty(Href))
{
    <a href=""@Href""
       class=""@(""inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring h-9 px-4 py-2 "" + (IsActive ? ""bg-accent text-accent-foreground"" : ""hover:bg-accent hover:text-accent-foreground"") + "" "" + ClassName)""
       @attributes=""AdditionalAttributes"">
        @ChildContent
    </a>
}
else
{
    <button type=""button""
            @onclick=""OnClick""
            disabled=""@Disabled""
            class=""@(""inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring h-9 px-4 py-2 "" + (Disabled ? ""opacity-50 cursor-not-allowed"" : """") + "" "" + (IsActive ? ""bg-accent text-accent-foreground"" : ""hover:bg-accent hover:text-accent-foreground"") + "" "" + ClassName)""
            @attributes=""AdditionalAttributes"">
        @ChildContent
    </button>
}

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string? Href { get; set; }
    
    [Parameter]
    public bool IsActive { get; set; }
    
    [Parameter]
    public bool Disabled { get; set; }
    
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

