using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-content",
        DisplayName = "Menubar Content",
        Description = "Popover content for the compositional Menubar pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menubar", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (Parent?.IsOpen == true)
{
    <div class=""@Shell.Cn(""absolute left-0 top-full z-50 mt-1 min-w-[12rem] overflow-hidden rounded-md border border-border bg-popover p-1 text-popover-foreground shadow-md animate-in fade-in-0 zoom-in-95"", Class)""
         @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] private MenubarMenu? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
