using System.Collections.Concurrent;
using System.Linq;

namespace ShellUI.Components;

public static class Shell
{
    // Joins class lists. When two classes set the same CSS property under the same variants, the later one wins
    // (like tailwind-merge), so a component's Class parameter overrides its defaults. Unknown classes are kept.
    public static string Cn(params string?[] classes)
    {
        var joined = string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
        if (joined.Length == 0) return "";
        if (Cache.TryGetValue(joined, out var cached)) return cached;
        var merged = Merge(joined);
        if (Cache.Count > 2000) Cache.Clear();
        Cache[joined] = merged;
        return merged;
    }

    private static readonly ConcurrentDictionary<string, string> Cache = new();

    private static string Merge(string classes)
    {
        var tokens = classes.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        var keep = new bool[tokens.Length];
        var taken = new System.Collections.Generic.HashSet<string>();
        for (var i = tokens.Length - 1; i >= 0; i--)
        {
            var (variants, important, utility) = Parse(tokens[i]);
            var group = Group(utility);
            if (group == null) { keep[i] = true; continue; }
            var scope = variants + (important ? "!" : "") + "|";
            if (!taken.Add(scope + group)) continue;
            keep[i] = true;
            if (Covers.TryGetValue(group, out var covered))
                foreach (var g in covered) taken.Add(scope + g);
        }
        return string.Join(" ", tokens.Where((_, i) => keep[i]));
    }

    private static (string Variants, bool Important, string Utility) Parse(string token)
    {
        var depth = 0;
        var cut = -1;
        for (var i = 0; i < token.Length; i++)
        {
            var c = token[i];
            if (c is '[' or '(') depth++;
            else if (c is ']' or ')') depth--;
            else if (c == ':' && depth == 0) cut = i;
        }
        var variants = cut < 0 ? "" : string.Join(":", token[..cut].Split(':').OrderBy(v => v, System.StringComparer.Ordinal));
        var utility = token[(cut + 1)..];
        var important = false;
        if (utility.StartsWith('!')) { important = true; utility = utility[1..]; }
        if (utility.EndsWith('!')) { important = true; utility = utility[..^1]; }
        if (utility.StartsWith('-')) utility = utility[1..];
        return (variants, important, utility);
    }

    private static string? Group(string u)
    {
        if (u.Length == 0) return null;
        if (u[0] == '[') return u.IndexOf(':') is > 1 and var colon ? "prop:" + u[1..colon] : null;
        if (Keywords.TryGetValue(u, out var keyword)) return keyword;

        foreach (var prefix in Prefixes)
        {
            if (u != prefix && !u.StartsWith(prefix + "-", System.StringComparison.Ordinal)) continue;
            var value = u.Length == prefix.Length ? "" : u[(prefix.Length + 1)..];
            return Classify(prefix, value);
        }
        return null;
    }

    private static string? Classify(string prefix, string value)
    {
        var bare = value.Split('/')[0];
        switch (prefix)
        {
            case "text":
                if (TextSizes.Contains(bare) || IsArbitraryLength(value)) return "font-size";
                if (value is "left" or "center" or "right" or "justify" or "start" or "end") return "text-align";
                if (value is "wrap" or "nowrap" or "balance" or "pretty") return "text-wrap";
                if (value is "ellipsis" or "clip") return "text-overflow";
                return value.Length == 0 ? null : "text-color";
            case "font":
                if (FontWeights.Contains(value) || (IsArbitrary(value) && char.IsDigit(value[1]))) return "font-weight";
                return value.Length == 0 ? null : "font-family";
            case "bg":
                if (value.Length == 0) return null;
                if (value is "none" || value.StartsWith("linear") || value.StartsWith("gradient") || value.StartsWith("radial") || value.StartsWith("conic") || value.StartsWith("[url(")) return "bg-image";
                if (value is "auto" or "cover" or "contain") return "bg-size";
                if (value is "fixed" or "local" or "scroll") return "bg-attachment";
                if (value.StartsWith("repeat") || value == "no-repeat") return "bg-repeat";
                if (value is "center" or "top" or "bottom" or "left" or "right" or "left-top" or "left-bottom" or "right-top" or "right-bottom") return "bg-position";
                if (value.StartsWith("clip-") || value.StartsWith("origin-") || value.StartsWith("blend-")) return "bg-" + value.Split('-')[0];
                return "bg-color";
            case "border" or "border-x" or "border-y" or "border-t" or "border-r" or "border-b" or "border-l" or "border-s" or "border-e":
                var side = prefix == "border" ? "" : prefix[6..];
                if (value.Length == 0 || IsNumber(value) || IsArbitraryLength(value)) return "border-w" + side;
                if (prefix == "border" && value is "solid" or "dashed" or "dotted" or "double" or "hidden" or "none") return "border-style";
                return "border-color" + side;
            case "ring":
                if (value.Length == 0 || IsNumber(value) || IsArbitraryLength(value)) return "ring-w";
                return value == "inset" ? "ring-inset" : "ring-color";
            case "ring-offset":
                return IsNumber(value) || IsArbitraryLength(value) ? "ring-offset-w" : "ring-offset-color";
            case "outline":
                if (value.Length == 0 || IsNumber(value) || IsArbitraryLength(value)) return "outline-w";
                if (value is "none" or "hidden" or "solid" or "dashed" or "dotted" or "double") return "outline-style";
                return "outline-color";
            case "shadow":
                if (value.Length == 0 || ShadowSizes.Contains(value) || IsArbitraryLength(value)) return "shadow";
                return "shadow-color";
            case "stroke":
                return IsNumber(value) || IsArbitraryLength(value) ? "stroke-w" : "stroke";
            case "from" or "via" or "to":
                return prefix + (value.EndsWith('%') ? "-position" : "-color");
            case "object":
                return value is "contain" or "cover" or "fill" or "none" or "scale-down" ? "object-fit" : "object-position";
            case "flex":
                return value.Length == 0 ? null : "flex";
            case "inset":
                return value.StartsWith("ring") || value.StartsWith("shadow") ? null : "inset";
            default:
                return prefix;
        }
    }

