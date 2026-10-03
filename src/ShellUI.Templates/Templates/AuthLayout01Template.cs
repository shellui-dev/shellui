using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AuthLayout01Template
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "auth-01",
        DisplayName = "Auth 01",
        Description = "Centered card on a muted background for the sign-in, sign-up and account recovery pages (shadcn login-02/03 style)",
        Category = ComponentCategory.Layout,
        FilePath = "AuthLayout01.razor",
        IsLayoutBlock = true,
        Tags = new List<string> { "auth", "login", "signup", "layout", "block", "card" }
    };

    public static string Content => @"@inherits LayoutComponentBase

<div class=""flex min-h-svh flex-col items-center justify-center gap-6 bg-muted p-6 md:p-10"">
    <div class=""flex w-full max-w-md flex-col gap-6"">
        <a href="""" class=""flex items-center gap-2 self-center font-medium"">
            <span class=""flex size-6 items-center justify-center rounded-md bg-primary text-xs font-bold text-primary-foreground"">A</span>
            My App
        </a>
        <main class=""rounded-xl border border-border bg-card p-6 text-card-foreground shadow-sm md:p-8"">
            @Body
        </main>
    </div>
</div>
";
}
