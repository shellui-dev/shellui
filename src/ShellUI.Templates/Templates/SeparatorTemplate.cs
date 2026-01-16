using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SeparatorTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "separator",
        DisplayName = "Separator",
        Description = "Horizontal or vertical divider line",
        Category = ComponentCategory.Layout,

        FilePath = "Separator.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""shrink-0 bg-border @(Orientation == ""vertical"" ? ""h-full w-[1px]"" : ""h-[1px] w-full"") @ClassName"" @attributes=""AdditionalAttributes""></div>

@code {
    [Parameter]
    public string Orientation { get; set; } = ""horizontal"";

    [Parameter]
    public string ClassName { get; set; } = """";

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}


