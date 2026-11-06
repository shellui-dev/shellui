using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ChartVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chart-variants",
        DisplayName = "Chart Variants",
        Description = "Chart theming and styling utilities for ShellUI charts",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Variants/ChartVariants.cs",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "theme", "styling", "data", "visualization" }
    };

    public static string Content => """"
namespace YourProjectNamespace.Components.UI.Variants;

public enum ChartTheme
{
    /// <summary>The theme's --chart-1 … --chart-5.</summary>
    Default,
    Colorful,
    Monochrome
}

/// <summary>Chart palettes. Default keeps the theme's colours; the others override them on the chart.</summary>
public static class ChartVariants
{
    public static string Palette(ChartTheme theme) => theme switch
    {
        ChartTheme.Colorful => Variables("#3b82f6", "#ef4444", "#10b981", "#f59e0b", "#8b5cf6"),
        ChartTheme.Monochrome => Variables("#64748b", "#94a3b8", "#cbd5e1", "#475569", "#334155"),
        _ => "",
    };

    private static string Variables(params string[] colors) =>
        string.Concat(colors.Select((color, i) => $"--chart-{i + 1}:{color};"));
}

/// <summary>How a ChartSeries is drawn.</summary>
public enum SeriesType
{
    Line,
    Area,
    Bar,
}
"""";
}
