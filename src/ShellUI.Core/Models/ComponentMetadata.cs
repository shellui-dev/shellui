namespace ShellUI.Core.Models;

public class ComponentMetadata
{
    public required string Name { get; set; }
    public required string DisplayName { get; set; }
    public required string Description { get; set; }
    public required ComponentCategory Category { get; set; }
    public List<string> Dependencies { get; set; } = new();
    
    // Relative to Components/UI folder
    public required string FilePath { get; set; }
    
    public bool IsAvailable { get; set; } = true;
    public string Version { get; set; } = "0.1.0";
    public List<string> Variants { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}

