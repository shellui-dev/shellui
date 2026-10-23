using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

/// Each package component's @code block must match its CLI template, ignoring comments, whitespace and namespace.
public class TemplateSyncTests
{
    // Component name → reason. Empty by default — fix the drift instead of adding entries.
    private static readonly Dictionary<string, string> AllowedDrift = new()
    {
        // The package imports the shellui.js module; the template calls window.ShellUI.
        ["command-palette"] = "JS-interop path differs between package (module import) and CLI template (window.ShellUI global).",
        ["input-otp"] = "JS-interop path differs between package (module import + fallback) and CLI template (window.ShellUI global).",
        ["copy-button"] = "JS-interop path differs between package (module import) and CLI template (window.ShellUI global).",
        ["sidebar-provider"] = "JS-interop path differs: the package imports shellui-sidebar.js; the template calls window.ShellUI.initSidebar, since a dynamic import 404s once compiled into a consumer's class library.",
        ["app-sidebar"] = "The template is the dashboard block that init rewrites with the app's routes; the package copy is a sample."
    };

    [Theory]
    [InlineData("sidebar-trigger", "SidebarTrigger.razor")]
    [InlineData("theme-toggle", "ThemeToggle.razor")]
    [InlineData("input-otp", "InputOTP.razor")]
    [InlineData("data-picker", "DataPicker.razor")]
    [InlineData("multi-select", "MultiSelect.razor")]
    [InlineData("tag-input", "TagInput.razor")]
    [InlineData("command-palette", "CommandPalette.razor")]
    [InlineData("donut-chart", "DonutChart.razor")]
    [InlineData("radar-chart", "RadarChart.razor")]
    [InlineData("radial-chart", "RadialChart.razor")]
    [InlineData("kbd", "Kbd.razor")]
    [InlineData("aspect-ratio", "AspectRatio.razor")]
    [InlineData("button-group", "ButtonGroup.razor")]
    [InlineData("toggle-group", "ToggleGroup.razor")]
    [InlineData("toggle-group-item", "ToggleGroupItem.razor")]
    [InlineData("input-group", "InputGroup.razor")]
    [InlineData("number-input", "NumberInput.razor")]
    [InlineData("stat-card", "StatCard.razor")]
    [InlineData("timeline", "Timeline.razor")]
    [InlineData("timeline-item", "TimelineItem.razor")]
    [InlineData("tree-view", "TreeView.razor")]
    [InlineData("tree-view-item", "TreeViewItem.razor")]
    [InlineData("qr-code", "QrCode.razor")]
    [InlineData("image-viewer", "ImageViewer.razor")]
    [InlineData("chat-message", "ChatMessage.razor")]
    [InlineData("chat-input", "ChatInput.razor")]
    [InlineData("chat", "Chat.razor")]
    [InlineData("command", "Command.razor")]
    [InlineData("select", "Select.razor")]
    [InlineData("select-trigger", "SelectTrigger.razor")]
    [InlineData("select-content", "SelectContent.razor")]
    [InlineData("select-item", "SelectItem.razor")]
    [InlineData("context-menu", "ContextMenu.razor")]
    [InlineData("context-menu-trigger", "ContextMenuTrigger.razor")]
    [InlineData("context-menu-content", "ContextMenuContent.razor")]
    [InlineData("context-menu-option", "ContextMenuOption.razor")]
    [InlineData("navigation-menu", "NavigationMenu.razor")]
    [InlineData("nav-list", "NavList.razor")]
    [InlineData("nav-item", "NavItem.razor")]
    [InlineData("nav-trigger", "NavTrigger.razor")]
    [InlineData("nav-content", "NavContent.razor")]
    [InlineData("carousel", "Carousel.razor")]
    [InlineData("carousel-list", "CarouselList.razor")]
    [InlineData("carousel-slide", "CarouselSlide.razor")]
    [InlineData("dropdown", "Dropdown.razor")]
    [InlineData("dropdown-trigger", "DropdownTrigger.razor")]
    [InlineData("dropdown-content", "DropdownContent.razor")]
    [InlineData("dropdown-item", "DropdownItem.razor")]
    [InlineData("popover", "Popover.razor")]
    [InlineData("popover-trigger", "PopoverTrigger.razor")]
    [InlineData("popover-content", "PopoverContent.razor")]
    [InlineData("hover-card", "HoverCard.razor")]
    [InlineData("hover-card-trigger", "HoverCardTrigger.razor")]
    [InlineData("hover-card-content", "HoverCardContent.razor")]
    [InlineData("tabs", "Tabs.razor")]
    [InlineData("accordion", "Accordion.razor")]
    [InlineData("accordion-item", "AccordionItem.razor")]
    [InlineData("accordion-trigger", "AccordionTrigger.razor")]
    [InlineData("accordion-content", "AccordionContent.razor")]
    [InlineData("breadcrumb", "Breadcrumb.razor")]
    [InlineData("breadcrumb-item", "BreadcrumbItem.razor")]
    [InlineData("breadcrumb-list", "BreadcrumbList.razor")]
    [InlineData("breadcrumb-link", "BreadcrumbLink.razor")]
    [InlineData("breadcrumb-page", "BreadcrumbPage.razor")]
    [InlineData("breadcrumb-separator", "BreadcrumbSeparator.razor")]
    [InlineData("breadcrumb-ellipsis", "BreadcrumbEllipsis.razor")]
    [InlineData("pagination-content", "PaginationContent.razor")]
    [InlineData("pagination-item", "PaginationItem.razor")]
    [InlineData("pagination-link", "PaginationLink.razor")]
    [InlineData("pagination-previous", "PaginationPrevious.razor")]
    [InlineData("pagination-next", "PaginationNext.razor")]
    [InlineData("pagination-ellipsis", "PaginationEllipsis.razor")]
    [InlineData("sheet-header", "SheetHeader.razor")]
    [InlineData("sheet-title", "SheetTitle.razor")]
    [InlineData("sheet-description", "SheetDescription.razor")]
    [InlineData("sheet-footer", "SheetFooter.razor")]
    [InlineData("sheet-close", "SheetClose.razor")]
    [InlineData("drawer-header", "DrawerHeader.razor")]
    [InlineData("drawer-title", "DrawerTitle.razor")]
    [InlineData("drawer-description", "DrawerDescription.razor")]
    [InlineData("drawer-footer", "DrawerFooter.razor")]
    [InlineData("drawer-close", "DrawerClose.razor")]
    [InlineData("alert-dialog", "AlertDialog.razor")]
    [InlineData("alert-dialog-trigger", "AlertDialogTrigger.razor")]
    [InlineData("alert-dialog-content", "AlertDialogContent.razor")]
    [InlineData("alert-dialog-header", "AlertDialogHeader.razor")]
    [InlineData("alert-dialog-title", "AlertDialogTitle.razor")]
    [InlineData("alert-dialog-description", "AlertDialogDescription.razor")]
    [InlineData("alert-dialog-footer", "AlertDialogFooter.razor")]
    [InlineData("alert-dialog-action", "AlertDialogAction.razor")]
    [InlineData("alert-dialog-cancel", "AlertDialogCancel.razor")]
    [InlineData("form", "Form.razor")]
    [InlineData("form-field", "FormField.razor")]
    [InlineData("form-item", "FormItem.razor")]
    [InlineData("form-label", "FormLabel.razor")]
    [InlineData("form-control", "FormControl.razor")]
    [InlineData("form-description", "FormDescription.razor")]
    [InlineData("form-message", "FormMessage.razor")]
    [InlineData("menubar-item", "MenubarItem.razor")]
    [InlineData("menubar-menu", "MenubarMenu.razor")]
    [InlineData("menubar-trigger", "MenubarTrigger.razor")]
    [InlineData("menubar-content", "MenubarContent.razor")]
    [InlineData("menubar-separator", "MenubarSeparator.razor")]
    [InlineData("command-input", "CommandInput.razor")]
    [InlineData("command-list", "CommandList.razor")]
    [InlineData("command-group", "CommandGroup.razor")]
    [InlineData("command-option", "CommandOption.razor")]
    [InlineData("command-empty", "CommandEmpty.razor")]
    [InlineData("command-separator", "CommandSeparator.razor")]
    [InlineData("label", "Label.razor")]
    [InlineData("file-upload", "FileUpload.razor")]
    [InlineData("carousel-item", "CarouselItem.razor")]
    [InlineData("menubar", "Menubar.razor")]
    [InlineData("sheet-content", "SheetContent.razor")]
    [InlineData("drawer-content", "DrawerContent.razor")]
    [InlineData("sidebar-menu-button", "SidebarMenuButton.razor")]
    [InlineData("table", "Table.razor")]
    [InlineData("table-body", "TableBody.razor")]
    [InlineData("table-cell", "TableCell.razor")]
    [InlineData("table-head", "TableHead.razor")]
    [InlineData("table-header", "TableHeader.razor")]
    [InlineData("table-row", "TableRow.razor")]
    [InlineData("separator", "Separator.razor")]
    [InlineData("slider", "Slider.razor")]
    [InlineData("navbar", "Navbar.razor")]
    [InlineData("checkbox", "Checkbox.razor")]
    [InlineData("switch", "Switch.razor")]
    [InlineData("tooltip", "Tooltip.razor")]
    [InlineData("date-range-picker", "DateRangePicker.razor")]
    [InlineData("radio-group", "RadioGroup.razor")]
    [InlineData("radio-group-item", "RadioGroupItem.razor")]
    [InlineData("empty-state", "EmptyState.razor")]
    [InlineData("pagination", "Pagination.razor")]
    [InlineData("skeleton", "Skeleton.razor")]
    [InlineData("scroll-area", "ScrollArea.razor")]
    [InlineData("toast", "Toast.razor")]
    [InlineData("time-picker", "TimePicker.razor")]
    [InlineData("date-picker", "DatePicker.razor")]
    [InlineData("navigation-menu-item", "NavigationMenuItem.razor")]
    [InlineData("stepper", "Stepper.razor")]
    [InlineData("stepper-step", "StepperStep.razor")]
    [InlineData("stepper-content", "StepperContent.razor")]
    [InlineData("input", "Input.razor")]
    [InlineData("textarea", "Textarea.razor")]
    [InlineData("sheet", "Sheet.razor")]
    [InlineData("drawer", "Drawer.razor")]
    [InlineData("prompt-suggestions", "PromptSuggestions.razor")]
    [InlineData("reasoning", "Reasoning.razor")]
    [InlineData("tool-call", "ToolCall.razor")]
    [InlineData("field", "Field.razor")]
    [InlineData("json-viewer", "JsonViewer.razor")]
    [InlineData("json-viewer-node", "JsonViewerNode.razor")]
    [InlineData("copy-button", "CopyButton.razor")]
    [InlineData("sidebar-provider", "SidebarProvider.razor")]
    [InlineData("app-sidebar", "AppSidebar.razor")]
    [InlineData("resizable", "ResizablePanelGroup.razor")]
    [InlineData("resizable-panel", "ResizablePanel.razor")]
    [InlineData("resizable-handle", "ResizableHandle.razor")]
    public void TemplateCodeBlock_MatchesLiveLibrary(string templateName, string razorFileName)
    {
        if (AllowedDrift.ContainsKey(templateName)) return;

        var liveContent = WithoutClassNameAlias(File.ReadAllText(GetLiveRazorPath(razorFileName)));
        var templateContent = WithoutClassNameAlias(ComponentRegistry.GetComponentContent(templateName)
            ?? throw new InvalidOperationException($"Template '{templateName}' not found in registry"));

        var liveCode = ExtractCodeBlock(liveContent)
            ?? throw new InvalidOperationException($"Live {razorFileName} has no @code block");
        var templateCode = ExtractCodeBlock(templateContent)
            ?? throw new InvalidOperationException($"Template {templateName} has no @code block");

        var normalizedLive = Normalize(liveCode);
        var normalizedTemplate = Normalize(templateCode);

        Assert.True(normalizedLive == normalizedTemplate,
            $"Drift detected between live {razorFileName} and template {templateName}.\n" +
            $"This usually means someone updated one but not the other. Sync them, or add " +
            $"\"{templateName}\" to AllowedDrift in TemplateSyncTests with a reason.\n\n" +
            DiffSummary(normalizedLive, normalizedTemplate));
    }

