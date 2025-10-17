using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class TableTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "table",
        DisplayName = "Table",
        Description = "Data table component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Table.razor",
        Version = "0.1.0",
        Tags = new List<string> { "table", "data", "grid", "display" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""relative w-full overflow-auto @ClassName"" @attributes=""AdditionalAttributes"">
    <table class=""w-full caption-bottom text-sm"">
        @ChildContent
    </table>
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

