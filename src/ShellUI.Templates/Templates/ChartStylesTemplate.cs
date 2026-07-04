using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ChartStylesTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chart-styles",
        DisplayName = "Chart Styles",
        Description = "CSS styles for ApexCharts integration with ShellUI theme system",
        Category = ComponentCategory.DataDisplay,
        // FilePath walks up out of Components/UI/ to project root then into wwwroot/css/,
        // matching the trick shellui-js uses for its own asset path.
        FilePath = "../../wwwroot/css/charts.css",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Variants = new List<string>(),
        Tags = new List<string> { "chart", "css", "styles", "apexcharts" }
    };

    public static string Content => @"/* ==========================================================
   ShellUI Chart Styles — ApexCharts chrome tuned to shadcn/ui
   Auto-installed by: shellui add chart
   Add to your App.razor or _Host.cshtml:
     <link href=""css/charts.css"" rel=""stylesheet"" />
   ========================================================== */

.apexcharts-canvas { background: transparent; }

/* Gridlines — subtle horizontal dashes */
.apexcharts-gridline {
    stroke: var(--border);
    stroke-opacity: 0.6;
}

/* Axis + legend text */
.apexcharts-text,
.apexcharts-xaxis-label,
.apexcharts-yaxis-label {
    fill: var(--muted-foreground);
    font-family: inherit;
}
.apexcharts-legend-text {
    color: var(--foreground) !important;
    font-family: inherit;
}

/* Title + subtitle */
.apexcharts-title-text { fill: var(--foreground); font-family: inherit; }
.apexcharts-subtitle-text { fill: var(--muted-foreground); font-family: inherit; }

/* Legend — top-left, circle markers */
.apexcharts-legend {
    display: flex !important;
    flex-wrap: wrap;
    gap: 12px;
    padding: 0 0 12px 0;
}
.apexcharts-legend-series {
    display: inline-flex !important;
    align-items: center !important;
    margin: 0 !important;
    gap: 6px;
}
.apexcharts-legend-marker {
    border-radius: 9999px !important;
    border: none !important;
    margin: 0 !important;
    vertical-align: middle !important;
    flex-shrink: 0;
}

/* Custom shellui tooltip — compact shadcn look */
.shellui-chart-tooltip {
    background: var(--popover);
    color: var(--popover-foreground);
    border: 1px solid var(--border);
    border-radius: calc(var(--radius) - 2px);
    box-shadow: 0 4px 12px -2px rgb(0 0 0 / 0.1);
    font-family: inherit;
    overflow: hidden;
    min-width: 0;
    max-width: 220px;
}
.shellui-chart-tooltip-title {
    font-size: 12px;
    font-weight: 600;
    color: var(--foreground);
    padding: 4px 8px;
    border-bottom: 1px solid var(--border);
    white-space: nowrap;
}
.shellui-chart-tooltip-body {
    display: flex;
    flex-direction: column;
    gap: 3px;
    padding: 4px 8px;
}
.shellui-chart-tooltip-row {
    display: grid;
    grid-template-columns: 10px 1fr auto;
    align-items: center;
    gap: 6px;
    font-size: 12px;
    line-height: 1.2;
    white-space: nowrap;
}
.shellui-chart-tooltip-marker {
    width: 10px; height: 10px;
    border-radius: 2px;
    display: block;
}
.shellui-chart-tooltip-label { color: var(--muted-foreground); font-weight: 500; }
.shellui-chart-tooltip-value {
    color: var(--foreground);
    font-weight: 600;
    font-variant-numeric: tabular-nums;
    padding-left: 12px;
}

/* Kill default ApexCharts tooltip wrapper — we render our own */
.apexcharts-tooltip {
    background: transparent !important;
    border: none !important;
    box-shadow: none !important;
    padding: 0 !important;
}

/* Crosshair on hover */
.apexcharts-xcrosshairs,
.apexcharts-ycrosshairs {
    stroke: var(--border);
    stroke-dasharray: 3;
}

/* Toolbar (opt-in via ShowToolbar) */
.apexcharts-toolbar { z-index: 10; }
.apexcharts-toolbar-item {
    color: var(--muted-foreground);
    transition: color 150ms;
}
.apexcharts-toolbar-item:hover { color: var(--foreground); }
.apexcharts-menu {
    background: var(--popover);
    border: 1px solid var(--border);
    border-radius: calc(var(--radius) - 2px);
    box-shadow: 0 10px 15px -3px rgb(0 0 0 / 0.1);
    padding: 4px;
}
.apexcharts-menu-item {
    color: var(--foreground);
    font-size: 13px;
    padding: 6px 10px;
    border-radius: calc(var(--radius) - 4px);
    transition: background-color 150ms;
}
.apexcharts-menu-item:hover {
    background: var(--accent);
    color: var(--accent-foreground);
}

/* Data labels */
.apexcharts-datalabel,
.apexcharts-datalabel-label,
.apexcharts-datalabel-value {
    fill: var(--foreground);
    font-family: inherit;
    font-weight: 500;
}
";
}
