using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DataTableModelsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "data-table-models",
        DisplayName = "DataTable Models",
        Description = "Models for DataTable component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Models/DataTableModels.cs",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Models/DataTableModels.cs");
}


