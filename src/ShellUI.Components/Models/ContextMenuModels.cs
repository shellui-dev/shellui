namespace ShellUI.Components.Models;

public class ContextMenuItem
{
    public string Label { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Shortcut { get; set; }
    public bool Disabled { get; set; }
    public bool IsSeparator { get; set; }
    public object? Data { get; set; }
}

