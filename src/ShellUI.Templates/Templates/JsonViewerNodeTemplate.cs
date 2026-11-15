using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class JsonViewerNodeTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "json-viewer-node",
        DisplayName = "JSON Viewer Node",
        Description = "One node of the JSON tree",
        Category = ComponentCategory.DataDisplay,
        FilePath = "JsonViewerNode.razor",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/JsonViewerNode.razor");
}
