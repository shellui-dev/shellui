using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-trigger",
        DisplayName = "Sheet Trigger",
        Description = "Trigger subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetTrigger.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "overlay", "sheet", "trigger" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @onclick=""Open"" role=""button"" tabindex=""0"" @onkeydown=""HandleKeyDown"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [CascadingParameter] private Sheet? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Open()
    {
        if (Parent != null) await Parent.SetOpen(true);
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if ((e.Key == ""Enter"" || e.Key == "" "") && Parent != null) await Parent.SetOpen(true);
    }
}
";
}
