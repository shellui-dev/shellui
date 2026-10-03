using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AuthLayout02Template
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "auth-02",
        DisplayName = "Auth 02",
        Description = "Split screen with the form beside a brand panel for the sign-in, sign-up and account recovery pages (shadcn login-04/05 style)",
        Category = ComponentCategory.Layout,
        FilePath = "AuthLayout02.razor",
        IsLayoutBlock = true,
        Tags = new List<string> { "auth", "login", "signup", "layout", "block", "split" }
    };

    public static string Content => @"@inherits LayoutComponentBase

<div class=""grid min-h-svh lg:grid-cols-2"">
    <div class=""flex flex-col gap-4 p-6 md:p-10"">
        <a href="""" class=""flex items-center gap-2 self-center font-medium md:self-start"">
            <span class=""flex size-6 items-center justify-center rounded-md bg-primary text-xs font-bold text-primary-foreground"">A</span>
            My App
        </a>
        <main class=""flex flex-1 items-center justify-center"">
            <div class=""w-full max-w-md"">
                @Body
            </div>
        </main>
    </div>
    <div class=""hidden flex-col justify-end gap-2 bg-primary p-10 text-primary-foreground lg:flex"">
        <p class=""text-2xl font-semibold tracking-tight"">My App</p>
        <p class=""text-sm opacity-80"">Sign in or create an account to continue.</p>
    </div>
</div>
";
}
