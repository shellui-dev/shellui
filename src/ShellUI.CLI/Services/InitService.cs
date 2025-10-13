using ShellUI.Core.Models;
using System.Text.Json;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public class InitService
{
    public static void Initialize(string style, bool force)
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");
        
        if (File.Exists(configPath) && !force)
        {
            AnsiConsole.MarkupLine("[yellow]ShellUI is already initialized in this project.[/]");
            AnsiConsole.MarkupLine("[dim]Use --force to reinitialize[/]");
            return;
        }

        AnsiConsole.Status()
            .Start("Initializing ShellUI...", ctx =>
            {
                // Step 1: Detect project
                ctx.Status("Detecting project type...");
                var projectInfo = ProjectDetector.DetectProject();
                AnsiConsole.MarkupLine($"[green]Detected:[/] {projectInfo.ProjectType}");
                AnsiConsole.MarkupLine($"[dim]Project: {projectInfo.ProjectName}[/]");
                AnsiConsole.MarkupLine($"[dim]Namespace: {projectInfo.RootNamespace}[/]");

                // Step 2: Create Components/UI folder
                ctx.Status("Creating component folders...");
                var componentsPath = Path.Combine(Directory.GetCurrentDirectory(), "Components", "UI");
                Directory.CreateDirectory(componentsPath);
                AnsiConsole.MarkupLine($"[green]Created:[/] Components/UI/");

                // Step 3: Create shellui.json
                ctx.Status("Creating configuration...");
                var config = new ShellUIConfig
                {
                    Style = style,
                    ComponentsPath = "Components/UI",
                    ProjectType = projectInfo.ProjectType,
                    Tailwind = new TailwindConfig
                    {
                        Enabled = true,
                        Version = "4.1.0",
                        CssPath = "wwwroot/app.css"
                    }
                };

                var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(configPath, json);
                AnsiConsole.MarkupLine($"[green]Created:[/] shellui.json");

                // Step 4: Create _Imports.razor if it doesn't exist
                ctx.Status("Setting up imports...");
                var importsPath = Path.Combine(Directory.GetCurrentDirectory(), "Components", "_Imports.razor");
                if (File.Exists(importsPath))
                {
                    var importsContent = File.ReadAllText(importsPath);
                    var usingStatement = $"@using {projectInfo.RootNamespace}.Components.UI";
                    
                    if (!importsContent.Contains(usingStatement))
                    {
                        File.AppendAllText(importsPath, $"\n{usingStatement}\n");
                        AnsiConsole.MarkupLine($"[green]Updated:[/] Components/_Imports.razor");
                    }
                }
            });

        AnsiConsole.MarkupLine("\n[green]ShellUI initialized successfully![/]");
        AnsiConsole.MarkupLine("\n[blue]Next steps:[/]");
        AnsiConsole.MarkupLine("  [dim]1. Add components:[/] dotnet shellui add button");
        AnsiConsole.MarkupLine("  [dim]2. Browse all:[/] dotnet shellui list");
    }
}

