using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormLabelTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-label",
        DisplayName = "Form Label",
        Description = "Styled label for the shadcn-style form pattern",
        Category = ComponentCategory.Form,
        FilePath = "FormLabel.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "label", "primitive" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<label class=""@Shell.Cn(""text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</label>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
