using ShellUI.Core.Models;
using ShellUI.Templates.Templates;

namespace ShellUI.Templates;

public static class ComponentRegistry
{
    public static readonly Dictionary<string, ComponentMetadata> Components = new()
    {
        { "button", ButtonTemplate.Metadata },
        { "theme-toggle", ThemeToggleTemplate.Metadata },
        { "input", InputTemplate.Metadata },
        { "card", CardTemplate.Metadata },
        { "alert", AlertTemplate.Metadata },
        { "badge", BadgeTemplate.Metadata },
        { "label", LabelTemplate.Metadata },
        { "textarea", TextareaTemplate.Metadata },
        { "dialog", DialogTemplate.Metadata },
        { "skeleton", SkeletonTemplate.Metadata },
        { "progress", ProgressTemplate.Metadata },
        { "separator", SeparatorTemplate.Metadata },
        { "select", SelectTemplate.Metadata },
        { "checkbox", CheckboxTemplate.Metadata },
        { "switch", SwitchTemplate.Metadata },
        { "tabs", TabsTemplate.Metadata },
        { "navbar", NavbarTemplate.Metadata },
        { "sidebar", SidebarTemplate.Metadata },
        { "dropdown", DropdownTemplate.Metadata },
        { "radio-group", RadioGroupTemplate.Metadata },
        { "radio-group-item", RadioGroupItemTemplate.Metadata },
        { "slider", SliderTemplate.Metadata },
        { "toggle", ToggleTemplate.Metadata },
        { "accordion", AccordionTemplate.Metadata },
        { "accordion-item", AccordionItemTemplate.Metadata },
        { "breadcrumb", BreadcrumbTemplate.Metadata },
        { "breadcrumb-item", BreadcrumbItemTemplate.Metadata },
        { "toast", ToastTemplate.Metadata },
        { "tooltip", TooltipTemplate.Metadata },
        { "popover", PopoverTemplate.Metadata },
        { "avatar", AvatarTemplate.Metadata },
        { "table", TableTemplate.Metadata },
        { "table-header", TableHeaderTemplate.Metadata },
        { "table-body", TableBodyTemplate.Metadata },
        { "table-row", TableRowTemplate.Metadata },
        { "table-head", TableHeadTemplate.Metadata },
        { "table-cell", TableCellTemplate.Metadata },
        { "form", FormTemplate.Metadata },
        { "input-otp", InputOTPTemplate.Metadata },
        { "combobox", ComboboxTemplate.Metadata },
        { "date-picker", DatePickerTemplate.Metadata },
        { "date-range-picker", DateRangePickerTemplate.Metadata },
        { "time-picker", TimePickerTemplate.Metadata },
        { "navigation-menu", NavigationMenuTemplate.Metadata },
        { "navigation-menu-item", NavigationMenuItemTemplate.Metadata },
        { "menubar", MenubarTemplate.Metadata },
        { "menubar-item", MenubarItemTemplate.Metadata },
        { "pagination", PaginationTemplate.Metadata },
        { "scroll-area", ScrollAreaTemplate.Metadata },
        { "resizable", ResizableTemplate.Metadata },
        { "sheet", SheetTemplate.Metadata },
        { "drawer", DrawerTemplate.Metadata },
        { "collapsible", CollapsibleTemplate.Metadata },
        { "data-table", DataTableTemplate.Metadata },
        { "alert-dialog", AlertDialogTemplate.Metadata },
        { "calendar", CalendarTemplate.Metadata },
        { "loading", LoadingTemplate.Metadata },
        { "hover-card", HoverCardTemplate.Metadata },
        { "command", CommandTemplate.Metadata }
    };

    public static string? GetComponentContent(string componentName)
    {
        return componentName.ToLower() switch
        {
            "button" => ButtonTemplate.Content,
            "theme-toggle" => ThemeToggleTemplate.Content,
            "input" => InputTemplate.Content,
            "card" => CardTemplate.Content,
            "alert" => AlertTemplate.Content,
            "badge" => BadgeTemplate.Content,
            "label" => LabelTemplate.Content,
            "textarea" => TextareaTemplate.Content,
            "dialog" => DialogTemplate.Content,
            "skeleton" => SkeletonTemplate.Content,
            "progress" => ProgressTemplate.Content,
            "separator" => SeparatorTemplate.Content,
            "select" => SelectTemplate.Content,
            "checkbox" => CheckboxTemplate.Content,
            "switch" => SwitchTemplate.Content,
            "tabs" => TabsTemplate.Content,
            "navbar" => NavbarTemplate.Content,
            "sidebar" => SidebarTemplate.Content,
            "dropdown" => DropdownTemplate.Content,
            "radio-group" => RadioGroupTemplate.Content,
            "radio-group-item" => RadioGroupItemTemplate.Content,
            "slider" => SliderTemplate.Content,
            "toggle" => ToggleTemplate.Content,
            "accordion" => AccordionTemplate.Content,
            "accordion-item" => AccordionItemTemplate.Content,
            "breadcrumb" => BreadcrumbTemplate.Content,
            "breadcrumb-item" => BreadcrumbItemTemplate.Content,
            "toast" => ToastTemplate.Content,
            "tooltip" => TooltipTemplate.Content,
            "popover" => PopoverTemplate.Content,
            "avatar" => AvatarTemplate.Content,
            "table" => TableTemplate.Content,
            "table-header" => TableHeaderTemplate.Content,
            "table-body" => TableBodyTemplate.Content,
            "table-row" => TableRowTemplate.Content,
            "table-head" => TableHeadTemplate.Content,
            "table-cell" => TableCellTemplate.Content,
            "form" => FormTemplate.Content,
            "input-otp" => InputOTPTemplate.Content,
            "combobox" => ComboboxTemplate.Content,
            "date-picker" => DatePickerTemplate.Content,
            "date-range-picker" => DateRangePickerTemplate.Content,
            "time-picker" => TimePickerTemplate.Content,
            "navigation-menu" => NavigationMenuTemplate.Content,
            "navigation-menu-item" => NavigationMenuItemTemplate.Content,
            "menubar" => MenubarTemplate.Content,
            "menubar-item" => MenubarItemTemplate.Content,
            "pagination" => PaginationTemplate.Content,
            "scroll-area" => ScrollAreaTemplate.Content,
            "resizable" => ResizableTemplate.Content,
            "sheet" => SheetTemplate.Content,
            "drawer" => DrawerTemplate.Content,
            "collapsible" => CollapsibleTemplate.Content,
            "data-table" => DataTableTemplate.Content,
            "alert-dialog" => AlertDialogTemplate.Content,
            "calendar" => CalendarTemplate.Content,
            "loading" => LoadingTemplate.Content,
            "hover-card" => HoverCardTemplate.Content,
            "command" => CommandTemplate.Content,
            _ => null
        };
    }

    public static IEnumerable<ComponentMetadata> GetByCategory(ComponentCategory category)
    {
        return Components.Values.Where(c => c.Category == category);
    }

    public static IEnumerable<ComponentMetadata> SearchByTag(string tag)
    {
        return Components.Values.Where(c => c.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase));
    }

    public static ComponentMetadata? GetMetadata(string componentName)
    {
        return Components.TryGetValue(componentName.ToLower(), out var metadata) ? metadata : null;
    }

    public static bool Exists(string componentName)
    {
        return Components.ContainsKey(componentName.ToLower());
    }
}

