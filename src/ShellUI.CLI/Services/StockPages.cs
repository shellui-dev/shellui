using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ShellUI.CLI.Services;

// init removes Bootstrap; unmodified sample pages get ShellUI's Tailwind classes and edited ones are only reported.
public static class StockPages
{
    // `dotnet new blazor` pages for net8.0/net9.0/net10.0 across --interactivity, --all-interactive and --auth.
    private static readonly Dictionary<string, HashSet<string>> StockHashes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Home.razor"] = new(StringComparer.Ordinal) { "ae4681c90c84510d13bf2a66fdb86d78d60412cfec5e611c9c226d79592229a2" },
        ["Counter.razor"] = new(StringComparer.Ordinal)
        {
            "38ec7d9ecc5a846bf6f089d36ccf3a254d9b88c28354f2a3c5a12a18ddce1241",
            "4d78d57a4503a20d5ede388695435aceec3609b5942f0e5acfe35cedaf745ef8",
            "8833e56b70f1ecd4a816d608a98d3e2c44a289a45b91c0a7faee645b91533267",
            "a85be73e8a44c638d5e3974288a7817eef7a4a756cf0d2420b5b4bfcf3d2a82c"
        },
        ["Weather.razor"] = new(StringComparer.Ordinal)
        {
            "1d988ab0af352917c3fc68bbd8becc441fec00f0b8ebc572d9b94905ef301bb8",
            "97afd80b92223a7ef963a7308a75938fa111ea67082e9cf66be4318c6ce6d688",
            "b9246ba86447cae02e41128b1438eb45ccaf1084369acab794a174778a78e675",
            "cbe95cabef0a65002c4f180c812f2de1bc1e526a779b4906a4ae122db232e5bc",
            "f28be79c3d24e11bc2687974c27fb4814c2647305146bfca44fc5be29469baf9",
            "febc623de76daa44cf9e05497c9f938fd999b0952daba5080d5b1cda20ca8b63"
        },
        ["Error.razor"] = new(StringComparer.Ordinal) { "0401924993372cfaa232b06ab63e5d91e1e6bf14618244c6c92df5124d6ca109" },
        ["NotFound.razor"] = new(StringComparer.Ordinal) { "de247b81953d245d07af36d09e794f44ffda9fffe73e9fd39387ed93ceb8653e" },
        ["Auth.razor"] = new(StringComparer.Ordinal)
        {
            "346f73f904b91e9a12e89e0f912f9a969225ab8e2bac25e19db9f2422e55ff4c",
            "ebc06a858a7e635646a392738c44e99515c140d293979c59cac2c7664269dcd1",
            "f6d65a3c128c136871cb96ca4bb0d8f2b28a06e07a44fc25a9df16764de0a1ea"
        }
    };

    private const string Heading = "text-2xl font-semibold tracking-tight";
    private const string TableHead = "h-12 px-4 text-left align-middle font-medium text-muted-foreground";

    private static readonly (string From, string To)[] Replacements =
    {
        ("<h1>", $"<h1 class=\"{Heading}\">"),
        ("<h1 class=\"text-danger\">", $"<h1 class=\"{Heading} text-destructive\">"),
        ("<h2 class=\"text-danger\">", "<h2 class=\"text-lg font-medium text-destructive\">"),
        ("<h3>", "<h3 class=\"text-lg font-semibold\">"),
        ("class=\"btn btn-primary\"", "class=\"inline-flex h-10 w-fit items-center justify-center whitespace-nowrap rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground ring-offset-background transition-colors hover:bg-primary/90 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2\""),
        ("<table class=\"table\">", "<table class=\"w-full caption-bottom text-sm\">"),
        ("<tr>", "<tr class=\"border-b border-border transition-colors hover:bg-muted/50\">"),
        ("<th>", $"<th class=\"{TableHead}\">"),
        ("<th aria-label=", $"<th class=\"{TableHead}\" aria-label="),
        ("<td>", "<td class=\"p-4 align-middle\">")
    };

    private static readonly Regex BootstrapClass = new(
        @"class=""(table|[^""]*\b(btn|btn-[a-z-]+|form-control|form-floating|form-label|form-check[a-z-]*|text-danger|text-success|alert-[a-z]+|nav-link|navbar-brand|table-(striped|bordered|hover|sm|dark|responsive))\b[^""]*)""",
        RegexOptions.Compiled);

    public record Result(List<string> Restyled, List<string> Notes);

    internal static bool IsStock(string fileName, string content)
    {
        if (!StockHashes.TryGetValue(fileName, out var hashes)) return false;
        var normalized = content.TrimStart('﻿').Replace("\r\n", "\n").TrimEnd();
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(normalized))).ToLowerInvariant();
        return hashes.Contains(hash);
    }

    // The stock Home only says "Hello, world!"; it becomes a pointer to theming and the docs.
    internal const string HomePage =
