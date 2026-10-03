using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AuthLayout03Template
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "auth-03",
        DisplayName = "Auth 03",
        Description = "Minimal centered form for the sign-in, sign-up and account recovery pages (shadcn login-01 style)",
        Category = ComponentCategory.Layout,
        FilePath = "AuthLayout03.razor",
        IsLayoutBlock = true,
        Tags = new List<string> { "auth", "login", "signup", "layout", "block", "minimal" }
    };

    public static string Content => @"@inherits LayoutComponentBase

<div class=""flex min-h-svh flex-col items-center justify-center p-6 md:p-10"">
    <main class=""w-full max-w-md"">
        <a href="""" class=""mb-8 flex items-center gap-2 font-medium"">
            <span class=""flex size-6 items-center justify-center rounded-md bg-primary text-xs font-bold text-primary-foreground"">A</span>
            My App
        </a>
        @Body
    </main>
</div>
";
}
