# ShellUI Component Roadmap

**Goal:** Build ALL components from shadcn/ui + extras needed for ShellDocs (Blazor fumadocs equivalent)

## Current Status: 68 Installable Components - v0.3.0-alpha

### Completed (68 install targets; sub-components, variants, models, services auto-installed)

Counts are top-level components users invoke directly via `shellui add <name>`. Anything that auto-installs as a dependency (e.g. `SidebarTrigger`, `DialogContent`, `ButtonVariants`, `SonnerService`) is not counted here.

**Form (17):** Button, Checkbox, Combobox, DatePicker, DateRangePicker, FileUpload, Form, Input, InputOTP, Label, RadioGroup, Select, Slider, Switch, Textarea, TimePicker, Toggle

**Layout (12):** Accordion, Breadcrumb, Card, Collapsible, DashboardLayout01, DashboardLayout02, LinkCard, Navbar, Resizable, ScrollArea, Separator, Sidebar

**Navigation (7):** ContextMenu, Menubar, NavigationMenu, Pagination, PrevNextNav, Stepper, Tabs

**Overlay (8):** AlertDialog, Command, Dialog, Drawer, Dropdown, HoverCard, Popover, Sheet

**Data Display (13):** AreaChart, Avatar, Badge, BarChart, Calendar, Carousel, Chart, ChartSeries, DataTable, LineChart, MultiSeriesChart, PieChart, Table

**Feedback (9):** Alert, Callout, EmptyState, Loading, Progress, Skeleton, Sonner, Toast, Tooltip

**Utility (2):** CopyButton, ThemeToggle

---

## Compositional Upgrades (shadcn-style)

Components upgraded to shadcn-style compositional API (explicit sub-components, no heavy ChildContent, explicit Value/Title props):

### ✅ Done
- **Tabs** → TabsList, TabsTrigger, TabsContent (Value-based)
- **Stepper** → StepperList, StepperStep, StepperContent (Value-based, Confirm on last)
- **Collapsible** → CollapsibleTrigger, CollapsibleContent
- **Dialog** → DialogTrigger, DialogContent, DialogHeader, DialogTitle, DialogDescription, DialogFooter, DialogClose
- **Card** → CardHeader, CardTitle, CardDescription, CardContent, CardFooter
- **Accordion** → AccordionTrigger, AccordionContent (Value optional; legacy Title + ChildContent still supported)

- **Dropdown** → DropdownTrigger, DropdownContent, DropdownItem (legacy Trigger/ChildContent supported)
- **Popover** → PopoverTrigger, PopoverContent (legacy Trigger/ChildContent supported)
- **HoverCard** → HoverCardTrigger, HoverCardContent (legacy ChildContent/CardContent supported)

- **Carousel** → CarouselList, CarouselSlide (Value-based; UseCarouselList=true)
- **ContextMenu** → ContextMenuTrigger, ContextMenuContent, ContextMenuOption (legacy Items supported)
- **NavigationMenu** → NavList, NavItem, NavTrigger, NavContent (UseNavList=true)
- **Select** → SelectTrigger, SelectContent, SelectItem (UseCustomSelect=true; native select default)

### 🔲 Candidates for upgrade

### Pattern
- Root component cascades context (state, callbacks)
- Sub-components use `Value` for selection, `Title`/`Description` for labels
- No monolithic `ChildContent` with implicit structure
- Styling via Tailwind classes matching shadcn defaults

---

## Remaining Components

### Priority 1: ShellDocs Essentials (Required for documentation site)
These components are needed to build ShellDocs - the Blazor equivalent of fumadocs.

- [ ] **CodeBlock** - Syntax highlighted code with copy button, line numbers, line highlighting, filename tab
- [ ] **MDX / MarkdownRenderer** - Render MDX/Markdown content with component support
- [ ] **Callout** - Info/warning/danger/tip callout boxes (like Docusaurus admonitions)
- [ ] **Steps** - Numbered step-by-step instructions (vertical)
- [ ] **FileTree** - Display file/folder structure with icons
- [ ] **Tabs (Docs variant)** - Code tabs for multi-language examples (npm/yarn/pnpm)
- [ ] **SearchDialog** - Full-text search overlay (Cmd+K style, extends Command)
- [ ] **CopyButton** - One-click copy to clipboard
- [ ] **TableOfContents** - Auto-generated from headings, scroll-spy active state
- [ ] **DocsSidebar** - Collapsible docs navigation with sections/groups
- [ ] **DocsHeader** - Top nav with search, theme toggle, github link
- [ ] **DocsBreadcrumb** - Auto-generated from route hierarchy
- [ ] **PrevNextNav** - Previous/Next page navigation at bottom of docs
- [ ] **TypeTable** - API/Props reference table (component name, type, default, description)
- [ ] **LinkCard** - Card-style links for related pages

### Priority 2: UI Enhancements
- [ ] **TreeView** - Hierarchical tree with expand/collapse, selection, drag-drop
- [ ] **Timeline** - Vertical event timeline with icons and content
- [ ] **AspectRatio** - Constrained aspect ratio container
- [ ] **ColorPicker** - Color selection with swatches, hex/rgb input
- [ ] **Toggle Group** - Group of toggles (single/multi select)
- [ ] **Number Input** - Increment/decrement number input

### Priority 3: Rich Content
- [ ] **RichTextEditor** - WYSIWYG editor (consider Quill or ProseMirror)
- [ ] **KanbanBoard** - Drag-and-drop columns and cards
- [ ] **VirtualScroll** - Virtualized list for large datasets
- [ ] **InfiniteScroll** - Load more on scroll

### Priority 4: Media & Utilities
- [ ] **ImageViewer** - Lightbox with zoom/pan
- [ ] **VideoPlayer** - Video playback controls
- [ ] **QRCode** - QR code generator
- [ ] **Barcode** - Barcode display

---

## ShellDocs Architecture Notes

ShellDocs will need:
1. **MDX Pipeline** - Parse .mdx files, extract frontmatter, render with Blazor components
2. **File-based routing** - Docs pages from folder structure (like fumadocs/Nextra)
3. **Search index** - Build-time index generation for full-text search
4. **Syntax highlighting** - Prism.js or Shiki via JS interop
5. **Component embedding** - Render ShellUI components inline in docs (live preview)

### Key ShellDocs Pages:
- Landing page (hero, features grid, code example)
- Component docs (props table, live preview, code examples)
- Getting started guide (steps, code blocks)
- API reference (type tables, method signatures)
- Changelog (timeline)

---

## Timeline

- v0.1.0 (Dec 2025) - 73 components, CLI + NuGet
- v0.1.1 (Dec 2025) - Hotfix, package publishing
- **v0.3.0-alpha (Feb 2026) - Current alpha: Charts, 68 installable components, Tailwind 4.1.18**
- v0.3.0 (Q1 2026) - ShellDocs components (CodeBlock, MDX, Callout, Steps, etc.)
- v0.4.0 (Q2 2026) - ShellDocs site launch
- v1.0.0 (Q2-Q3 2026) - .NET 10, stable API, comprehensive docs
