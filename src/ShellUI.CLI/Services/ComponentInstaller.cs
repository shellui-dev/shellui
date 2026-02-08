using ShellUI.Core.Models;
using ShellUI.Templates;
using System.Text.Json;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public class ComponentInstaller
{
    public static async Task InstallComponents(string[] components, bool force)
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

        // Track installed components to avoid duplicates
        var installedSet = new HashSet<string>();
        
        // Show dependency information
        foreach (var componentName in componentList)
        {
            var metadata = ComponentRegistry.GetMetadata(componentName);
            if (metadata != null && metadata.Dependencies?.Any() == true)
            {
                AnsiConsole.MarkupLine($"[green]●[/] [bold]{componentName}[/] requires: [yellow]{string.Join(", ", metadata.Dependencies)}[/]");
            }
        }
        
        AnsiConsole.MarkupLine("");
        
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("green"))
            .StartAsync("Installing components...", ctx =>
            {
                foreach (var componentName in componentList)
                {
                    ctx.Status($"Installing {componentName}...");
                    InstallComponentWithDependencies(componentName, config, projectInfo, force, installedSet, ref successCount, ref skippedCount, failedComponents);
                }
                return Task.CompletedTask;
            });

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

    public static Task InstallComponentForInitAsync(string componentName, ProjectInfo projectInfo)
    {
        var metadata = ComponentRegistry.Components.GetValueOrDefault(componentName.ToLower());
        if (metadata == null) return Task.CompletedTask;

        var config = new ShellUIConfig
        {
            ComponentsPath = "Components/UI",
            ProjectType = projectInfo.ProjectType,
            Style = "default"
        };

        var result = InstallComponentInternal(componentName, config, projectInfo, false);

        if (result == InstallResult.Success)
        {
            AnsiConsole.MarkupLine($"[green]✅ Installed:[/] {componentName}");
        }

        return Task.CompletedTask;
    }

    public static void InstallComponent(string componentName, ComponentMetadata metadata, bool force, bool skipConfig = false)
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");
        var configJson = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<ShellUIConfig>(configJson);
        
        if (config == null) return;
        
        var projectInfo = ProjectDetector.DetectProject();
        var result = InstallComponentInternal(componentName, config, projectInfo, force);
        
        if (!skipConfig && result == InstallResult.Success)
        {
            var updatedJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configPath, updatedJson);
        }
    }

    private static InstallResult InstallComponent(string componentName, ShellUIConfig config, ProjectInfo projectInfo, bool force)
    {
        return InstallComponentInternal(componentName, config, projectInfo, force);
    }

    private static InstallResult InstallComponentInternal(string componentName, ShellUIConfig config, ProjectInfo projectInfo, bool force)
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

    private static void InstallComponentWithDependencies(string componentName, ShellUIConfig config, ProjectInfo projectInfo, bool force, HashSet<string> installedSet, ref int successCount, ref int skippedCount, List<string> failedComponents)
    {
        if (installedSet.Contains(componentName))
            return; // Already processed
        
        if (!ComponentRegistry.Exists(componentName))
        {
            AnsiConsole.MarkupLine($"[red]Component '{componentName}' not found[/]");
            failedComponents.Add(componentName);
            return;
        }

        var metadata = ComponentRegistry.GetMetadata(componentName);
        if (metadata == null)
        {
            AnsiConsole.MarkupLine($"[red]Failed to get metadata for '{componentName}'[/]");
            failedComponents.Add(componentName);
            return;
        }

        // Install dependencies first
        if (metadata.Dependencies?.Any() == true)
        {
            AnsiConsole.MarkupLine($"[dim]Installing dependencies for [bold]{componentName}[/]: {string.Join(", ", metadata.Dependencies)}[/]");
            foreach (var dep in metadata.Dependencies)
            {
                if (!installedSet.Contains(dep))
                {
                    InstallComponentWithDependencies(dep, config, projectInfo, force, installedSet, ref successCount, ref skippedCount, failedComponents);
                }
            }
        }

        // Install the component itself
        var result = InstallComponentInternal(componentName, config, projectInfo, force);
        
        if (result == InstallResult.Success)
        {
            successCount++;
            installedSet.Add(componentName);
        }
        else if (result == InstallResult.Skipped)
        {
            skippedCount++;
            installedSet.Add(componentName);
        }
        else
        {
            failedComponents.Add(componentName);
        }
    }

    private enum InstallResult
    {
        Success,
        Skipped,
        Failed
    }
}

