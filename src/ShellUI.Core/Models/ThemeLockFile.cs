namespace ShellUI.Core.Models;

/// Records the tweakcn theme applied by `shellui theme apply`, so `shellui theme update` knows where to re-fetch from.
public class ThemeLockFile
{
    public required string SourceUrl { get; set; }
    public required string ContentSha256 { get; set; }
    public required string ThemeName { get; set; }
    public required DateTime AppliedAt { get; set; }
}