    [Theory]
    [InlineData("label", "Label.razor")]
    [InlineData("file-upload", "FileUpload.razor")]
    [InlineData("carousel-item", "CarouselItem.razor")]
    [InlineData("menubar", "Menubar.razor")]
    [InlineData("sheet-content", "SheetContent.razor")]
    [InlineData("drawer-content", "DrawerContent.razor")]
    [InlineData("sidebar-menu-button", "SidebarMenuButton.razor")]
    [InlineData("theme-toggle", "ThemeToggle.razor")]
    [InlineData("table", "Table.razor")]
    [InlineData("table-body", "TableBody.razor")]
    [InlineData("table-cell", "TableCell.razor")]
    [InlineData("table-head", "TableHead.razor")]
    [InlineData("table-header", "TableHeader.razor")]
    [InlineData("table-row", "TableRow.razor")]
    [InlineData("separator", "Separator.razor")]
    [InlineData("slider", "Slider.razor")]
    [InlineData("navbar", "Navbar.razor")]
    [InlineData("checkbox", "Checkbox.razor")]
    [InlineData("switch", "Switch.razor")]
    [InlineData("tooltip", "Tooltip.razor")]
    [InlineData("date-range-picker", "DateRangePicker.razor")]
    [InlineData("radio-group", "RadioGroup.razor")]
    [InlineData("radio-group-item", "RadioGroupItem.razor")]
    [InlineData("empty-state", "EmptyState.razor")]
    [InlineData("pagination", "Pagination.razor")]
    [InlineData("skeleton", "Skeleton.razor")]
    [InlineData("scroll-area", "ScrollArea.razor")]
    [InlineData("toast", "Toast.razor")]
    [InlineData("time-picker", "TimePicker.razor")]
    [InlineData("date-picker", "DatePicker.razor")]
    [InlineData("navigation-menu-item", "NavigationMenuItem.razor")]
    [InlineData("stepper", "Stepper.razor")]
    [InlineData("stepper-step", "StepperStep.razor")]
    [InlineData("stepper-content", "StepperContent.razor")]
    [InlineData("input", "Input.razor")]
    [InlineData("textarea", "Textarea.razor")]
    [InlineData("sheet", "Sheet.razor")]
    [InlineData("drawer", "Drawer.razor")]
    [InlineData("prompt-suggestions", "PromptSuggestions.razor")]
    [InlineData("reasoning", "Reasoning.razor")]
    [InlineData("tool-call", "ToolCall.razor")]
    [InlineData("field", "Field.razor")]
    [InlineData("json-viewer", "JsonViewer.razor")]
    [InlineData("json-viewer-node", "JsonViewerNode.razor")]
    [InlineData("resizable", "ResizablePanelGroup.razor")]
    [InlineData("resizable-panel", "ResizablePanel.razor")]
    [InlineData("resizable-handle", "ResizableHandle.razor")]
    public void TemplateMarkup_MatchesLiveLibrary(string templateName, string razorFileName)
    {
        var liveContent = WithoutClassNameAlias(File.ReadAllText(GetLiveRazorPath(razorFileName)));
        var templateContent = WithoutClassNameAlias(ComponentRegistry.GetComponentContent(templateName)
            ?? throw new InvalidOperationException($"Template '{templateName}' not found in registry"));

        var normalizedLive = Normalize(ExtractMarkup(liveContent));
        var normalizedTemplate = Normalize(ExtractMarkup(templateContent));

        Assert.True(normalizedLive == normalizedTemplate,
            $"Markup drift between live {razorFileName} and template {templateName}.\n\n" +
            DiffSummary(normalizedLive, normalizedTemplate));
    }

