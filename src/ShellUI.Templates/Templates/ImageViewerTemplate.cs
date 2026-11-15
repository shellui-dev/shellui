using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ImageViewerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "image-viewer",
        DisplayName = "Image Viewer",
        Description = "Image thumbnail that opens a zoomable lightbox",
        Category = ComponentCategory.Media,
        FilePath = "ImageViewer.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "image", "lightbox", "media", "zoom" }
    };

    public static string Content => ComponentSource.Read("Components/ImageViewer.razor");
}
