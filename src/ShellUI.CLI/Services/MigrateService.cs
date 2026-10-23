using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using ShellUI.Core.Models;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public record MigrationEdit(string Path, string Before, string After, IReadOnlyList<string> Applied);

public record MigrationNote(string Path, int Line, string Message);

public record MigrationPlan(IReadOnlyList<MigrationEdit> Edits, IReadOnlyList<MigrationNote> Notes);

// Rewrites an app's own .razor and .cs files for template API changes listed in the release notes.
public static class MigrateService
{
    private record Migration(string Component, string Summary, Func<string, string> Rewrite);

    private static readonly Migration[] Migrations =
    {
        new("input", "Input.InputVariant.X → InputVariant.X (0.4.0-alpha.3)",
            text => text.Replace("Input.InputVariant.", "InputVariant.")),
        new("date-picker", "DatePicker SelectedDate → Value (0.4.0-alpha.3)",
            text => RewriteTags(text, "DatePicker", tag => RenameParameter(tag, "SelectedDate", "Value"))),
        new("time-picker", "TimePicker SelectedTime → Value (0.4.0-alpha.3)",
            text => RewriteTags(text, "TimePicker", tag => RenameParameter(tag, "SelectedTime", "Value"))),
        new("pagination", "Pagination OnPageChange → CurrentPageChanged (0.4.0-alpha.3)",
            text => RewriteTags(text, "Pagination", tag =>
                // @bind-CurrentPage already sets CurrentPageChanged, so a tag with both can't be renamed.
                Regex.IsMatch(tag, @"@bind-CurrentPage\b") ? tag : Regex.Replace(tag, @"(?<=\s)OnPageChange(?=\s*=)", "CurrentPageChanged"))),
    };

    // calendar only reports handlers to fix by hand.
    public static IReadOnlyList<string> Components => Migrations.Select(m => m.Component).Append("calendar").Distinct().ToList();

    public static int Run(string[] components, bool dryRun, bool yes)
    {
        var unknown = components.Where(c => !Components.Contains(c, StringComparer.OrdinalIgnoreCase)).ToList();
        if (unknown.Count > 0)
        {
            AnsiConsole.MarkupLine($"[red]No migrations for:[/] {Markup.Escape(string.Join(", ", unknown))}. Migrations exist for {Markup.Escape(string.Join(", ", Components))}.");
            return 1;
        }

        var root = Directory.GetCurrentDirectory();
        var plan = Plan(root, components);

        foreach (var edit in plan.Edits)
        {
            AnsiConsole.MarkupLine($"[bold]{Markup.Escape(Path.GetRelativePath(root, edit.Path))}[/] [dim]({Markup.Escape(string.Join("; ", edit.Applied))})[/]");
            foreach (var line in Diff(edit.Before, edit.After))
                AnsiConsole.MarkupLine(line.StartsWith('-') ? $"[red]{Markup.Escape(line)}[/]" : $"[green]{Markup.Escape(line)}[/]");
            AnsiConsole.MarkupLine("");
        }
        foreach (var note in plan.Notes)
            AnsiConsole.MarkupLine($"[yellow]![/] {Markup.Escape(Path.GetRelativePath(root, note.Path))}:{note.Line} {Markup.Escape(note.Message)}");

        if (plan.Edits.Count == 0)
        {
            AnsiConsole.MarkupLine(plan.Notes.Count == 0 ? "[green]Nothing to migrate.[/]" : "[green]Nothing to rewrite.[/] Check the notes above by hand.");
            return 0;
        }
        if (dryRun)
        {
            AnsiConsole.MarkupLine($"[dim]Dry run: {plan.Edits.Count} file(s) would change.[/]");
            return 0;
        }
        if (!yes && !AnsiConsole.Profile.Capabilities.Interactive)
        {
            AnsiConsole.MarkupLine("[yellow]Not an interactive terminal, so nothing was written.[/] Run again with --yes to write the changes.");
            return 1;
        }
        if (!yes && !AnsiConsole.Confirm($"Write these changes to {plan.Edits.Count} file(s)?", false))
        {
            AnsiConsole.MarkupLine("[dim]Nothing written.[/]");
            return 0;
        }

        foreach (var edit in plan.Edits)
            File.WriteAllText(edit.Path, edit.After);
        AnsiConsole.MarkupLine($"[green]Migrated {plan.Edits.Count} file(s).[/] Build the app to check.");
        return 0;
    }

