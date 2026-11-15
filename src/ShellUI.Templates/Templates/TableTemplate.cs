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

        Tags = new List<string> { "table", "data", "grid", "display" },
        Dependencies = new List<string> { "table-header", "table-body", "table-row", "table-cell", "table-head" }
    };

    public static string Content => ComponentSource.Read("Components/Table.razor");
}


