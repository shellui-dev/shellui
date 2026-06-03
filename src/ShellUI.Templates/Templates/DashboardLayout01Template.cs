using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DashboardLayout01Template
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dashboard-01",
        DisplayName = "Dashboard 01",
        Description = "Sidebar + content layout. Header scrolls with content (shadcn sidebar-01 style)",
        Category = ComponentCategory.Layout,
        FilePath = "DashboardLayout01.razor",
        IsLayoutBlock = true,
        Dependencies = new List<string> { "sidebar", "breadcrumb", "separator", "theme-toggle", "app-sidebar" },
        Tags = new List<string> { "dashboard", "sidebar", "layout", "block" }
    };

    public static string Content => @"@inherits LayoutComponentBase
@implements IDisposable
@using YourProjectNamespace.Components.UI
@using Microsoft.AspNetCore.Components.Routing
@inject NavigationManager Navigation

@* Dashboard layout — sidebar + content. Header scrolls with page. *@
<SidebarProvider>
    <AppSidebar />
    <SidebarInset>
        <header class=""flex h-16 shrink-0 items-center gap-2 border-b border-border"">
            <div class=""flex min-w-0 flex-1 items-center gap-2 px-4"">
                <SidebarTrigger Class=""-ml-1 shrink-0"" />
                <Separator Orientation=""vertical"" Class=""mr-2 h-4 shrink-0"" />
                @if (HeaderContent != null)
                {
                    @HeaderContent
                }
                else
                {
                    <div class=""flex min-w-0 flex-1 items-center overflow-x-auto"">
                        <Breadcrumb Class=""flex flex-1 min-w-0 items-center"">
                            @foreach (var item in _breadcrumbItems)
                            {
                                <BreadcrumbItem Href=""@(item.IsLast ? null : item.Href)"" IsLast=""@item.IsLast"">@item.Label</BreadcrumbItem>
                            }
                        </Breadcrumb>
                    </div>
                }
            </div>
            <div class=""ml-auto px-4"">
                <ThemeToggle Size=""sm"" Variant=""outline"" />
            </div>
        </header>

        <div class=""flex flex-1 flex-col gap-4 p-4 pt-0"">
            @Body
        </div>
    </SidebarInset>
</SidebarProvider>

@code {
    [Parameter] public RenderFragment? HeaderContent { get; set; }

    private List<BreadcrumbItemData> _breadcrumbItems = new();

    protected override void OnInitialized()
    {
        BuildBreadcrumb();
        Navigation.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        BuildBreadcrumb();
        InvokeAsync(StateHasChanged);
    }

    public void Dispose() => Navigation.LocationChanged -= OnLocationChanged;

    private void BuildBreadcrumb()
    {
        var uri = new Uri(Navigation.Uri);
        var path = uri.AbsolutePath.TrimEnd('/');
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

        _breadcrumbItems.Clear();
        if (segments.Length == 0 || (segments.Length == 1 && segments[0] == ""))
        {
            _breadcrumbItems.Add(new BreadcrumbItemData { Label = ""Home"", Href = ""/"", IsLast = true });
            return;
        }

        var href = """";
        for (var i = 0; i < segments.Length; i++)
        {
            href += ""/"" + segments[i];
            var label = TitleCase(segments[i]);
            _breadcrumbItems.Add(new BreadcrumbItemData { Label = label, Href = href, IsLast = i == segments.Length - 1 });
        }
    }

    private static string TitleCase(string s) => string.IsNullOrEmpty(s) ? s : char.ToUpperInvariant(s[0]) + s[1..].ToLowerInvariant();

    private class BreadcrumbItemData { public string Label { get; set; } = """"; public string Href { get; set; } = """"; public bool IsLast { get; set; } }
}
";
}
