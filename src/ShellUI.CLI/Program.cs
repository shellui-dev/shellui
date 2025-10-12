using System.CommandLine;
using Spectre.Console;

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
            
            AnsiConsole.MarkupLine("\n[yellow]⏳ Coming soon in Milestone 1![/]");
            AnsiConsole.MarkupLine("[dim]This will:[/]");
            AnsiConsole.MarkupLine("[dim]  • Detect your project type (Server/WASM/SSR)[/]");
            AnsiConsole.MarkupLine("[dim]  • Create Components/UI folder[/]");
            AnsiConsole.MarkupLine("[dim]  • Download Tailwind standalone CLI (no Node.js!)[/]");
            AnsiConsole.MarkupLine("[dim]  • Create shellui.json config[/]");
            AnsiConsole.MarkupLine("[dim]  • Set up Tailwind CSS v4[/]");
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
            AnsiConsole.MarkupLine("[green]Adding components:[/]");
            
            var table = new Table();
            table.AddColumn("Component");
            table.AddColumn("Status");
            
            foreach (var component in components)
            {
                table.AddRow(component, "[yellow]Coming soon[/]");
            }
            
            AnsiConsole.Write(table);
            
            if (force)
            {
                AnsiConsole.MarkupLine("\n[yellow]Force mode: Will overwrite existing components[/]");
            }
            
            AnsiConsole.MarkupLine("\n[dim]Usage will be:[/]");
            AnsiConsole.MarkupLine("[dim]  dotnet shellui add button card alert[/]");
            AnsiConsole.MarkupLine("[dim]  dotnet shellui add button,card,alert[/]");
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
            table.AddColumn("Status");
            table.AddColumn("Category");
            table.AddColumn("Description");
            
            // Mock data for demonstration
            table.AddRow("button", "[green]Available[/]", "Form", "Interactive button");
            table.AddRow("card", "[green]Available[/]", "Layout", "Content container");
            table.AddRow("alert", "[green]Available[/]", "Feedback", "Alert messages");
            table.AddRow("input", "[green]Available[/]", "Form", "Text input field");
            table.AddRow("dialog", "[green]Available[/]", "Overlay", "Modal dialogs");
            
            AnsiConsole.Write(table);
            
            AnsiConsole.MarkupLine("\n[yellow]⏳ Coming soon: 40+ components![/]");
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
                AnsiConsole.MarkupLine($"  • {component}");
            }
            AnsiConsole.MarkupLine("\n[yellow]⏳ Coming soon in Milestone 1![/]");
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
                    AnsiConsole.MarkupLine($"  • {component}");
                }
            }
            AnsiConsole.MarkupLine("\n[yellow]⏳ Coming soon in Milestone 1![/]");
        }, componentsArg, allOption);

        return command;
    }
}
