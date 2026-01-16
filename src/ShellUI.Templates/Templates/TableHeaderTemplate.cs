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

        Tags = new List<string> { "table", "header", "thead" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<thead class=""[&_tr]:border-b @ClassName"" @attributes=""AdditionalAttributes"">
    @ChildContent
</thead>

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}


