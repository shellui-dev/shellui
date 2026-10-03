using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarAccountTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-account",
        DisplayName = "Sidebar Account",
        Description = "Log in, Register and Log out for the dashboard sidebar in apps that use ASP.NET Core Identity",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarAccount.razor",
        Dependencies = new List<string> { "sidebar", "avatar" },
        IsAvailable = false,
        Tags = new List<string> { "sidebar", "identity", "auth", "account" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using Microsoft.AspNetCore.Components.Authorization
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@implements IDisposable
@inject NavigationManager Navigation

<SidebarMenu>
    <AuthorizeView>
        <Authorized>
            <SidebarMenuItem>
                <SidebarMenuButton Href=""Account/Manage"" Tooltip=""Manage account"" Size=""lg"" Class=""group-data-[collapsible=icon]:!size-8"">
                    <Avatar Fallback=""@Initial(context.User.Identity?.Name)"" Class=""h-8 w-8 shrink-0 rounded-full group-data-[collapsible=icon]:!size-8"" />
                    <div class=""grid min-w-0 flex-1 text-left text-sm leading-tight group-data-[collapsible=icon]:hidden"">
                        <span class=""truncate font-semibold"">@context.User.Identity?.Name</span>
                        <span class=""truncate text-xs text-sidebar-foreground/70"">Manage account</span>
                    </div>
                </SidebarMenuButton>
            </SidebarMenuItem>
            <SidebarMenuItem>
                <form action=""Account/Logout"" method=""post"">
                    <AntiforgeryToken />
                    <input type=""hidden"" name=""ReturnUrl"" value=""@_currentUrl"" />
                    <SidebarMenuButton type=""submit"" Tooltip=""Log out"">
                        <svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""size-4""><path d=""M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4""/><path d=""m16 17 5-5-5-5""/><path d=""M21 12H9""/></svg>
                        <span>Log out</span>
                    </SidebarMenuButton>
                </form>
            </SidebarMenuItem>
        </Authorized>
        <NotAuthorized>
            <SidebarMenuItem>
                <SidebarMenuButton Href=""Account/Login"" Tooltip=""Log in"">
                    <svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""size-4""><path d=""M15 3h4a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2h-4""/><path d=""m10 17 5-5-5-5""/><path d=""M15 12H3""/></svg>
                    <span>Log in</span>
                </SidebarMenuButton>
            </SidebarMenuItem>
            <SidebarMenuItem>
                <SidebarMenuButton Href=""Account/Register"" Tooltip=""Register"">
                    <svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""size-4""><path d=""M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2""/><circle cx=""9"" cy=""7"" r=""4""/><path d=""M19 8v6""/><path d=""M22 11h-6""/></svg>
                    <span>Register</span>
                </SidebarMenuButton>
            </SidebarMenuItem>
        </NotAuthorized>
    </AuthorizeView>
</SidebarMenu>

@code {
    private string? _currentUrl;

    protected override void OnInitialized()
    {
        _currentUrl = Navigation.ToBaseRelativePath(Navigation.Uri);
        Navigation.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _currentUrl = Navigation.ToBaseRelativePath(e.Location);
        InvokeAsync(StateHasChanged);
    }

    private static string Initial(string? name) => string.IsNullOrEmpty(name) ? ""?"" : name[..1].ToUpperInvariant();

    public void Dispose() => Navigation.LocationChanged -= OnLocationChanged;
}
";
}
