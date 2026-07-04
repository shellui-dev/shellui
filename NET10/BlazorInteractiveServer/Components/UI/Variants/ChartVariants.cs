using System.Collections.Generic;

namespace BlazorInteractiveServer.Components.UI.Variants;

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
   is styled via companion CSS (shellui-theme.css / chart-styles). */
public static class ChartVariants
{
    public static ApexCharts.ApexChartOptions<TItem> GetOptions<TItem>(ChartTheme theme = ChartTheme.Default, bool showToolbar = false, bool showLegend = true) where TItem : class
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
                },
                Animations = new ApexCharts.Animations
                {
                    Enabled = true,
                    Speed = 800,
                    AnimateGradually = new ApexCharts.AnimateGradually
                    {
                        Enabled = true,
                        Delay = 150
                    },
                    DynamicAnimation = new ApexCharts.DynamicAnimation
                    {
                        Enabled = true,
                        Speed = 350
                    }
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

            Fill = new ApexCharts.Fill { Opacity = 0.85 },

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
                Show = showLegend,
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
                FollowCursor = false,
                Style = new ApexCharts.TooltipStyle { FontSize = "12px" },
                Custom = @"function({ series, seriesIndex, dataPointIndex, w }) {
                    const type = (w.config.chart && w.config.chart.type) || '';
                    const isCircular = ['pie', 'donut', 'radialBar', 'polarArea'].indexOf(type) >= 0
                        || (Array.isArray(series) && series.length > 0 && typeof series[0] === 'number');

                    // Resolve x-axis label from the first source that has a real string.
                    // ApexCharts stashes category names in different globals depending on axis type.
                    function xLabelAt(i) {
                        const cats = (w.globals.categoryLabels && w.globals.categoryLabels.length ? w.globals.categoryLabels
                            : (w.config.xaxis && w.config.xaxis.categories) || w.globals.labels || []);
                        const v = cats[i];
                        return v !== undefined && v !== null ? String(v) : '';
                    }

                    let html = '<div class=""shellui-chart-tooltip"">';
                    if (isCircular) {
                        const label = (w.globals.labels && w.globals.labels[seriesIndex]) || '';
                        const value = series[seriesIndex];
                        const color = w.config.colors[seriesIndex];
                        html += '<div class=""shellui-chart-tooltip-body"">' +
                               '<div class=""shellui-chart-tooltip-row"">' +
                               '<span class=""shellui-chart-tooltip-marker"" style=""background-color: ' + color + ';""></span>' +
                               '<span class=""shellui-chart-tooltip-label"">' + label + '</span>' +
                               '<span class=""shellui-chart-tooltip-value"">' + value + '</span>' +
                               '</div></div>';
                    } else {
                        const title = xLabelAt(dataPointIndex);
                        if (title) {
                            html += '<div class=""shellui-chart-tooltip-title"">' + title + '</div>';
                        }
                        html += '<div class=""shellui-chart-tooltip-body"">';
                        (w.globals.initialSeries || []).forEach((s, idx) => {
                            const color = w.config.colors[idx];
                            const name = (s && s.name) || '';
                            const value = series[idx] && series[idx][dataPointIndex] !== undefined
                                         ? series[idx][dataPointIndex]
                                         : '-';
                            html += '<div class=""shellui-chart-tooltip-row"">' +
                                   '<span class=""shellui-chart-tooltip-marker"" style=""background-color: ' + color + ';""></span>' +
                                   '<span class=""shellui-chart-tooltip-label"">' + name + '</span>' +
                                   '<span class=""shellui-chart-tooltip-value"">' + value + '</span>' +
                                   '</div>';
                        });
                        html += '</div>';
                    }
                    html += '</div>';
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
                    ColumnWidth = "55%",
                    BorderRadiusApplication = ApexCharts.BorderRadiusApplication.End
                },
                Pie = new ApexCharts.PlotOptionsPie
                {
                    ExpandOnClick = true,
                    DataLabels = new ApexCharts.PieDataLabels
                    {
                        MinAngleToShowLabel = 360 // never show the slice callout labels — tooltip does the talking
                    },
                    Donut = new ApexCharts.PlotOptionsDonut
                    {
                        Size = "70%",
                        Background = "transparent"
                    }
                },
                Radar = new ApexCharts.PlotOptionsRadar
                {
                    Polygons = new ApexCharts.RadarPolygons
                    {
                        StrokeColors = "var(--border)",
                        ConnectorColors = "var(--border)"
                    }
                },
                RadialBar = new ApexCharts.PlotOptionsRadialBar
                {
                    Hollow = new ApexCharts.Hollow
                    {
                        Size = "60%"
                    },
                    Track = new ApexCharts.Track
                    {
                        Background = "var(--muted)"
                    },
                    DataLabels = new ApexCharts.RadialBarDataLabels
                    {
                        Name = new ApexCharts.RadialBarDataLabelsName { Show = true, FontSize = "13px" },
                        Value = new ApexCharts.RadialBarDataLabelsValue { Show = true, FontSize = "22px", FontWeight = "600" }
                    }
                }
            }
        };
    }

    /* Chart palettes.
       ApexCharts doesn't support CSS variables in color arrays, so we use literal colors.
       Text and borders use CSS variables (styled via CSS) for automatic dark mode support. */
    private static List<string> GetThemeColors(ChartTheme theme)
    {
        return theme switch
        {
            ChartTheme.Default => new List<string>
            {
                "#2563eb", // blue-600
                "#dc2626", // red-600
                "#16a34a", // green-600
                "#ca8a04", // yellow-600
                "#9333ea"  // purple-600
            },
            ChartTheme.Colorful => new List<string>
            {
                "#3b82f6", // blue-500
                "#ef4444", // red-500
                "#10b981", // green-500
                "#f59e0b", // yellow-500
                "#8b5cf6", // purple-500
                "#ec4899", // pink-500
                "#06b6d4"  // cyan-500
            },
            ChartTheme.Monochrome => new List<string>
            {
                "#64748b", // slate-500
                "#94a3b8", // slate-400
                "#cbd5e1", // slate-300
                "#475569", // slate-600
                "#334155"  // slate-700
            },
            _ => GetThemeColors(ChartTheme.Default)
        };
    }
}
