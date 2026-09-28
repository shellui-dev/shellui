using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NavTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "nav-trigger",
        DisplayName = "Nav Trigger",
        Description = "Trigger button for NavItem dropdown",
        Category = ComponentCategory.Navigation,
        FilePath = "NavTrigger.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "nav", "trigger" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<button type=""button""
        @onclick=""Toggle""
        aria-expanded=""@((Parent?.IsOpenState == true).ToString().ToLowerInvariant())""
        class=""group inline-flex h-10 w-max items-center justify-center rounded-md bg-background px-4 py-2 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground focus:outline-none disabled:pointer-events-none disabled:opacity-50""
        @attributes=""AdditionalAttributes"">
    @ChildContent
    @if (Parent?.HasContent == true)
    {
        <svg class=""@Shell.Cn(""relative top-[1px] ml-1 h-3 w-3 transition duration-200"", Parent?.IsOpenState == true ? ""rotate-180"" : """")"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M19 9l-7 7-7-7"" />
        </svg>
    }
</button>

@code {
    [CascadingParameter] public NavItem? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private Task Toggle() => Parent?.ToggleAsync() ?? Task.CompletedTask;
}
";
}
