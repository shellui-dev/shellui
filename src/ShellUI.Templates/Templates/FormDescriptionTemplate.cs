using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-description",
        DisplayName = "Form Description",
        Description = "Helper text below a form control",
        Category = ComponentCategory.Form,
        FilePath = "FormDescription.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "description", "primitive" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<p class=""@Shell.Cn(""text-sm text-muted-foreground"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</p>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
