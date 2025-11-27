using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AvatarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "avatar",
        DisplayName = "Avatar",
        Description = "User avatar image component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Avatar.razor",
        Version = "0.1.0",
        Tags = new List<string> { "avatar", "image", "user", "profile" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<span class=""@(""relative flex shrink-0 overflow-hidden rounded-full "" + (Size == ""sm"" ? ""h-8 w-8"" : Size == ""lg"" ? ""h-12 w-12"" : Size == ""xl"" ? ""h-16 w-16"" : ""h-10 w-10"") + "" "" + ClassName)"" @attributes=""AdditionalAttributes"">
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
            <span class=""material-symbols-outlined text-muted-foreground @(Size == ""sm"" ? ""text-base"" : Size == ""lg"" ? ""text-2xl"" : Size == ""xl"" ? ""text-3xl"" : ""text-xl"")"">person</span>
        </span>
    }
</span>

@code {
    [Parameter]
    public string? Src { get; set; }
    
    [Parameter]
    public string? Alt { get; set; }
    
    [Parameter]
    public string? Fallback { get; set; }
    
    [Parameter]
    public string Size { get; set; } = ""default"";
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

