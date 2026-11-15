using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NumberInputTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "number-input",
        DisplayName = "Number Input",
        Description = "Numeric input with increment and decrement buttons",
        Category = ComponentCategory.Form,
        FilePath = "NumberInput.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "input", "number", "stepper" }
    };

    public static string Content => ComponentSource.Read("Components/NumberInput.razor");
}
