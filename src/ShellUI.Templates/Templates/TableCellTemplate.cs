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
        IsAvailable = false,
        Tags = new List<string> { "table", "cell", "td" }
    };

    public static string Content => ComponentSource.Read("Components/TableCell.razor");
}


