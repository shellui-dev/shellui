using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CollapsibleTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "collapsible-trigger",
        DisplayName = "Collapsible Trigger",
        Description = "Trigger for Collapsible component",
        Category = ComponentCategory.Layout,
        FilePath = "CollapsibleTrigger.razor",
        Dependencies = new List<string>(),
        IsAvailable = false,
        Tags = new List<string> { "layout", "collapsible", "trigger", "toggle" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""flex w-full items-center justify-between"" @onclick=""Toggle"" @attributes=""AdditionalAttributes"">
    @ChildContent
    <svg class=""@Shell.Cn(""h-4 w-4 shrink-0 transition-transform duration-200"", Parent?.IsOpen == true ? ""rotate-180"" : """")"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M19 9l-7 7-7-7"" />
    </svg>
</div>

@code {
    [CascadingParameter] public Collapsible? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Toggle()
    {
        if (Parent != null)
        {
            await Parent.ToggleAsync();
        }
    }
}
";
}
