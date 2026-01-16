using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class TableHeadTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "table-head",
        DisplayName = "Table Head",
        Description = "Table header cell component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "TableHead.razor",

        Tags = new List<string> { "table", "header", "th" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<th class=""h-12 px-4 text-left align-middle font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 @ClassName"" @attributes=""AdditionalAttributes"">
    @ChildContent
</th>

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