    private static bool IsNumber(string v) => v.Length > 0 && v.All(c => char.IsDigit(c) || c == '.');
    private static bool IsArbitrary(string v) => v.Length > 2 && v[0] == '[' && v[^1] == ']';
    private static bool IsArbitraryLength(string v) =>
        IsArbitrary(v) && (v.StartsWith("[length:") || char.IsDigit(v[1]) || v[1] == '.');

    private static readonly System.Collections.Generic.HashSet<string> TextSizes =
        new() { "xs", "sm", "base", "lg", "xl", "2xl", "3xl", "4xl", "5xl", "6xl", "7xl", "8xl", "9xl" };
    private static readonly System.Collections.Generic.HashSet<string> FontWeights =
        new() { "thin", "extralight", "light", "normal", "medium", "semibold", "bold", "extrabold", "black" };
    private static readonly System.Collections.Generic.HashSet<string> ShadowSizes =
        new() { "2xs", "xs", "sm", "md", "lg", "xl", "2xl", "inner", "none" };

    private static readonly System.Collections.Generic.Dictionary<string, string> Keywords = new()
    {
        ["block"] = "display", ["inline-block"] = "display", ["inline"] = "display", ["flex"] = "display",
        ["inline-flex"] = "display", ["grid"] = "display", ["inline-grid"] = "display", ["hidden"] = "display",
        ["contents"] = "display", ["table"] = "display", ["table-row"] = "display", ["table-cell"] = "display",
        ["flow-root"] = "display", ["list-item"] = "display",
        ["static"] = "position", ["fixed"] = "position", ["absolute"] = "position", ["relative"] = "position", ["sticky"] = "position",
        ["visible"] = "visibility", ["invisible"] = "visibility", ["collapse"] = "visibility",
        ["uppercase"] = "text-transform", ["lowercase"] = "text-transform", ["capitalize"] = "text-transform", ["normal-case"] = "text-transform",
        ["italic"] = "font-style", ["not-italic"] = "font-style",
        ["underline"] = "text-decoration", ["overline"] = "text-decoration", ["line-through"] = "text-decoration", ["no-underline"] = "text-decoration",
        ["flex-row"] = "flex-direction", ["flex-row-reverse"] = "flex-direction", ["flex-col"] = "flex-direction", ["flex-col-reverse"] = "flex-direction",
        ["flex-wrap"] = "flex-wrap", ["flex-wrap-reverse"] = "flex-wrap", ["flex-nowrap"] = "flex-wrap",
        ["border-collapse"] = "border-collapse", ["border-separate"] = "border-collapse",
        ["sr-only"] = "sr-only", ["not-sr-only"] = "sr-only",
    };

