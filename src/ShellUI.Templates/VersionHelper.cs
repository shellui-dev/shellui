using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ShellUI.Templates;

public static class VersionHelper
{
    private static string? _cachedVersion;

    // Gets the current ShellUI version from Directory.Build.props
    public static string GetCurrentVersion()
    {
        if (_cachedVersion != null)
            return _cachedVersion;

        try
        {
            var solutionRoot = FindSolutionRoot();
            if (solutionRoot != null)
            {
                var propsFile = Path.Combine(solutionRoot, "Directory.Build.props");
                if (File.Exists(propsFile))
                {
                    var content = File.ReadAllText(propsFile);
                    var match = Regex.Match(content, @"<ShellUIVersion>([^<]+)</ShellUIVersion>");
                    if (match.Success)
                    {
                        var version = match.Groups[1].Value.Trim();
                        var suffixMatch = Regex.Match(content, @"<ShellUIVersionSuffix>([^<]*)</ShellUIVersionSuffix>");
                        if (suffixMatch.Success && !string.IsNullOrEmpty(suffixMatch.Groups[1].Value.Trim()))
                        {
                            version += "-" + suffixMatch.Groups[1].Value.Trim();
                        }
                        _cachedVersion = version;
                        return version;
                    }
                }
            }
        }
        catch
        {
        }

        // Fallback: the assembly version set from Directory.Build.props
        var assembly = typeof(VersionHelper).Assembly;
        var assemblyVersion = assembly.GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>();
        if (assemblyVersion != null)
        {
            _cachedVersion = assemblyVersion.InformationalVersion.Split('+')[0];
            return _cachedVersion;
        }

        return "0.3.0";
    }

    private static string? FindSolutionRoot()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var dir = new DirectoryInfo(currentDir);

        while (dir != null)
        {
            if (dir.GetFiles("*.sln").Any() || dir.Name == "src")
            {
                return dir.FullName;
            }
            dir = dir.Parent;
        }

        return null;
    }
}