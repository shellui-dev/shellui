using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class TableCellTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "table-cell",
        DisplayName = "Table Cell",
        Description = "Table data cell component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "TableCell.razor",

        Tags = new List<string> { "table", "cell", "td" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<td class=""p-4 align-middle [&:has([role=checkbox])]:pr-0 @ClassName"" @attributes=""AdditionalAttributes"">
    @ChildContent
</td>

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


