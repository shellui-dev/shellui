using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tabs",
        DisplayName = "Tabs",
        Description = "Tabbed interface for organizing content",
        Category = ComponentCategory.Layout,
        Version = "0.1.0",
        FilePath = "Tabs.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@ClassName"" @attributes=""AdditionalAttributes"">
    <div class=""inline-flex h-10 items-center justify-center rounded-md bg-muted p-1 text-muted-foreground"">
        @TabHeaders
    </div>
    <div class=""mt-2"">
        @TabContent
    </div>
</div>

@code {
    [Parameter]
    public RenderFragment? TabHeaders { get; set; }

    [Parameter]
    public RenderFragment? TabContent { get; set; }

    [Parameter]
    public string ClassName { get; set; } = """";

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

