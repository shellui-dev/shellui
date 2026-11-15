using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class TableBodyTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "table-body",
        DisplayName = "Table Body",
        Description = "Table body wrapper component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "TableBody.razor",
        IsAvailable = false,
        Tags = new List<string> { "table", "body", "tbody" }
    };

    public static string Content => ComponentSource.Read("Components/TableBody.razor");
}


