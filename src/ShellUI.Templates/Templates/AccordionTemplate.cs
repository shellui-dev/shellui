using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AccordionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "accordion",
        DisplayName = "Accordion",
        Description = "Collapsible content sections",
        Category = ComponentCategory.Layout,
        FilePath = "Accordion.razor",
        Version = "0.1.0",
        Tags = new List<string> { "layout", "collapsible", "accordion" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@ClassName"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private List<AccordionItem> _items = new();
    
    public void RegisterItem(AccordionItem item)
    {
        _items.Add(item);
    }
    
    public void UnregisterItem(AccordionItem item)
    {
        _items.Remove(item);
    }
}
";
}

