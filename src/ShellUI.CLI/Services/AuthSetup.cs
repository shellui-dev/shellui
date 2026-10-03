using System.Text.RegularExpressions;
using ShellUI.Core.Models;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public static class AuthSetup
{
    public static readonly IReadOnlyDictionary<string, string> Layouts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["auth-01"] = "AuthLayout01",
        ["auth-02"] = "AuthLayout02",
        ["auth-03"] = "AuthLayout03"
    };

    internal const string Hint = "Style the Identity pages under Components/Account with a block: shellui add auth-01 (centered card), auth-02 (split screen) or auth-03 (minimal).";

    private static readonly Regex LayoutDirective = new(@"^([ \t]*@layout[ \t]+)([\w.:]+)([ \t]*)\r?$", RegexOptions.Compiled | RegexOptions.Multiline);

    public static async Task WireAsync(string component, ProjectInfo projectInfo, ShellUIConfig config)
    {
        var cwd = Directory.GetCurrentDirectory();
        var layoutName = Layouts[component];
        var changes = new List<string>();
        var notes = new List<string>();

        var imports = Path.Combine(cwd, "Components", "Account", "Pages", "_Imports.razor");
        if (!DashboardSetup.IsIdentityApp(cwd) || !File.Exists(imports))
        {
            notes.Add($"{component} styles the ASP.NET Core Identity pages, and this app has none. Create the app with `dotnet new blazor --auth Individual`, or put @layout {layoutName} on your own sign-in pages.");
        }
        else
        {
            var typeRef = LayoutTypeName(projectInfo.RootNamespace, config.LayoutPath, layoutName);
            var original = await File.ReadAllTextAsync(imports);
            var updated = SetLayout(original, typeRef);
            if (updated == null)
                notes.Add($"Components/Account/Pages/_Imports.razor sets its own layout. Change it to @layout {typeRef} to use {component}.");
            else if (updated != original)
            {
                await File.WriteAllTextAsync(imports, updated);
                changes.Add($"Components/Account/Pages/_Imports.razor: sign-in pages use {layoutName}");
            }

            var pages = AccountPages.Apply(cwd, projectInfo.RootNamespace);
            if (pages.Restyled.Count > 0)
                changes.Add($"Restyled {pages.Restyled.Count} Identity page(s) under Components/Account (Bootstrap classes → Tailwind)");
            notes.AddRange(pages.Notes);
        }

        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine($"[cyan]Auth pages ({layoutName}):[/]");
        foreach (var change in changes)
            AnsiConsole.MarkupLine($"  [green]✓[/] {Markup.Escape(change)}");
        foreach (var note in notes)
            AnsiConsole.MarkupLine($"  [yellow]![/] {Markup.Escape(note)}");
        if (changes.Count == 0 && notes.Count == 0)
            AnsiConsole.MarkupLine("  [dim]Already set up, nothing changed.[/]");
    }

    internal static string LayoutTypeName(string rootNamespace, string layoutPath, string layoutName)
    {
        var parts = layoutPath.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries).Where(p => p != ".");
        return string.Join(".", new[] { rootNamespace }.Concat(parts).Append(layoutName));
    }

    // .NET 9 and 10 set no layout here and .NET 8 uses its AccountLayout; the Manage pages keep ManageLayout through their own _Imports.
    internal static string? SetLayout(string imports, string typeRef)
    {
        var match = LayoutDirective.Match(imports);
        if (!match.Success)
        {
            var newline = imports.Contains("\r\n") ? "\r\n" : "\n";
            return imports.TrimEnd() + newline + $"@layout {typeRef}" + newline;
        }

        var current = match.Groups[2].Value.Split('.').Last();
        if (current != "AccountLayout" && !Layouts.Values.Contains(current)) return null;
        return imports[..match.Groups[2].Index] + typeRef + imports[(match.Groups[2].Index + match.Groups[2].Length)..];
    }
}
