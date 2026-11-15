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
        IsAvailable = false,
        Tags = new List<string> { "table", "row", "tr" }
    };

    public static string Content => ComponentSource.Read("Components/TableRow.razor");
}


