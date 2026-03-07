using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AccordionTypeTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "accordion-type",
        DisplayName = "Accordion Type",
        Description = "Accordion behavior enum (Single/Multiple)",
        Category = ComponentCategory.Utility,
        FilePath = "AccordionType.cs",
        IsAvailable = false,
        Tags = new List<string> { "accordion", "enum", "utility" }
    };

    public static string Content => @"namespace YourProjectNamespace.Components.UI;

public enum AccordionType
{
    Single,   // one open at a time
    Multiple  // multiple open
}
";
}
