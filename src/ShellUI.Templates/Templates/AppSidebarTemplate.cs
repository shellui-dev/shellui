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
                    @foreach (var link in Links)
                    {
                        <SidebarMenuItem>
                            <SidebarMenuButton Href=""@link.Href"" IsActive=""@IsCurrentPath(link.Href)"" Tooltip=""@link.Title"">
                                @switch (link.Icon)
                                {
                                    case ""home"":
                                        <svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" class=""size-4""><path d=""M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2z""/><path d=""M9 22V12h6v10""/></svg>
                                        break;
                                    case ""counter"":
                                        <svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" class=""size-4""><circle cx=""12"" cy=""12"" r=""10""/><path d=""M12 8v8M8 12h8""/></svg>
                                        break;
                                    case ""weather"":
                                        <svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" class=""size-4""><path d=""M17.5 19H9a7 7 0 1 1 6.71-9h1.79a4.5 4.5 0 1 1 0 9z""/></svg>
                                        break;
                                    default:
                                        <svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" class=""size-4""><path d=""M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z""/><path d=""M14 2v6h6""/></svg>
                                        break;
                                }
                                <span>@link.Title</span>
                            </SidebarMenuButton>
                        </SidebarMenuItem>
                    }
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

    private record NavLink(string Title, string Href, string Icon);

    private static readonly NavLink[] Links =
    [
        new(""Home"", ""/"", ""home""),
    ];

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
