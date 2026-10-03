using System.Text.Json;
using Spectre.Console;
using ShellUI.Core.Models;
using ShellUI.Templates;

namespace ShellUI.CLI.Services;

public static class ComponentManager
{
    private const string ConfigFileName = "shellui.json";

    public static List<string> GetInstalledComponents()
    {
        if (!File.Exists(ConfigFileName))
        {
            return new List<string>();
        }

        try
        {
            var json = File.ReadAllText(ConfigFileName);
            var config = JsonSerializer.Deserialize<ShellUIConfig>(json);
            return config?.InstalledComponents?.Select(c => c.Name).ToList() ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    public static void ListComponents(bool showOnlyInstalled, bool showOnlyAvailable)
    {
        var installed = GetInstalledComponents();
        
        LogoLoader.WriteHeader("Components");

        // Narrow terminals wrap every cell into slivers, so drop the widest columns first.
        var width = AnsiConsole.Profile.Width;
        var showDescription = width >= 100;
        var showVersion = width >= 70;

        var table = new Table();
        table.Border(TableBorder.Simple);
        table.AddColumn(new TableColumn("[bold]Component[/]"));
        table.AddColumn(new TableColumn("[bold]Status[/]"));
        if (showVersion) table.AddColumn(new TableColumn("[bold]Version[/]"));
        table.AddColumn(new TableColumn("[bold]Category[/]"));
        if (showDescription) table.AddColumn(new TableColumn("[bold]Description[/]"));

        int installedCount = 0;
        int availableCount = 0;

        foreach (var component in ComponentRegistry.Components.Values.OrderBy(c => c.Category).ThenBy(c => c.Name))
        {
            if (!component.IsAvailable) continue;

            bool isInstalled = installed.Contains(component.Name);
            
            if (showOnlyInstalled && !isInstalled) continue;
            if (showOnlyAvailable && isInstalled) continue;

            var status = isInstalled 
                ? "[green]Installed[/]" 
                : "[dim]Available[/]";
            
            var componentName = isInstalled 
                ? $"[green]{component.Name}[/]" 
                : component.Name;

            var cells = new List<string> { componentName, status };
            if (showVersion) cells.Add($"[dim]{component.Version}[/]");
            cells.Add($"[blue]{component.Category}[/]");
            if (showDescription) cells.Add($"[dim]{Markup.Escape(component.Description)}[/]");
            table.AddRow(cells.ToArray());

            if (isInstalled) installedCount++;
            else availableCount++;
        }
        
        AnsiConsole.Write(table);
        
        AnsiConsole.WriteLine();
        var panel = new Panel(
            new Markup($"[green]{installedCount} installed[/] | [blue]{availableCount} available[/] | [yellow]{ComponentRegistry.Components.Values.Count(c => c.IsAvailable)} total[/]")
        );
        panel.Header = new PanelHeader("[bold]Summary[/]");
        panel.Border = BoxBorder.Rounded;
        AnsiConsole.Write(panel);

        if (!showOnlyInstalled && !showOnlyAvailable)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[dim]Tip: Use 'shellui add <component>' to install a component[/]");
        }
    }

    public static void RemoveComponents(string[] componentNames)
    {
        if (!File.Exists(ConfigFileName))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] ShellUI not initialized. Run 'shellui init' first.");
            return;
        }

        var installed = GetInstalledComponents();
        var componentsPath = "Components/UI";

