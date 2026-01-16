using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar",
        DisplayName = "Sidebar",
        Description = "Side navigation panel with toggle",
        Category = ComponentCategory.Layout,

        FilePath = "Sidebar.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<aside class=""fixed left-0 top-0 z-40 h-screen w-64 border-r border-border bg-background transition-transform @(IsOpen ? ""translate-x-0"" : ""-translate-x-full"") @ClassName"" @attributes=""AdditionalAttributes"">
    <div class=""h-full overflow-y-auto px-3 py-4"">
        @ChildContent
    </div>
</aside>

@code {
    [Parameter]
    public bool IsOpen { get; set; } = true;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string ClassName { get; set; } = """";

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}


