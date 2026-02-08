using System;
using System.IO;
using System.Reflection;

namespace ShellUI.Core.Models;

public class ComponentMetadata
{
    public required string Name { get; set; }
    public required string DisplayName { get; set; }
    public required string Description { get; set; }
    public required ComponentCategory Category { get; set; }
    public List<string> Dependencies { get; set; } = new();

    // Relative to Components/UI folder
    public required string FilePath { get; set; }

    public bool IsAvailable { get; set; } = true;

    // Version is now computed from centralized version
    public string Version => GetCurrentVersion();

    public List<string> Variants { get; set; } = new();
    public List<string> Tags { get; set; } = new();

    private static string GetCurrentVersion()
    {
        // Try to read version from Directory.Build.props at runtime
        try
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            var dir = new DirectoryInfo(currentDir);

            while (dir != null)
            {
                var propsFile = Path.Combine(dir.FullName, "Directory.Build.props");
                if (File.Exists(propsFile))
                {
                    var content = File.ReadAllText(propsFile);
                    var match = System.Text.RegularExpressions.Regex.Match(content, @"<ShellUIVersion>([^<]+)</ShellUIVersion>");
                    if (match.Success)
                    {
                        var version = match.Groups[1].Value.Trim();
                        var suffixMatch = System.Text.RegularExpressions.Regex.Match(content, @"<ShellUIVersionSuffix>([^<]*)</ShellUIVersionSuffix>");
                        if (suffixMatch.Success && !string.IsNullOrEmpty(suffixMatch.Groups[1].Value.Trim()))
                        {
                            version += "-" + suffixMatch.Groups[1].Value.Trim();
                        }
                        return version;
                    }
                }
                dir = dir.Parent;
            }
        }
        catch
        {
            // Ignore errors
        }

        // Fallback: read from assembly version (set at build time by Directory.Build.props)
        var assembly = typeof(ComponentMetadata).Assembly;
        var assemblyVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (assemblyVersion != null)
            return assemblyVersion.InformationalVersion.Split('+')[0]; // strip build metadata

        return "0.2.0";
    }
}

