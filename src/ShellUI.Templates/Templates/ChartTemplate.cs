using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chart",
        DisplayName = "Chart",
        Description = "Base chart component with ShellUI theming and ApexCharts integration",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Chart.razor",
        Dependencies = new List<string> { "chart-variants", "chart-styles" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "data", "visualization", "apexcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using ApexCharts
@using YourProjectNamespace.Components.UI.Variants
@typeparam TItem where TItem : class

<div class=""@ComputedClass"" data-chart-theme=""@Theme.ToString().ToLower()"">
    <ApexChart TItem=""TItem""
               Title=""@Title""
               Options=""@ChartOptions""
               Height=""@Height""
               Width=""@Width"">
        @ChildContent
    </ApexChart>
</div>

@code {
    [Parameter] public ChartTheme Theme { get; set; } = ChartTheme.Default;
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Subtitle { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Height { get; set; } = ""400px"";
    [Parameter] public string? Width { get; set; } = ""100%"";
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private ApexCharts.ApexChartOptions<TItem>? _chartOptions;

    protected ApexCharts.ApexChartOptions<TItem> ChartOptions
    {
        get
        {
            if (_chartOptions == null)
            {
                _chartOptions = ChartVariants.GetOptions<TItem>(Theme);
                
                // Apply title if provided
                if (!string.IsNullOrEmpty(Title))
                {
                    _chartOptions.Title = _chartOptions.Title ?? new ApexCharts.Title();
                    _chartOptions.Title.Text = Title;
                }
                
                // Apply subtitle if provided
                if (!string.IsNullOrEmpty(Subtitle))
                {
                    _chartOptions.Subtitle = _chartOptions.Subtitle ?? new ApexCharts.Subtitle();
                    _chartOptions.Subtitle.Text = Subtitle;
                }
            }
            return _chartOptions;
        }
        set => _chartOptions = value;
    }

    protected string ComputedClass => Shell.Cn(
        ""border border-border bg-card text-card-foreground overflow-hidden"",
        ""[border-radius:var(--radius)] [box-shadow:var(--shadow)]"",
        Class
    );
}
";
}