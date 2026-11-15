using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class TableHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "table-header",
        DisplayName = "Table Header",
        Description = "Table header wrapper component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "TableHeader.razor",
        IsAvailable = false,
        Tags = new List<string> { "table", "header", "thead" }
    };

    public static string Content => ComponentSource.Read("Components/TableHeader.razor");
}


