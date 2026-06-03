using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarSeparatorTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-separator",
        DisplayName = "Sidebar Separator",
        Description = "Horizontal divider for sidebar sections",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarSeparator.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<hr data-sidebar=""separator""
    class=""@Shell.Cn(""mx-2 w-auto bg-sidebar-border h-px border-0"", Class)""
    @attributes=""AdditionalAttributes"" />

@code {
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
