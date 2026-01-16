namespace ShellUI.Components.Models;

public class CommandItem
{
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Shortcut { get; set; }
    public Func<Task>? Action { get; set; }
}

