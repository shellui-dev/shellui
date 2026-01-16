using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class TableRowTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "table-row",
        DisplayName = "Table Row",
        Description = "Table row component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "TableRow.razor",

        Tags = new List<string> { "table", "row", "tr" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<tr class=""border-b border-border transition-colors hover:bg-muted/50 data-[state=selected]:bg-muted @ClassName"" @attributes=""AdditionalAttributes"">
    @ChildContent
</tr>

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


