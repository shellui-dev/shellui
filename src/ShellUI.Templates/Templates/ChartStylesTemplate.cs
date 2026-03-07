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
        FilePath = "wwwroot/css/charts.css",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Variants = new List<string>(),
        Tags = new List<string> { "chart", "css", "styles", "apexcharts" }
    };

    public static string Content => @"/* ==========================================================================
   ShellUI Chart Styles - ApexCharts Theme Integration
   Auto-installed by: shellui add chart
   Add to your App.razor or _Host.cshtml:
     <link href=""css/charts.css"" rel=""stylesheet"" />
   ========================================================================== */

/* Canvas */
.apexcharts-canvas {
    background: transparent !important;
}

/* Tooltip - Uses ShellUI theme variables with fallbacks */
.apexcharts-tooltip {
    z-index: 9999 !important;
    position: absolute !important;
    background: var(--popover, #fff) !important;
    border: 1px solid var(--border, #e5e7eb) !important;
    border-radius: 8px !important;
    box-shadow: 0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1) !important;
    padding: 6px 8px !important;
    font-family: inherit !important;
    font-size: 11px !important;
    line-height: 1.4 !important;
    color: var(--popover-foreground, #09090b) !important;
}

/* Backdrop blur with browser support fallback */
@supports ((-webkit-backdrop-filter: none) or (backdrop-filter: none)) {
    .apexcharts-tooltip {
        backdrop-filter: blur(10px) !important;
        -webkit-backdrop-filter: blur(10px) !important;
    }
}

/* Tooltip title */
.apexcharts-tooltip-title {
    background: var(--muted, #f4f4f5) !important;
    border-bottom: 1px solid var(--border, #e5e7eb) !important;
    color: var(--foreground, #09090b) !important;
    font-family: inherit !important;
    font-weight: 600 !important;
    font-size: 11px !important;
    padding: 4px 6px !important;
    margin: -6px -8px 4px -8px !important;
}

/* Custom Tooltip - Full control over structure */
.custom-tooltip {
    padding: 0 !important;
    display: flex !important;
    flex-direction: column !important;
    gap: 6px !important;
}

.custom-tooltip-title {
    font-size: 12px !important;
    font-weight: 600 !important;
    color: var(--foreground, #09090b) !important;
    line-height: 1.2 !important;
    padding-bottom: 4px !important;
    border-bottom: 1px solid var(--border, #e5e7eb) !important;
    margin-bottom: 2px !important;
}

.custom-tooltip-item {
    display: flex !important;
    align-items: center !important;
    gap: 8px !important;
    padding: 0 !important;
    line-height: 1 !important;
}

.custom-tooltip-marker {
    width: 8px !important;
    height: 8px !important;
    border-radius: 2px !important;
    flex-shrink: 0 !important;
    display: block !important;
}

.custom-tooltip-label {
    font-size: 12px !important;
    font-weight: 500 !important;
    color: var(--muted-foreground, #71717a) !important;
    line-height: 1 !important;
}

.custom-tooltip-value {
    font-size: 12px !important;
    font-weight: 600 !important;
    color: var(--popover-foreground, #09090b) !important;
    line-height: 1 !important;
    margin-left: 4px !important;
}

/* Fallback for default ApexCharts tooltip */
.apexcharts-tooltip-series-group {
    padding: 3px 0 !important;
    display: grid !important;
    grid-template-columns: 8px 1fr !important;
    align-items: center !important;
    gap: 8px !important;
}

.apexcharts-tooltip-marker {
    width: 8px !important;
    height: 8px !important;
    border-radius: 2px !important;
    margin: 0 !important;
    padding: 0 !important;
}

.apexcharts-tooltip-text {
    font-family: inherit !important;
    font-size: 11px !important;
    color: var(--popover-foreground, #09090b) !important;
    padding: 0 !important;
    margin: 0 !important;
    line-height: 1.1 !important;
}

.apexcharts-tooltip-text-y-label,
.apexcharts-tooltip-text-y-value {
    line-height: 1.1 !important;
}

.apexcharts-tooltip-text-y-label {
    font-weight: 500 !important;
    color: var(--muted-foreground, #71717a) !important;
}

.apexcharts-tooltip-text-y-value {
    font-weight: 600 !important;
    color: var(--popover-foreground, #09090b) !important;
    margin-left: 4px !important;
}

/* Axis Tooltips */
.apexcharts-xaxistooltip,
.apexcharts-yaxistooltip {
    z-index: 9998 !important;
    background: var(--popover, #fff) !important;
    border: 1px solid var(--border, #e5e7eb) !important;
    border-radius: 4px !important;
    box-shadow: 0 4px 6px -1px rgb(0 0 0 / 0.1) !important;
    font-family: inherit !important;
    font-size: 11px !important;
    color: var(--popover-foreground, #09090b) !important;
    padding: 4px 8px !important;
}

.apexcharts-xaxistooltip-bottom:before,
.apexcharts-xaxistooltip-bottom:after {
    border-bottom-color: var(--border, #e5e7eb) !important;
}

/* Legend */
.apexcharts-legend {
    padding: 8px 0 4px 0 !important;
}

.apexcharts-legend-series {
    display: inline-flex !important;
    align-items: center !important;
    margin: 0 8px 4px 0 !important;
    gap: 6px !important;
}

.apexcharts-legend-text {
    color: var(--foreground, #09090b) !important;
    font-family: inherit !important;
    font-size: 13px !important;
    font-weight: 500 !important;
}

.apexcharts-legend-marker {
    border-radius: 2px !important;
    flex-shrink: 0 !important;
}

/* Toolbar */
.apexcharts-toolbar {
    z-index: 100 !important;
}

.apexcharts-menu {
    z-index: 9999 !important;
    background: var(--popover, #fff) !important;
    border: 1px solid var(--border, #e5e7eb) !important;
    border-radius: 8px !important;
    box-shadow: 0 10px 15px -3px rgb(0 0 0 / 0.1) !important;
    padding: 4px !important;
}

.apexcharts-menu-item {
    color: var(--foreground, #09090b) !important;
    font-family: inherit !important;
    font-size: 13px !important;
    padding: 6px 12px !important;
    border-radius: 4px !important;
    transition: background-color 150ms !important;
}

.apexcharts-menu-item:hover {
    background: var(--accent, #f4f4f5) !important;
    color: var(--accent-foreground, #18181b) !important;
}

.apexcharts-toolbar-item {
    color: var(--muted-foreground, #71717a) !important;
    transition: color 150ms !important;
}

.apexcharts-toolbar-item:hover {
    color: var(--foreground, #09090b) !important;
}

/* Grid and Axis */
.apexcharts-gridline {
    stroke: var(--border, #e5e7eb) !important;
    stroke-opacity: 0.5 !important;
}

.apexcharts-text {
    fill: var(--muted-foreground, #71717a) !important;
    font-family: inherit !important;
}

.apexcharts-xaxis-label,
.apexcharts-yaxis-label {
    fill: var(--muted-foreground, #71717a) !important;
}

/* Data Labels */
.apexcharts-datalabel,
.apexcharts-datalabel-label,
.apexcharts-datalabel-value {
    fill: var(--foreground, #09090b) !important;
    font-family: inherit !important;
    font-weight: 500 !important;
}
";
}
