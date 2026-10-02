using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-item",
        DisplayName = "Form Item",
        Description = "Vertical spacing wrapper for a single form row",
        Category = ComponentCategory.Form,
        FilePath = "FormItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "item", "primitive" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""space-y-2"", Class)"" @attributes=""AdditionalAttributes"">
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
