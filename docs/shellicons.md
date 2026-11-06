# Using ShellIcons with ShellUI

ShellUI draws its own chrome icons (chevrons, close buttons, checkmarks) as inline SVG, so it needs no icon package. For icons in your own content, any library that renders SVG works. [ShellIcons](https://shellicons.shellui.dev) is the one made for ShellUI: the Lucide icons as Blazor components, rendered as inline `<svg>` with no JavaScript or CSS.

## 1. Install the package

```bash
dotnet add package ShellIcons.Blazor --prerelease
```

## 2. Import `ShellIcons`, not `ShellIcons.Icons`

Add one line to `_Imports.razor`:

```razor
@using ShellIcons
```

That gives you three ways to render an icon:

| Form | Example | Use it for |
|---|---|---|
| `{Name}Icon` component | `<ChevronRightIcon />` | Icons in markup |
| `Icon.{Name}()` | `Icon.ChevronRight()` | Icons passed as values, such as a ShellUI `Icon` parameter |
| `ShellIcon` | `<ShellIcon Name="chevron-right" />` | Names only known at run time, such as from a database |

Don't add `@using ShellIcons.Icons`. It brings in every icon under its plain name, and ShellUI's `Badge`, `Calendar`, `Table`, `Command`, `Sheet` and `Timeline` are also Lucide icons, so the build fails with RZ9985 (more than one component matches the tag). A namespace alias such as `@using Icons = ShellIcons.Icons` doesn't help either: Razor ignores it, and `<Icons.Badge />` renders nothing.

## 3. Pass icons to ShellUI

These components take an icon as a parameter. Pass it with `Icon.{Name}()`:

```razor
<StatCard Title="Revenue" Value="$45,231" Icon="@Icon.DollarSign()" />
<TreeViewItem Text="src" Icon="@Icon.Folder()" />
<TimelineItem Title="Deployed" Icon="@Icon.Rocket()" />
```

`StatCard` and `TreeViewItem` size their icon to 16px and `TimelineItem` to 14px, so you don't need to set a size. The others show the icon at the size you give it, and ShellIcons' default is 24px:

| Component | Parameter | Suggested size |
|---|---|---|
| `Alert` | `Icon` | `Icon.TriangleAlert(size: "16")` |
| `Callout` | `Icon` | `Icon.Info(size: "16")` |
| `ContextMenuOption` | `IconContent` | `Icon.Copy(size: "14")` |

```razor
<Alert Variant="AlertVariant.Destructive" Icon="@Icon.TriangleAlert(size: "16")">
    Your session has expired.
</Alert>

<ContextMenuOption IconContent="@Icon.Copy(size: "14")">Copy</ContextMenuOption>
```

## 4. Put icons in buttons and text

Anywhere else, use the `{Name}Icon` component. `Button` doesn't size or space its content, so give the icon a size and a margin:

```razor
<Button>
    <PlusIcon Size="16" Class="mr-2" />
    New project
</Button>

<Button Variant="ButtonVariant.Ghost" Size="ButtonSize.Icon" aria-label="Settings">
    <SettingsIcon Size="16" />
</Button>
```

Icons take the text colour around them, so ShellUI's colour classes work on them directly:

```razor
<CircleCheckIcon Size="16" Class="text-muted-foreground" />
```

An icon without `Title` is hidden from screen readers. Give it a `Title` when it's the only thing that says what it means, or put an `aria-label` on the button around it, as above.

For a click handler, wrap the icon in a `<button>` rather than putting `@onclick` on the icon. On a component, Razor passes `@onclick` through as a plain string, and ShellIcons throws to say so.
