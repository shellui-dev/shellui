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
using System.Collections.Generic;

namespace YourProjectNamespace.Components.UI.Variants;

/* Chart color themes.
   Default matches the theme file's --chart-1..5 shadcn warm palette;
   the other two are alternates for emphasis or minimal aesthetic. */
public enum ChartTheme
{
    Default,
    Colorful,
    Monochrome
}

/* ApexCharts configuration tuned to the shadcn/ui chart aesthetic.
   ApexCharts accepts colors as JS strings only, so palettes use oklch literals
   mirroring the theme's --chart-N tokens. Grid, axis, tooltip, and legend chrome
   is styled via companion CSS (charts.css). */
public static class ChartVariants
{
    public static ApexCharts.ApexChartOptions<TItem> GetOptions<TItem>(ChartTheme theme = ChartTheme.Default, bool showToolbar = false) where TItem : class
    {
        return new ApexCharts.ApexChartOptions<TItem>
        {
            Chart = new ApexCharts.Chart
            {
                Background = "transparent",
                FontFamily = "inherit",
                Toolbar = new ApexCharts.Toolbar
                {
                    Show = showToolbar
                }
            },

            Theme = new ApexCharts.Theme(),
            Colors = GetThemeColors(theme),

            Stroke = new ApexCharts.Stroke
            {
                Width = 2,
                Curve = ApexCharts.Curve.Smooth,
                LineCap = ApexCharts.LineCap.Round
            },

            Fill = new ApexCharts.Fill { Opacity = 0.9 },

            DataLabels = new ApexCharts.DataLabels { Enabled = false },

            Grid = new ApexCharts.Grid
            {
                BorderColor = "transparent",
                StrokeDashArray = 4,
                Position = ApexCharts.GridPosition.Back
            },

            Xaxis = new ApexCharts.XAxis
            {
                Type = ApexCharts.XAxisType.Category,
                Labels = new ApexCharts.XAxisLabels
                {
                    Style = new ApexCharts.AxisLabelStyle
                    {
                        FontSize = "11px",
                        FontWeight = "500"
                    },
                    OffsetY = 2
                },
                AxisBorder = new ApexCharts.AxisBorder { Show = false },
                AxisTicks = new ApexCharts.AxisTicks { Show = false }
            },

            Yaxis = new List<ApexCharts.YAxis>
            {
                new ApexCharts.YAxis
                {
                    Labels = new ApexCharts.YAxisLabels
                    {
                        Style = new ApexCharts.AxisLabelStyle
                        {
                            FontSize = "11px",
                            FontWeight = "500"
                        }
                    },
                    AxisBorder = new ApexCharts.AxisBorder { Show = false },
                    AxisTicks = new ApexCharts.AxisTicks { Show = false }
                }
            },

            Legend = new ApexCharts.Legend
            {
                Show = true,
                Position = ApexCharts.LegendPosition.Top,
                HorizontalAlign = ApexCharts.Align.Left,
                FontSize = "12px",
                FontWeight = "500",
                OffsetY = 4,
                Markers = new ApexCharts.LegendMarkers
                {
                    Width = 8,
                    Height = 8,
                    Radius = 12,
                    OffsetX = -4
                }
            },

            Tooltip = new ApexCharts.Tooltip
            {
                Enabled = true,
                Shared = true,
                Intersect = false,
                FollowCursor = true,
                Style = new ApexCharts.TooltipStyle { FontSize = "12px" },
                Custom = @"function({ series, seriesIndex, dataPointIndex, w }) {
                    const xLabel = w.globals.labels[dataPointIndex] || '';
                    let html = '<div class=""shellui-chart-tooltip"">';
                    if (xLabel) {
                        html += '<div class=""shellui-chart-tooltip-title"">' + xLabel + '</div>';
                    }
                    html += '<div class=""shellui-chart-tooltip-body"">';
                    w.globals.initialSeries.forEach((s, idx) => {
                        const color = w.config.colors[idx];
                        const name = s.name || '';
                        const value = series[idx] && series[idx][dataPointIndex] !== undefined
                                     ? series[idx][dataPointIndex]
                                     : '-';
                        html += '<div class=""shellui-chart-tooltip-row"">' +
                               '<span class=""shellui-chart-tooltip-marker"" style=""background-color: ' + color + ';""></span>' +
                               '<span class=""shellui-chart-tooltip-label"">' + name + '</span>' +
                               '<span class=""shellui-chart-tooltip-value"">' + value + '</span>' +
                               '</div>';
                    });
                    html += '</div></div>';
                    return html;
                }"
            },

            Title = new ApexCharts.Title
            {
                Style = new ApexCharts.TitleStyle
                {
                    FontSize = "14px",
                    FontWeight = "600"
                }
            },

            Subtitle = new ApexCharts.Subtitle
            {
                Style = new ApexCharts.SubtitleStyle { FontSize = "12px" }
            },

            PlotOptions = new ApexCharts.PlotOptions
            {
                Bar = new ApexCharts.PlotOptionsBar
                {
                    BorderRadius = 4,
                    ColumnWidth = "60%"
                },
                Pie = new ApexCharts.PlotOptionsPie { ExpandOnClick = true }
            }
        };
    }

    /* Chart palettes.
       Default mirrors the theme's --chart-1..5 tokens (shadcn warm palette).
       ApexCharts requires literal colors in its config, so we can't inject var(--chart-N) —
       users overriding theme via `shellui theme apply` should also override this palette. */
    private static List<string> GetThemeColors(ChartTheme theme)
    {
        return theme switch
        {
            ChartTheme.Default => new List<string>
            {
                "oklch(0.68 0.19 41)",
                "oklch(0.55 0.10 190)",
                "oklch(0.30 0.05 220)",
                "oklch(0.80 0.14 84)",
                "oklch(0.73 0.17 51)"
            },
            ChartTheme.Colorful => new List<string>
            {
                "oklch(0.62 0.22 25)",
                "oklch(0.64 0.18 145)",
                "oklch(0.58 0.20 260)",
                "oklch(0.75 0.16 86)",
                "oklch(0.60 0.24 305)",
                "oklch(0.66 0.19 350)",
                "oklch(0.68 0.14 195)"
            },
            ChartTheme.Monochrome => new List<string>
            {
                "oklch(0.55 0 0)",
                "oklch(0.68 0 0)",
                "oklch(0.78 0 0)",
                "oklch(0.42 0 0)",
                "oklch(0.32 0 0)"
            },
            _ => GetThemeColors(ChartTheme.Default)
        };
    }
}
"""";
}
