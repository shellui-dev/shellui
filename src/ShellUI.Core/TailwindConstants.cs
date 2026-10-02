namespace ShellUI.Core;

/// The Tailwind CSS version ShellUI targets; everything else reads it from here.
public static class TailwindConstants
{
    public const string Version = "4.3.2";
    public const string GitHubTag = "v" + Version;
    public const string NpmRange = "^" + Version;
}
