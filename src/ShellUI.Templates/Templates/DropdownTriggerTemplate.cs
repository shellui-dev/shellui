using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DropdownTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dropdown-trigger",
        DisplayName = "Dropdown Trigger",
        Description = "Trigger for Dropdown (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "DropdownTrigger.razor",
        Dependencies = new List<string> { "dropdown" },
        IsAvailable = false,
        Tags = new List<string> { "overlay", "dropdown", "trigger" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @onclick=""Toggle"" @attributes=""AdditionalAttributes"" role=""button"" tabindex=""0"" @onkeydown=""HandleKeyDown"">
    @ChildContent
</div>

@code {
    [CascadingParameter] public Dropdown? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Toggle()
    {
        if (Parent != null)
            await Parent.ToggleAsync();
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key is ""Enter"" or "" "" && Parent != null)
            await Parent.ToggleAsync();
    }
}
";
}
