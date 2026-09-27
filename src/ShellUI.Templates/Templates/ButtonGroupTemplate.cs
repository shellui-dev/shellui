using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ButtonGroupTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "button-group",
        DisplayName = "Button Group",
        Description = "Joins adjacent buttons into a single horizontal or vertical group",
        Category = ComponentCategory.Form,
        FilePath = "ButtonGroup.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "button", "group", "toolbar" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div role=""group"" class=""@Shell.Cn(""inline-flex"", IsVertical ? VerticalClass : HorizontalClass, Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [Parameter] public string Orientation { get; set; } = ""horizontal"";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool IsVertical => string.Equals(Orientation, ""vertical"", StringComparison.OrdinalIgnoreCase);

    private const string HorizontalClass = ""flex-row [&>*]:rounded-none [&>*:first-child]:rounded-l-md [&>*:last-child]:rounded-r-md [&>*:not(:first-child)]:-ml-px [&>*]:focus-visible:z-10"";
    private const string VerticalClass = ""flex-col [&>*]:rounded-none [&>*:first-child]:rounded-t-md [&>*:last-child]:rounded-b-md [&>*:not(:first-child)]:-mt-px [&>*]:focus-visible:z-10"";
}
";
}
