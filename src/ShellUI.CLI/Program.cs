using System.CommandLine;
using Spectre.Console;
using ShellUI.Templates;
using ShellUI.Core.Models;
using ShellUI.CLI.Services;

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
        var tailwindOption = new Option<string>(
            "--tailwind",
            getDefaultValue: () => "standalone",
            "Choose Tailwind method (standalone, npm)");
        var nonInteractiveOption = new Option<bool>(
            "--yes",
            "Run in non-interactive mode with default options");

        command.AddOption(forceOption);
        command.AddOption(styleOption);
        command.AddOption(tailwindOption);
        command.AddOption(nonInteractiveOption);

        command.SetHandler(async (force, style, tailwind, nonInteractive) =>
        {
            try
            {
                // Beautiful ASCII art logo for SHELLUI
                var logo = @"
 ███████╗██╗  ██╗███████╗██╗     ██╗     ██╗   ██╗██╗
 ██╔════╝██║  ██║██╔════╝██║     ██║     ██║   ██║██║
 ███████╗███████║█████╗  ██║     ██║     ██║   ██║██║
 ╚════██║██╔══██║██╔══╝  ██║     ██║     ██║   ██║██║
 ███████║██║  ██║███████╗███████╗███████╗╚██████╔╝██║
 ╚══════╝╚═╝  ╚═╝╚══════╝╚══════╝╚══════╝ ╚═════╝ ╚═╝

";

                AnsiConsole.Markup($"[blue]{logo}[/]");
                await InitService.InitializeAsync(style, force, tailwind, nonInteractive);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message.Replace("[", "[[").Replace("]", "]]")}");
            }
        }, forceOption, styleOption, tailwindOption, nonInteractiveOption);

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

        command.SetHandler(async (components, force) =>
        {
            try
            {
                await ComponentInstaller.InstallComponents(components, force);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message.Replace("[", "[[").Replace("]", "]]")}");
            }
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
            try
            {
                ComponentManager.ListComponents(installed, available);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message.Replace("[", "[[").Replace("]", "]]")}");
            }
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
            try
            {
                ComponentManager.RemoveComponents(components);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message.Replace("[", "[[").Replace("]", "]]")}");
            }
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
            try
            {
                ComponentManager.UpdateComponents(components, all);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message.Replace("[", "[[").Replace("]", "]]")}");
            }
        }, componentsArg, allOption);

        return command;
    }
}
