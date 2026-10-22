using System.Text.Json;
using System.Text.RegularExpressions;
using ShellUI.Core.Models;
using ShellUI.Templates;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public enum DoctorSeverity { Ok, Warning, Error }

public record DoctorFinding(DoctorSeverity Severity, string Message, string? Fix = null);

public static class DoctorService
{
    public static int Run()
    {
        var findings = Diagnose(Directory.GetCurrentDirectory());
        foreach (var finding in findings)
        {
            var (mark, color) = finding.Severity switch
            {
                DoctorSeverity.Ok => ("✓", "green"),
                DoctorSeverity.Warning => ("!", "yellow"),
                _ => ("✗", "red"),
            };
            AnsiConsole.MarkupLine($"[{color}]{mark}[/] {Markup.Escape(finding.Message)}");
            if (finding.Fix != null)
                AnsiConsole.MarkupLine($"  [dim]{Markup.Escape(finding.Fix)}[/]");
        }

        var errors = findings.Count(f => f.Severity == DoctorSeverity.Error);
        var warnings = findings.Count(f => f.Severity == DoctorSeverity.Warning);
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine(errors + warnings == 0
            ? "[green]No problems found.[/]"
            : $"[{(errors > 0 ? "red" : "yellow")}]{errors} problem(s), {warnings} warning(s).[/]");
        return errors > 0 ? 1 : 0;
    }

    public static List<DoctorFinding> Diagnose(string root)
    {
        var findings = new List<DoctorFinding>();
        var csproj = Directory.GetFiles(root, "*.csproj").FirstOrDefault();
        if (csproj == null)
        {
            findings.Add(new(DoctorSeverity.Error, "No .csproj in this folder.", "Run shellui doctor from the Blazor project's folder."));
            return findings;
        }

        var configPath = Path.Combine(root, "shellui.json");
        if (!File.Exists(configPath))
        {
            findings.Add(new(DoctorSeverity.Error, "shellui.json is missing, so ShellUI isn't set up here.", "Run shellui init."));
            return findings;
        }

        ShellUIConfig? config;
        try
        {
            config = JsonSerializer.Deserialize<ShellUIConfig>(File.ReadAllText(configPath));
        }
        catch (JsonException ex)
        {
            findings.Add(new(DoctorSeverity.Error, $"shellui.json isn't valid JSON: {ex.Message}", "Fix the file, or run shellui init --force to write a new one."));
            return findings;
        }
        if (config == null)
        {
            findings.Add(new(DoctorSeverity.Error, "shellui.json is empty.", "Run shellui init --force."));
            return findings;
        }
        findings.Add(new(DoctorSeverity.Ok, "shellui.json is valid."));

        var componentsDir = Path.Combine(root, config.ComponentsPath);
        findings.Add(Directory.Exists(componentsDir)
            ? new(DoctorSeverity.Ok, $"{config.ComponentsPath} exists.")
            : new(DoctorSeverity.Error, $"ComponentsPath {config.ComponentsPath} doesn't exist.", "Fix ComponentsPath in shellui.json, or run shellui update to reinstall the components."));

        var project = ProjectDetector.DetectProject(root);
        var projectFile = File.ReadAllText(csproj);
        if (Regex.IsMatch(projectFile, @"<PackageReference\s+Include=""ShellUI\.Components""", RegexOptions.IgnoreCase))
            findings.Add(new(DoctorSeverity.Warning, "The project uses both the ShellUI.Components package and CLI-installed components, so components with the same name are ambiguous.",
                "Use one: remove the ShellUI.Components PackageReference, or remove shellui.json and Components/UI."));

        CheckTailwind(root, config, findings);
        CheckHost(root, project, findings);
        CheckComponents(root, config, project, findings);
        return findings;
    }

    private static void CheckTailwind(string root, ShellUIConfig config, List<DoctorFinding> findings)
    {
        if (config.Tailwind.Method == "npm")
        {
            findings.Add(Directory.Exists(Path.Combine(root, "node_modules", "@tailwindcss", "cli"))
                ? new(DoctorSeverity.Ok, "Tailwind CLI (npm) is installed.")
                : new(DoctorSeverity.Error, "The Tailwind npm packages aren't installed.", "Run npm install."));
        }
        else
        {
            var projectCopy = TailwindDownloader.GetTailwindPath(root);
            var shared = TailwindDownloader.CachedPath();
            findings.Add(File.Exists(projectCopy) || File.Exists(shared)
                ? new(DoctorSeverity.Ok, $"Tailwind CLI found ({(File.Exists(projectCopy) ? projectCopy : shared)}).")
                : new(DoctorSeverity.Warning, "The Tailwind CLI isn't downloaded yet.", "Run dotnet build; it downloads the CLI and builds wwwroot/app.css."));

            var targets = Path.Combine(root, "Build", "ShellUI.targets");
            if (!File.Exists(targets))
                findings.Add(new(DoctorSeverity.Error, "Build/ShellUI.targets is missing, so dotnet build doesn't rebuild app.css.", "Run shellui init --force."));
        }

        var input = Path.Combine(root, "wwwroot", "input.css");
        var output = Path.Combine(root, config.Tailwind.CssPath);
        if (!File.Exists(output))
        {
            findings.Add(new(DoctorSeverity.Error, $"{config.Tailwind.CssPath} doesn't exist.", "Run dotnet build."));
            return;
        }

        var built = File.GetLastWriteTimeUtc(output);
        var componentsDir = Path.Combine(root, config.ComponentsPath);
        var newer = new[] { input }.Where(File.Exists)
            .Concat(Directory.Exists(componentsDir) ? Directory.EnumerateFiles(componentsDir, "*.razor", SearchOption.AllDirectories) : Enumerable.Empty<string>())
            .Any(f => File.GetLastWriteTimeUtc(f) > built);
        findings.Add(newer
            ? new(DoctorSeverity.Warning, $"{config.Tailwind.CssPath} is older than input.css or a component, so new classes may be missing.", "Run dotnet build.")
            : new(DoctorSeverity.Ok, $"{config.Tailwind.CssPath} is up to date."));
    }

