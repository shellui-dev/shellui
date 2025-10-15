using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class LabelTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "label",
        DisplayName = "Label",
        Description = "Form label component",
        Category = ComponentCategory.Form,
        Version = "0.1.0",
        FilePath = "Label.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<label 
    for=""@For""
    class=""text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70""
    @attributes=""AdditionalAttributes"">
    @ChildContent
</label>

@code {
    [Parameter]
    public string? For { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

