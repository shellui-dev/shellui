using ShellUI.Core.Models;
using ShellUI.Templates;
using System.Text.Json;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public class ComponentInstaller
{
    private const string ShellUiJsSidebarApiMarker = "initSidebar: function (handle, dotNetRef)";

    // Returns false when anything the consumer needs to compile is missing: an unknown or
    // failed component (including a dependency) or a NuGet package that could not be added.
    public static async Task<bool> InstallComponents(string[] components, bool force)
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");

        if (!File.Exists(configPath))
        {
            AnsiConsole.MarkupLine("[red]ShellUI not initialized![/]");
            AnsiConsole.MarkupLine("[yellow]Run 'shellui init' first[/]");
            return false;
        }

        // Load config
        var configJson = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<ShellUIConfig>(configJson);

        if (config == null)
        {
            AnsiConsole.MarkupLine("[red]Failed to read shellui.json[/]");
            return false;
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
        // Track NuGet packages requested this batch so we don't re-invoke `dotnet add package` for the same dep
        var requestedPackages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var pendingNuGetDeps = new List<NuGetDependency>();

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
                    InstallComponentWithDependencies(componentName, config, projectInfo, force, installedSet, requestedPackages, pendingNuGetDeps, ref successCount, ref skippedCount, failedComponents);
                }
                return Task.CompletedTask;
            });

        // Install collected NuGet dependencies once, after all source files are in place
        // (so `dotnet add package` doesn't restore between every component).
        var failedPackages = await InstallNuGetDependenciesAsync(projectInfo, pendingNuGetDeps);

        // Wire any installed wwwroot/ stylesheets into the host so the user doesn't
        // have to add <link> tags by hand. Detected via FilePath, which uses the
        // `../../wwwroot/` traversal trick that asset templates already follow.
        foreach (var name in installedSet)
        {
            var metadata = ComponentRegistry.GetMetadata(name);
            if (metadata == null) continue;
            var href = ResolveHostStylesheetHref(metadata.FilePath);
            if (href != null)
            {
                await InitService.InjectStylesheetIntoHostAsync(href);
            }
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
        foreach (var pkg in failedPackages)
            AnsiConsole.MarkupLine($"[red]Missing NuGet package:[/] {pkg.PackageId} — run `dotnet add package {pkg.PackageId} --version {pkg.Version}`");

        return failedComponents.Count == 0 && failedPackages.Count == 0;
    }

    public static Task<bool> InstallComponentForInitAsync(
        string componentName,
        ProjectInfo projectInfo,
        ShellUIConfig? config = null)
    {
        var metadata = ComponentRegistry.Components.GetValueOrDefault(componentName.ToLower());
        if (metadata == null) return Task.FromResult(false);

        var installConfig = config ?? new ShellUIConfig
        {
            ComponentsPath = "Components/UI",
            ProjectType = projectInfo.ProjectType,
            Style = "default"
        };

        var result = InstallComponentInternal(componentName, installConfig, projectInfo, false);

        if (result == InstallResult.Success)
        {
            AnsiConsole.MarkupLine($"[green]✅ Installed:[/] {componentName}");
        }
        else if (result == InstallResult.Failed)
        {
            AnsiConsole.MarkupLine($"[red]Failed to install:[/] {componentName}");
        }

        return Task.FromResult(result != InstallResult.Failed);
    }

    public static void InstallComponent(string componentName, ComponentMetadata metadata, bool force, bool skipConfig = false)
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");
        var configJson = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<ShellUIConfig>(configJson);

        if (config == null) return;

        var projectInfo = ProjectDetector.DetectProject();
        var result = InstallComponentInternal(componentName, config, projectInfo, force);

        if (!skipConfig && result != InstallResult.Failed)
        {
            var updatedJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configPath, updatedJson);
        }
    }

    public static bool EnsureShellUiJs()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");
        if (!File.Exists(configPath)) return false;

        var config = JsonSerializer.Deserialize<ShellUIConfig>(File.ReadAllText(configPath));
        if (config == null) return false;

        var metadata = ComponentRegistry.GetMetadata("shellui-js");
        if (metadata == null) return false;

        var result = InstallComponentInternal("shellui-js", config, ProjectDetector.DetectProject(), force: false);
        if (result == InstallResult.Failed) return false;

        var updatedJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(configPath, updatedJson);
        return true;
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

        var content = ComponentRegistry.GetComponentContent(componentName);
        if (content == null)
        {
            AnsiConsole.MarkupLine($"[red]Failed to get content for '{componentName}'[/]");
            return InstallResult.Failed;
        }

        var basePath = metadata.IsLayoutBlock
            ? Path.Combine(Directory.GetCurrentDirectory(), config.LayoutPath ?? "Components/Layout")
            : Path.Combine(Directory.GetCurrentDirectory(), config.ComponentsPath);
        var componentPath = Path.GetFullPath(Path.Combine(basePath, metadata.FilePath));
        var existing = config.InstalledComponents.FirstOrDefault(c => c.Name == componentName);

        if (File.Exists(componentPath) && !force)
        {
            if (componentName.Equals("shellui-js", StringComparison.OrdinalIgnoreCase) &&
                !File.ReadAllText(componentPath).Contains(ShellUiJsSidebarApiMarker, StringComparison.Ordinal))
            {
                if (existing?.IsCustomized == true)
                {
                    AnsiConsole.MarkupLine("[yellow]shellui.js is customized and does not expose the sidebar API; update it before installing Sidebar.[/]");
                    return InstallResult.Failed;
                }

                content = content.Replace("YourProjectNamespace", projectInfo.RootNamespace);
                File.WriteAllText(componentPath, content);
                RecordInstalledComponent(config, metadata, componentName, resetCustomization: true);
                AnsiConsole.MarkupLine($"[green]Updated '{componentName}'[/] [dim](added sidebar interop)[/]");
                return InstallResult.Success;
            }

            RecordInstalledComponent(config, metadata, componentName, resetCustomization: false);
            AnsiConsole.MarkupLine($"[yellow]Skipped '{componentName}' (already exists)[/]");
            return InstallResult.Skipped;
        }

        content = content.Replace("YourProjectNamespace", projectInfo.RootNamespace);

        var directory = Path.GetDirectoryName(componentPath);
        if (directory != null)
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(componentPath, content);
        RecordInstalledComponent(config, metadata, componentName, resetCustomization: true);
        AnsiConsole.MarkupLine($"[green]Installed '{componentName}'[/] [dim]({metadata.FilePath})[/]");
        return InstallResult.Success;
    }

    private static void RecordInstalledComponent(
        ShellUIConfig config,
        ComponentMetadata metadata,
        string componentName,
        bool resetCustomization)
    {
        var existing = config.InstalledComponents.FirstOrDefault(c => c.Name == componentName);
        if (existing != null)
        {
            existing.Version = metadata.Version;
            if (resetCustomization) existing.IsCustomized = false;
            return;
        }

        config.InstalledComponents.Add(new InstalledComponent
        {
            Name = componentName,
            Version = metadata.Version,
            InstalledAt = DateTime.UtcNow,
            IsCustomized = false
        });
    }

    private static bool InstallComponentWithDependencies(
        string componentName,
        ShellUIConfig config,
        ProjectInfo projectInfo,
        bool force,
        HashSet<string> installedSet,
        HashSet<string> requestedPackages,
        List<NuGetDependency> pendingNuGetDeps,
        ref int successCount,
        ref int skippedCount,
        List<string> failedComponents)
    {
        if (installedSet.Contains(componentName)) return true;

        if (!ComponentRegistry.Exists(componentName))
        {
            AnsiConsole.MarkupLine($"[red]Component '{componentName}' not found[/]");
            var suggestion = ComponentRegistry.FindClosestMatch(componentName);
            if (suggestion != null)
            {
                AnsiConsole.MarkupLine($"[yellow]Did you mean '[bold]{suggestion}[/]'?[/]");
            }
            failedComponents.Add(componentName);
            return false;
        }

        var metadata = ComponentRegistry.GetMetadata(componentName);
        if (metadata == null)
        {
            AnsiConsole.MarkupLine($"[red]Failed to get metadata for '{componentName}'[/]");
            failedComponents.Add(componentName);
            return false;
        }

        if (metadata.Dependencies?.Any() == true)
        {
            AnsiConsole.MarkupLine($"[dim]Installing dependencies for [bold]{componentName}[/]: {string.Join(", ", metadata.Dependencies)}[/]");
            foreach (var dep in metadata.Dependencies)
            {
                if (!installedSet.Contains(dep) &&
                    !InstallComponentWithDependencies(
                        dep,
                        config,
                        projectInfo,
                        force,
                        installedSet,
                        requestedPackages,
                        pendingNuGetDeps,
                        ref successCount,
                        ref skippedCount,
                        failedComponents))
                {
                    failedComponents.Add(componentName);
                    return false;
                }
            }
        }

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
            return false;
        }

        if (metadata.NuGetDependencies?.Any() == true)
        {
            foreach (var pkg in metadata.NuGetDependencies)
            {
                if (requestedPackages.Add(pkg.PackageId))
                {
                    pendingNuGetDeps.Add(pkg);
                }
            }
        }

        return true;
    }

    // Returns the packages that could not be added.
    private static async Task<List<NuGetDependency>> InstallNuGetDependenciesAsync(ProjectInfo projectInfo, List<NuGetDependency> deps)
    {
        var failed = new List<NuGetDependency>();
        if (deps.Count == 0) return failed;

        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine($"[cyan]Adding {deps.Count} NuGet package reference(s)...[/]");

        foreach (var dep in deps)
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "dotnet",
                ArgumentList = { "add", projectInfo.ProjectPath, "package", dep.PackageId, "--version", dep.Version },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using var process = System.Diagnostics.Process.Start(startInfo);
                if (process == null)
                {
                    AnsiConsole.MarkupLine($"[yellow]Warning:[/] could not start `dotnet add package` for {dep.PackageId}");
                    failed.Add(dep);
                    continue;
                }

                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    AnsiConsole.MarkupLine($"[green]Added:[/] {dep.PackageId} {dep.Version}");
                }
                else
                {
                    var err = (await process.StandardError.ReadToEndAsync()).Trim();
                    AnsiConsole.MarkupLine($"[yellow]Warning:[/] failed to add {dep.PackageId}: {err.Replace("[", "[[").Replace("]", "]]")}");
                    failed.Add(dep);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[yellow]Warning:[/] could not add {dep.PackageId}: {ex.Message.Replace("[", "[[").Replace("]", "]]")}");
                failed.Add(dep);
            }
        }

        return failed;
    }

    internal static bool IsShellUiJsCompatible()
    {
        var metadata = ComponentRegistry.GetMetadata("shellui-js");
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");
        if (metadata == null || !File.Exists(configPath)) return false;

        try
        {
            var config = JsonSerializer.Deserialize<ShellUIConfig>(File.ReadAllText(configPath));
            if (config == null) return false;

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), config.ComponentsPath);
            var shellUiPath = Path.GetFullPath(Path.Combine(basePath, metadata.FilePath));
            return File.Exists(shellUiPath) &&
                File.ReadAllText(shellUiPath).Contains(ShellUiJsSidebarApiMarker, StringComparison.Ordinal);
        }
        catch
        {
            return false;
        }
    }

    // Returns the href that should appear in the host's <link> tag, or null if the
    // component's FilePath isn't a CSS asset under wwwroot/. Strips the `../../wwwroot/`
    // prefix that asset templates use to escape Components/UI/.
    internal static string? ResolveHostStylesheetHref(string filePath)
    {
        if (!filePath.EndsWith(".css", StringComparison.OrdinalIgnoreCase)) return null;
        var normalized = filePath.Replace('\\', '/');
        const string prefix = "../../wwwroot/";
        if (!normalized.StartsWith(prefix, StringComparison.Ordinal)) return null;
        return normalized.Substring(prefix.Length);
    }

    private enum InstallResult
    {
        Success,
        Skipped,
        Failed
    }
}

