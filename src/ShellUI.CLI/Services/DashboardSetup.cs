using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ShellUI.Core.Models;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public enum LayoutSwitch
{
    Ask,
    Always
}

public static class DashboardSetup
{
    public static readonly IReadOnlyDictionary<string, string> Layouts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["dashboard-01"] = "DashboardLayout01",
        ["dashboard-02"] = "DashboardLayout02"
    };

    internal const string StockProjectName = "ShellStockProbe";

    // `dotnet new blazor` for net8.0/net9.0/net10.0 across --interactivity, --all-interactive, --auth and --empty, after NormalizeStock.
    private static readonly Dictionary<string, HashSet<string>> StockHashes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["MainLayout.razor"] = new(StringComparer.Ordinal)
        {
            "99a50a55f6b55aa0446c1ef889a9096f3a8e5a7b16b71d393df3b57f00f9347f",
            "9ecf2f10a5db9e896b46991e6134e197224734ef47ae608daa44558c11a78786",
            "af6d8151880d2a7f521abe08f7c5aee59917a569ef58aac2d4653b32077daed2",
            "b3c5ef0717f18d50d12bb510a8665a6db2c00da224169816531ed4eabb02752b",
            "f444829e5638cff745e4071e6af5bbbec673cbe4fd9d8541e97632c788270d5e",
            "ff9995ff35fcb1744a698536009d0997ae63e2641dbeb993e5153683e5d46e3c"
        },
        ["MainLayout.razor.css"] = new(StringComparer.Ordinal)
        {
            "0ba85a9f9b54e905e9d3859d6767c95a49a21b3492d097cf04fcdf52f5696eae",
            "210414cc64ad9c676390ddfecd53fbda3288e2d15cbf038e62bd3f568925c821",
            "936a82896c36bfebfe22868aa84e5fd3e0c0f0029f5c9c2adb9a11125b7bc6dd",
            "fc92498e1743ce82b2fb69a2ebe2c4648c7e0adeba40417ffe1a124b9a0d1095"
        },
        ["NavMenu.razor"] = new(StringComparer.Ordinal)
        {
            "1ff6d08f3776aa632df2c209a09c1841ecd0f1004f9e372df77198c4a1b1f2a1",
            "6a476727de0169cbcc03462b4359175fcab17ce4d27b7f57796db8c28fb1554d",
            "7d0ce8b35944c09ae20a6d18c1bf642857e0ffc8a567d6854a6d831ff19d94b3",
            "83da60817516398d55eba3a8a76aa06ef340edaeb488b6fc57ab6352a00a7403",
            "a020cfd72aedbdffc131c929f9257ed35f0b359a23a8ee691551a92cf2894743",
            "b7b968c84028e0d855091f60db00349099594254e75afd2eac11592f2559d474",
            "c6b71e7459510a281e9177ed4e0be7b7fe9ccc4248615bc282d77bb73271c860",
            "f6bdc02b2599e90105e5515ba6f579b31dcb02d9b100b72577e7d6939a9bafe5"
        },
        ["NavMenu.razor.css"] = new(StringComparer.Ordinal)
        {
            "4db5ecd06678f42f1c2fd32887ba74e0a790bec15407d381853f7ce7320aa66b",
            "5542c1c9e78ed89c670306c4cbd975cc1ca47d14560029bf63e554bf8b23abb8",
            "7b623080b93bbd68c171e657f5555f4197c9537d20b92ae3254dfbc5b9eaea21",
            "9570d629a7155ae9a77bcdd856d70e4b46887391c90ebed05d5662ae35a6ead1"
        }
    };

    private static readonly Regex RouteViewPattern = new(@"<(Authorize)?RouteView\b", RegexOptions.Compiled);
    private static readonly Regex DefaultLayoutPattern = new(@"DefaultLayout=""(@?)typeof\(([^)]*)\)""", RegexOptions.Compiled);
    private static readonly Regex LinksPattern = new(@"(private static readonly NavLink\[\] Links =\s*\[)(.*?)(\s*\];)", RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex PagePattern = new(@"^\s*@page\s+""([^""]*)""", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex PageTitlePattern = new(@"<PageTitle>([^<@]+)</PageTitle>", RegexOptions.Compiled);

    internal const string DefaultLinks = @"new(""Home"", ""/"", ""home""),";

    internal const string ErrorUi =
@"    <div id=""blazor-error-ui"" data-nosnippet class=""fixed inset-x-0 bottom-0 z-[1000] hidden border-t border-border bg-background px-5 py-3 text-sm text-foreground shadow-lg"">
        An unhandled error has occurred.
        <a href=""."" class=""reload font-medium underline underline-offset-4"">Reload</a>
        <span class=""dismiss absolute right-4 top-3 cursor-pointer"">🗙</span>
    </div>
";

    /// Returns "dashboard-01", "dashboard-02", "none", or null when the option was not given.
    public static string? ParseDashboardOption(string? value)
    {
        if (value is null) return null;
        return value.Trim().ToLowerInvariant() switch
        {
            "none" or "no" or "false" => "none",
            "01" or "1" or "dashboard-01" => "dashboard-01",
            "02" or "2" or "dashboard-02" => "dashboard-02",
            _ => throw new ArgumentException($"Unknown --dashboard value '{value}'. Use 01, 02 or none.")
        };
    }

    public static async Task WireAsync(string component, ProjectInfo projectInfo, ShellUIConfig config, LayoutSwitch layoutSwitch)
    {
        var cwd = Directory.GetCurrentDirectory();
        var layoutName = Layouts[component];
        var changes = new List<string>();
        var notes = new List<string>();
        var usesDashboard = false;

        var routesPath = FindRoutesFile(cwd);
        if (routesPath == null)
        {
            notes.Add($"No RouteView found. Set DefaultLayout=\"typeof({layoutName})\" on your RouteView.");
        }
        else
        {
            var routes = await File.ReadAllTextAsync(routesPath);
            var current = FindDefaultLayout(routes);
            var currentName = current?.Split('.').Last();

            if (current == null || currentName == null)
            {
                notes.Add($"{Rel(cwd, routesPath)} has no DefaultLayout. Set DefaultLayout=\"typeof({layoutName})\" on the RouteView.");
            }
            else if (currentName == layoutName)
            {
                usesDashboard = true;
            }
            else
            {
                var mainLayout = Path.Combine(cwd, "Components", "Layout", "MainLayout.razor");
                var replaceable = currentName.StartsWith("DashboardLayout", StringComparison.Ordinal)
                    || (currentName == "MainLayout" && IsStockFile(mainLayout));

                if (replaceable || ConfirmReplace(currentName, layoutName, layoutSwitch, notes))
                {
                    var typeRef = LayoutTypeReference(Path.GetDirectoryName(routesPath)!, cwd, projectInfo.RootNamespace, config.LayoutPath, layoutName);
                    await File.WriteAllTextAsync(routesPath, RewriteDefaultLayout(routes, typeRef));
                    changes.Add($"{Rel(cwd, routesPath)}: default layout {currentName} → {layoutName}");
                    if (Layouts.Values.Contains(currentName))
                        RetargetLayoutDirectives(cwd, currentName, layoutName, changes);
                    usesDashboard = true;
                }
            }
        }

        if (usesDashboard)
        {
            await EnsureErrorUiAsync(cwd, changes);
            RemoveStockLayout(cwd, config, layoutName, changes, notes);
        }

        await UpdateSidebarLinksAsync(cwd, config, changes);
        await UseAccountMenuAsync(cwd, config, changes, notes);

        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine($"[cyan]Dashboard setup ({layoutName}):[/]");
        foreach (var change in changes)
            AnsiConsole.MarkupLine($"  [green]✓[/] {Markup.Escape(change)}");
        foreach (var note in notes)
            AnsiConsole.MarkupLine($"  [yellow]![/] {Markup.Escape(note)}");
        if (changes.Count == 0 && notes.Count == 0)
            AnsiConsole.MarkupLine("  [dim]Already set up, nothing changed.[/]");
    }

    private static bool ConfirmReplace(string current, string layoutName, LayoutSwitch layoutSwitch, List<string> notes)
    {
        if (layoutSwitch == LayoutSwitch.Always) return true;

        if (AnsiConsole.Profile.Capabilities.Interactive && !Console.IsInputRedirected)
            return AnsiConsole.Confirm($"Your app uses [bold]{Markup.Escape(current)}[/] as its default layout. Switch to [bold]{layoutName}[/]?", false);

        notes.Add($"Default layout {current} was left unchanged. Re-run with --replace-layout to switch to {layoutName}.");
        return false;
    }

    internal static string? FindRoutesFile(string cwd)
    {
        var candidates = new[]
        {
            Path.Combine(cwd, "Components", "Routes.razor"),
            Path.Combine(cwd, "Routes.razor"),
            Path.Combine(cwd, "App.razor"),
            Path.Combine(cwd, "Components", "App.razor")
        };
        return candidates.FirstOrDefault(p => File.Exists(p) && RouteViewPattern.IsMatch(File.ReadAllText(p)));
    }

    internal static string? FindDefaultLayout(string routesContent)
    {
        var match = DefaultLayoutPattern.Match(routesContent);
        return match.Success ? match.Groups[2].Value.Trim() : null;
    }

    internal static string RewriteDefaultLayout(string routesContent, string typeRef) =>
        DefaultLayoutPattern.Replace(routesContent, m => $"DefaultLayout=\"{m.Groups[1].Value}typeof({typeRef})\"", 1);

    internal static string LayoutTypeReference(string routesDir, string cwd, string rootNamespace, string layoutPath, string layoutName)
    {
        string Ns(string relative)
        {
            var parts = relative.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries).Where(p => p != ".");
            return string.Join(".", new[] { rootNamespace }.Concat(parts));
        }

        var routesNs = Ns(Path.GetRelativePath(cwd, routesDir));
        var layoutNs = Ns(layoutPath);
        if (layoutNs == routesNs) return layoutName;
        if (layoutNs.StartsWith(routesNs + ".", StringComparison.Ordinal)) return $"{layoutNs[(routesNs.Length + 1)..]}.{layoutName}";
        return $"global::{layoutNs}.{layoutName}";
    }

    // The project name only appears in the stock NavMenu brand link.
    private static readonly Regex BrandPattern = new(@"(<a class=""navbar-brand"" href="""">)[^<]*(</a>)", RegexOptions.Compiled);

    internal static string NormalizeStock(string content)
    {
        var text = content.TrimStart('﻿').Replace("\r\n", "\n").TrimEnd();
        return BrandPattern.Replace(text, $"$1{StockProjectName}$2", 1);
    }

    internal static bool IsStockContent(string fileName, string content)
    {
        if (!StockHashes.TryGetValue(fileName, out var hashes)) return false;
        var normalized = NormalizeStock(content);
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(normalized))).ToLowerInvariant();
        return hashes.Contains(hash);
    }

    private static bool IsStockFile(string path) =>
        File.Exists(path) && IsStockContent(Path.GetFileName(path), File.ReadAllText(path));

    internal static string AddErrorUi(string appRazor)
    {
        if (appRazor.Contains("blazor-error-ui", StringComparison.Ordinal)) return appRazor;
        var index = appRazor.LastIndexOf("</body>", StringComparison.OrdinalIgnoreCase);
        if (index < 0) return appRazor;
        var newline = appRazor.Contains("\r\n") ? "\r\n" : "\n";
        return appRazor.Insert(index, ErrorUi.Replace("\r\n", "\n").Replace("\n", newline));
    }

    private static async Task EnsureErrorUiAsync(string cwd, List<string> changes)
    {
        var appRazor = Path.Combine(cwd, "Components", "App.razor");
        if (!File.Exists(appRazor)) return;
        var original = await File.ReadAllTextAsync(appRazor);
        var patched = AddErrorUi(original);
        if (patched == original) return;
        await File.WriteAllTextAsync(appRazor, patched);
        changes.Add("Components/App.razor: added the Blazor error bar (it lived in MainLayout)");
    }

    private static void RemoveStockLayout(string cwd, ShellUIConfig config, string layoutName, List<string> changes, List<string> notes)
    {
        var layoutDir = Path.Combine(cwd, "Components", "Layout");
        var mainLayout = Path.Combine(layoutDir, "MainLayout.razor");
        var sameFolder = config.LayoutPath.Replace('\\', '/').Trim('/').Equals("Components/Layout", StringComparison.OrdinalIgnoreCase);
        if (sameFolder && IsStockFile(mainLayout))
            RetargetLayoutDirectives(cwd, "MainLayout", layoutName, changes);

        RemoveStockGroup(cwd, layoutDir, "MainLayout", changes, notes);
        if (!File.Exists(Path.Combine(layoutDir, "MainLayout.razor")))
            RemoveStockGroup(cwd, layoutDir, "NavMenu", changes, notes);
    }

    // Pages such as the .NET 10 NotFound page pin `@layout MainLayout`.
    internal static string RetargetLayoutDirective(string content, string from, string to) =>
        Regex.Replace(content, $@"^([ \t]*@layout[ \t]+(?:[\w.]+\.)?){Regex.Escape(from)}(?=[ \t]*\r?$)", m => m.Groups[1].Value + to, RegexOptions.Multiline);

    private static void RetargetLayoutDirectives(string cwd, string from, string to, List<string> changes)
    {
        foreach (var file in EnumerateSources(cwd, "*.razor"))
        {
            var content = File.ReadAllText(file);
            var retargeted = RetargetLayoutDirective(content, from, to);
            if (retargeted == content) continue;
            File.WriteAllText(file, retargeted);
            changes.Add($"{Rel(cwd, file)}: @layout {from} → {to}");
        }
    }

    private static void RemoveStockGroup(string cwd, string layoutDir, string component, List<string> changes, List<string> notes)
    {
        var files = new[] { $"{component}.razor", $"{component}.razor.css" }
            .Select(f => Path.Combine(layoutDir, f))
            .Where(File.Exists)
            .ToList();
        if (files.Count == 0) return;

        if (!files.All(f => IsStockFile(f)))
        {
            notes.Add($"Kept Components/Layout/{component}.razor because it was modified. Delete it yourself if it is no longer needed.");
            return;
        }

        if (IsReferencedElsewhere(cwd, component, files))
        {
            notes.Add($"Kept Components/Layout/{component}.razor because other files still use it.");
            return;
        }

        foreach (var file in files)
        {
            File.Delete(file);
            changes.Add($"Removed stock {Rel(cwd, file)}");
        }
    }

    private static bool IsReferencedElsewhere(string cwd, string component, List<string> own)
    {
        var pattern = new Regex($@"\b{component}\b");
        return EnumerateSources(cwd, "*.razor").Concat(EnumerateSources(cwd, "*.cs"))
            .Where(f => !own.Contains(f, StringComparer.OrdinalIgnoreCase))
            .Any(f => pattern.IsMatch(File.ReadAllText(f)));
    }

    internal static IEnumerable<string> EnumerateSources(string cwd, string pattern) =>
        Directory.EnumerateFiles(cwd, pattern, SearchOption.AllDirectories)
            .Where(f =>
            {
                var rel = Path.GetRelativePath(cwd, f).Replace('\\', '/');
                return !rel.StartsWith("bin/", StringComparison.OrdinalIgnoreCase)
                    && !rel.StartsWith("obj/", StringComparison.OrdinalIgnoreCase)
                    && !rel.StartsWith(".shellui/", StringComparison.OrdinalIgnoreCase)
                    && !rel.StartsWith("node_modules/", StringComparison.OrdinalIgnoreCase);
            });

    internal record PageLink(string Title, string Href, string Icon);

    internal static List<PageLink> ScanPages(string cwd, ShellUIConfig config)
    {
        var skipDirs = new[] { config.ComponentsPath, config.LayoutPath }
            .Where(p => !string.IsNullOrEmpty(p))
            .Select(p => p.Replace('\\', '/').TrimEnd('/') + "/")
            .ToList();

        var links = new Dictionary<string, PageLink>(StringComparer.OrdinalIgnoreCase);
        foreach (var file in EnumerateSources(cwd, "*.razor"))
        {
            var rel = Path.GetRelativePath(cwd, file).Replace('\\', '/');
            if (skipDirs.Any(d => rel.StartsWith(d, StringComparison.OrdinalIgnoreCase))) continue;

            var content = File.ReadAllText(file);
            var page = PagePattern.Match(content);
            if (!page.Success) continue;

            var href = page.Groups[1].Value.Trim();
            if (!IsNavigableRoute(href) || links.ContainsKey(href)) continue;

            var name = Path.GetFileNameWithoutExtension(file);
            var titleMatch = PageTitlePattern.Match(content);
            var title = titleMatch.Success ? titleMatch.Groups[1].Value.Trim() : Humanize(name);
            links[href] = new PageLink(title, href, IconFor(href, name));
        }

        return links.Values
            .OrderBy(l => l.Href == "/" ? 0 : 1)
            .ThenBy(l => l.Href, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    internal static bool IsNavigableRoute(string href)
    {
        if (!href.StartsWith('/') || href.Contains('{')) return false;
        var excluded = new[] { "/Account", "/Error", "/not-found", "/notfound", "/_" };
        return !excluded.Any(e => href.Equals(e, StringComparison.OrdinalIgnoreCase)
            || href.StartsWith(e + "/", StringComparison.OrdinalIgnoreCase));
    }

    private static string IconFor(string href, string fileName)
    {
        if (href == "/") return "home";
        var key = fileName.ToLowerInvariant();
        if (key.Contains("counter")) return "counter";
        if (key.Contains("weather")) return "weather";
        return "page";
    }

    private static string Humanize(string name) => Regex.Replace(name, "(?<=[a-z0-9])(?=[A-Z])", " ");

    internal static string? RewriteSidebarLinks(string sidebar, IReadOnlyList<PageLink> links)
    {
        var match = LinksPattern.Match(sidebar);
        if (!match.Success || links.Count == 0) return null;

        var existing = Regex.Replace(match.Groups[2].Value, @"\s+", "");
        if (existing != Regex.Replace(DefaultLinks, @"\s+", "")) return null;

        var newline = sidebar.Contains("\r\n") ? "\r\n" : "\n";
        var body = string.Concat(links.Select(l => $"{newline}        new(\"{Escape(l.Title)}\", \"{Escape(l.Href)}\", \"{l.Icon}\"),"));
        var rewritten = sidebar[..match.Groups[2].Index] + body + sidebar[(match.Groups[2].Index + match.Groups[2].Length)..];
        return rewritten == sidebar ? null : rewritten;
    }

    private static string Escape(string value) => value.Replace("\\", "\\\\").Replace("\"", "\\\"");

    private static async Task UpdateSidebarLinksAsync(string cwd, ShellUIConfig config, List<string> changes)
    {
        var sidebarPath = Path.Combine(cwd, config.ComponentsPath, "AppSidebar.razor");
        if (!File.Exists(sidebarPath)) return;

        var links = ScanPages(cwd, config);
        var rewritten = RewriteSidebarLinks(await File.ReadAllTextAsync(sidebarPath), links);
        if (rewritten == null) return;

        await File.WriteAllTextAsync(sidebarPath, rewritten);
        changes.Add($"AppSidebar.razor: links for your pages ({string.Join(", ", links.Select(l => l.Title))})");
    }

    private static readonly Regex IdentityRegistration = new(@"\.Add(Identity|IdentityCore|DefaultIdentity)<", RegexOptions.Compiled);
    private static readonly Regex FooterPattern = new(@"<SidebarFooter>.*?</SidebarFooter>", RegexOptions.Compiled | RegexOptions.Singleline);

    internal const string AccountFooter = "<SidebarFooter>\n        <SidebarAccount />\n    </SidebarFooter>";

    internal static bool IsIdentityApp(string cwd) =>
        Directory.Exists(Path.Combine(cwd, "Components", "Account"))
        && EnumerateSources(cwd, "*.cs").Any(f => IdentityRegistration.IsMatch(File.ReadAllText(f)));

    // Only the template's placeholder user footer is replaced; a customized footer is left alone.
    internal static string? RewriteSidebarFooter(string sidebar, string templateFooter)
    {
        var match = FooterPattern.Match(sidebar);
        if (!match.Success) return null;
        static string Squash(string s) => Regex.Replace(s, @"\s+", "");
        if (Squash(match.Value) != Squash(templateFooter)) return null;

        var newline = sidebar.Contains("\r\n") ? "\r\n" : "\n";
        return sidebar[..match.Index] + AccountFooter.Replace("\n", newline) + sidebar[(match.Index + match.Length)..];
    }

    private static async Task UseAccountMenuAsync(string cwd, ShellUIConfig config, List<string> changes, List<string> notes)
    {
        var sidebarPath = Path.Combine(cwd, config.ComponentsPath, "AppSidebar.razor");
        if (!File.Exists(sidebarPath) || !File.Exists(Path.Combine(cwd, config.ComponentsPath, "SidebarAccount.razor"))) return;

        var sidebar = await File.ReadAllTextAsync(sidebarPath);
        if (sidebar.Contains("<SidebarAccount", StringComparison.Ordinal)) return;

        var templateFooter = FooterPattern.Match(ShellUI.Templates.Templates.AppSidebarTemplate.Content).Value;
        var rewritten = RewriteSidebarFooter(sidebar, templateFooter);
        if (rewritten == null)
        {
            notes.Add("AppSidebar.razor has a custom footer. Add <SidebarAccount /> to it for Log in, Register and Log out.");
            return;
        }

        await File.WriteAllTextAsync(sidebarPath, rewritten);
        changes.Add("AppSidebar.razor: footer shows Log in, Register and Log out (ASP.NET Core Identity)");
    }

    private static string Rel(string cwd, string path) => Path.GetRelativePath(cwd, path).Replace('\\', '/');
}