        if (!Directory.Exists(componentsPath))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Components directory not found.");
            return;
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule("[red]Removing Components[/]").RuleStyle("red dim"));
        AnsiConsole.WriteLine();

        foreach (var componentName in componentNames)
        {
            var normalizedName = componentName.ToLower().Replace(",", "").Trim();
            
            if (!ComponentRegistry.Components.TryGetValue(normalizedName, out var metadata))
            {
                AnsiConsole.MarkupLine($"[yellow]Warning:[/] Unknown component '{componentName}'");
                continue;
            }

            if (!installed.Contains(normalizedName))
            {
                AnsiConsole.MarkupLine($"[yellow]Warning:[/] Component '{metadata.DisplayName}' is not installed");
                continue;
            }

            if (normalizedName == "sidebar-js")
            {
                var providerPath = Path.Combine(componentsPath, "SidebarProvider.razor");
                if (File.Exists(providerPath) &&
                    File.ReadAllText(providerPath).Contains("shellui-sidebar.js", StringComparison.Ordinal))
                {
                    AnsiConsole.MarkupLine("[yellow]Warning:[/] sidebar-js cannot be removed while SidebarProvider uses the legacy module");
                    continue;
                }
            }

            var componentPath = Path.Combine(componentsPath, metadata.FilePath);
            
            if (File.Exists(componentPath))
            {
                File.Delete(componentPath);
                AnsiConsole.MarkupLine($"[green]Removed:[/] {metadata.DisplayName}");
                
                installed.Remove(normalizedName);
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]Warning:[/] File not found: {componentPath}");
            }
        }

        UpdateConfig(installed);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[green]Successfully removed {componentNames.Length} component(s)[/]");
    }

    public static void UpdateComponents(string[] componentNames, bool updateAll)
    {
        if (!File.Exists(ConfigFileName))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] ShellUI not initialized. Run 'shellui init' first.");
            return;
        }

        var installed = GetInstalledComponents();

        if (installed.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No components installed yet.[/]");
            return;
        }

        var toUpdate = updateAll || componentNames.Length == 0 
            ? installed.ToArray() 
            : componentNames;

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule("[blue]Updating Components[/]").RuleStyle("blue dim"));
        AnsiConsole.WriteLine();

        var updated = 0;
        var skipped = 0;
        var failed = new List<string>();

        Loaders.SnakeStatus().Start("Updating components...", ctx =>
        {
            foreach (var componentName in toUpdate)
            {
                var normalizedName = componentName.ToLower().Replace(",", "").Trim();
                ctx.Status($"Updating {normalizedName}...");

                if (!ComponentRegistry.Components.TryGetValue(normalizedName, out var metadata))
                {
                    AnsiConsole.MarkupLine($"[yellow]Warning:[/] Unknown component '{componentName}'");
                    skipped++;
                    continue;
                }

                if (!installed.Contains(normalizedName))
                {
                    AnsiConsole.MarkupLine($"[yellow]Skipped:[/] Component '{metadata.DisplayName}' is not installed");
                    skipped++;
                    continue;
                }

                if (metadata.Dependencies.Contains("shellui-js") &&
                    !ComponentInstaller.IsShellUiJsCompatible() &&
                    !ComponentInstaller.EnsureShellUiJs())
                {
                    AnsiConsole.MarkupLine($"[yellow]Skipped:[/] Component '{metadata.DisplayName}' requires a compatible wwwroot/shellui.js.");
                    skipped++;
                    continue;
                }

                if (ComponentInstaller.InstallComponent(normalizedName, metadata, force: true, skipConfig: false))
                {
                    AnsiConsole.MarkupLine($"[green]Updated:[/] {metadata.DisplayName} to v{metadata.Version}");
                    updated++;
                }
                else
                {
                    failed.Add(metadata.DisplayName);
                }
            }
        });

        AnsiConsole.WriteLine();
        if (updated > 0)
            AnsiConsole.MarkupLine($"[green]Updated {updated} component(s)[/]");
        if (skipped > 0)
            AnsiConsole.MarkupLine($"[yellow]Skipped {skipped} component(s)[/]");
        if (failed.Count > 0)
            AnsiConsole.MarkupLine($"[red]Failed: {Markup.Escape(string.Join(", ", failed))}[/]");
    }

    private static void UpdateConfig(List<string> installedComponentNames)
    {
        try
        {
            var json = File.ReadAllText(ConfigFileName);
            var config = JsonSerializer.Deserialize<ShellUIConfig>(json);
            
            if (config != null)
            {
                config.InstalledComponents = config.InstalledComponents
                    .Where(c => installedComponentNames.Contains(c.Name))
                    .ToList();
                    
                var updatedJson = JsonSerializer.Serialize(config, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(ConfigFileName, updatedJson);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[yellow]Warning:[/] Failed to update config: {ex.Message}");
        }
    }
}

