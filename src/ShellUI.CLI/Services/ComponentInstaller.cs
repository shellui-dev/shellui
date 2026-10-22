using ShellUI.Core.Models;
using ShellUI.Templates;
using System.Text.Json;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public class ComponentInstaller
{
    private const string ShellUiJsSidebarApiMarker = "initSidebar: function (handle, dotNetRef)";

    public static async Task<bool> InstallComponents(string[] components, bool force, LayoutSwitch layoutSwitch = LayoutSwitch.Ask, string[]? except = null)
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");

        if (!File.Exists(configPath))
        {
            AnsiConsole.MarkupLine("[red]ShellUI not initialized![/]");
            AnsiConsole.MarkupLine("[yellow]Run 'shellui init' first[/]");
            return false;
        }

        var configJson = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<ShellUIConfig>(configJson);

        if (config == null)
        {
            AnsiConsole.MarkupLine("[red]Failed to read shellui.json[/]");
            return false;
        }

        var projectInfo = ProjectDetector.DetectProject();

        var componentList = new List<string>();
        foreach (var comp in components)
        {
            componentList.AddRange(comp.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }
        var addAll = componentList.RemoveAll(c => c.Equals("all", StringComparison.OrdinalIgnoreCase)) > 0;
        if (addAll)
            componentList.AddRange(LibraryComponents().Where(name => !componentList.Contains(name, StringComparer.OrdinalIgnoreCase)));
        if (except is { Length: > 0 })
        {
            var skip = except.SelectMany(e => e.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var name in skip.Where(n => ComponentRegistry.GetMetadata(n) == null))
                AnsiConsole.MarkupLine($"[yellow]Unknown component in --except:[/] {Markup.Escape(name)}");
            componentList.RemoveAll(skip.Contains);
            // Leaving out chart also leaves out bar-chart and the rest, so its NuGet package isn't added.
            var dependents = componentList.Where(name => DependsOnAny(name, skip)).ToList();
            if (dependents.Count > 0)
            {
                componentList.RemoveAll(dependents.Contains);
                AnsiConsole.MarkupLine($"[dim]Also left out, because they need an excluded component: {Markup.Escape(string.Join(", ", dependents))}[/]");
            }
        }
        if (addAll)
            AnsiConsole.MarkupLine($"[blue]Adding all {componentList.Count} components[/] [dim](layout blocks such as dashboard-02 and auth-01 are left out; add them by name)[/]");
        if (componentList.Any(DashboardSetup.Layouts.ContainsKey) && DashboardSetup.IsIdentityApp(Directory.GetCurrentDirectory()))
            componentList.Add("sidebar-account");

        var successCount = 0;
        var skippedCount = 0;
        var failedComponents = new List<string>();

        var installedSet = new HashSet<string>();
        var requestedPackages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var pendingNuGetDeps = new List<NuGetDependency>();

        foreach (var componentName in addAll ? Enumerable.Empty<string>() : componentList)
        {
            var metadata = ComponentRegistry.GetMetadata(componentName);
            if (metadata != null && metadata.Dependencies?.Any() == true)
            {
                AnsiConsole.MarkupLine($"[green]●[/] [bold]{componentName}[/] requires: [yellow]{string.Join(", ", metadata.Dependencies)}[/]");
            }
        }

        AnsiConsole.MarkupLine("");

        await Loaders.SnakeStatus()
            .StartAsync("Installing components...", ctx =>
            {
                foreach (var componentName in componentList)
                {
                    ctx.Status($"Installing {componentName}...");
                    InstallComponentWithDependencies(componentName, config, projectInfo, force, installedSet, requestedPackages, pendingNuGetDeps, ref successCount, ref skippedCount, failedComponents);
                }
                return Task.CompletedTask;
            });

        // Once, after all files are written, so restore doesn't run per component.
        var failedPackages = await InstallNuGetDependenciesAsync(projectInfo, pendingNuGetDeps);

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

        EnsureImports(projectInfo, installedSet);

        foreach (var dashboard in componentList.Where(c => DashboardSetup.Layouts.ContainsKey(c) && installedSet.Contains(c)))
        {
            await DashboardSetup.WireAsync(dashboard, projectInfo, config, layoutSwitch);
        }
        foreach (var auth in componentList.Where(c => AuthSetup.Layouts.ContainsKey(c) && installedSet.Contains(c)))
        {
            await AuthSetup.WireAsync(auth, projectInfo, config);
        }

        var updatedJson = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(configPath, updatedJson);

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

    internal static bool DependsOnAny(string name, IReadOnlySet<string> names, HashSet<string>? seen = null)
    {
        seen ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (!seen.Add(name)) return false;
        var dependencies = ComponentRegistry.GetMetadata(name)?.Dependencies ?? new List<string>();
        return dependencies.Any(d => names.Contains(d) || DependsOnAny(d, names, seen));
    }

    // What `add all` installs: every target `list` shows except the layout blocks, which rewire the app.
    internal static IEnumerable<string> LibraryComponents() =>
        ComponentRegistry.Components.Values
            .Where(m => m.IsAvailable && !m.IsLayoutBlock)
            .Select(m => m.Name)
            .OrderBy(name => name, StringComparer.Ordinal);

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

    public static bool InstallComponent(string componentName, ComponentMetadata metadata, bool force, bool skipConfig = false)
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");
        var configJson = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<ShellUIConfig>(configJson);

        if (config == null) return false;

        var projectInfo = ProjectDetector.DetectProject();
        var result = InstallComponentInternal(componentName, config, projectInfo, force);

        if (!skipConfig && result != InstallResult.Failed)
        {
            var updatedJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configPath, updatedJson);
        }
        return result != InstallResult.Failed;
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

        var componentPath = InstalledFilePath(metadata, config);
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
                RecordInstalledComponent(config, metadata, componentName, resetCustomization: true, ContentHash(content));
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
        RecordInstalledComponent(config, metadata, componentName, resetCustomization: true, ContentHash(content));
        AnsiConsole.MarkupLine($"[green]Installed '{componentName}'[/] [dim]({metadata.FilePath})[/]");
        return InstallResult.Success;
    }

    internal static string InstalledFilePath(ComponentMetadata metadata, ShellUIConfig config)
    {
        var basePath = metadata.IsLayoutBlock
            ? Path.Combine(Directory.GetCurrentDirectory(), config.LayoutPath ?? "Components/Layout")
            : Path.Combine(Directory.GetCurrentDirectory(), config.ComponentsPath);
        return Path.GetFullPath(Path.Combine(basePath, metadata.FilePath));
    }

    // The template as install would write it into this project.
    internal static string? RenderedContent(string componentName, string rootNamespace) =>
        ComponentRegistry.GetComponentContent(componentName)?.Replace("YourProjectNamespace", rootNamespace);

    // Line endings are normalized so a checkout that converts CRLF doesn't count as an edit.
    public static string ContentHash(string content)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(content.Replace("\r\n", "\n"));
        return Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(bytes)).ToLowerInvariant();
    }

    private static void RecordInstalledComponent(
        ShellUIConfig config,
        ComponentMetadata metadata,
        string componentName,
        bool resetCustomization,
        string? hash = null)
    {
        var existing = config.InstalledComponents.FirstOrDefault(c => c.Name == componentName);
        if (existing != null)
        {
            existing.Version = metadata.Version;
            if (resetCustomization) existing.IsCustomized = false;
            if (hash != null) existing.Hash = hash;
            return;
        }

        config.InstalledComponents.Add(new InstalledComponent
        {
            Name = componentName,
            Version = metadata.Version,
            InstalledAt = DateTime.UtcNow,
            IsCustomized = false,
            Hash = hash
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

    private static async Task<List<NuGetDependency>> InstallNuGetDependenciesAsync(ProjectInfo projectInfo, List<NuGetDependency> deps)
    {
        var failed = new List<NuGetDependency>();

        // dotnet add package --version sets that version even when the project has a newer one, so only add or raise.
        var needed = new List<NuGetDependency>();
        foreach (var dep in deps)
        {
            var existing = ExistingPackageVersion(projectInfo.ProjectPath, dep.PackageId);
            if (existing != null && SatisfiesMinimum(existing, dep.Version))
                AnsiConsole.MarkupLine($"[dim]Kept {dep.PackageId} {(existing.Length > 0 ? existing : "(version set elsewhere)")}; ShellUI needs {dep.Version} or later[/]");
            else
                needed.Add(dep);
        }
        deps = needed;
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

    // The version a project already references: from its PackageReference, or Directory.Packages.props under central
    // package management. "" when referenced without a version we can find, null when not referenced.
    internal static string? ExistingPackageVersion(string projectPath, string packageId)
    {
        if (!File.Exists(projectPath)) return null;
        bool Matches(System.Xml.Linq.XElement e) =>
            string.Equals((string?)e.Attribute("Include") ?? (string?)e.Attribute("Update"), packageId, StringComparison.OrdinalIgnoreCase);
        string? VersionOf(System.Xml.Linq.XElement e) =>
            (string?)e.Attribute("VersionOverride") ?? (string?)e.Attribute("Version")
            ?? e.Elements().FirstOrDefault(c => c.Name.LocalName is "VersionOverride" or "Version")?.Value;

        var reference = System.Xml.Linq.XDocument.Load(projectPath).Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "PackageReference" && Matches(e));
        if (reference == null) return null;
        if (VersionOf(reference) is { Length: > 0 } version) return version.Trim();

        for (var dir = Path.GetDirectoryName(Path.GetFullPath(projectPath)); dir != null; dir = Path.GetDirectoryName(dir))
        {
            var props = Path.Combine(dir, "Directory.Packages.props");
            if (!File.Exists(props)) continue;
            var central = System.Xml.Linq.XDocument.Load(props).Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "PackageVersion" && Matches(e));
            return central != null && VersionOf(central) is { Length: > 0 } pinned ? pinned.Trim() : "";
        }
        return "";
    }

    // Floating versions, ranges and unknown versions count as satisfied: the project chose them, so ShellUI leaves them.
    internal static bool SatisfiesMinimum(string existing, string minimum)
    {
        if (existing.Length == 0 || existing.IndexOfAny(new[] { '*', '[', '(', '$' }) >= 0) return true;
        static (Version Release, string? Pre) Parse(string v)
        {
            var dash = v.IndexOf('-');
            var core = (dash < 0 ? v : v[..dash]).Split('+')[0];
            var parts = core.Split('.').Select(p => int.TryParse(p, out var n) ? n : 0).Concat(new[] { 0, 0, 0, 0 }).Take(4).ToArray();
            return (new Version(parts[0], parts[1], parts[2], parts[3]), dash < 0 ? null : v[(dash + 1)..]);
        }
        var (have, havePre) = Parse(existing);
        var (need, needPre) = Parse(minimum);
        if (have != need) return have > need;
        if (havePre == null || needPre == null) return havePre == null;
        return string.CompareOrdinal(havePre, needPre) >= 0;
    }

    // Pages use helper types such as ButtonVariant from sub-namespaces, imported once an installed file declares them.
    public static IEnumerable<string> RequiredImports(string rootNamespace, IEnumerable<string> templateContents)
    {
        var contents = templateContents.ToList();
        foreach (var ns in new[] { "Components.UI.Variants", "Components.Models" })
        {
            var declaration = new Regex($@"^\s*namespace\s+YourProjectNamespace\.{Regex.Escape(ns)}\s*[;{{]", RegexOptions.Multiline);
            if (contents.Any(c => declaration.IsMatch(c)))
                yield return $"@using {rootNamespace}.{ns}";
        }
    }

    private static void EnsureImports(ProjectInfo projectInfo, IEnumerable<string> installed)
    {
        var importsPath = new[] { Path.Combine("Components", "_Imports.razor"), "_Imports.razor" }
            .Select(p => Path.Combine(Directory.GetCurrentDirectory(), p))
            .FirstOrDefault(File.Exists);
        if (importsPath == null) return;

        var existing = File.ReadAllLines(importsPath).Select(l => l.Trim()).ToHashSet();
        var contents = installed
            .Where(name => ComponentRegistry.GetMetadata(name) is { IsLayoutBlock: false })
            .Select(ComponentRegistry.GetComponentContent)
            .OfType<string>();
        var missing = RequiredImports(projectInfo.RootNamespace, contents).Where(u => !existing.Contains(u)).ToList();
        if (missing.Count == 0) return;

        File.AppendAllText(importsPath, Environment.NewLine + string.Join(Environment.NewLine, missing) + Environment.NewLine);
        AnsiConsole.MarkupLine($"[green]Updated:[/] {Path.GetRelativePath(Directory.GetCurrentDirectory(), importsPath)} ({string.Join(", ", missing)})");
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

    // href for the host's <link>, or null when the file isn't a wwwroot CSS asset.
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

