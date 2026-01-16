# ShellUI Component Dependencies

This document lists all components and their dependencies. When a user installs a component, all its dependencies are automatically installed (shadcn-style).

## Components with Dependencies

### Overlay Components
- **alert-dialog** → `["dialog", "button"]`
  - Uses Dialog component
  - Uses Button for actions
  
- **dialog** → `["button"]`
  - Uses Button in footer/actions
  
- **command** → `["dialog"]`
  - Uses Dialog as container

### Form Components
- **form** → `["label", "input", "button"]`
  - Typically uses Label, Input, and Button
  
- **data-table** → `["table", "input", "checkbox", "select", "button", "dropdown"]`
  - Uses Table components
  - Uses Input for filtering
  - Uses Checkbox for selection
  - Uses Select for pagination
  - Uses Button for actions
  - Uses Dropdown for row actions

### Navigation Components
- **stepper** → `["button"]`
  - Uses Button for navigation
  
- **carousel** → `["carousel-item", "carousel-content", "carousel-previous", "carousel-next", "carousel-dots"]`
  - Uses CarouselItem for slides
  - Uses CarouselContent wrapper
  - Uses CarouselPrevious/Next for navigation
  - Uses CarouselDots for indicators

- **accordion** → `["accordion-item"]`
  - Uses AccordionItem for sections
  
- **tabs** → (no dependencies - standalone)
  
- **breadcrumb** → `["breadcrumb-item"]`
  - Uses BreadcrumbItem for links
  
- **radio-group** → `["radio-group-item"]`
  - Uses RadioGroupItem for options
  
- **navigation-menu** → `["navigation-menu-item"]`
  - Uses NavigationMenuItem for items
  
- **menubar** → `["menubar-item"]`
  - Uses MenubarItem for items

### Data Display Components
- **table** → `["table-header", "table-body", "table-row", "table-cell", "table-head"]`
  - Uses TableHeader, TableBody, TableRow, TableCell, TableHead

### Feedback Components
- **empty-state** → `["button"]`
  - Uses Button via Action RenderFragment

### Standalone Components (No Dependencies)
- button
- input
- label
- textarea
- select
- checkbox
- switch
- slider
- toggle
- card
- alert
- badge
- separator
- progress
- skeleton
- theme-toggle
- avatar
- tooltip
- popover
- toast
- loading
- calendar
- hover-card
- context-menu
- file-upload
- combobox
- date-picker
- date-range-picker
- time-picker
- input-otp
- pagination
- scroll-area
- resizable
- sheet
- drawer
- collapsible
- sidebar
- navbar
- dropdown

## Dependency Resolution

When installing a component:
1. All dependencies are automatically installed first
2. Dependencies are installed recursively (if a dependency has dependencies, those are installed too)
3. Already installed components are skipped (unless `--force` is used)
4. User is notified of all dependencies being installed

Example:
```bash
dotnet shellui add alert-dialog
# Installs: dialog, button (dependencies)
# Then installs: alert-dialog
```

