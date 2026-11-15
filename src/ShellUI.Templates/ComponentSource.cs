using System.Text.RegularExpressions;

namespace ShellUI.Templates;

// A CLI template read from the ShellUI.Components source it would otherwise copy, so the component is written once.
// The package's namespace becomes the placeholder the installer replaces with the app's own namespace.
public static class ComponentSource
{
    private static readonly Regex PackageNamespace = new(@"^(@namespace|namespace) ShellUI\.Components\b", RegexOptions.Multiline | RegexOptions.Compiled);

    // `path` is relative to src/ShellUI.Components, for example "Components/Kbd.razor".
    public static string Read(string path)
    {
        using var stream = typeof(ComponentSource).Assembly.GetManifestResourceStream($"ShellUI.Components/{path}")
            ?? throw new InvalidOperationException($"ShellUI.Components/{path} is not embedded in ShellUI.Templates.");
        using var reader = new StreamReader(stream);
        var text = PackageNamespace.Replace(reader.ReadToEnd(), "$1 YourProjectNamespace.Components.UI");
        // One line break at the end, whatever blank lines the source file ends with.
        return text.TrimEnd() + (text.Contains("\r\n") ? "\r\n" : "\n");
    }
}
