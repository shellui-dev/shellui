using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AvatarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "avatar",
        DisplayName = "Avatar",
        Description = "User avatar image component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Avatar.razor",
        Version = "0.1.0",
        Dependencies = new List<string> { "avatar-variants" },
        Tags = new List<string> { "avatar", "image", "user", "profile" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<span class=""@ComputedClass"" @attributes=""AdditionalAttributes"">
    @if (!string.IsNullOrEmpty(Src))
    {
        <img src=""@Src"" alt=""@Alt"" class=""aspect-square h-full w-full object-cover"" />
    }
    else if (!string.IsNullOrEmpty(Fallback))
    {
        <span class=""flex h-full w-full items-center justify-center rounded-full bg-muted text-sm font-medium"">
            @Fallback
        </span>
    }
    else
    {
        <span class=""flex h-full w-full items-center justify-center rounded-full bg-muted"">
            <svg class=""h-1/2 w-1/2 text-muted-foreground"" fill=""currentColor"" viewBox=""0 0 24 24"">
                <path d=""M24 20.993V24H0v-2.996A14.977 14.977 0 0112.004 15c4.904 0 9.26 2.354 11.996 5.993zM16.002 8.999a4 4 0 11-8 0 4 4 0 018 0z"" />
            </svg>
        </span>
    }
</span>

@code {
    [Parameter] public string? Src { get; set; }
    [Parameter] public string? Alt { get; set; }
    [Parameter] public string? Fallback { get; set; }
    [Parameter] public AvatarSize Size { get; set; } = AvatarSize.Default;
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private string ComputedClass => AvatarVariants.Get(Size, Class);
}
";
}