    // Longest first, so "border-t" wins over "border" and "ring-offset" over "ring".
    private static readonly string[] Prefixes = new[]
    {
        "inset-x", "inset-y", "inset", "top", "right", "bottom", "left", "start", "end", "z",
        "order", "grow", "shrink", "basis", "flex",
        "grid-cols", "grid-rows", "col-span", "col-start", "col-end", "row-span", "row-start", "row-end",
        "gap-x", "gap-y", "gap",
        "justify-items", "justify-self", "justify", "items", "self", "place-items", "place-content", "place-self",
        "p", "px", "py", "pt", "pr", "pb", "pl", "ps", "pe",
        "m", "mx", "my", "mt", "mr", "mb", "ml", "ms", "me", "space-x", "space-y",
        "w", "min-w", "max-w", "h", "min-h", "max-h", "size",
        "overflow-x", "overflow-y", "overflow",
        "leading", "tracking", "line-clamp", "whitespace", "align",
        "opacity", "cursor", "pointer-events", "select", "resize",
        "rounded", "rounded-s", "rounded-e", "rounded-t", "rounded-r", "rounded-b", "rounded-l",
        "rounded-ss", "rounded-se", "rounded-ee", "rounded-es", "rounded-tl", "rounded-tr", "rounded-br", "rounded-bl",
        "border", "border-x", "border-y", "border-t", "border-r", "border-b", "border-l", "border-s", "border-e",
        "ring-offset", "ring", "outline-offset", "outline", "shadow",
        "text", "font", "bg", "fill", "stroke", "from", "via", "to", "accent", "caret",
        "transition", "duration", "ease", "delay", "animate",
        "translate-x", "translate-y", "translate", "scale-x", "scale-y", "scale", "rotate", "skew-x", "skew-y", "origin",
        "object", "aspect", "blur", "backdrop-blur",
    }.OrderByDescending(p => p.Length).ToArray();

    // A wider utility replaces earlier narrower ones (p-4 after px-2), but not the other way round.
    private static readonly System.Collections.Generic.Dictionary<string, string[]> Covers = new()
    {
        ["inset"] = new[] { "inset-x", "inset-y", "top", "right", "bottom", "left", "start", "end" },
        ["inset-x"] = new[] { "left", "right", "start", "end" },
        ["inset-y"] = new[] { "top", "bottom" },
        ["flex"] = new[] { "grow", "shrink", "basis" },
        ["gap"] = new[] { "gap-x", "gap-y" },
        ["p"] = new[] { "px", "py", "pt", "pr", "pb", "pl", "ps", "pe" },
        ["px"] = new[] { "pr", "pl", "ps", "pe" },
        ["py"] = new[] { "pt", "pb" },
        ["m"] = new[] { "mx", "my", "mt", "mr", "mb", "ml", "ms", "me" },
        ["mx"] = new[] { "mr", "ml", "ms", "me" },
        ["my"] = new[] { "mt", "mb" },
        ["size"] = new[] { "w", "h" },
        ["overflow"] = new[] { "overflow-x", "overflow-y" },
        ["rounded"] = new[] { "rounded-s", "rounded-e", "rounded-t", "rounded-r", "rounded-b", "rounded-l", "rounded-ss", "rounded-se", "rounded-ee", "rounded-es", "rounded-tl", "rounded-tr", "rounded-br", "rounded-bl" },
        ["rounded-t"] = new[] { "rounded-tl", "rounded-tr" },
        ["rounded-r"] = new[] { "rounded-tr", "rounded-br" },
        ["rounded-b"] = new[] { "rounded-br", "rounded-bl" },
        ["rounded-l"] = new[] { "rounded-tl", "rounded-bl" },
        ["rounded-s"] = new[] { "rounded-ss", "rounded-es" },
        ["rounded-e"] = new[] { "rounded-se", "rounded-ee" },
        ["border-w"] = new[] { "border-w-x", "border-w-y", "border-w-t", "border-w-r", "border-w-b", "border-w-l", "border-w-s", "border-w-e" },
        ["border-w-x"] = new[] { "border-w-r", "border-w-l", "border-w-s", "border-w-e" },
        ["border-w-y"] = new[] { "border-w-t", "border-w-b" },
        ["border-color"] = new[] { "border-color-x", "border-color-y", "border-color-t", "border-color-r", "border-color-b", "border-color-l", "border-color-s", "border-color-e" },
        ["border-color-x"] = new[] { "border-color-r", "border-color-l", "border-color-s", "border-color-e" },
        ["border-color-y"] = new[] { "border-color-t", "border-color-b" },
        ["translate"] = new[] { "translate-x", "translate-y" },
        ["scale"] = new[] { "scale-x", "scale-y" },
    };
}
