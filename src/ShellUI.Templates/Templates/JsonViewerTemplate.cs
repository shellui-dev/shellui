using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class JsonViewerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "json-viewer",
        DisplayName = "JSON Viewer",
        Description = "Collapsible, highlighted JSON tree with copy",
        Category = ComponentCategory.DataDisplay,
        FilePath = "JsonViewer.razor",
        Dependencies = new List<string> { "json-viewer-node", "copy-button" },
        Tags = new List<string> { "json", "tree", "viewer", "code" }
    };

    public static string Content => ComponentSource.Read("Components/JsonViewer.razor");
}
