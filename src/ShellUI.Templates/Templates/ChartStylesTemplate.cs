using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ChartStylesTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chart-styles",
        DisplayName = "Chart Styles",
        Description = "Loads the ShellCharts stylesheet, which reads the theme's colours",
        Category = ComponentCategory.DataDisplay,
        // ../../wwwroot/ escapes Components/UI/, like shellui-js.
        FilePath = "../../wwwroot/css/charts.css",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Variants = new List<string>(),
        Tags = new List<string> { "chart", "css", "styles", "shellcharts" }
    };

    public static string Content => @"/* ==========================================================
   ShellUI Chart Styles
   Auto-installed by: shellui add chart
   Add to your App.razor or _Host.cshtml:
     <link href=""css/charts.css"" rel=""stylesheet"" />
   ShellCharts draws with the theme's --chart-1 … --chart-5,
   --border, --muted-foreground and --card.
   ========================================================== */

@import url(""../_content/ShellCharts.Blazor/shellcharts.css"");
";
}
