using System.Collections.Generic;

namespace BlazorInteractiveServer.Components.UI.Variants;

/* Chart color themes */
public enum ChartTheme
{
    Default,    // Balanced color palette suitable for light and dark modes
    Colorful,   // Vibrant colors for emphasis
    Monochrome  // Grayscale for minimal aesthetic
}

/* ApexCharts configuration optimized for ShellUI theme system.
   Note: ApexCharts doesn't support CSS variables in color arrays, so we use literal colors.
   Text and borders use CSS variables (styled via CSS) for automatic dark mode support. */
public static class ChartVariants
{
    // Creates chart options with theme-aware styling
    public static ApexCharts.ApexChartOptions<TItem> GetOptions<TItem>(ChartTheme theme = ChartTheme.Default) where TItem : class
    {
        return new ApexCharts.ApexChartOptions<TItem>
        {
            Chart = new ApexCharts.Chart
            {
                Background = "transparent",
                FontFamily = "inherit", // Inherits from CSS
                Toolbar = new ApexCharts.Toolbar
                {
                    Show = true,
                    Tools = new ApexCharts.Tools
                    {
                        Download = true,
                        Selection = true,
                        Zoom = true,
                        Zoomin = true,
                        Zoomout = true,
                        Pan = true,
                        Reset = true
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
            
            DataLabels = new ApexCharts.DataLabels
            {
                Enabled = false
            },
            
            Grid = new ApexCharts.Grid
            {
                StrokeDashArray = 3,
                Position = ApexCharts.GridPosition.Back
            },
            
            Xaxis = new ApexCharts.XAxis
            {
                Type = ApexCharts.XAxisType.Category, // Use category for proper label display
                Labels = new ApexCharts.XAxisLabels
                {
                    Style = new ApexCharts.AxisLabelStyle
                    {
                        FontSize = "12px"
                    }
                },
                AxisBorder = new ApexCharts.AxisBorder
                {
                    Show = true
                },
                AxisTicks = new ApexCharts.AxisTicks
                {
                    Show = true
                }
            },
            
            Yaxis = new List<ApexCharts.YAxis>
            {
                new ApexCharts.YAxis
                {
                    Labels = new ApexCharts.YAxisLabels
                    {
                        Style = new ApexCharts.AxisLabelStyle
                        {
                            FontSize = "12px"
                        }
                    }
                }
            },
            
            Legend = new ApexCharts.Legend
            {
                Show = true,
                Position = ApexCharts.LegendPosition.Bottom,
                FontSize = "13px",
                Markers = new ApexCharts.LegendMarkers
                {
                    Width = 10,
                    Height = 10,
                    Radius = 2
                }
            },
            
            Tooltip = new ApexCharts.Tooltip
            {
                Enabled = true,
                Shared = true,
                Intersect = false,
                Style = new ApexCharts.TooltipStyle
                {
                    FontSize = "12px"
                },
                Custom = @"function({ series, seriesIndex, dataPointIndex, w }) {
                    const xLabel = w.globals.labels[dataPointIndex] || '';
                    let html = '<div class=""custom-tooltip"">';
                    
                    if (xLabel) {
                        html += '<div class=""custom-tooltip-title"">' + xLabel + '</div>';
                    }
                    
                    w.globals.initialSeries.forEach((s, idx) => {
                        const color = w.config.colors[idx];
                        const name = s.name || '';
                        const value = series[idx] && series[idx][dataPointIndex] !== undefined 
                                     ? series[idx][dataPointIndex] 
                                     : '-';
                        
                        html += '<div class=""custom-tooltip-item"">' +
                               '<span class=""custom-tooltip-marker"" style=""background-color: ' + color + ';""></span>' +
                               '<span class=""custom-tooltip-label"">' + name + ':</span>' +
                               '<span class=""custom-tooltip-value"">' + value + '</span>' +
                               '</div>';
                    });
                    
                    html += '</div>';
                    return html;
                }"
            },
            
            Title = new ApexCharts.Title
            {
                Style = new ApexCharts.TitleStyle
                {
                    FontSize = "16px",
                    FontWeight = "600"
                }
            },
            
            Subtitle = new ApexCharts.Subtitle
            {
                Style = new ApexCharts.SubtitleStyle
                {
                    FontSize = "14px"
                }
            },
            
            PlotOptions = new ApexCharts.PlotOptions
            {
                Bar = new ApexCharts.PlotOptionsBar
                {
                    BorderRadius = 4,
                    ColumnWidth = "60%"
                },
                Pie = new ApexCharts.PlotOptionsPie
                {
                    ExpandOnClick = true
                }
            }
        };
    }

    /* Returns color palette based on selected theme.
       Colors are literal values because ApexCharts doesn't support CSS variables in color arrays. */
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
                "#64748b", // slate-500 - works in both light/dark
                "#94a3b8", // slate-400
                "#cbd5e1", // slate-300
                "#475569", // slate-600
                "#334155"  // slate-700
            },
            _ => GetThemeColors(ChartTheme.Default)
        };
    }
}
