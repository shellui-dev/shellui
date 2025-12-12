using Microsoft.AspNetCore.Components;

namespace ShellUI.Components.Models;

public class TabItem
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Badge { get; set; }
    public RenderFragment? Content { get; set; }
    public bool Disabled { get; set; }
}