@"@page ""/""

<PageTitle>Home</PageTitle>

<h1 class=""text-2xl font-semibold tracking-tight"">Hello, world!</h1>

<p class=""text-muted-foreground"">Welcome to your new app.</p>

<div class=""flex flex-1 items-center justify-center py-8"">
    <div class=""w-full max-w-lg rounded-xl border border-border bg-card p-6 text-card-foreground shadow-sm"">
        <h2 class=""text-lg font-semibold"">Make it yours</h2>
        <p class=""mt-1 text-sm text-muted-foreground"">Pick or build a theme on tweakcn, copy its URL, then run:</p>
        <pre class=""mt-3 overflow-x-auto rounded-md bg-muted px-3 py-2 font-mono text-sm""><code>shellui theme apply https://tweakcn.com/themes/&lt;id&gt;</code></pre>
        <p class=""mt-3 text-sm text-muted-foreground"">Add components with <code class=""rounded bg-muted px-1 py-0.5 font-mono"">shellui add button card</code>, or list them all with <code class=""rounded bg-muted px-1 py-0.5 font-mono"">shellui list</code>.</p>
        <div class=""mt-5 flex flex-wrap gap-2"">
            <a href=""https://tweakcn.com"" target=""_blank"" rel=""noopener"" class=""inline-flex h-9 items-center justify-center rounded-md bg-primary px-4 text-sm font-medium text-primary-foreground transition-colors hover:bg-primary/90"">Browse themes</a>
            <a href=""https://shellui.dev"" target=""_blank"" rel=""noopener"" class=""inline-flex h-9 items-center justify-center rounded-md border border-input bg-background px-4 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground"">ShellUI docs</a>
        </div>
    </div>
</div>
";

    internal static string Restyle(string fileName, string content)
    {
        if (fileName.Equals("Home.razor", StringComparison.OrdinalIgnoreCase))
            return HomePage.Replace("\r\n", "\n").Replace("\n", content.Contains("\r\n") ? "\r\n" : "\n");
        return Replacements.Aggregate(content, (text, r) => text.Replace(r.From, r.To));
    }

    internal static bool UsesBootstrap(string content) => BootstrapClass.IsMatch(content);

    public static Result Apply(string cwd, IEnumerable<string> skipDirs)
    {
        var skip = skipDirs.Select(d => d.Replace('\\', '/').Trim('/') + "/").ToList();
        var restyled = new List<string>();
        var leftovers = new List<string>();

        foreach (var file in DashboardSetup.EnumerateSources(cwd, "*.razor"))
        {
            var rel = Path.GetRelativePath(cwd, file).Replace('\\', '/');
            if (skip.Any(d => rel.StartsWith(d, StringComparison.OrdinalIgnoreCase))) continue;
            if (rel.Contains("Components/Account/", StringComparison.OrdinalIgnoreCase)) continue;

            var content = File.ReadAllText(file);
            if (IsStock(Path.GetFileName(file), content))
            {
                var updated = Restyle(Path.GetFileName(file), content);
                if (updated == content) continue;
                File.WriteAllText(file, updated);
                restyled.Add(rel);
            }
            else if (UsesBootstrap(content))
            {
                leftovers.Add(rel);
            }
        }

        var notes = leftovers
            .Select(f => $"Kept {f} because it was modified. It still uses Bootstrap classes, which no longer have styles.")
            .ToList();
        return new Result(restyled, notes);
    }
}
