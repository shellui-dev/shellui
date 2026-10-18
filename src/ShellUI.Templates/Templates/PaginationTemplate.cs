using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination",
        DisplayName = "Pagination",
        Description = "Pagination component for navigating pages",
        Category = ComponentCategory.Navigation,
        FilePath = "Pagination.razor",
        Dependencies = new List<string> { "pagination-content", "pagination-item", "pagination-link", "pagination-previous", "pagination-next", "pagination-ellipsis" },

        Tags = new List<string> { "navigation", "pagination", "pages" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<nav role=""navigation"" aria-label=""pagination"" class=""@Shell.Cn(""mx-auto flex w-full justify-center"", ClassName, Class)"" @attributes=""AdditionalAttributes"">
    @if (ChildContent != null)
    {
        @ChildContent
    }
    else
    {
        <ul class=""flex flex-row items-center gap-1"">
            <li>
                <button type=""button""
                        @onclick=""PreviousPage""
                        disabled=""@(CurrentPage <= 1)""
                        class=""@Shell.Cn(""inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring h-9 px-2.5 py-2 gap-1 sm:px-4 sm:pl-2.5"", CurrentPage <= 1 ? ""pointer-events-none opacity-50"" : ""hover:bg-accent hover:text-accent-foreground"")"">
                    <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M15 19l-7-7 7-7"" />
                    </svg>
                    <span class=""hidden sm:inline"">Previous</span>
                </button>
            </li>
        
            @for (int i = 1; i <= TotalPages; i++)
            {
                var pageNum = i;
                <li>
                    <button type=""button""
                            @onclick=""@(() => GoToPage(pageNum))""
                            class=""@Shell.Cn(""inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring h-9 w-9"", CurrentPage == pageNum ? ""border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground"" : ""hover:bg-accent hover:text-accent-foreground"")"">
                        @pageNum
                    </button>
                </li>
            }
        
            <li>
                <button type=""button""
                        @onclick=""NextPage""
                        disabled=""@(CurrentPage >= TotalPages)""
                        class=""@Shell.Cn(""inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring h-9 px-2.5 py-2 gap-1 sm:px-4 sm:pr-2.5"", CurrentPage >= TotalPages ? ""pointer-events-none opacity-50"" : ""hover:bg-accent hover:text-accent-foreground"")"">
                    <span class=""hidden sm:inline"">Next</span>
                    <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M9 5l7 7-7 7"" />
                    </svg>
                </button>
            </li>
        </ul>
    }
</nav>

@code {
    [Parameter] public int CurrentPage { get; set; } = 1;
    [Parameter] public EventCallback<int> CurrentPageChanged { get; set; }
    // Deprecated: use CurrentPageChanged or @bind-CurrentPage.
    [Parameter] public EventCallback<int> OnPageChange { get; set; }
    [Parameter] public int TotalPages { get; set; } = 1;
    // Set to build the pagination from PaginationContent, PaginationItem and PaginationLink.
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    // Deprecated: use Class.
    [Parameter] public string ClassName { get; set; } = """";
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private async Task GoToPage(int page)
    {
        if (page < 1 || page > TotalPages || page == CurrentPage) return;
        CurrentPage = page;
        await CurrentPageChanged.InvokeAsync(page);
        await OnPageChange.InvokeAsync(page);
    }

    private Task PreviousPage() => GoToPage(CurrentPage - 1);
    private Task NextPage() => GoToPage(CurrentPage + 1);
}
";
}


