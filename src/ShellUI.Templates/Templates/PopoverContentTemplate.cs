using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class PopoverContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "popover-content",
        DisplayName = "Popover Content",
        Description = "Content panel for Popover (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "PopoverContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "popover", "content" }
    };

    public static string Content => ComponentSource.Read("Components/PopoverContent.razor");
}
