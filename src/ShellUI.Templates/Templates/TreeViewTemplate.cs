using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TreeViewTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tree-view",
        DisplayName = "Tree View",
        Description = "Expandable, selectable hierarchical tree",
        Category = ComponentCategory.Navigation,
        FilePath = "TreeView.razor",
        Dependencies = new List<string> { "tree-view-item" },
        Tags = new List<string> { "tree", "hierarchy", "navigation" }
    };

    public static string Content => ComponentSource.Read("Components/TreeView.razor");
}
