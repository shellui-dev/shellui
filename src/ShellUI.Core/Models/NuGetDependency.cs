namespace ShellUI.Core.Models;

public class NuGetDependency
{
    public required string PackageId { get; init; }
    public required string Version { get; init; }
}