    [Fact]
    public void ChartVariantsTemplate_MatchesLiveLibrary()
    {
        var livePath = Path.Combine(Path.GetDirectoryName(GetLiveRazorPath("Chart.razor"))!, "..", "Variants", "ChartVariants.cs");
        var template = ComponentRegistry.GetComponentContent("chart-variants")
            ?? throw new InvalidOperationException("Template 'chart-variants' not found in registry");

        var live = Normalize(File.ReadAllText(livePath));
        var generated = Normalize(template.Replace("namespace YourProjectNamespace.Components.UI.Variants;", "namespace ShellUI.Components;"));

        Assert.True(live == generated, "Drift detected between live ChartVariants.cs and template chart-variants.\n\n" + DiffSummary(live, generated));
    }

    private static string GetLiveRazorPath(string razorFileName, [CallerFilePath] string thisFile = "")
    {
        var testDir = Path.GetDirectoryName(thisFile) ?? throw new InvalidOperationException("CallerFilePath is empty");
        var repoRoot = Path.GetFullPath(Path.Combine(testDir, ".."));
        return Path.Combine(repoRoot, "src", "ShellUI.Components", "Components", razorFileName);
    }

    // CLI templates keep the deprecated ClassName alias for existing projects; the package may not have it.
    private static string WithoutClassNameAlias(string content)
    {
        var withoutParameter = Regex.Replace(content, @"^[ \t]*\[Parameter\]\s*public string ClassName \{ get; set; \} = ""\s*"";[ \t]*\r?\n", string.Empty, RegexOptions.Multiline);
        return Regex.Replace(withoutParameter, @"\bClassName,\s*", string.Empty);
    }

