using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ResizableTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "resizable",
        DisplayName = "Resizable",
        Description = "Resizable panel component",
        Category = ComponentCategory.Layout,
        FilePath = "Resizable.razor",
        Version = "0.1.0",
        Tags = new List<string> { "layout", "resizable", "panel", "split" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@(""resize overflow-auto rounded-md border border-border bg-card p-4 "" + ClassName)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

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

