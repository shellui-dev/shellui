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
        
        AnsiConsole.Write(new FigletText("ShellUI").Color(Color.Blue));
        AnsiConsole.MarkupLine("[dim]Available Components[/]\n");
        
        var table = new Table();
        table.Border(TableBorder.Simple);
        table.AddColumn(new TableColumn("[bold]Component[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Status[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Version[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Category[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Description[/]"));

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

            table.AddRow(
                componentName,
                status,
                $"[dim]{component.Version}[/]",
                $"[blue]{component.Category}[/]",
                $"[dim]{component.Description}[/]"
            );

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

        var config = JsonSerializer.Deserialize<ShellUIConfig>(File.ReadAllText(ConfigFileName));
        if (config == null)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Failed to read shellui.json");
            return;
        }

        var installed = config.InstalledComponents.Select(c => c.Name).ToList();
        var removed = 0;

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

            if (normalizedName == "sidebar-js" && ComponentRegistry.GetMetadata("sidebar-provider") is { } provider)
            {
                var providerPath = ComponentInstaller.InstalledFilePath(provider, config);
                if (File.Exists(providerPath) &&
                    File.ReadAllText(providerPath).Contains("shellui-sidebar.js", StringComparison.Ordinal))
                {
                    AnsiConsole.MarkupLine("[yellow]Warning:[/] sidebar-js cannot be removed while SidebarProvider uses the legacy module");
                    continue;
                }
            }

            var componentPath = ComponentInstaller.InstalledFilePath(metadata, config);
            if (File.Exists(componentPath))
            {
                File.Delete(componentPath);
                AnsiConsole.MarkupLine($"[green]Removed:[/] {metadata.DisplayName}");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Removed:[/] {metadata.DisplayName} [dim](its file was already gone)[/]");
            }
            installed.Remove(normalizedName);
            removed++;

            if (metadata.IsLayoutBlock)
            {
                var layout = Path.GetFileNameWithoutExtension(metadata.FilePath);
                foreach (var file in FilesReferencing(layout))
                    AnsiConsole.MarkupLine($"[yellow]Note:[/] {Markup.Escape(file)} still uses {layout}; point it at another layout.");
            }
        }

        UpdateConfig(installed);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(removed > 0
            ? $"[green]Removed {removed} component(s)[/]"
            : "[yellow]Nothing removed[/]");
    }

    // Razor files that still name a type, so a removed layout doesn't break the build unnoticed.
    internal static IEnumerable<string> FilesReferencing(string typeName)
    {
        var root = Directory.GetCurrentDirectory();
        var pattern = new System.Text.RegularExpressions.Regex($@"\b{System.Text.RegularExpressions.Regex.Escape(typeName)}\b");
        return Directory.EnumerateFiles(root, "*.razor", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
            .Where(f => pattern.IsMatch(File.ReadAllText(f)))
            .Select(f => Path.GetRelativePath(root, f))
            .OrderBy(f => f);
    }

    public static async Task UpdateComponents(string[] componentNames, bool updateAll, bool force = false)
    {
        if (!File.Exists(ConfigFileName))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] ShellUI not initialized. Run 'shellui init' first.");
            return;
        }

        var config = JsonSerializer.Deserialize<ShellUIConfig>(File.ReadAllText(ConfigFileName));
        var installed = config?.InstalledComponents.Select(c => c.Name).ToList() ?? new List<string>();

        foreach (var note in InitService.RefreshTailwindSetup(Directory.GetCurrentDirectory()))
            AnsiConsole.MarkupLine(note);

        if (config == null || installed.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No components installed yet.[/]");
            return;
        }

        var rootNamespace = ProjectDetector.DetectProject().RootNamespace;

        var toUpdate = updateAll || componentNames.Length == 0
            ? installed.ToArray()
            : componentNames;

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule("[blue]Updating Components[/]").RuleStyle("blue dim"));
        AnsiConsole.WriteLine();

        var updated = 0;
        var skipped = 0;
        var failed = new List<string>();
        var updatedNames = new List<string>();
        var customized = new List<string>();

        AnsiConsole.Status().Start("Updating components...", ctx =>
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

                var path = ComponentInstaller.InstalledFilePath(metadata, config);
                if (!force && File.Exists(path))
                {
                    var entry = config.InstalledComponents.First(c => c.Name == normalizedName);
                    if (HasLocalChanges(entry, metadata, File.ReadAllText(path), ComponentInstaller.RenderedContent(normalizedName, rootNamespace)))
                    {
                        AnsiConsole.MarkupLine($"[yellow]Skipped:[/] {metadata.DisplayName} has local changes [dim](use --force to overwrite)[/]");
                        customized.Add(normalizedName);
                        skipped++;
                        continue;
                    }
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
                    updatedNames.Add(normalizedName);
                    updated++;
                }
                else
                {
                    failed.Add(metadata.DisplayName);
                }
            }
        });

        if (customized.Count > 0) MarkCustomized(customized);

        AnsiConsole.WriteLine();
        if (updated > 0)
            AnsiConsole.MarkupLine($"[green]Updated {updated} component(s)[/]");
        if (skipped > 0)
            AnsiConsole.MarkupLine($"[yellow]Skipped {skipped} component(s)[/]");
        if (failed.Count > 0)
            AnsiConsole.MarkupLine($"[red]Failed: {Markup.Escape(string.Join(", ", failed))}[/]");

        // Templates gain dependencies between versions (breadcrumb-list, tab-models); add them like `add` does.
        var missing = MissingDependencies(updatedNames, GetInstalledComponents());
        if (missing.Count > 0)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[blue]Installing new dependencies:[/] {string.Join(", ", missing)}");
            await ComponentInstaller.InstallComponents(missing.ToArray(), force: false);
        }
    }

    // Blocks apps are told to edit. Entries from before hashes were recorded can only be compared with the current template.
    private static bool IsEditableBlock(ComponentMetadata metadata) =>
        metadata.IsLayoutBlock || metadata.Name is "app-sidebar" or "sidebar-account";

    public static bool HasLocalChanges(InstalledComponent entry, ComponentMetadata metadata, string current, string? template)
    {
        var hash = ComponentInstaller.ContentHash(current);
        if (entry.Hash != null) return hash != entry.Hash;
        return IsEditableBlock(metadata) && template != null && hash != ComponentInstaller.ContentHash(template);
    }

    public static List<string> MissingDependencies(IEnumerable<string> components, ICollection<string> installed)
    {
        var missing = new List<string>();
        var seen = new HashSet<string>(installed);
        var queue = new Queue<string>(components);
        while (queue.Count > 0)
        {
            var metadata = ComponentRegistry.GetMetadata(queue.Dequeue());
            if (metadata == null) continue;
            foreach (var dep in metadata.Dependencies)
            {
                if (!seen.Add(dep)) continue;
                missing.Add(dep);
                queue.Enqueue(dep);
            }
        }
        return missing;
    }

    private static void MarkCustomized(List<string> names)
    {
        var config = JsonSerializer.Deserialize<ShellUIConfig>(File.ReadAllText(ConfigFileName));
        if (config == null) return;
        foreach (var entry in config.InstalledComponents.Where(c => names.Contains(c.Name)))
            entry.IsCustomized = true;
        File.WriteAllText(ConfigFileName, JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
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