    private static string Normalize(string code)
    {
        var withoutBlock = Regex.Replace(code, @"/\*.*?\*/", string.Empty, RegexOptions.Singleline);
        var lines = withoutBlock.Split('\n')
            .Select(l => Regex.Replace(l, @"//.*$", string.Empty))
            .Select(l => Regex.Replace(l.Trim(), @"\s+", " "))
            .Where(l => !string.IsNullOrWhiteSpace(l));
        return string.Join("\n", lines);
    }

    private static string DiffSummary(string live, string template)
    {
        var liveLines = live.Split('\n');
        var tmplLines = template.Split('\n');
        var max = Math.Max(liveLines.Length, tmplLines.Length);
        var diffs = new System.Text.StringBuilder();
        var shown = 0;
        for (var i = 0; i < max && shown < 5; i++)
        {
            var l = i < liveLines.Length ? liveLines[i] : "<missing>";
            var t = i < tmplLines.Length ? tmplLines[i] : "<missing>";
            if (l != t)
            {
                diffs.AppendLine($"  line {i + 1}");
                diffs.AppendLine($"    live:     {l}");
                diffs.AppendLine($"    template: {t}");
                shown++;
            }
        }
        return diffs.Length == 0 ? "(no per-line diff — file lengths differ)" : diffs.ToString();
    }

