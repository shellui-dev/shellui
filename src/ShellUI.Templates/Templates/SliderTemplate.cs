using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SliderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "slider",
        DisplayName = "Slider",
        Description = "Range slider input component",
        Category = ComponentCategory.Form,
        FilePath = "Slider.razor",

        Tags = new List<string> { "form", "input", "range", "slider" }
    };

    public static string Content => ComponentSource.Read("Components/Slider.razor");
}


