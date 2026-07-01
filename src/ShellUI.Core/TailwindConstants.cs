namespace ShellUI.Core;

/// Single source of truth for the Tailwind CSS version ShellUI ships against.
/// Bump this and every consumer (downloader, npm install string, config default,
/// docs sweep) picks it up via a rebuild.
public static class TailwindConstants
{
    public const string Version = "4.3.2";
    public const string GitHubTag = "v" + Version;
    public const string NpmRange = "^" + Version;
}
