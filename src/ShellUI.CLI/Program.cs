using System.CommandLine;
using Spectre.Console;
using ShellUI.Templates;
using ShellUI.Core.Models;

namespace ShellUI.CLI;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("ShellUI - CLI-first Blazor component library")
        {
            Description = "Add beautiful, accessible components to your Blazor app. Inspired by shadcn/ui."
        };

        // Add commands
        rootCommand.AddCommand(CreateInitCommand());
        rootCommand.AddCommand(CreateAddCommand());
        rootCommand.AddCommand(CreateListCommand());
        rootCommand.AddCommand(CreateRemoveCommand());
        rootCommand.AddCommand(CreateUpdateCommand());

        return await rootCommand.InvokeAsync(args);
    }

    static Command CreateInitCommand()
    {
        var command = new Command("init", "Initialize ShellUI in your Blazor project");
        
        var forceOption = new Option<bool>(
            "--force",
            "Reinitialize even if already initialized");
        var styleOption = new Option<string>(
            "--style",
            getDefaultValue: () => "default",
            "Choose component style (default, new-york, minimal)");
        
        command.AddOption(forceOption);
        command.AddOption(styleOption);

        command.SetHandler((force, style) =>
        {
            AnsiConsole.Write(new FigletText("ShellUI").Color(Color.Blue));
            AnsiConsole.MarkupLine("[green]Initializing ShellUI...[/]");
            AnsiConsole.MarkupLine($"[dim]Style: {style}[/]");
            
            if (force)
            {
                AnsiConsole.MarkupLine("[yellow]Force mode enabled[/]");
            }
            
            AnsiConsole.MarkupLine("\n[yellow]Coming soon in Milestone 1![/]");
            AnsiConsole.MarkupLine("[dim]This will:[/]");
            AnsiConsole.MarkupLine("[dim]  - Detect your project type (Server/WASM/SSR)[/]");
            AnsiConsole.MarkupLine("[dim]  - Create Components/UI folder[/]");
            AnsiConsole.MarkupLine("[dim]  - Download Tailwind standalone CLI (no Node.js!)[/]");
            AnsiConsole.MarkupLine("[dim]  - Create shellui.json config[/]");
            AnsiConsole.MarkupLine("[dim]  - Set up Tailwind CSS v4[/]");
        }, forceOption, styleOption);

        return command;
    }

    static Command CreateAddCommand()
    {
        var command = new Command("add", "Add component(s) to your project");
        
        var componentsArg = new Argument<string[]>(
            "components",
            "Component name(s) to add (space or comma-separated)")
        {
            Arity = ArgumentArity.OneOrMore
        };
        command.AddArgument(componentsArg);

        var forceOption = new Option<bool>(
            "--force",
            "Overwrite existing components");
        command.AddOption(forceOption);

        command.SetHandler((components, force) =>
        {
            AnsiConsole.MarkupLine("[green]Adding components:[/]\n");
            
            // Parse comma-separated components
            var componentList = new List<string>();
            foreach (var comp in components)
            {
                componentList.AddRange(comp.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            }
            
            var table = new Table();
            table.AddColumn("Component");
            table.AddColumn("Status");
            table.AddColumn("Version");
            
            foreach (var componentName in componentList)
            {
                var exists = ComponentRegistry.Exists(componentName);
                if (exists)
                {
                    var metadata = ComponentRegistry.GetMetadata(componentName);
                    table.AddRow(
                        componentName,
                        "[yellow]Implementation pending[/]",
                        $"[dim]{metadata?.Version ?? "0.1.0"}[/]"
                    );
                }
                else
                {
                    table.AddRow(
                        componentName,
                        "[red]Not found[/]",
                        "[dim]-[/]"
                    );
                }
            }
            
            AnsiConsole.Write(table);
            
            if (force)
            {
                AnsiConsole.MarkupLine("\n[yellow]Force mode: Will overwrite existing components[/]");
            }
            
            AnsiConsole.MarkupLine("\n[dim]Multiple component syntax supported:[/]");
            AnsiConsole.MarkupLine("[dim]  dotnet shellui add button card alert[/]");
            AnsiConsole.MarkupLine("[dim]  dotnet shellui add button,card,alert[/]");
            
            AnsiConsole.MarkupLine("\n[yellow]Component copying implementation coming soon![/]");
        }, componentsArg, forceOption);

        return command;
    }

    static Command CreateListCommand()
    {
        var command = new Command("list", "List available components");

        var installedOption = new Option<bool>(
            "--installed",
            "Show only installed components");
        var availableOption = new Option<bool>(
            "--available",
            "Show only available components");
        
        command.AddOption(installedOption);
        command.AddOption(availableOption);

        command.SetHandler((installed, available) =>
        {
            AnsiConsole.MarkupLine("[blue]ShellUI Components[/]\n");
            
            var table = new Table();
            table.AddColumn("Component");
            table.AddColumn("Version");
            table.AddColumn("Category");
            table.AddColumn("Description");
            
            // Get components from registry
            foreach (var component in ComponentRegistry.Components.Values)
            {
                if (component.IsAvailable)
                {
                    table.AddRow(
                        component.Name,
                        $"[dim]{component.Version}[/]",
                        component.Category.ToString(),
                        component.Description
                    );
                }
            }
            
            AnsiConsole.Write(table);
            
            var totalCount = ComponentRegistry.Components.Count;
            AnsiConsole.MarkupLine($"\n[green]{totalCount} component(s) available[/]");
            AnsiConsole.MarkupLine("[yellow]More coming soon: Target 40+ components![/]");
        }, installedOption, availableOption);

        return command;
    }

    static Command CreateRemoveCommand()
    {
        var command = new Command("remove", "Remove component(s) from your project");
        
        var componentsArg = new Argument<string[]>(
            "components",
            "Component name(s) to remove")
        {
            Arity = ArgumentArity.OneOrMore
        };
        command.AddArgument(componentsArg);

        command.SetHandler((components) =>
        {
            AnsiConsole.MarkupLine("[red]Removing components:[/]");
            foreach (var component in components)
            {
                AnsiConsole.MarkupLine($"  - {component}");
            }
            AnsiConsole.MarkupLine("\n[yellow]Coming soon in Milestone 1![/]");
        }, componentsArg);

        return command;
    }

    static Command CreateUpdateCommand()
    {
        var command = new Command("update", "Update component(s) to latest version");
        
        var componentsArg = new Argument<string[]>(
            "components",
            "Component name(s) to update (empty = all)")
        {
            Arity = ArgumentArity.ZeroOrMore
        };
        command.AddArgument(componentsArg);

        var allOption = new Option<bool>(
            "--all",
            "Update all installed components");
        command.AddOption(allOption);

        command.SetHandler((components, all) =>
        {
            if (all || components.Length == 0)
            {
                AnsiConsole.MarkupLine("[blue]Updating all components...[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[blue]Updating specific components:[/]");
                foreach (var component in components)
                {
                    AnsiConsole.MarkupLine($"  - {component}");
                }
            }
            AnsiConsole.MarkupLine("\n[yellow]Coming soon in Milestone 1![/]");
        }, componentsArg, allOption);

        return command;
    }
}
