using ShellUI.Core.Models;
using ShellUI.Templates;
using System.Text.Json;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public class ComponentInstaller
{
    public static void InstallComponents(string[] components, bool force)
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");
        
        if (!File.Exists(configPath))
        {
            AnsiConsole.MarkupLine("[red]ShellUI not initialized![/]");
            AnsiConsole.MarkupLine("[yellow]Run 'dotnet shellui init' first[/]");
            return;
        }

        // Load config
        var configJson = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<ShellUIConfig>(configJson);
        
        if (config == null)
        {
            AnsiConsole.MarkupLine("[red]Failed to read shellui.json[/]");
            return;
        }

        // Detect project for namespace
        var projectInfo = ProjectDetector.DetectProject();

        // Parse comma-separated components
        var componentList = new List<string>();
        foreach (var comp in components)
        {
            componentList.AddRange(comp.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }

        var successCount = 0;
        var skippedCount = 0;
        var failedComponents = new List<string>();

        foreach (var componentName in componentList)
        {
            var result = InstallComponent(componentName, config, projectInfo, force);
            
            if (result == InstallResult.Success)
                successCount++;
            else if (result == InstallResult.Skipped)
                skippedCount++;
            else
                failedComponents.Add(componentName);
        }

        // Update config
        var updatedJson = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(configPath, updatedJson);

        // Summary
        AnsiConsole.MarkupLine("");
        if (successCount > 0)
            AnsiConsole.MarkupLine($"[green]Installed {successCount} component(s) successfully![/]");
        if (skippedCount > 0)
            AnsiConsole.MarkupLine($"[yellow]Skipped {skippedCount} component(s) (already exists, use --force to overwrite)[/]");
        if (failedComponents.Count > 0)
            AnsiConsole.MarkupLine($"[red]Failed: {string.Join(", ", failedComponents)}[/]");
    }

    private static InstallResult InstallComponent(string componentName, ShellUIConfig config, ProjectInfo projectInfo, bool force)
    {
        if (!ComponentRegistry.Exists(componentName))
        {
            AnsiConsole.MarkupLine($"[red]Component '{componentName}' not found[/]");
            return InstallResult.Failed;
        }

        var metadata = ComponentRegistry.GetMetadata(componentName);
        if (metadata == null)
        {
            AnsiConsole.MarkupLine($"[red]Failed to get metadata for '{componentName}'[/]");
            return InstallResult.Failed;
        }

        var componentPath = Path.Combine(Directory.GetCurrentDirectory(), config.ComponentsPath, metadata.FilePath);
        
        // Check if already exists
        if (File.Exists(componentPath) && !force)
        {
            AnsiConsole.MarkupLine($"[yellow]Skipped '{componentName}' (already exists)[/]");
            return InstallResult.Skipped;
        }

        // Get component content
        var content = ComponentRegistry.GetComponentContent(componentName);
        if (content == null)
        {
            AnsiConsole.MarkupLine($"[red]Failed to get content for '{componentName}'[/]");
            return InstallResult.Failed;
        }

        // Replace namespace placeholder
        content = content.Replace("YourProjectNamespace", projectInfo.RootNamespace);

        // Ensure directory exists
        var directory = Path.GetDirectoryName(componentPath);
        if (directory != null)
        {
            Directory.CreateDirectory(directory);
        }

        // Write file
        File.WriteAllText(componentPath, content);

        // Update config
        var existing = config.InstalledComponents.FirstOrDefault(c => c.Name == componentName);
        if (existing != null)
        {
            existing.Version = metadata.Version;
            existing.InstalledAt = DateTime.UtcNow;
            existing.IsCustomized = false;
        }
        else
        {
            config.InstalledComponents.Add(new InstalledComponent
            {
                Name = componentName,
                Version = metadata.Version,
                InstalledAt = DateTime.UtcNow,
                IsCustomized = false
            });
        }

        AnsiConsole.MarkupLine($"[green]Installed '{componentName}'[/] [dim]({metadata.FilePath})[/]");
        return InstallResult.Success;
    }

    private enum InstallResult
    {
        Success,
        Skipped,
        Failed
    }
}

