# ShellUI Component Roadmap

**Goal:** Build the components and tooling needed for a Tailwind-first Blazor design system.

.NET 10 · Tailwind CSS `4.3.2`

## Current inventory

`ComponentRegistry` is the source of truth for component availability, names, and dependencies. The current registry contains **239 entries**:

- **90 direct CLI targets** shown by `shellui list`
- **149 hidden entries** for sub-components, variants, models, services, and support assets
- **Packable projects:** `ShellUI.CLI` and `ShellUI.Components`

Hidden entries are not counted as direct targets. They can still be installed recursively when a parent target declares them.

### Implemented direct targets (90)

The following categories describe the direct targets in the current registry, not a promise about future scope.

- [x] **Form (26):** Button, ButtonGroup, ChatInput, Checkbox, Combobox, DataPicker, DatePicker, DateRangePicker, FileUpload, Form, Input, InputGroup, InputOTP, Label, MultiSelect, NumberInput, RadioGroup, Select, Slider, Switch, TagInput, Textarea, TimePicker, Toggle, ToggleGroup, TypedSelect
- [x] **Layout (13):** Accordion, AspectRatio, Breadcrumb, Card, Collapsible, DashboardLayout01, DashboardLayout02, LinkCard, Navbar, Resizable, ScrollArea, Separator, Sidebar
- [x] **Feedback (9):** Alert, Callout, EmptyState, Loading, Progress, Skeleton, Sonner, Toast, Tooltip
- [x] **Overlay (9):** AlertDialog, Command, CommandPalette, Dialog, Drawer, Dropdown, HoverCard, Popover, Sheet
- [x] **Navigation (8):** ContextMenu, Menubar, NavigationMenu, Pagination, PrevNextNav, Stepper, Tabs, TreeView
- [x] **Data Display (21):** AreaChart, Avatar, Badge, BarChart, Calendar, Carousel, Chart, ChartSeries, Chat, ChatMessage, DataTable, DonutChart, LineChart, MultiSeriesChart, PieChart, QrCode, RadarChart, RadialChart, StatCard, Table, Timeline
- [x] **Typography (1):** Kbd
- [x] **Media (1):** ImageViewer
- [x] **Utility (2):** CopyButton, ThemeToggle

### Added in 0.4

- [x] **Kbd** — keyboard key hint
- [x] **AspectRatio** — fixed-ratio container
- [x] **ButtonGroup** — joined buttons, horizontal or vertical
- [x] **ToggleGroup** — single or multiple selection (`ToggleGroupItem`)
- [x] **InputGroup** — input with prefix and suffix slots
- [x] **NumberInput** — increment/decrement input with min, max and step
- [x] **StatCard** — KPI tile with value, change and trend
- [x] **Timeline** — vertical event timeline (`TimelineItem`)
- [x] **TreeView** — expandable, selectable tree (`TreeViewItem`)
- [x] **QrCode** — QR code rendered as SVG (QRCoder)
- [x] **ImageViewer** — thumbnail with zoomable lightbox
- [x] **Chat, ChatMessage, ChatInput** — AI chat panel, message bubbles with streaming indicator, and a prompt input (Enter sends, Shift+Enter adds a newline)

## ShellDocs (done)

Documentation-site components are implemented in [ShellDocs](https://github.com/shellui-dev/shelldocs), which builds on ShellUI. They are not ShellUI CLI targets.

- [x] **CodeBlock** — `CodeGroup`, `CodeTab`, and syntax highlighting via `CodeBlockEnhancer`
- [x] **MarkdownRenderer** — `MarkdownContent`
- [x] **FileTree** — `FileTree`, `FileTreeItem`
- [x] **Full-text SearchDialog** — `SearchDialog` backed by `SearchIndex`
- [x] **TableOfContents**
- [x] **DocsSidebar, DocsHeader, DocsBreadcrumb** wrappers
- [x] **TypeTable** — `TypeTable` and `AutoTypeTable`
- [x] **Tabs (Docs variant)** — `CodeGroup` / `CodeTab`
- [x] **ShellDocs pipeline** — frontmatter parsing, routing, search index, and live previews (`ComponentPreview`, `PreviewFrame`)

ShellUI building blocks used by ShellDocs: Callout, CopyButton, LinkCard, PrevNextNav, Breadcrumb, Tabs, Stepper, Command/CommandPalette, Sidebar, Navbar, ThemeToggle and Table.

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
- [x] **Drawer and Sheet** — explicit trigger/content and variant support

### Package only (CLI templates pending)

These parts exist in the `ShellUI.Components` NuGet package, but their CLI templates are empty placeholders, so CLI installs only get the parent component.

- [ ] **Select** — `SelectTrigger`, `SelectContent`, `SelectItem` (the native select works in both)
- [ ] **ContextMenu** — `ContextMenuTrigger`, `ContextMenuContent`, `ContextMenuOption`
- [ ] **NavigationMenu** — `NavList`, `NavItem`, `NavTrigger`, `NavContent`
- [ ] **Carousel** — `CarouselList`, `CarouselSlide`

The intended pattern is explicit child components, cascaded state and callbacks, value/label parameters, and Tailwind utility classes matching the shadcn-style API.

## Unscheduled ideas

These are unscheduled ideas. They are not current registry targets, and no date or release commitment is implied.

### AI

- [ ] **PromptSuggestions** — clickable suggestion chips for empty chats
- [ ] **Reasoning / ToolCall** — collapsible blocks for model reasoning and tool results
- [ ] **CodeBlock (ShellUI)** — copyable code block for chat responses

### Forms and input

- [ ] **Field** — label, description and validation message wrapper
- [ ] **ColorPicker** — swatches and HEX/RGB input
- [ ] **Rating** — star rating input
- [ ] **SignaturePad** — draw-to-sign canvas

### Data and layout

- [ ] **VirtualList** — wrapper over Blazor `Virtualize` with ShellUI styling
- [ ] **InfiniteScroll** — load more on scroll
- [ ] **KanbanBoard** — drag-and-drop columns and cards
- [ ] **SortableList** — drag-and-drop reordering
- [ ] **JsonViewer** — collapsible JSON tree with copy

### Media

- [ ] **VideoPlayer** — video playback with ShellUI controls
- [ ] **AudioPlayer** — audio playback with waveform-style progress
- [ ] **ImageGallery** — grid of images sharing one lightbox with next/previous
- [ ] **Barcode** — barcode display

### Rich content

- [ ] **RichTextEditor** — WYSIWYG editor

## Roadmap maintenance

- Derive direct counts, names, categories, availability, and dependencies from `ComponentRegistry`.
- Move an idea into the implemented section only when it has a registered target and a working CLI installation path.
- Do not infer that a component is standalone from a prose list; use the dependency metadata described in [COMPONENT_DEPENDENCIES.md](COMPONENT_DEPENDENCIES.md).
- Keep future ideas explicitly unscheduled until a release owner assigns a scope and date.
