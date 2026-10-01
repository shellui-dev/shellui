using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandSeparatorTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-separator",
        DisplayName = "Command Separator",
        Description = "Divider between Command groups",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandSeparator.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "separator" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div role=""separator"" class=""@Shell.Cn(""-mx-1 h-px bg-border"", Class)"" @attributes=""AdditionalAttributes""></div>

@code {
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
