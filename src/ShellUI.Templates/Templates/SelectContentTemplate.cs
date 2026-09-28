using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SelectContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "select-content",
        DisplayName = "Select Content",
        Description = "Dropdown content for custom Select",
        Category = ComponentCategory.Form,
        FilePath = "SelectContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "select", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (Parent?.IsOpen == true)
{
    <div class=""absolute top-full left-0 z-50 mt-1 w-full rounded-md border border-border bg-popover p-1 text-popover-foreground shadow-md animate-in fade-in-0 zoom-in-95"" role=""listbox"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] public Select? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
