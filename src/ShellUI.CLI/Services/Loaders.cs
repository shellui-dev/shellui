using Spectre.Console;
using Spectre.Console.Rendering;

namespace ShellUI.CLI.Services;

public static class Loaders
{
    public static Status SnakeStatus() =>
        AnsiConsole.Status().Spinner(SnakeSpinner.Instance).SpinnerStyle(Style.Parse("green"));
}

// The Loading "snake" grid as a two-character Braille spinner; Spectre falls back to ASCII without Unicode.
public sealed class SnakeSpinner : Spinner
{
    private static readonly int[] Path = { 0, 1, 2, 5, 8, 7, 6, 3 };
    private static readonly int[,] Bits = { { 0x01, 0x08 }, { 0x02, 0x10 }, { 0x04, 0x20 } };

    public static readonly SnakeSpinner Instance = new();

    private SnakeSpinner()
    {
        Frames = Enumerable.Range(0, Path.Length)
            .Select(step => Braille(i => IsLit(Array.IndexOf(Path, i), step)))
            .ToList();
    }

    public override TimeSpan Interval => TimeSpan.FromMilliseconds(110);
    public override bool IsUnicode => true;
    public override IReadOnlyList<string> Frames { get; }

    private static bool IsLit(int position, int step) =>
        position >= 0 && Enumerable.Range(0, 3).Any(k => (step - k + Path.Length) % Path.Length == position);

    internal static string Braille(Func<int, bool> lit)
    {
        var chars = new int[2];
        for (var i = 0; i < 9; i++)
            if (lit(i)) chars[i % 3 / 2] |= Bits[i / 3, i % 3 % 2];
        return string.Concat(chars.Select(c => (char)(0x2800 + c)));
    }
}

// The Loading "logo" variant beside the current step; the snake spinner is used when there is no terminal.
public static class LogoLoader
{
    private static readonly string[] Pattern = { "X...X", ".X..X", "..X.X", ".X..X", "X...X" };
    private const int Cycle = 30;

    public static async Task RunAsync(string initialStatus, Func<Action<string>, Task> work)
    {
        var caps = AnsiConsole.Profile.Capabilities;
        if (!caps.Interactive || !caps.Unicode || Console.IsOutputRedirected)
        {
            await Loaders.SnakeStatus().StartAsync(initialStatus, ctx => work(s => ctx.Status(s)));
            return;
        }

        var status = initialStatus;
        await AnsiConsole.Live(Render(0, status))
            .AutoClear(true)
            .StartAsync(async ctx =>
            {
                var task = work(s => status = s);
                for (var frame = 1; !task.IsCompleted; frame++)
                {
                    ctx.UpdateTarget(Render(frame, status));
                    await Task.WhenAny(task, Task.Delay(70));
                }
                await task;
            });
    }

    // Lit dots switch on along the diagonal, hold, then switch off the same way.
    internal static string[] Frame(int frame)
    {
        var t = frame % Cycle;
        return Enumerable.Range(0, 5).Select(r => string.Concat(Enumerable.Range(0, 5).Select(c =>
        {
            var diagonal = r + c;
            var on = Pattern[r][c] == 'X' && (t < Cycle / 2 ? diagonal <= t : diagonal > t - Cycle / 2);
            return on ? 'X' : '.';
        }))).ToArray();
    }

    private static IRenderable Render(int frame, string status)
    {
        var grid = new Grid().AddColumn().AddColumn();
        grid.AddRow(new Markup(Dots(Frame(frame))), new Markup($"\n\n  {Markup.Escape(status)}"));
        return grid;
    }

    private static string Dots(IEnumerable<string> rows) =>
        string.Join("\n", rows.Select(row => string.Join(" ", row.Select(ch => ch == 'X' ? "[white]●[/]" : "[grey19]●[/]"))));

    // The static mark with the name, version and a subtitle beside it (used by init and list).
    public static void WriteHeader(string subtitle)
    {
        var version = typeof(LogoLoader).Assembly
            .GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), false)
            .OfType<System.Reflection.AssemblyInformationalVersionAttribute>()
            .FirstOrDefault()?.InformationalVersion.Split('+')[0] ?? "";

        if (!AnsiConsole.Profile.Capabilities.Unicode)
        {
            AnsiConsole.MarkupLine($"[bold]ShellUI[/] [dim]{Markup.Escape(version)}[/]  {Markup.Escape(subtitle)}\n");
            return;
        }

        var grid = new Grid().AddColumn().AddColumn();
        grid.AddRow(
            new Markup(Dots(Pattern)),
            new Markup($"\n  [bold]ShellUI[/] [dim]{Markup.Escape(version)}[/]\n  [dim]{Markup.Escape(subtitle)}[/]"));
        AnsiConsole.WriteLine();
        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();
    }
}
