namespace ShellUI.Core.Models;

public class ShellUIConfig
{
    public string Schema { get; set; } = "https://shellui.dev/schema.json";
    public string Style { get; set; } = "default"; // default, new-york, minimal
    public string ComponentsPath { get; set; } = "Components/UI";
    public TailwindConfig Tailwind { get; set; } = new();
    public List<InstalledComponent> InstalledComponents { get; set; } = new();
    public ProjectType ProjectType { get; set; }
}

public class TailwindConfig
{
    public bool Enabled { get; set; } = true;
    public string Version { get; set; } = "4.1.17";
    public string Method { get; set; } = "standalone"; // "standalone" or "npm"
    public string ConfigPath { get; set; } = "tailwind.config.js";
    public string CssPath { get; set; } = "wwwroot/app.css";
}

public class InstalledComponent
{
    public required string Name { get; set; }
    public required string Version { get; set; }
    public DateTime InstalledAt { get; set; } = DateTime.UtcNow;
    public bool IsCustomized { get; set; }
}

public enum ProjectType
{
    Unknown,
    BlazorServer,
    BlazorWebAssembly,
    BlazorServerSideRendering,
    BlazorUnited
}