    public static MigrationPlan Plan(string root, IEnumerable<string>? components = null)
    {
        var wanted = components?.ToList() is { Count: > 0 } list
            ? Migrations.Where(m => list.Contains(m.Component, StringComparer.OrdinalIgnoreCase)).ToList()
            : Migrations.ToList();
        var checkCalendar = components?.Any() != true || components.Contains("calendar", StringComparer.OrdinalIgnoreCase);

        var edits = new List<MigrationEdit>();
        var notes = new List<MigrationNote>();
        foreach (var path in SourceFiles(root))
        {
            var before = File.ReadAllText(path);
            var after = before;
            var applied = new List<string>();
            foreach (var migration in wanted)
            {
                var next = migration.Rewrite(after);
                if (next == after) continue;
                after = next;
                applied.Add(migration.Summary);
            }
            if (after != before) edits.Add(new MigrationEdit(path, before, after, applied));
            if (checkCalendar) notes.AddRange(CalendarHandlerNotes(path, after));
        }
        return new MigrationPlan(edits, notes);
    }

    // The app's own sources: not the installed components (update rewrites those) or build output.
    private static IEnumerable<string> SourceFiles(string root)
    {
        var componentsPath = "Components/UI";
        var configPath = Path.Combine(root, "shellui.json");
        if (File.Exists(configPath))
        {
            try { componentsPath = JsonSerializer.Deserialize<ShellUIConfig>(File.ReadAllText(configPath))?.ComponentsPath ?? componentsPath; }
            catch (JsonException) { }
        }
        var skip = new[] { "bin", "obj", "node_modules", ".shellui", ".git" };
        var componentsDir = Path.GetFullPath(Path.Combine(root, componentsPath)) + Path.DirectorySeparatorChar;

        return Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories)
            .Where(p => p.EndsWith(".razor", StringComparison.OrdinalIgnoreCase) || p.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            .Where(p => !Path.GetRelativePath(root, p).Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Any(part => skip.Contains(part, StringComparer.OrdinalIgnoreCase)))
            .Where(p => !Path.GetFullPath(p).StartsWith(componentsDir, StringComparison.OrdinalIgnoreCase))
            .OrderBy(p => p, StringComparer.Ordinal);
    }

    // Applies rewrite to every <Name …> opening tag, scanning to the closing > outside quotes.
    internal static string RewriteTags(string text, string name, Func<string, string> rewrite)
    {
        var result = new StringBuilder();
        var position = 0;
        foreach (Match match in Regex.Matches(text, $@"<{name}(?=[\s/>])"))
        {
            if (match.Index < position) continue;
            var end = TagEnd(text, match.Index + match.Length);
            result.Append(text, position, match.Index - position);
            result.Append(rewrite(text[match.Index..end]));
            position = end;
        }
        result.Append(text, position, text.Length - position);
        return result.ToString();
    }

    private static int TagEnd(string text, int start)
    {
        char? quote = null;
        for (var i = start; i < text.Length; i++)
        {
            var c = text[i];
            if (quote != null) { if (c == quote) quote = null; }
            else if (c is '"' or '\'') quote = c;
            else if (c == '>') return i + 1;
        }
        return text.Length;
    }

    // Renames Old, OldChanged, @bind-Old and @bind-Old:after (and so on) to the new name.
    private static string RenameParameter(string tag, string oldName, string newName) =>
        Regex.Replace(tag, $@"(?<=\s)(@bind-)?{oldName}(Changed)?(?=[\s:=])", m => m.Groups[1].Value + newName + m.Groups[2].Value);

    // Calendar.SelectedDateChanged became EventCallback<DateTime?> in 0.4.0-alpha.1; a handler taking DateTime no longer compiles.
    private static IEnumerable<MigrationNote> CalendarHandlerNotes(string path, string text)
    {
        var handlers = new HashSet<string>();
        RewriteTags(text, "Calendar", tag =>
        {
            foreach (Match m in Regex.Matches(tag, @"(?<=\s)SelectedDateChanged\s*=\s*""@?(\w+)"""))
                handlers.Add(m.Groups[1].Value);
            return tag;
        });
        foreach (var handler in handlers)
        {
            foreach (Match m in Regex.Matches(text, $@"\b{handler}\s*\(\s*DateTime\s+\w+"))
            {
                var line = text[..m.Index].Count(c => c == '\n') + 1;
                yield return new MigrationNote(path, line, $"{handler} handles Calendar.SelectedDateChanged with a DateTime; it now needs a DateTime? (0.4.0-alpha.1).");
            }
        }
    }

    // Changed lines only, prefixed - and +, with their line numbers.
    private static IEnumerable<string> Diff(string before, string after)
    {
        var a = before.Split('\n');
        var b = after.Split('\n');
        for (var i = 0; i < Math.Max(a.Length, b.Length); i++)
        {
            var left = i < a.Length ? a[i].TrimEnd('\r') : null;
            var right = i < b.Length ? b[i].TrimEnd('\r') : null;
            if (left == right) continue;
            if (left != null) yield return $"-{i + 1,5}: {left.Trim()}";
            if (right != null) yield return $"+{i + 1,5}: {right.Trim()}";
        }
    }
}
