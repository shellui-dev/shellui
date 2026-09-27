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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<CascadingValue Value=""this"" IsFixed=""true"">
    <ul role=""tree"" class=""@Shell.Cn(""space-y-0.5 text-sm"", Class)"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </ul>
</CascadingValue>

@code {
    [Parameter] public string? SelectedValue { get; set; }
    [Parameter] public EventCallback<string?> SelectedValueChanged { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private readonly List<TreeViewItem> _items = new();

    internal void Register(TreeViewItem item) => _items.Add(item);
    internal void Unregister(TreeViewItem item) => _items.Remove(item);

    public async Task SelectAsync(string value)
    {
        SelectedValue = value;
        await SelectedValueChanged.InvokeAsync(value);
        foreach (var item in _items.ToArray()) item.Refresh();
    }
}
";
}
