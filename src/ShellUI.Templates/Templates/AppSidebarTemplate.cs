using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AppSidebarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "app-sidebar",
        DisplayName = "App Sidebar",
        Description = "Dashboard app sidebar with navigation groups (shadcn sidebar-01 style)",
        Category = ComponentCategory.Layout,
        FilePath = "AppSidebar.razor",
        Dependencies = new List<string> { "sidebar", "avatar" },
        IsAvailable = false,
        Tags = new List<string> { "sidebar", "navigation", "dashboard", "app" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@inject NavigationManager Navigation

@* AppSidebar - Inset sidebar. SidebarProvider > AppSidebar + SidebarInset. *@
<Sidebar Variant=""SidebarVariant.Inset"" Collapsible=""@Collapsible"" Class=""@Class"">
    <SidebarHeader>
        <SidebarMenu>
            <SidebarMenuItem>
                <SidebarMenuButton Size=""lg"" Href=""/"" Tooltip=""Home"" Class=""group-data-[collapsible=icon]:!bg-sidebar-primary group-data-[collapsible=icon]:text-sidebar-primary-foreground"">
                    <div class=""flex size-8 shrink-0 items-center justify-center rounded-lg bg-sidebar-primary text-sidebar-primary-foreground group-data-[collapsible=icon]:!size-8"">
                        <span class=""text-sm font-bold"">A</span>
                    </div>
                    <div class=""grid min-w-0 flex-1 text-left text-sm leading-tight group-data-[collapsible=icon]:hidden"">
                        <span class=""truncate font-semibold"">My App</span>
                        <span class=""truncate text-xs text-sidebar-foreground/70"">Dashboard</span>
                    </div>
                </SidebarMenuButton>
            </SidebarMenuItem>
        </SidebarMenu>
    </SidebarHeader>

    <SidebarContent>
        <SidebarGroup>
            <SidebarGroupLabel>Navigation</SidebarGroupLabel>
            <SidebarGroupContent>
                <SidebarMenu>
                    <SidebarMenuItem>
                        <SidebarMenuButton Href=""/"" IsActive=""@IsCurrentPath(""/"")"" Tooltip=""Home"">
                            <svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" class=""size-4""><path d=""M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2z""/><path d=""M9 22V12h6v10""/></svg>
                            <span>Home</span>
                        </SidebarMenuButton>
                    </SidebarMenuItem>
                    <SidebarMenuItem>
                        <SidebarMenuButton Href=""/dashboard"" IsActive=""@IsCurrentPath(""/dashboard"")"" Tooltip=""Dashboard"">
                            <svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" class=""size-4""><rect x=""3"" y=""3"" width=""7"" height=""7""/><rect x=""14"" y=""3"" width=""7"" height=""7""/><rect x=""14"" y=""14"" width=""7"" height=""7""/><rect x=""3"" y=""14"" width=""7"" height=""7""/></svg>
                            <span>Dashboard</span>
                        </SidebarMenuButton>
                    </SidebarMenuItem>
                </SidebarMenu>
            </SidebarGroupContent>
        </SidebarGroup>

        <SidebarGroup Class=""mt-auto"">
            <SidebarGroupContent>
                <SidebarMenu>
                    <SidebarMenuItem>
                        <SidebarMenuButton Href=""/settings"" Tooltip=""Settings"" Size=""sm"">
                            <svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" class=""size-4""><circle cx=""12"" cy=""12"" r=""3""/><path d=""M19.4 15a1.65 1.65 0 00.33 1.82l.06.06a2 2 0 010 2.83 2 2 0 01-2.83 0l-.06-.06a1.65 1.65 0 00-1.82-.33 1.65 1.65 0 00-1 1.51V21a2 2 0 01-2 2 2 2 0 01-2-2v-.09A1.65 1.65 0 009 19.4a1.65 1.65 0 00-1.82.33l-.06.06a2 2 0 01-2.83 0 2 2 0 010-2.83l.06-.06a1.65 1.65 0 00.33-1.82 1.65 1.65 0 00-1.51-1H3a2 2 0 01-2-2 2 2 0 012-2h.09A1.65 1.65 0 004.6 9a1.65 1.65 0 00-.33-1.82l-.06-.06a2 2 0 010-2.83 2 2 0 012.83 0l.06.06a1.65 1.65 0 001.82.33H9a1.65 1.65 0 001-1.51V3a2 2 0 012-2 2 2 0 012 2v.09a1.65 1.65 0 001 1.51 1.65 1.65 0 001.82-.33l.06-.06a2 2 0 012.83 0 2 2 0 010 2.83l-.06.06a1.65 1.65 0 00-.33 1.82V9a1.65 1.65 0 001.51 1H21a2 2 0 012 2 2 2 0 01-2 2h-.09a1.65 1.65 0 00-1.51 1z""/></svg>
                            <span>Settings</span>
                        </SidebarMenuButton>
                    </SidebarMenuItem>
                </SidebarMenu>
            </SidebarGroupContent>
        </SidebarGroup>
    </SidebarContent>

    <SidebarFooter>
        <SidebarMenu>
            <SidebarMenuItem>
                <SidebarMenuButton Tooltip=""Account"" Size=""lg"" Class=""group-data-[collapsible=icon]:!size-8"">
                    <Avatar Fallback=""U"" Class=""h-8 w-8 shrink-0 rounded-full group-data-[collapsible=icon]:!size-8"" />
                    <div class=""grid min-w-0 flex-1 text-left text-sm leading-tight group-data-[collapsible=icon]:hidden"">
                        <span class=""truncate font-semibold"">User</span>
                        <span class=""truncate text-xs text-sidebar-foreground/70"">user@example.com</span>
                    </div>
                </SidebarMenuButton>
            </SidebarMenuItem>
        </SidebarMenu>
    </SidebarFooter>
</Sidebar>

@code {
    [Parameter] public SidebarCollapsible Collapsible { get; set; } = SidebarCollapsible.Offcanvas;
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool IsCurrentPath(string path)
    {
        var uri = new Uri(Navigation.Uri);
        var currentPath = uri.AbsolutePath.TrimEnd('/');
        var targetPath = path.TrimEnd('/');
        return currentPath.Equals(targetPath, StringComparison.OrdinalIgnoreCase);
    }
}
";
}
