using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SonnerServiceTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sonner-service",
        DisplayName = "Sonner Service",
        Description = "Service for showing and managing Sonner toast notifications",
        Category = ComponentCategory.Feedback,
        FilePath = "Services/SonnerService.cs",
        IsAvailable = false,
        Tags = new List<string> { "sonner", "toast", "service", "feedback" }
    };

    public static string Content => ComponentSource.Read("Services/SonnerService.cs");
}
