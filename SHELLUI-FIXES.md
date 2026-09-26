# ShellUI fixes

Problems hit while adding ShellUI 0.3.0-rc.1 components to `src/FDMS.UI` and how each was handled. The fixes now live in the current `0.4.0-alpha.1` source templates; they are not all present in the published `0.3.0-rc.1` package. The legacy `sidebar-js` module remains only for projects that still contain an old generated provider.

## Edits to generated components

1. `Tabs.razor` **does not compile.** The template emits `private string _effectiveValue = ";` (an unterminated string). Fixed to `= "";`.
2. `Select.razor` **depends on an icon font nothing loads.** It draws the chevron with `<span class="material-symbols-outlined">expand_more</span>`, which needs the Material Symbols font, so without it the arrow would render as the plain text "expand_more". Replaced with an inline SVG chevron.
3. `SidebarProvider.razor` **never detects mobile.** The old template imported `./shellui-sidebar.js`, which resolves against the page URL and fails when the component is compiled into a Razor class library. New CLI installs use the host-loaded `shellui.js` and call `ShellUI.initSidebar`; no library-specific path is required. The legacy `shellui-sidebar.js` module remains available only for projects that still use the old generated provider.
4. `SidebarInset.razor` **lets wide content push the page sideways.** The `<main>` is a flex child without `min-w-0`, so a wide table stretches it past the viewport instead of scrolling inside its own container. Added `min-w-0`.
5. `Skeleton.razor` **uses `ClassName` while the component API and demo use `Class`.** Calls such as `<Skeleton Class="h-4 w-full" />` therefore pass `Class` through unmatched attributes instead of merging it with the skeleton classes, which can remove the pulse/background styling. The component now exposes `Class`, retains `ClassName` for compatibility, and renders `ChildContent` like the ShellUI demo.



## Customised from the dashboard-02 template

- `Components/Layout/DashboardLayout02.razor`: breadcrumb labels, redirect to the account page while a password change is pending, `min-w-0` on the content area.
- `Components/UI/AppSidebar.razor`: FDMS navigation, admin-only Users link, account and sign-out entries.



## Things to know when using it

- **Class overrides do not merge.** `Shell.Cn` joins class strings and does not resolve conflicts like tailwind-merge does. Passing `Class="max-w-2xl"` to something that already sets `max-w-lg` or `sm:max-w-sm` is a coin toss. Use the important modifier, for example `sm:max-w-2xl!` (see `InvoiceDetailSheet.razor`).
- **Styles and scripts come from the library.** `FDMS.UI` builds `wwwroot/app.css` with the Tailwind CLI (`Build/ShellUI.targets`, needs Node). Hosts must reference `_content/FDMS.UI/app.css` and `_content/FDMS.UI/shellui.js` and must not copy the CSS into their own `wwwroot`.
- **Tailwind only scans** `FDMS.UI` **by default.** Classes used in a host's own markup (for example `FDMS.Web/Components`) are not generated unless the host is listed with `@source` in `wwwroot/input.css`.
- **Theme flash.** `ThemeToggle` assumes dark when nothing is stored, and it only touches the `dark` class after the first render. The host needs the small inline script in `FDMS.Web/Components/App.razor` that sets a default and adds `dark` before Blazor starts.
- **No prerender with browser storage.** The signed-in session lives in browser storage, so `FDMS.Web` renders `Routes` and `HeadOutlet` with `prerender: false`. `ThemeToggle` and `SidebarProvider` also need JS interop and skip it during prerender.
- `data-table` pulls in `System.Linq.Dynamic.Core`. It filters and sorts by property name, so columns need a real `PropertyName`.
- **Icons.** The `ShellIcons.Blazor` package (Lucide names, for example `<ShellIcon Name="refresh-cw" />`) is used for icons rather than inline SVGs.
- **Run the CLI from** `src/FDMS.UI`, next to `shellui.json`.

