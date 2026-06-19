using Microsoft.AspNetCore.Components;

namespace BlazorInteractiveServer.Components.Models;

public class StepItem
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RenderFragment? Content { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsOptional { get; set; }
}
