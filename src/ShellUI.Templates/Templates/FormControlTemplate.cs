using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormControlTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-control",
        DisplayName = "Form Control",
        Description = "Wrapper around the input control for the shadcn-style form pattern",
        Category = ComponentCategory.Form,
        FilePath = "FormControl.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "control", "primitive" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Class"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
