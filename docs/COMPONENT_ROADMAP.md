# ShellUI Component Roadmap

**Goal:** Build the components and tooling needed for a Tailwind-first Blazor design system, including the foundations for ShellDocs.

.NET 10 · Tailwind CSS `4.3.2`

## Current inventory

`ComponentRegistry` is the source of truth for component availability, names, and dependencies. The current registry contains **176 entries**:

- **76 direct CLI targets** shown by `shellui list`
- **100 hidden entries** for sub-components, variants, models, services, and support assets
- **Packable projects:** `ShellUI.CLI` and `ShellUI.Components`

Hidden entries are not counted as direct targets. They can still be installed recursively when a parent target declares them.

### Implemented direct targets (76)

The following categories describe the direct targets in the current registry, not a promise about future scope.

- [x] **Form (21):** Button, Checkbox, Combobox, DataPicker, DatePicker, DateRangePicker, FileUpload, Form, Input, InputOTP, Label, MultiSelect, RadioGroup, Select, Slider, Switch, TagInput, Textarea, TimePicker, Toggle, TypedSelect
- [x] **Layout (12):** Accordion, Breadcrumb, Card, Collapsible, DashboardLayout01, DashboardLayout02, LinkCard, Navbar, Resizable, ScrollArea, Separator, Sidebar
- [x] **Feedback (9):** Alert, Callout, EmptyState, Loading, Progress, Skeleton, Sonner, Toast, Tooltip
- [x] **Overlay (9):** AlertDialog, Command, CommandPalette, Dialog, Drawer, Dropdown, HoverCard, Popover, Sheet
- [x] **Navigation (7):** ContextMenu, Menubar, NavigationMenu, Pagination, PrevNextNav, Stepper, Tabs
- [x] **Data Display (16):** AreaChart, Avatar, Badge, BarChart, Calendar, Carousel, Chart, ChartSeries, DataTable, DonutChart, LineChart, MultiSeriesChart, PieChart, RadarChart, RadialChart, Table
- [x] **Utility (2):** CopyButton, ThemeToggle

### Implemented ShellDocs building blocks

- [x] **Callout** — information, warning, danger, and tip callouts
- [x] **CopyButton** — clipboard copy interaction
- [x] **LinkCard** — card-style related links
- [x] **PrevNextNav** — previous/next page navigation
- [x] **Breadcrumb** — route and section breadcrumbs
- [x] **Tabs** — value-based tab navigation
- [x] **Steps / Stepper** — step-by-step UI is available as `stepper`; there is no separate `steps` registry target
- [x] **Command and CommandPalette** — command overlay and global hotkey wrapper
- [x] **Sidebar, Navbar, ThemeToggle, and Table** — building blocks for a documentation shell and API reference

The components above are implemented foundations. Dedicated documentation-site wrappers, content rendering, and search infrastructure are tracked separately below.

## Compositional upgrades (done)

The current registry provides explicit sub-components for these parent components. Where a parent declares them, the installer resolves them recursively; other parents may expose parts through a separate composition path.

- [x] **Tabs** — `TabsList`, `TabsTrigger`, `TabsContent`, value-based state
- [x] **Stepper** — `StepperList`, `StepperStep`, `StepperContent`, optional confirmation on the last step
- [x] **Collapsible** — `CollapsibleTrigger`, `CollapsibleContent`
- [x] **Dialog** — `DialogTrigger`, `DialogContent`, `DialogHeader`, `DialogTitle`, `DialogDescription`, `DialogFooter`, `DialogClose`
- [x] **Card** — `CardHeader`, `CardTitle`, `CardDescription`, `CardContent`, `CardFooter`
- [x] **Accordion** — `AccordionItem`, `AccordionTrigger`, `AccordionContent`
- [x] **Dropdown** — `DropdownTrigger`, `DropdownContent`, `DropdownItem`
- [x] **Popover** — `PopoverTrigger`, `PopoverContent`
- [x] **HoverCard** — `HoverCardTrigger`, `HoverCardContent`
- [x] **Carousel** — `CarouselList`, `CarouselSlide`, and navigation parts
- [x] **ContextMenu** — `ContextMenuTrigger`, `ContextMenuContent`, `ContextMenuOption`
- [x] **NavigationMenu** — `NavList`, `NavItem`, `NavTrigger`, `NavContent`
- [x] **Select** — `SelectTrigger`, `SelectContent`, `SelectItem`; the native select remains the default path
- [x] **Drawer and Sheet** — explicit trigger/content and variant support

The intended pattern is explicit child components, cascaded state and callbacks, value/label parameters, and Tailwind utility classes matching the shadcn-style API.

## Unscheduled ideas

These are genuinely unscheduled ideas. They are not current registry targets, and no date or release commitment is implied.

### ShellDocs-specific components and infrastructure

- [ ] **CodeBlock** — syntax highlighting, copy action, line numbers, highlighting, and filename tabs
- [ ] **MDX / MarkdownRenderer** — Markdown or MDX parsing and component embedding
- [ ] **FileTree** — file and folder navigation with icons
- [ ] **Full-text SearchDialog** — a searchable documentation index; `CommandPalette` currently provides the command overlay, not a full-text index
- [ ] **TableOfContents** — heading extraction and scroll-spy state
- [ ] **DocsSidebar wrapper** — documentation-specific navigation assembled from the existing `Sidebar`
- [ ] **DocsHeader wrapper** — documentation header composed from existing navigation, search, and theme pieces
- [ ] **DocsBreadcrumb wrapper** — route-aware breadcrumb generation using the existing `Breadcrumb`
- [ ] **TypeTable** — generated API and props reference table
- [ ] **Tabs (Docs variant)** — a documentation-specific code-tab presentation built on the existing `Tabs`
- [ ] **ShellDocs pipeline** — frontmatter parsing, file-based routing, search-index generation, syntax highlighting, and live component previews

### UI enhancements

- [ ] **TreeView** — hierarchical tree with expansion, selection, and drag-and-drop
- [ ] **Timeline** — vertical event timeline with icons and content
- [ ] **AspectRatio** — constrained aspect-ratio container
- [ ] **ColorPicker** — swatches and HEX/RGB input
- [ ] **Toggle Group** — single- or multi-select toggle groups
- [ ] **Number Input** — increment and decrement number input

### Rich content

- [ ] **RichTextEditor** — WYSIWYG editor
- [ ] **KanbanBoard** — drag-and-drop columns and cards
- [ ] **VirtualScroll** — virtualized list for large datasets
- [ ] **InfiniteScroll** — load more on scroll

### Media and utilities

- [ ] **ImageViewer** — lightbox with zoom and pan
- [ ] **VideoPlayer** — video playback controls
- [ ] **QRCode** — QR code generation
- [ ] **Barcode** — barcode display

## Roadmap maintenance

- Derive direct counts, names, categories, availability, and dependencies from `ComponentRegistry`.
- Move an idea into the implemented section only when it has a registered target and a working CLI installation path.
- Do not infer that a component is standalone from a prose list; use the dependency metadata described in [COMPONENT_DEPENDENCIES.md](COMPONENT_DEPENDENCIES.md).
- Keep future ideas explicitly unscheduled until a release owner assigns a scope and date.
