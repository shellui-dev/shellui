using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class QrCodeTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "qr-code",
        DisplayName = "QR Code",
        Description = "QR code rendered as SVG",
        Category = ComponentCategory.DataDisplay,
        FilePath = "QrCode.razor",
        Dependencies = new List<string>(),
        NuGetDependencies = new List<NuGetDependency>
        {
            new() { PackageId = "QRCoder", Version = "1.8.0" }
        },
        Tags = new List<string> { "qr", "code", "share" }
    };

    public static string Content => ComponentSource.Read("Components/QrCode.razor");
}
