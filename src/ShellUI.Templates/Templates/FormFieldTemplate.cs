using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormFieldTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-field",
        DisplayName = "Form Field",
        Description = "Generic field context primitive for the shadcn-style form pattern",
        Category = ComponentCategory.Form,
        FilePath = "FormField.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "field", "primitive" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using System.Linq.Expressions
@using Microsoft.AspNetCore.Components.Forms

<CascadingValue Value=""Field"" Name=""ShellUIFormField"">
    @ChildContent
</CascadingValue>

@code {
    [CascadingParameter] private EditContext? EditContext { get; set; }
    [Parameter] public Expression<Func<object?>>? For { get; set; }
    [Parameter] public string? Name { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    // The field FormMessage reports on: from For, or from Name on the EditForm model.
    public FieldIdentifier Field => For != null
        ? FieldIdentifier.Create(For)
        : EditContext != null && !string.IsNullOrEmpty(Name) ? new FieldIdentifier(EditContext.Model, Name) : default;
}
";
}
