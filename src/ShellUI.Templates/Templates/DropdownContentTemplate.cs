using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DropdownContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dropdown-content",
        DisplayName = "Dropdown Content",
        Description = "Content panel for Dropdown (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "DropdownContent.razor",
        Dependencies = new List<string> { "dropdown" },
        IsAvailable = false,
        Tags = new List<string> { "overlay", "dropdown", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (Parent?.IsOpen == true)
{
    <div class=""absolute right-0 z-50 mt-2 w-56 origin-top-right rounded-md border border-border bg-popover p-1 text-popover-foreground shadow-md animate-in fade-in-0 zoom-in-95"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] public Dropdown? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