    private static void CheckHost(string root, ProjectInfo project, List<DoctorFinding> findings)
    {
        var appRazor = Path.Combine(root, "Components", "App.razor");
        var indexHtml = Path.Combine(root, "wwwroot", "index.html");
        var host = File.Exists(appRazor) ? appRazor : File.Exists(indexHtml) ? indexHtml : null;
        if (host == null)
        {
            if (InitService.RazorClassLibraryAssetBase(project.ProjectPath) != null)
                findings.Add(new(DoctorSeverity.Ok, "Razor class library: the host page checks apply to each app that uses it.", $"See {InitService.RazorClassLibraryGuide}"));
            else
                findings.Add(new(DoctorSeverity.Warning, "No Components/App.razor or wwwroot/index.html, so the host page wasn't checked."));
            return;
        }

        var name = Path.GetRelativePath(root, host).Replace('\\', '/');
        var content = File.ReadAllText(host);
        findings.Add(content.Contains("app.css", StringComparison.Ordinal)
            ? new(DoctorSeverity.Ok, $"{name} links app.css.")
            : new(DoctorSeverity.Error, $"{name} doesn't link app.css, so components are unstyled.", "Add <link rel=\"stylesheet\" href=\"app.css\" /> to <head>."));
        findings.Add(content.Contains("shellui.js", StringComparison.Ordinal)
            ? new(DoctorSeverity.Ok, $"{name} loads shellui.js.")
            : new(DoctorSeverity.Error, $"{name} doesn't load shellui.js, so dialogs, dropdowns and the theme toggle can't call it.", "Add <script src=\"shellui.js\"></script> before the Blazor script, or run shellui init --force."));
        findings.Add(content.Contains("ShellUI theme bootstrap", StringComparison.Ordinal) || content.Contains("localStorage.getItem('theme')", StringComparison.Ordinal)
            ? new(DoctorSeverity.Ok, $"{name} applies the saved theme before first paint.")
            : new(DoctorSeverity.Warning, $"{name} has no theme script, so dark mode flashes light on load.", "Run shellui init --force, or copy the theme script from a new init."));

        if (host == indexHtml) return;
        if (content.Contains("@rendermode", StringComparison.Ordinal))
            findings.Add(new(DoctorSeverity.Ok, "App.razor sets an interactive render mode."));
        else if (Directory.EnumerateFiles(Path.Combine(root, "Components"), "*.razor", SearchOption.AllDirectories)
                     .Any(f => File.ReadAllText(f).Contains("@rendermode", StringComparison.Ordinal)))
            findings.Add(new(DoctorSeverity.Warning, "Only some pages set a render mode; ShellUI components don't respond to clicks on the others.",
                "Set it app-wide: <Routes @rendermode=\"InteractiveServer\" /> in App.razor."));
        else
            findings.Add(new(DoctorSeverity.Error, "No interactive render mode, so ShellUI components render but don't respond to clicks.",
                "Add @rendermode=\"InteractiveServer\" to <Routes /> and <HeadOutlet /> in App.razor."));
    }

    private static void CheckComponents(string root, ShellUIConfig config, ProjectInfo project, List<DoctorFinding> findings)
    {
        var installed = config.InstalledComponents.Select(c => c.Name).ToList();
        var missingFiles = config.InstalledComponents
            .Where(c => ComponentRegistry.GetMetadata(c.Name) is { } m && !File.Exists(ComponentInstaller.InstalledFilePath(m, config, root)))
            .Select(c => c.Name).ToList();
        if (missingFiles.Count > 0)
            findings.Add(new(DoctorSeverity.Error, $"Installed but the file is gone: {string.Join(", ", missingFiles)}.", $"Run shellui add {string.Join(" ", missingFiles)} --force, or shellui remove them."));

        var missingDeps = ComponentManager.MissingDependencies(installed, installed);
        findings.Add(missingDeps.Count == 0
            ? new(DoctorSeverity.Ok, $"{installed.Count} installed component(s), none missing a dependency.")
            : new(DoctorSeverity.Error, $"Missing dependencies: {string.Join(", ", missingDeps)}.", "Run shellui update; it installs them."));

        var imports = new[] { Path.Combine(root, "Components", "_Imports.razor"), Path.Combine(root, "_Imports.razor") }.FirstOrDefault(File.Exists);
        if (imports == null)
        {
            findings.Add(new(DoctorSeverity.Warning, "No _Imports.razor, so pages need their own @using for the components."));
            return;
        }
        var lines = File.ReadAllLines(imports).Select(l => l.Trim()).ToHashSet();
        var contents = installed.Where(n => ComponentRegistry.GetMetadata(n) is { IsLayoutBlock: false })
            .Select(ComponentRegistry.GetComponentContent).OfType<string>();
        var required = new[] { $"@using {project.RootNamespace}.Components.UI" }
            .Concat(ComponentInstaller.RequiredImports(project.RootNamespace, contents));
        var missingUsings = required.Where(u => !lines.Contains(u)).ToList();
        var importsName = Path.GetRelativePath(root, imports).Replace('\\', '/');
        findings.Add(missingUsings.Count == 0
            ? new(DoctorSeverity.Ok, $"{importsName} has the ShellUI usings.")
            : new(DoctorSeverity.Error, $"{importsName} is missing {string.Join(", ", missingUsings)}.", "Add those lines to it."));
    }
}
