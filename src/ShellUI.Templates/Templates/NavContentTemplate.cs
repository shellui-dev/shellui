using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NavContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "nav-content",
        DisplayName = "Nav Content",
        Description = "Dropdown content for NavItem",
        Category = ComponentCategory.Navigation,
        FilePath = "NavContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "nav", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@* NavContent must be used inside NavItem. NavItem cascades itself. *@
@if (Parent?.IsOpenState == true)
{
    <div class=""absolute left-0 top-full mt-1 w-56 rounded-md border border-border bg-popover p-1 shadow-lg animate-in fade-in-0 zoom-in-95"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] public NavItem? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
