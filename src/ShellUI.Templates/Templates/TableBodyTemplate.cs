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

        Tags = new List<string> { "table", "body", "tbody" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<tbody class=""[&_tr:last-child]:border-0 @ClassName"" @attributes=""AdditionalAttributes"">
    @ChildContent
</tbody>

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


