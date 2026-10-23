using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class FieldTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "field",
        DisplayName = "Field",
        Description = "Label, control, description and validation message as one unit",
        Category = ComponentCategory.Form,
        FilePath = "Field.razor",
        Dependencies = new List<string> { "form-message" },
        Tags = new List<string> { "form", "field", "label", "validation" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using System.Linq.Expressions
@using Microsoft.AspNetCore.Components.Forms

<CascadingValue Value=""FieldIdentifier"" Name=""ShellUIFormField"">
    <div role=""group"" class=""@Shell.Cn(IsHorizontal ? ""flex flex-row items-start justify-between gap-4"" : ""flex flex-col gap-2"", Class)"" @attributes=""AdditionalAttributes"">
        @if (IsHorizontal)
        {
            <div class=""flex flex-col gap-1.5"">
                @if (!string.IsNullOrEmpty(Label))
                {
                    <label for=""@LabelFor"" class=""text-sm font-medium leading-none"">
                        @Label
                        @if (Required)
                        {
                            <span class=""ml-0.5 text-destructive"" aria-hidden=""true"">*</span>
                        }
                    </label>
                }
                @if (!string.IsNullOrEmpty(Description))
                {
                    <p class=""text-sm text-muted-foreground"">@Description</p>
                }
                <FormMessage Message=""@Error"" />
            </div>
            <div class=""shrink-0"">@ChildContent</div>
        }
        else
        {
            @if (!string.IsNullOrEmpty(Label))
            {
                <label for=""@LabelFor"" class=""text-sm font-medium leading-none"">
                    @Label
                    @if (Required)
                    {
                        <span class=""ml-0.5 text-destructive"" aria-hidden=""true"">*</span>
                    }
                </label>
            }
            @ChildContent
            @if (!string.IsNullOrEmpty(Description))
            {
                <p class=""text-sm text-muted-foreground"">@Description</p>
            }
            <FormMessage Message=""@Error"" />
        }
    </div>
</CascadingValue>

@code {
    [Parameter] public string? Label { get; set; }
    // The id of the control inside, so clicking the label focuses it.
    [Parameter] public string? LabelFor { get; set; }
    [Parameter] public string? Description { get; set; }
    // Shown instead of the EditForm's validation messages for For.
    [Parameter] public string? Error { get; set; }
    // The model property whose validation messages to show, as in FormField.
    [Parameter] public Expression<Func<object?>>? For { get; set; }
    [Parameter] public bool Required { get; set; }
    // ""vertical"" or ""horizontal"" (label and description beside the control, for switches and checkboxes).
    [Parameter] public string Orientation { get; set; } = ""vertical"";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool IsHorizontal => string.Equals(Orientation, ""horizontal"", StringComparison.OrdinalIgnoreCase);

    private FieldIdentifier FieldIdentifier => For != null ? FieldIdentifier.Create(For) : default;
}
";
}
