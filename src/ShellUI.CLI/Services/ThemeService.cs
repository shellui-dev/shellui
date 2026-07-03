using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using ShellUI.Core.Models;
using Spectre.Console;

namespace ShellUI.CLI.Services;

/// Fetches themes from tweakcn.com and bakes them into a project's input.css at build time.
public static class ThemeService
{
    private const string LockFileName = "shellui.theme.lock";

    // User content OUTSIDE these markers survives re-applies.
    internal const string BeginMarker = "/* BEGIN shellui theme — managed by `shellui theme apply` */";
    internal const string EndMarker = "/* END shellui theme */";

    public static string NormalizeTweakcnUrl(string input)
    {
        input = input.Trim();
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("theme URL or id is empty", nameof(input));

        if (!input.Contains('/') && !input.Contains(':'))
            return $"https://tweakcn.com/r/themes/{input}";

        var editorMatch = Regex.Match(input, @"^https?://tweakcn\.com/themes/([^/?#]+)");
        if (editorMatch.Success)
            return $"https://tweakcn.com/r/themes/{editorMatch.Groups[1].Value}";

        if (Regex.IsMatch(input, @"^https?://tweakcn\.com/r/themes/"))
            return input;

        throw new ArgumentException(
            $"Unrecognized tweakcn URL: '{input}'. Expected https://tweakcn.com/themes/<id> or a bare theme id.",
            nameof(input));
    }

    // Kept separate from ParseTheme so callers can hash the exact bytes for the lock file.
    public static async Task<string> FetchThemeJsonAsync(string url, HttpClient? httpClient = null)
    {
        var normalized = NormalizeTweakcnUrl(url);
        var owned = httpClient is null;
        var client = httpClient ?? new HttpClient();
        try
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            using var response = await client.GetAsync(normalized);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"tweakcn returned {(int)response.StatusCode} for {normalized}. " +
                    $"Check the theme id — is the URL correct? Is the theme public?");
            }
            return await response.Content.ReadAsStringAsync();
        }
        finally
        {
            if (owned) client.Dispose();
        }
    }

    public static TweakcnTheme ParseTheme(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        var name = root.TryGetProperty("name", out var nameEl) ? nameEl.GetString() ?? "theme" : "theme";
        var cssVars = root.GetProperty("cssVars");

        return new TweakcnTheme
        {
            Name = name,
            ThemeVars = ExtractDict(cssVars, "theme"),
            LightVars = ExtractDict(cssVars, "light"),
            DarkVars = ExtractDict(cssVars, "dark"),
        };
    }

    private static Dictionary<string, string> ExtractDict(JsonElement parent, string prop)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        if (!parent.TryGetProperty(prop, out var el) || el.ValueKind != JsonValueKind.Object)
            return result;
        foreach (var kv in el.EnumerateObject())
        {
            var value = kv.Value.ValueKind switch
            {
                JsonValueKind.String => kv.Value.GetString(),
                JsonValueKind.Number => kv.Value.GetRawText(),
                _ => null,
            };
            if (!string.IsNullOrEmpty(value)) result[kv.Name] = value!;
        }
        return result;
    }

    public static string BuildThemeCss(TweakcnTheme theme)
    {
        var sb = new StringBuilder();
        sb.AppendLine(BeginMarker);
        sb.AppendLine($"/* Theme: {theme.Name} — fetched from tweakcn */");

        WriteBlock(sb, ":root", theme.LightVars);
        WriteBlock(sb, ".dark", theme.DarkVars);

        if (theme.ThemeVars.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("@theme inline {");
            foreach (var (k, v) in theme.ThemeVars.OrderBy(kv => kv.Key, StringComparer.Ordinal))
                sb.AppendLine($"  --{k}: {v};");
            sb.AppendLine("}");
        }

        sb.AppendLine(EndMarker);
        return sb.ToString();
    }

    private static void WriteBlock(StringBuilder sb, string selector, Dictionary<string, string> vars)
    {
        if (vars.Count == 0) return;
        sb.AppendLine();
        sb.AppendLine($"{selector} {{");
        foreach (var (k, v) in vars.OrderBy(kv => kv.Key, StringComparer.Ordinal))
            sb.AppendLine($"  --{k}: {v};");
        sb.AppendLine("}");
    }

    /// Idempotent — the sentinel-marked region is replaced entirely; content outside it is preserved verbatim.
    public static void ApplyToInputCss(string inputCssPath, TweakcnTheme theme)
    {
        var newBlock = BuildThemeCss(theme).TrimEnd() + Environment.NewLine;

        if (!File.Exists(inputCssPath))
        {
            var starter = "@import \"tailwindcss\";" + Environment.NewLine
                + "@custom-variant dark (&:is(.dark *));" + Environment.NewLine
                + Environment.NewLine
                + newBlock;
            Directory.CreateDirectory(Path.GetDirectoryName(inputCssPath)!);
            File.WriteAllText(inputCssPath, starter);
            return;
        }

        var content = File.ReadAllText(inputCssPath);
        var beginIdx = content.IndexOf(BeginMarker, StringComparison.Ordinal);
        var endIdx = content.IndexOf(EndMarker, StringComparison.Ordinal);

        if (beginIdx >= 0 && endIdx > beginIdx)
        {
            var before = content.Substring(0, beginIdx);
            var after = content.Substring(endIdx + EndMarker.Length);
            File.WriteAllText(inputCssPath, before + newBlock.TrimEnd() + after);
            return;
        }

        var trailing = content.EndsWith(Environment.NewLine) ? "" : Environment.NewLine;
        File.WriteAllText(inputCssPath, content + trailing + Environment.NewLine + newBlock);
    }

    /// For Path A/D consumers: writes only the theme block so they can link it AFTER the precompiled bundle.
    public static void EmitOverride(string outputPath, TweakcnTheme theme)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        File.WriteAllText(outputPath, BuildThemeCss(theme));
    }

    public static void WriteLockFile(string projectRoot, string sourceUrl, string jsonBody, string themeName)
    {
        var lockFile = new ThemeLockFile
        {
            SourceUrl = sourceUrl,
            ContentSha256 = Sha256Hex(jsonBody),
            ThemeName = themeName,
            AppliedAt = DateTime.UtcNow,
        };
        var path = Path.Combine(projectRoot, LockFileName);
        File.WriteAllText(path, JsonSerializer.Serialize(lockFile, new JsonSerializerOptions { WriteIndented = true }));
    }

    public static ThemeLockFile? ReadLockFile(string projectRoot)
    {
        var path = Path.Combine(projectRoot, LockFileName);
        if (!File.Exists(path)) return null;
        try
        {
            return JsonSerializer.Deserialize<ThemeLockFile>(File.ReadAllText(path));
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string Sha256Hex(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}

public class TweakcnTheme
{
    public required string Name { get; init; }
    public required Dictionary<string, string> ThemeVars { get; init; }
    public required Dictionary<string, string> LightVars { get; init; }
    public required Dictionary<string, string> DarkVars { get; init; }
}
