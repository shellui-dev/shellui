using System.Text.RegularExpressions;

namespace ShellUI.Templates;

// A CLI template read from the ShellUI.Components source it would otherwise copy, so the component is written once.
// The package's namespaces become the placeholders the installer replaces with the app's own namespace.
public static class ComponentSource
{
    private static readonly Regex PackageNamespace = new(@"^(@namespace|namespace) ShellUI\.Components\b", RegexOptions.Multiline | RegexOptions.Compiled);

    // `path` is relative to src/ShellUI.Components, for example "Components/Kbd.razor".
    // `variantsNamespace` is for the templates that install variants into Components.UI.Variants:
    // a .cs file gets that namespace and a .razor file a @using for it.
    public static string Read(string path, bool variantsNamespace = false)
    {
        using var stream = typeof(ComponentSource).Assembly.GetManifestResourceStream($"ShellUI.Components/{path}")
            ?? throw new InvalidOperationException($"ShellUI.Components/{path} is not embedded in ShellUI.Templates.");
        using var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        var newline = text.Contains("\r\n") ? "\r\n" : "\n";

        // Installed models and services keep the namespaces the CLI has always given them.
        text = text.Replace("ShellUI.Components.Models", "YourProjectNamespace.Components.Models")
                   .Replace("ShellUI.Components.Services", "YourProjectNamespace.Services");

        var isRazor = path.EndsWith(".razor", StringComparison.OrdinalIgnoreCase);
        var replacement = !variantsNamespace ? "$1 YourProjectNamespace.Components.UI"
            : isRazor ? $"$1 YourProjectNamespace.Components.UI{newline}@using YourProjectNamespace.Components.UI.Variants"
            : "$1 YourProjectNamespace.Components.UI.Variants";
        text = PackageNamespace.Replace(text, replacement);

        // One line break at the end, whatever blank lines the source file ends with.
        return text.TrimEnd() + newline;
    }
}