    // Everything before `@code`, without the @namespace and @using lines that differ by design.
    private static string ExtractMarkup(string razor)
    {
        var match = Regex.Match(razor, @"@code\s*\{");
        var head = match.Success ? razor.Substring(0, match.Index) : razor;
        var lines = head.Split('\n').Where(l => !Regex.IsMatch(l.Trim(), @"^@(namespace|using)\b"));
        return string.Join("\n", lines);
    }

    /// Body of the first `@code { }` block, or null if missing or unbalanced.
    private static string? ExtractCodeBlock(string razor)
    {
        var match = Regex.Match(razor, @"@code\s*\{");
        if (!match.Success) return null;

        var start = match.Index + match.Length;
        var depth = 1;
        var inString = false;
        var inVerbatimString = false;
        var inCharLiteral = false;
        var inLineComment = false;
        var inBlockComment = false;

        for (var i = start; i < razor.Length; i++)
        {
            var c = razor[i];
            var next = i + 1 < razor.Length ? razor[i + 1] : '\0';

            if (inLineComment)
            {
                if (c == '\n') inLineComment = false;
                continue;
            }
            if (inBlockComment)
            {
                if (c == '*' && next == '/') { inBlockComment = false; i++; }
                continue;
            }
            if (inVerbatimString)
            {
                if (c == '"' && next == '"') { i++; continue; }
                if (c == '"') inVerbatimString = false;
                continue;
            }
            if (inString)
            {
                if (c == '\\' && next != '\0') { i++; continue; }
                if (c == '"') inString = false;
                continue;
            }
            if (inCharLiteral)
            {
                if (c == '\\' && next != '\0') { i++; continue; }
                if (c == '\'') inCharLiteral = false;
                continue;
            }

            if (c == '/' && next == '/') { inLineComment = true; i++; continue; }
            if (c == '/' && next == '*') { inBlockComment = true; i++; continue; }
            if (c == '@' && next == '"') { inVerbatimString = true; i++; continue; }
            if (c == '"') { inString = true; continue; }
            if (c == '\'') { inCharLiteral = true; continue; }

            if (c == '{') depth++;
            else if (c == '}')
            {
                depth--;
                if (depth == 0) return razor.Substring(start, i - start);
            }
        }
        return null;
    }
}
