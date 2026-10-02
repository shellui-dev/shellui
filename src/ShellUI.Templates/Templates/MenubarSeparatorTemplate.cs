using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarSeparatorTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-separator",
        DisplayName = "Menubar Separator",
        Description = "Horizontal divider for the Menubar dropdown",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarSeparator.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menubar", "separator" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""-mx-1 my-1 h-px bg-muted"", Class)"" @attributes=""AdditionalAttributes""></div>

@code {
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
