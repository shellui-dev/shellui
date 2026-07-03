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

        rootCommand.AddCommand(CreateInitCommand());
        rootCommand.AddCommand(CreateAddCommand());
        rootCommand.AddCommand(CreateListCommand());
        rootCommand.AddCommand(CreateRemoveCommand());
        rootCommand.AddCommand(CreateUpdateCommand());
        rootCommand.AddCommand(CreateThemeCommand());

        return await rootCommand.InvokeAsync(args);
    }

    static Command CreateThemeCommand()
    {
        var theme = new Command("theme", "Fetch and apply themes from tweakcn.com at build time.");
        theme.AddCommand(CreateThemeInitCommand());
        theme.AddCommand(CreateThemeApplyCommand());
        theme.AddCommand(CreateThemeUpdateCommand());
        return theme;
    }

    static Command CreateThemeInitCommand()
    {
        var command = new Command("init", "Initialize ShellUI in a fresh Blazor project and bake in a tweakcn theme in one shot.");

        var urlArg = new Argument<string>("url",
            "tweakcn URL or theme id. Accepts https://tweakcn.com/themes/<id>, the raw <id>, or the public https://tweakcn.com/r/themes/<id> endpoint.");
        command.AddArgument(urlArg);

        var forceOpt = new Option<bool>("--force", "Reinitialize even if already initialized");
        var styleOpt = new Option<string>("--style", getDefaultValue: () => "default",
            "Component style: default, new-york, minimal");
        var tailwindOpt = new Option<string>("--tailwind", getDefaultValue: () => "standalone",
            "Tailwind method: standalone, npm");
        var yesOpt = new Option<bool>("--yes", "Non-interactive mode with default options");
        command.AddOption(forceOpt);
        command.AddOption(styleOpt);
        command.AddOption(tailwindOpt);
        command.AddOption(yesOpt);

        command.SetHandler(async (url, force, style, tailwind, nonInteractive) =>
        {
            try
            {
                AnsiConsole.MarkupLine("[cyan]Step 1/2:[/] initializing ShellUI…");
                await InitService.InitializeAsync(style, force, tailwind, nonInteractive);
                AnsiConsole.MarkupLine("");
                AnsiConsole.MarkupLine("[cyan]Step 2/2:[/] applying tweakcn theme…");
                await ApplyThemeAsync(url, emitOverride: null);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message.Replace("[", "[[").Replace("]", "]]")}");
                Environment.Exit(1);
            }
        }, urlArg, forceOpt, styleOpt, tailwindOpt, yesOpt);

        return command;
    }

    static Command CreateThemeApplyCommand()
    {
        var command = new Command("apply", "Fetch a theme from tweakcn and bake it into wwwroot/input.css");

        var urlArg = new Argument<string>("url",
            "tweakcn URL or theme id. Accepts https://tweakcn.com/themes/<id>, the raw <id>, or the public https://tweakcn.com/r/themes/<id> endpoint.");
        command.AddArgument(urlArg);

        var emitOverrideOpt = new Option<string?>("--emit-override",
            "Write the theme block to a standalone CSS file (Path A/D users link this after shellui-all.css). If omitted, updates wwwroot/input.css in-place.");
        command.AddOption(emitOverrideOpt);

        command.SetHandler(async (url, emitOverride) =>
        {
            try
            {
                await ApplyThemeAsync(url, emitOverride);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message.Replace("[", "[[").Replace("]", "]]")}");
                Environment.Exit(1);
            }
        }, urlArg, emitOverrideOpt);

        return command;
    }

    static Command CreateThemeUpdateCommand()
    {
        var command = new Command("update", "Re-fetch the theme recorded in shellui.theme.lock and re-apply it.");

        command.SetHandler(async () =>
        {
            try
            {
                var cwd = Directory.GetCurrentDirectory();
                var lockFile = ThemeService.ReadLockFile(cwd);
                if (lockFile is null)
                {
                    AnsiConsole.MarkupLine("[yellow]No shellui.theme.lock in this directory.[/] Run [bold]shellui theme apply <url>[/] first.");
                    Environment.Exit(2);
                    return;
                }
                AnsiConsole.MarkupLine($"[cyan]Updating theme[/] [bold]{lockFile!.ThemeName}[/] from [dim]{lockFile.SourceUrl}[/]");
                await ApplyThemeAsync(lockFile.SourceUrl, emitOverride: null);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message.Replace("[", "[[").Replace("]", "]]")}");
                Environment.Exit(1);
            }
        });

        return command;
    }

    private static async Task ApplyThemeAsync(string url, string? emitOverride)
    {
        var cwd = Directory.GetCurrentDirectory();

        AnsiConsole.MarkupLine($"[cyan]Fetching theme:[/] {url.Replace("[", "[[").Replace("]", "]]")}");
        var json = await ThemeService.FetchThemeJsonAsync(url);
        var theme = ThemeService.ParseTheme(json);
        AnsiConsole.MarkupLine($"[green]✓[/] Fetched: [bold]{theme.Name}[/] ({theme.LightVars.Count} light + {theme.DarkVars.Count} dark vars)");

        if (!string.IsNullOrEmpty(emitOverride))
        {
            var outPath = Path.IsPathRooted(emitOverride) ? emitOverride : Path.Combine(cwd, emitOverride);
            ThemeService.EmitOverride(outPath, theme);
            AnsiConsole.MarkupLine($"[green]✓[/] Wrote override: [dim]{outPath}[/]");
            AnsiConsole.MarkupLine("");
            AnsiConsole.MarkupLine("[yellow]Next step:[/] link the override AFTER shellui-all.css in your App.razor <head>:");
            AnsiConsole.MarkupLine($"  [dim]<link rel=\"stylesheet\" href=\"_content/ShellUI.Components/shellui-all.css\" />[/]");
            AnsiConsole.MarkupLine($"  [bold]<link rel=\"stylesheet\" href=\"{Path.GetFileName(outPath)}\" />[/]");
        }
        else
        {
            var inputCss = Path.Combine(cwd, "wwwroot", "input.css");
            ThemeService.ApplyToInputCss(inputCss, theme);
            AnsiConsole.MarkupLine($"[green]✓[/] Updated: [dim]wwwroot/input.css[/]");
            AnsiConsole.MarkupLine("");
            AnsiConsole.MarkupLine("[yellow]Next step:[/] rebuild Tailwind (or run [bold]dotnet build[/] if the CLI wired that up during init).");
        }

        ThemeService.WriteLockFile(cwd, url, json, theme.Name);
        AnsiConsole.MarkupLine($"[dim]Lock file written: shellui.theme.lock[/]");
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
