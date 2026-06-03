using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarModelsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-models",
        DisplayName = "Sidebar Models",
        Description = "Enum models for Sidebar component (variant, collapsible, side)",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarModels.cs",
        IsAvailable = false
    };

    public static string Content => @"namespace YourProjectNamespace.Components.UI;

public enum SidebarVariant
{
    Sidebar,
    Floating,
    Inset
}

public enum SidebarCollapsible
{
    Offcanvas,
    Icon,
    None
}

public enum SidebarSide
{
    Left,
    Right
}
";
}
