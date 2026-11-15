using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TreeViewItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tree-view-item",
        DisplayName = "Tree View Item",
        Description = "Node for TreeView",
        Category = ComponentCategory.Navigation,
        FilePath = "TreeViewItem.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "tree" }
    };

    public static string Content => ComponentSource.Read("Components/TreeViewItem.razor");
}
