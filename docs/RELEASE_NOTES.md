# ShellUI Release Notes

# ShellUI v0.4.0-alpha.3 🧪

> The third 0.4 prerelease. A component's `Class` now overrides its own classes, dialogs and sheets keep keyboard focus inside, `add` never downgrades your NuGet packages, and the CLI templates have been brought in line with the NuGet package, fixing several template bugs on the way. It also adds Razor class library support. It is a prerelease, so installs need `--version`. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## ✨ Improvements

- **`Class` overrides a component's own classes.** `Shell.Cn` used to join strings, so `<Card Class="max-w-lg">` on a component that sets `max-w-md` put both on the element and the stylesheet decided. Now, when two classes set the same CSS property under the same variants, the later one (yours) wins, like tailwind-merge: `p-4` replaces an earlier `px-2`, text colour and text size stay separate, and unknown classes are kept. You no longer need the `!` modifier. The NuGet package and the CLI's `Shell.cs` share the same code.
- **Focus stays inside open overlays.** `Dialog`, `AlertDialog`, `Sheet` and `Drawer` keep Tab and Shift+Tab inside while they're open, and return focus to where it was when they close. The legacy (non-compositional) `Sheet` and `Drawer` panels now also take focus and close on Escape.
- **`add` never downgrades a NuGet package.** It used to set the exact version a component pins, so adding `chart` to an app on Blazor-ApexCharts 7 moved it back to 6.0.2. Now it reads the project's `PackageReference` (or `Directory.Packages.props`), adds a missing package or raises an older one, and leaves the same or newer, floating versions and ranges alone.
- **Razor class libraries.** `init` recognises a library shared by several apps and prints what each app needs, with the library's `_content` path, and adds its `@using` to a root `_Imports.razor`. [Using ShellUI from a Razor class library](https://github.com/shellui-dev/shellui/blob/main/docs/razor-class-library.md) has the full setup for Blazor Web App and MAUI Blazor Hybrid hosts.

## 🐛 Fixes: CLI templates now match the package

The CLI templates and the NuGet package had drifted apart. They're reconciled (210 of 217 components identical, the rest differ by design), and a test now compares them. Bugs this fixed:

- **`RadioGroup`** (template): clicking an item did nothing, because the group never passed itself to its items. `RadioGroupItem` also gains `Disabled`.
- **`ScrollArea`** (template): long content was clipped instead of scrolling. `Height` and `MaxHeight` now size the scrolling element.
- **`DatePicker`** (package): day 1 was always drawn under Sunday. It now sits under its real weekday, and the picker opens on the selected month.
- **`TimePicker`**: `Step` now filters the minutes (it was ignored), the picker opens on the current value, and Cancel or an outside click closes it.
- **`DateRangePicker`** (template) closes on an outside click; **`EmptyState`** (template) applies `Class`.
- **`Sheet` and `Drawer`**: clicks inside the panel no longer reach parent click handlers; the close button shows when only `Description` is set; the templates switch to compositional mode by themselves (no `Compositional="true"` needed).
- **`Label`** renders `For` as the `for` attribute; **`FileUpload`** uses an inline SVG icon instead of the Material Symbols font; **`CarouselItem`** handles zero slides.
- **`Pagination`** supports `@bind-CurrentPage` in both copies and ignores clicks on the current page; **`Toast`** has success and warning variants, `Position`, auto-dismiss after `Duration` (0 keeps it open) and a labelled close button; **`Skeleton`** has `Variant` (`default`, `circle`, `text`); **`NavigationMenuItem`** is a dropdown when `Title` is set and a link or button otherwise.
- `Table`, `Separator`, `Slider`, `Navbar`, `Checkbox`, `Switch`, `Tooltip`, `SidebarMenuButton`, `Menubar` and `ThemeToggle` templates now match the package's markup and classes.

## ⚠️ Breaking changes (CLI templates, after `shellui update`)

- **Input variants** moved out of the component: `Input.InputVariant.Error` is now `InputVariant.Error`, from the new `input-variants` template in `Components/UI/Variants` (like `ButtonVariant`). `Textarea` gains `Variant`, `ReadOnly`, `OnFocus` and `OnBlur` through `textarea-variants`.
- **Stepper** parts read the stepper's `ActiveStep` instead of taking a `CurrentStep` parameter, and the template gains the package's data-driven `StepItems` mode (`StepItem` comes from the new `stepper-models` template).
- **ScrollArea** drops its `scrollbar-*` classes, which needed a Tailwind plugin nobody shipped.
- Still working but deprecated: `Pagination.OnPageChange` (use `@bind-CurrentPage`), `DatePicker.SelectedDate` and `TimePicker.SelectedTime` (use `Value` / `@bind-Value`).

`shellui update` installs the new `input-variants`, `textarea-variants` and `stepper-models` templates for you, and skips files you've changed since installing them.

## ⬆️ Upgrading from 0.4.0-alpha.2

- **CLI:** update the tool, then run `shellui update`. Commit first so you can review the changes, and fix any `Input.InputVariant` and Stepper `CurrentStep` uses (see above).
- **NuGet package:** update `ShellUI.Components` to `0.4.0-alpha.3`.
- If you used `Class="…!"` to beat a component's own classes, you can drop the `!`.

## 📦 Installation

```bash
# CLI (prerelease: the version is required)
dotnet tool install -g ShellUI.CLI --version 0.4.0-alpha.3
# or upgrade an existing install
dotnet tool update -g ShellUI.CLI --version 0.4.0-alpha.3
```

```bash
# NuGet package
dotnet add package ShellUI.Components --version 0.4.0-alpha.3
```

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.4.0-alpha.2...v0.4.0-alpha.3

# ShellUI v0.4.0-alpha.2 🧪

> The second 0.4 prerelease. It fixes `shellui update`, which could leave a project upgraded from 0.3 unable to build, and makes `update` keep the files you changed. The Tailwind CLI is now downloaded once per machine instead of into every project, and the download survives slow connections. It also fixes `remove`, the dark theme and the package `TimePicker`. It is a prerelease, so installs need `--version`. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## 🌬️ Tailwind CLI: once per machine

- **Projects no longer hold the binary.** The standalone Tailwind CLI (about 110 MB) is downloaded once into `~/.shellui/bin` (or `SHELLUI_CACHE_DIR`), and `init` and the build run it from there. A new project went from about 112 MB to 130 KB, and later projects set up offline. A copy in a project's `.shellui/bin` still takes precedence. The binary is a build tool and never shipped with your app.
- **Nothing in `.shellui/` is committed:** it now contains a `.gitignore` of `*`.
- **Slow connections work.** The download streams to a `.partial` file, resumes after a dropped connection, retries, and only gives up after a minute with no data. Before, `init` gave up after five minutes and left the project uninitialised. The build's own download uses the same cache and retries too.
- **A failed download no longer stops `init`.** It finishes the rest of the setup and warns, and the next build tries again; its warning names the URL and where to save the file.

## 🔧 `shellui update` and `remove`

- **`update` installs new dependencies.** Components gain parts between versions; in 0.4, `breadcrumb` gained `breadcrumb-list`, `-link`, `-page`, `-separator` and `-ellipsis`, and `tabs` gained `tab-models`. `update` only rewrote existing files, so a project updated from 0.3 failed to build (`BreadcrumbList`, `TabItem` not found). It now installs missing dependencies the way `add` does, with their NuGet packages and `_Imports.razor` usings.
- **`update` keeps your changes.** `shellui.json` now records a hash of each file the CLI writes. `update` skips files you have changed since and says so; `--force` overwrites them. Before, `update` with no names rewrote everything, including `app-sidebar` and the dashboard layouts that apps are meant to edit.
- **`remove` finds files the way install writes them.** It always looked in `Components/UI`, so it ignored a custom `ComponentsPath`, could not remove dashboard or auth layouts, and reported every name as removed. It now removes layouts too, lists files that still use a removed layout (usually `Routes.razor`), and counts what it removed.

## 🐛 Fixes

- **Dark theme:** the browser drew its own controls (the `<input type="date">` calendar, number spinners, scrollbars, autofill, `<select>` lists) light on a dark page. The themes now set `color-scheme`: in the theme `init` and `theme apply` write, and in the package's `shellui-theme.css` and `shellui-all.css`.
- **NuGet package `TimePicker`** threw a `FormatException` whenever `Value` was set. It now shows the time.
- **NuGet package `shellui-theme.css`** logged a 404 on every page that linked it directly, because it started with `@import "tailwindcss";`. The file now holds only the theme; `shellui-all.css` is unchanged.
- **NuGet packages** now have the ShellUI icon and show the logo at the top of their README.
- Everything fixed in 0.3.3 and 0.3.4 is included.

## ⬆️ Upgrading from 0.4.0-alpha.1

- **CLI:** update the tool, then run `shellui update`. Files installed by alpha.1 have no recorded hash, so this first update keeps `app-sidebar`, `sidebar-account` and the dashboard and auth layouts if they differ from the template, and rewrites the other components. Commit first so you can review the changes. From then on, every file you edit is kept.
- **Tailwind CLI:** the same `shellui update` replaces `Build/ShellUI.targets` if you haven't edited it, adds `.shellui/.gitignore`, and deletes the project's copy of the Tailwind CLI when it's the same as the shared one, so the project switches to the shared CLI. An edited targets file is left alone with a note.
- **Existing `input.css`:** `update` doesn't rewrite it. Add `color-scheme: light;` inside `:root` and `color-scheme: dark;` inside `.dark`, or run `shellui theme apply` again if you use a tweakcn theme.
- **NuGet package:** update `ShellUI.Components` to `0.4.0-alpha.2`.

Upgrading from 0.3: see the 0.4.0-alpha.1 notes below, then the steps above.

## 📦 Installation

```bash
# CLI (prerelease: the version is required)
dotnet tool install -g ShellUI.CLI --version 0.4.0-alpha.2
# or upgrade an existing install
dotnet tool update -g ShellUI.CLI --version 0.4.0-alpha.2
```

```bash
# NuGet package
dotnet add package ShellUI.Components --version 0.4.0-alpha.2
```

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.4.0-alpha.1...v0.4.0-alpha.2

# ShellUI v0.3.5

> A patch release that brings the 0.4 prerelease fixes to the stable line. `init` no longer downloads the Tailwind CLI into every project, `update` keeps your changes, `add` never downgrades a NuGet package, a component's `Class` overrides its own classes, overlays keep keyboard focus inside, and several component bugs are fixed. It also adds Razor class library support.

## 🛠️ CLI

- **One Tailwind CLI per machine.** `init` used to download the 112 MB Tailwind CLI into every project, hold it in memory and give up after five minutes, so on a slow connection it failed and left the project half set up. The CLI now lives once in `~/.shellui/bin` (set `SHELLUI_CACHE_DIR` to move it), and `init` and the build both run it from there; a copy in a project's `.shellui/bin` still wins. Downloads resume after a dropped connection, retry, and only stop after 60 seconds without data. If the download fails, `init` finishes everything else and the next build tries again. `.shellui/` gets its own `.gitignore`, so the folder stays out of git.
- **`update` keeps your changes.** `shellui.json` now records a hash of each file the CLI writes. `update` skips files you've changed since and says so (`--force` overwrites them), and installs dependencies a component gained since you added it, the same way `add` does. Files installed by an earlier CLI have no hash; for those, `update` keeps `app-sidebar` and the `dashboard-*` layouts when they differ from the template.
- **`update` moves existing projects onto the shared Tailwind CLI.** It replaces a `Build/ShellUI.targets` written by an earlier CLI (unless you edited it) and deletes the project's copy of the Tailwind CLI when it is identical to the shared one.
- **`remove` finds what `install` wrote.** It always looked in `Components/UI`, so it ignored a custom `ComponentsPath`, couldn't remove the dashboard layouts, and reported every name as removed. It now uses the install path, reports only what it removed, and lists files that still use a removed layout.
- **`add` never downgrades a NuGet package.** It used to set the exact version a component pins, so adding `chart` to an app on Blazor-ApexCharts 7 moved it back to 6.0.2. Now it adds a missing package or raises an older one, and leaves the same or newer versions, floating versions and ranges alone (including `Directory.Packages.props`).
- **Razor class libraries.** `init` recognises a library shared by several apps, prints what each app needs with the library's `_content` path, and adds its `@using` to a root `_Imports.razor`. [Using ShellUI from a Razor class library](https://github.com/shellui-dev/shellui/blob/main/docs/razor-class-library.md) covers Blazor Web App and MAUI Blazor Hybrid hosts.

## ✨ Components (package and CLI templates)

- **`Class` overrides a component's own classes.** `Shell.Cn` used to join strings, so `Class="max-w-lg"` on a component that sets `max-w-md` put both on the element and the stylesheet decided. Now, when two classes set the same CSS property under the same variants, the later one (yours) wins, like tailwind-merge: `p-4` replaces an earlier `px-2`, text colour and text size stay separate, and unknown classes are kept. You no longer need the `!` modifier. Run `shellui update shell` to get it in a CLI project.
- **Focus stays inside open overlays.** `Dialog`, `AlertDialog`, `Sheet` and `Drawer` keep Tab and Shift+Tab inside while open and return focus to where it was when they close. The non-compositional `Sheet` and `Drawer` panels now also take focus and close on Escape.

## 🐛 Fixes

- **`RadioGroup`** (template): clicking an item did nothing, because the group never passed itself to its items. `RadioGroupItem` also gains `Disabled`.
- **`ScrollArea`**: long content was clipped instead of scrolling. `Height` and `MaxHeight` now size the scrolling element. The `scrollbar-*` classes, which needed a Tailwind plugin nobody shipped, are gone.
- **`DatePicker`** (package): day 1 was always drawn under Sunday. It now sits under its real weekday, and the picker opens on the selected month.
- **`TimePicker`**: `Step` now filters the minutes (it was ignored), the picker opens on the current value, and Cancel or an outside click closes it.
- **`DateRangePicker`** (template) closes on an outside click; **`EmptyState`** (template) applies `Class` and extra attributes.
- **`Sheet` and `Drawer`**: clicks inside the panel no longer reach parent click handlers, and the `Sheet` close button shows when only `Description` is set.
- **`Label`** (package) renders `For` as the `for` attribute; **`FileUpload`** (package) uses an inline SVG icon instead of the Material Symbols font; **`CarouselItem`** (template) handles zero slides.
- Still working but deprecated: `DatePicker.SelectedDate` and `TimePicker.SelectedTime`. Use `Value` / `@bind-Value`.

## ⬆️ Upgrading

- **CLI:** update the tool, then run `shellui update` in each project. Commit first so you can review the changes. If the project still has a `.shellui/bin/tailwindcss` binary afterwards, you can delete it: the next build downloads the shared copy.
- **NuGet package:** update `ShellUI.Components` to `0.3.5`.
- If you used `Class="…!"` to beat a component's own classes, you can drop the `!`.

## 📦 Installation

```bash
dotnet tool install -g ShellUI.CLI --version 0.3.5
# or upgrade an existing install
dotnet tool update -g ShellUI.CLI --version 0.3.5
```

```bash
dotnet add package ShellUI.Components --version 0.3.5
```

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.3.4...v0.3.5

# ShellUI v0.3.4

> A patch release with a `TimePicker` fix for the `ShellUI.Components` NuGet package and dark-theme fixes for both the package and the CLI.

## 🐛 Fixes

- NuGet package: `TimePicker` threw a `FormatException` whenever `Value` was set, because it formatted the `TimeSpan` with a date pattern. It now shows the time, the same as the CLI template.
- Dark theme: the browser kept drawing its own controls light on a dark page (the `<input type="date">` calendar, number spinners, scrollbars, autofill and `<select>` lists). The themes now set `color-scheme` to `light` in `:root` and `dark` in `.dark`: in `shellui-theme.css` and `shellui-all.css` in the package, and in the theme `shellui init` and `shellui theme apply` write.

A new test renders every package component that takes a value with a sample value set, so a component that only fails once it has a value fails CI.

## ⬆️ Upgrading

- **NuGet package:** update to `0.3.4`.
- **CLI:** update the tool. Existing projects keep their `input.css`: add `color-scheme: light;` inside `:root` and `color-scheme: dark;` inside `.dark`, or run `shellui theme apply` again if you use a tweakcn theme.

# ShellUI v0.3.3

> A patch release with fixes for both the `ShellUI.Components` NuGet package and the CLI templates.

## 🐛 Fixes

- NuGet package: linking `shellui-theme.css` directly logged a 404 on every page load, because the file started with `@import "tailwindcss";` and the browser requested `tailwindcss` as a URL. The theme file now holds only the theme; the precompiled `shellui-all.css` is unchanged.
- `Command` ignored `CommandItem.Action`: selecting an item only raised `CommandSelected`. It now runs the item's `Action`, then raises `CommandSelected`, the same as `CommandPalette`.
- `Dropdown` and `Popover` now close on Escape.
- CLI `SidebarProvider` logged a `JSDisconnectedException` twice on every full page reload, typed URL or closed tab. It now ignores the disconnected circuit when it cleans up.

New tests cover `Command` running `Action` and Escape closing `Dropdown` and `Popover`.

## ⬆️ Upgrading

- **NuGet package:** update to `0.3.3`.
- **CLI:** update the tool, then run `shellui update` to rewrite installed components from the new templates. `update` overwrites the files, so commit or back up any components you customized first.

# ShellUI v0.4.0-alpha.1 🧪

> The first 0.4 prerelease. It adds 14 components, compositional parts for eight existing ones, dashboard and sign-in page blocks, and support for apps that use ASP.NET Core Identity. It is a prerelease, so installs need `--version`. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## ✨ New components

The CLI now has 93 direct targets, up from 76 in 0.3.

| Target | What it is |
|---|---|
| `kbd` | Keyboard key hint |
| `aspect-ratio` | Fixed-ratio container |
| `button-group` | Joined buttons, horizontal or vertical |
| `toggle-group` | Single- or multi-select toggles (`@bind-Value` / `@bind-Values`) |
| `input-group` | Input with `Prefix` / `Suffix` slots |
| `number-input` | −/+ input with min, max and step |
| `stat-card` | KPI tile with a trend badge |
| `timeline` | Vertical event timeline |
| `tree-view` | Expandable, selectable tree (`@bind-SelectedValue`) |
| `qr-code` | QR code rendered as SVG (adds the `QRCoder` package) |
| `image-viewer` | Thumbnail with a zoomable lightbox |
| `chat`, `chat-message`, `chat-input` | AI chat panel, message bubbles with a streaming indicator, and a prompt input (Enter sends, Shift+Enter adds a line) |

## 🧩 Compositional parts

Eight more components can be built from parts, shadcn-style. Each mode is opt-in, so existing markup keeps working, and the parts install with their parent.

- **AlertDialog:** `AlertDialogTrigger`, `Content`, `Header`, `Title`, `Description`, `Footer`, `Action`, `Cancel`. Escape acts as Cancel.
- **Breadcrumb:** `BreadcrumbList`, `Link`, `Page`, `Separator`, `Ellipsis`.
- **Command:** `CommandInput`, `List`, `Group`, `CommandOption`, `Empty`, `Separator`. The part is `CommandOption` because `CommandItem` is the model.
- **Form:** `FormField`, `Item`, `Label`, `Control`, `Description`, `Message`, with live validation inside an `EditForm`.
- **Menubar:** `MenubarMenu`, `Trigger`, `Content`, `Separator`. A `MenubarItem` with a `Title` is a dropdown; without one it is a clickable item.
- **Pagination:** `PaginationContent`, `Item`, `Link`, `Previous`, `Next`, `Ellipsis`.
- **Sheet and Drawer:** `Header`, `Title`, `Description`, `Footer`, `Close`. CLI templates use them with `Compositional="true"`.

## 🧱 Blocks and `shellui init`

- **Dashboard setup in `init`:** `init` asks for a layout (sticky header `dashboard-02`, scrolling header `dashboard-01`, or none), or takes `--dashboard 01|02|none`. Adding a dashboard wires it in: `Routes.razor` uses it, the stock `MainLayout` and `NavMenu` are removed when unmodified, the error bar moves to `App.razor`, and the sidebar links come from your pages. A custom layout is only replaced after you confirm or pass `--replace-layout`.
- **Sample pages restyled:** unmodified `dotnet new blazor` pages (Home, Counter, Weather, Error, NotFound, Auth) get ShellUI's Tailwind classes, and Home becomes a short guide to themes and components. Edited pages are listed, not changed.
- **ASP.NET Core Identity apps (`--auth Individual`):**
  - The `/Account` pages render statically while the rest of the app is interactive. Before, `init` made every page interactive and the Identity pages returned Not Found.
  - The dashboard sidebar shows Log in and Register, or the signed-in user and Log out.
  - New blocks `auth-01` (centered card), `auth-02` (split screen) and `auth-03` (minimal) give the sign-in, sign-up and recovery pages their own layout and restyle the stock Identity pages for .NET 8, 9 and 10.
- **Tailwind CLI:** in projects set up by this version, the build downloads the pinned Tailwind CLI when `.shellui/bin` has none, for example after a fresh clone, and warns if it can't instead of silently skipping Tailwind.
- **Terminal:** `init` shows the ShellUI logo and an animated logo loader; `add`, `update` and theme commands use a compact spinner. Both fall back to plain lines without an interactive terminal.

## 🎨 Component changes

- **Loading** is rebuilt with 23 variants, including `logo`, `snake`, `wave`, `typing` and `shimmer`. Its keyframes ship inside the component, so the animations also work with the NuGet package. It follows `currentColor`, has `role="status"`, and slows down under reduced motion.
- **Command** is rewritten: it filters as you type, supports ↑/↓/Home/End/Enter, optional `Group` headings and a footer with key hints. Selecting an item now runs its `CommandItem.Action`, then raises `CommandSelected`; before, `Action` was ignored. `CommandPalette` gets the same behavior and a bindable `IsOpen`.
- **Dropdown and Popover** close on Escape.

## 🐛 Fixes

- The CLI's `SidebarProvider` logged `JSDisconnectedException` twice on every page reload. It no longer does.
- `init --tailwind standalone|npm` now skips the Tailwind prompt, which ignored the flag. An unknown value fails before `init` changes the project, and `--yes` says that standalone is the default.
- `shellui update` reported every requested component as updated. It now counts updated, skipped and failed components separately.
- Everything fixed in 0.3.1 and 0.3.2 is included.

## ⚠️ Breaking changes

- **Calendar:** `SelectedDateChanged` is now `EventCallback<DateTime?>`, so `@bind-SelectedDate` works with a `DateTime?` field. Handlers that take a `DateTime` must take a `DateTime?`.

## ⬆️ Upgrading from 0.3

- **CLI:** update the tool to this version, then run `shellui update` to rewrite installed components from the new templates. `update` overwrites the files, so commit or back up any components you customized first. `Build/ShellUI.targets` is only written by `init`, so projects set up with 0.3 keep the old Tailwind build step.
- **NuGet package:** update `ShellUI.Components` to `0.4.0-alpha.1`. If you handle `Calendar.SelectedDateChanged`, change the handler's parameter to `DateTime?`.

## 📦 Installation

```bash
# CLI (prerelease: the version is required)
dotnet tool install -g ShellUI.CLI --version 0.4.0-alpha.1
# or upgrade an existing install
dotnet tool update -g ShellUI.CLI --version 0.4.0-alpha.1
```

```bash
# NuGet package
dotnet add package ShellUI.Components --version 0.4.0-alpha.1
```

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.3.2...v0.4.0-alpha.1

# ShellUI v0.3.2

> A patch release with component fixes for both the `ShellUI.Components` NuGet package and the CLI templates.

## 🐛 Fixes

- `MultiSeriesChart` threw on every render in the NuGet package. It now renders, inside the same card as `Chart`.
- NuGet package: `ThemeToggle` flipped its icon but did not change the theme, dialogs, sheets and drawers did not lock page scrolling, and close-on-scroll dropdowns stayed open. The package's `shellui.js` now loads automatically through a Blazor JS initializer, with no `<script>` tag needed, and includes the functions `ThemeToggle`, `ThemeService` and `FileUpload` call.
- `ThemeToggle` kept a list of instances shared by every user on Blazor Server, so one user's toggle tried to re-render other users' components. Toggles now follow the page's `dark` class, so all toggles on a page stay in sync and start from the page's actual theme.
- `DatePicker` and `DateRangePicker`: the calendar popover has a width, so it is no longer squeezed in a flex row.
- `DataPicker` and `MultiSelect`: option rows are left-aligned, so custom `OptionTemplate`s no longer render centered.
- Removed leftover files from the Razor class library template (`Component1`, `ExampleJsInterop`, `background.png`) from the package.

A new test renders every package component and fails on parameter errors like the `MultiSeriesChart` one.

## ⬆️ Upgrading

- **NuGet package:** update to `0.3.2`.
- **CLI:** update the tool, then run `shellui update` to rewrite installed components, including `shellui.js`, from the new templates. `update` overwrites the files, so commit or back up any components you customized first.

# ShellUI v0.3.1

> A patch release for the `ShellUI.Components` NuGet package. The CLI and its templates have no changes beyond the version number.

## 🐛 Fixes

- `Calendar`, `Command`, `DataTable`, `FileUpload`, `CarouselContent`, `CarouselDots`, `CarouselNext` and `CarouselPrevious` compiled into the `ShellUI.Components.Components` namespace, so `@using ShellUI.Components` did not find them. They are now in `ShellUI.Components` like every other component, and a test checks that every package component declares that namespace.

## ⬆️ Upgrading

Update the package to `0.3.1`. If you added `@using ShellUI.Components.Components` to work around this, remove it: that namespace no longer exists, so the line now fails the build.

# ShellUI v0.3.0 🎉

> The first stable release of the 0.3 line. It builds on .NET 10 and Tailwind CSS 4.3.2 and ships everything from the 0.3.0 alphas and release candidates. A plain `dotnet tool install` now picks it up, so `--version` is no longer needed. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## Highlights since 0.2.1

- **Blazor on .NET 8, 9 and 10.** The CLI needs the .NET 10 SDK, and the components it installs build in projects on .NET 8, 9 and 10; CI now checks all three. The `ShellUI.Components` NuGet package targets .NET 10 only.
- **Tailwind CSS 4.3.2.**
- **`shellui init` produces an app that builds and runs.** It patches `App.razor` (render mode, theme bootstrap, `shellui.js`), writes the full theme to `input.css`, and wires Tailwind into the build.
- **`shellui add` resolves everything a component needs:** its sub-components, models and variants, NuGet packages (`Blazor-ApexCharts`, `System.Linq.Dynamic.Core`), and the `@using` lines in `_Imports.razor`. Typos get a "did you mean" suggestion.
- **76 components**, including `command-palette`, `data-picker`, `multi-select`, `tag-input`, `typed-select`, and the `donut-chart`, `radar-chart` and `radial-chart` charts.
- **Composable APIs** for Select, Dropdown, Popover, ContextMenu, NavigationMenu, Carousel, HoverCard, Accordion and Tabs, installed together with their parent.
- **Themes from [tweakcn](https://tweakcn.com):** `shellui theme init | apply | update`.
- **NuGet package** with a precompiled `shellui-all.css` bundle and a Tailwind safelist, so it works without a Tailwind build step.
- **Accessibility:** Dialog, Sheet and Drawer take focus on open, close on Escape, and set `role="dialog"`.
- `Class` is accepted everywhere; `ClassName` still works but is deprecated.

See the release candidate sections below for the full list of changes.

## 🐛 Fixes since rc.3

- Adding certain components on their own produced a project that did not build:
  - `avatar`, `alert`, `badge`, `sonner`, `toggle`: `shellui add` wrote `@using ….Components.UI.Variants` to `_Imports.razor`, but those variants files declare `.Components.UI`. Imports now follow the namespace each installed file declares.
  - `input`, `alert`, `badge`, `toggle`: the component files imported that same namespace themselves. The import is removed.
  - `chart-series`: now installs `chart` and the `Blazor-ApexCharts` package.

  Every component was installed and built on its own in a fresh app for this release, and a new test checks that each component installs the namespaces and packages it imports.
- On .NET 8 and 9 projects whose name is not a valid namespace, such as `my-app`, installed files got `namespace my-app.Components.UI` and did not build. The template writes `<RootNamespace>my-app</RootNamespace>`, while the compiler uses `my_app`; the CLI now does the same.
- `shellui init` on .NET 8 left Bootstrap active: the template keeps it in `wwwroot/bootstrap/`, which was not removed. It is now, and the local Bootstrap `<link>` is removed from `App.razor` (on .NET 9 and 10 it pointed at deleted files). Bootstrap loaded from a CDN is left alone.
- On Windows the CLI now writes UTF-8, so ✅ no longer prints as `?` and the spinner no longer falls back to ASCII.
- A failed `shellui init` now exits with code 1.

## ⬆️ Upgrading

- **From a 0.3.0 release candidate:** update the tool, then run `shellui update` to rewrite the installed components from the new templates. `update` overwrites the files, so commit or back up any components you customized first.
- **From 0.2.1:** install the .NET 10 SDK to run the CLI. Your project can stay on .NET 8 or 9 when you use the CLI; the NuGet package needs .NET 10. Read the rc.1 notes below for what changed in `init` and the templates.

## 📦 Installation

```bash
# CLI
dotnet tool install -g ShellUI.CLI
# or upgrade an existing install
dotnet tool update -g ShellUI.CLI
```

```bash
# NuGet package
dotnet add package ShellUI.Components
```

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.2.1...v0.3.0

---

# ShellUI v0.3.0-rc.3 🚦

> Third release candidate for v0.3.0. It contains only fixes, mostly to what `shellui add` installs. There are no new components and no breaking changes. If nothing critical comes up during testing, v0.3.0 ships from this code with the suffix dropped. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## 🐛 Fixes

### Components that installed incomplete or didn't compile
- **Composable parts are installed.** `SelectTrigger`, `SelectContent`, `SelectItem`, `ContextMenuTrigger`, `ContextMenuContent`, `ContextMenuOption`, `NavList`, `NavItem`, `NavTrigger`, `NavContent`, `CarouselList` and `CarouselSlide` were empty templates, so composable markup failed to compile after `shellui add`. They now ship with their parent component.
- The Dropdown, Popover, HoverCard and Accordion parts (`*Trigger`, `*Content`, `DropdownItem`) were never pulled in by their parent, and the Dropdown, Popover and HoverCard CLI templates didn't support composable use. The CLI templates now match the NuGet package.
- `Tabs` supports `Items` / `TabItems` and `ActiveTab` as in the package. Its `TabModels.cs` now installs to `Components/UI/Models/` instead of a nested `Components/UI/Components/Models/` folder.
- `command` no longer declares its own `CommandItem`, which clashed with `command-palette`. It now installs the shared `CommandModels.cs`.
- `alert-dialog` was missing its `Variants` using.
- `shellui add` adds the `@using` lines that installed components need (`.Components.UI.Variants` and `.Components.Models`) to `_Imports.razor`.

### Behavior
- Compositional Dropdown, Popover, ContextMenu and NavigationMenu items now open and close without binding `IsOpen`. ContextMenu and Dropdown close on an outside click.
- A value-based `Carousel` (`CarouselSlide Value="..."`) shows its dots and the correct width on first render.
- `HoverCardContent` stays open while the pointer is over it.
- `SelectItem` marks the selected option (`data-selected`, `aria-selected`), and triggers expose `aria-expanded`.
- Dialog, Sheet and Drawer take focus when they open, close on Escape, and set `role="dialog"` and `aria-modal`.
- Keyboard shortcuts such as `Ctrl+K` accept Ctrl or Cmd, as intended. Before, they required both.
- `Loading` animations work in CLI projects. Their keyframes ship with the component, and the grid delays no longer depend on the server culture.
- Headings focused by Blazor navigation no longer show a focus outline.

### Tooling
- CI runs on `release/**` branches and installs every component into a fresh app and builds it, so a template that doesn't compile or a missing dependency fails the build.
- New tests fail when a registry entry has empty content, when a hidden part isn't reachable from any installable component, or when a CLI template drifts from its package component.

## 📦 Installation

```bash
# CLI (prerelease: the version is required)
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.3
# or upgrade an existing install
dotnet tool update -g ShellUI.CLI --version 0.3.0-rc.3
```

```bash
# NuGet package
dotnet add package ShellUI.Components --version 0.3.0-rc.3
```

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.3.0-rc.2...v0.3.0-rc.3

---

# ShellUI v0.3.0-rc.2 🚦

> Second release candidate for v0.3.0. It moves ShellUI to .NET 10, adds new components, and fixes bugs found in real projects using rc.1. If nothing critical comes up during testing, v0.3.0 ships from this code with the suffix dropped. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## ⚠️ Breaking: .NET 10 required

- `ShellUI.Components` now targets `net10.0` (rc.1 targeted `net9.0`). Projects on .NET 9 should stay on rc.1 until they upgrade.
- The `shellui` CLI tool needs the .NET 10 runtime to run.
- Tailwind CSS goes from `4.1.18` to `4.3.2`.

## ✨ New

- **Components:** `command-palette`, `data-picker`, `multi-select`, `tag-input`, `typed-select` (#25, #27)
- **Charts:** new `donut-chart`, `radar-chart` and `radial-chart`, in both the CLI and the NuGet package. All charts are restyled to match shadcn (#26)
- **DataTable:** server-side mode via `DataTableRequest` / `DataTableResponse` (#27)
- **Sonner:** toast variants and per-toast duration (#27)
- **Themes:** `shellui theme init | apply | update` bake a [tweakcn](https://tweakcn.com) theme into `wwwroot/input.css` and record it in `shellui.theme.lock` (#23, #24)
- **NuGet package:** ships a precompiled `shellui-all.css` bundle and a Tailwind safelist, so it works without a Tailwind build step (#20, #22)

## 🐛 Fixes

- `shellui add accordion` failed because `accordion-type` was not registered (#29)
- `Tabs` generated an unterminated string and did not compile (#29)
- `Select` and `FileUpload` showed icon names such as `expand_more` as text, because they relied on the Material Symbols font. Both now use inline SVG (#29)
- `DashboardLayout01` breadcrumb code did not compile (#29)
- `context-menu` referenced a `ContextMenuItem` model that was never installed (#29)
- `SidebarInset`: wide content no longer pushes the page sideways (#29)
- Sidebar mobile detection and Ctrl/Cmd+B now work when components are compiled into a Razor Class Library (#30)
- `Class` is now accepted by the 30 templates and 8 package components that only took `ClassName`. Before, `Class="..."` could drop a component's base styling. `ClassName` still works but is deprecated
- `shellui add` exits with 1 when any component, dependency or NuGet package fails. `init` fails loudly if its base files can't be written, and `update` records the new version in `shellui.json` (#30)
- Chart tooltips: pie, donut and radial tooltips showed empty rows, radar charts showed none, and bar/line tooltips lost their x-axis label. CLI-installed charts also used an outdated copy of the chart options (palette, animations, legend). The CLI templates now match the NuGet package
- Overlays close on dismiss, lock body scroll, and clean up asynchronously (#27)
- Stepper highlights the active step correctly

## 📦 Installation

```bash
# CLI (prerelease: the version is required)
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.2
# or upgrade an existing install
dotnet tool update -g ShellUI.CLI --version 0.3.0-rc.2
```

```bash
# NuGet package
dotnet add package ShellUI.Components --version 0.3.0-rc.2
```

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.3.0-rc.1...v0.3.0-rc.2

---

# ShellUI v0.3.0-rc.1 🚦

> Release candidate for v0.3.0. Five branches of integration-tested fixes against the alpha series, surfaced from real-world Blazor Server consumer use. If no critical reports come in during the soak window, this code ships as `v0.3.0` stable with the suffix dropped — no further code changes. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## TL;DR

`shellui init` + `shellui add` now produce a project that compiles and runs end-to-end without manual host patching. Every alpha.3 install-time papercut documented in the integration notes is fixed and guarded by tests + CI:

- Three chart/dashboard templates no longer ship uncompilable C# verbatim strings
- `SidebarTrigger` mobile hamburger renders (was an invisible FontAwesome class)
- `ThemeToggle` no longer uses `eval` and no longer crashes during Blazor Server prerender
- `shellui init` patches `App.razor` with `@rendermode`, theme bootstrap script, and `shellui.js` link tag — and writes the full default theme to `input.css` instead of just `@import "tailwindcss";`
- `shellui add data-table` actually installs `DataTableModels.cs` and auto-runs `dotnet add package System.Linq.Dynamic.Core`
- `shellui add chart` auto-runs `dotnet add package Blazor-ApexCharts` and the chart tooltip CSS finally renders readable text instead of invisible white-on-white
- New `did you mean …?` typo suggestion (`shellui add datatable` → `Did you mean 'data-table'?`)

Component count corrected across docs: **68 installable** top-level components (not the 100 previously claimed; sub-components and variants ship as auto-installed dependencies).

## 🐛 Critical fixes

### Templates that shipped uncompilable C#
- **`ChartVariants`** — every JS-attribute quote inside the tooltip `Custom = @"..."` block now escapes as `""x""` (the verbatim-string form) instead of bare `"x"` (which terminated the outer string)
- **`PieChart`** — same class of bug but using `\"x\"` (which decoded to literal backslashes in rendered HTML); fixed to `""x""`
- **`DashboardLayout02`** — unterminated empty-string literal in `BuildBreadcrumb` (`segments[0] == ""))` → `segments[0] == """"))`)
- Live `PieChart.razor` in the library had the same backslash bug; fixed to remove visible `\"` from rendered tooltip HTML

### SidebarTrigger + ThemeToggle runtime
- **`SidebarTrigger`** — `<i class="fa-solid fa-bars-staggered">` swapped for inline SVG hamburger. FontAwesome is not a ShellUI dependency, so mobile users on a fresh install previously saw an invisible toggle button
- **`ThemeToggle`** — `JSRuntime.InvokeVoidAsync("eval", …)` dropped in favor of the `ShellUI.addClassToDocument` / `ShellUI.removeClassFromDocument` helpers already shipped in `shellui.js`. No more CSP-blockable `eval`, no new JS surface area
- **`ThemeToggle`** — localStorage read moved from `OnInitializedAsync` (where `IJSRuntime` is unavailable during Blazor Server prerender) to `OnAfterRenderAsync(firstRender)`
- **`InputOTP`** — same `eval` removal applied; now uses `ShellUI.focusElement` for digit-to-digit focus
- **`ThemeService`** — same `eval` cleanup as `ThemeToggle`

### `shellui init` produces a working host
- **App.razor patching** — `<HeadOutlet />` → `<HeadOutlet @rendermode="InteractiveServer" />`, same for `<Routes />`. Existing `@rendermode` values are preserved (won't overwrite `InteractiveAuto`/`InteractiveWebAssembly`)
- **Theme bootstrap `<script>` injected into `<head>`** — reads `localStorage.theme` and applies the `dark` class before paint to avoid the light-flash on dark pages
- **`<script src="shellui.js">` injected** before `_framework/blazor.web.js` (handles both the modern `@Assets[...]` wrapper and the bare form)
- **`wwwroot/index.html` patched** for Blazor WebAssembly standalone projects (theme bootstrap + `shellui.js` only — no render-mode pattern in WASM)
- **`input.css` ships the full default theme** — `:root` light vars, `.dark` dark vars, `@theme inline` mapping for Tailwind v4, `@custom-variant dark`, `@layer base` defaults, and Loading-component animation keyframes. Previously emitted only `@import "tailwindcss";`, so any component using theme variables rendered unstyled
- **`tailwind.config.js` (npm)** — dropped the redundant `hsl(var(--x))` color block; Tailwind v4 reads the palette from `@theme inline` in `input.css`
- **All patching is idempotent** — running `shellui init` twice produces no duplicate scripts or tags

### `shellui add data-table` is no longer a half-install
- **`data-table-models` registered** — the template existed on disk but was missing from `ComponentRegistry`, so the CLI reported `Component 'data-table-models' not found` and left the consumer project unable to compile
- **NuGet auto-install** — new `NuGetDependencies` field on `ComponentMetadata`. `data-table` declares `System.Linq.Dynamic.Core 1.7.1`, `chart` declares `Blazor-ApexCharts 6.0.2`. The CLI walks the dep graph and runs `dotnet add package` once per unique package after all source files are written
- **Did-you-mean suggestions** — Levenshtein-based hint surfaces close matches when a user mistypes a component name. `shellui add datatable` → `Did you mean 'data-table'?` Hidden sub-components are excluded from suggestions

### `shellui add chart` tooltip is finally readable
- **`chart-styles` registered** — the CSS template existed but, like `data-table-models`, was never wired into the registry. Hovering a chart point previously showed an invisible white-on-white tooltip because nothing styled the `.custom-tooltip-*` classes the chart HTML emits
- **`chart` depends on `chart-styles`** — the recursive install walk transitively pulls the CSS in for every chart-family component
- **CSS lands in the right place** — `FilePath` corrected from a path that would have buried the file in `Components/UI/wwwroot/css/` (unreachable) to `../../wwwroot/css/charts.css` (project root)
- **`<link>` auto-injected into App.razor** — same idempotent rewriter pattern used for `shellui.js`. Generic by design: any future `wwwroot/`-targeting CSS asset gets the same treatment

## 🔧 Improvements

- **`docs/RELEASE_NOTES.md`** is now the source of truth that `release.yml` uses as the GitHub Release body — no more drift between the tag description and the file
- **Component count audit** — README, ARCHITECTURE, COMPONENT_ROADMAP, PROJECT_STATUS, COMPARISON, FAQ, QUICKSTART, CLI_SYNTAX, VERSIONING_STRATEGY, and the three package READMEs all now report **68 installable components**, with category breakdowns rederived from `ComponentRegistry` (Form 17, Layout 12, Navigation 7, Overlay 8, Data Display 13, Feedback 9, Utility 2)
- **Preview pipeline restored** — `actions/configure-pages@v4` now passes `enablement: true` so the GitHub Pages site is created on first run instead of erroring with `HttpError: Not Found`

## 🧪 Tests + CI

This release adds three independent layers of regression coverage. The previous alpha shipped with zero of these:

- **`TemplateCompileTests`** (8 tests) — Roslyn `CSharpSyntaxTree.ParseText` over every fixed template's generated content. Pure-C# templates parse whole content; Razor templates extract the `@code` block via a quote/comment-aware brace-balancing tokenizer. When extraction fails (unterminated string), falls back to a class-wrapped parse filtered to literal-related diagnostic IDs (`CS1010`, etc.) so the failure message points at the offending line
- **`TemplateSyncTests`** (3 tests) — for every component that has both a live `.razor` in `src/ShellUI.Components/Components/` and a CLI template, compares the `@code` blocks after normalization (strip comments, blank lines, whitespace). Catches the class of bug that shipped in alpha.3 where someone added a parameter to the live `ThemeToggle.razor` but forgot to mirror it in the template. `AllowedDrift` exception list exists but is empty
- **`InitBootstrapTests`** (8 tests) — direct tests of the App.razor / index.html rewriter. Covers render-mode injection, theme bootstrap placement, script-tag ordering, idempotency, existing-render-mode preservation, and both the modern `@Assets[...]` and bare `<script src="…">` forms
- **`NuGetDepsAndSuggestionsTests`** (15 tests) — Levenshtein matcher, `did-you-mean` exclusions, NuGet metadata, chart-family transitive dependency proofs, namespace convention
- **`ChartStylesTests`** (10 tests) — registry wiring, `FilePath` targets wwwroot, CSS content covers both `.custom-tooltip-*` and `.apexcharts-*` classes with theme variables, link injector + `ResolveHostStylesheetHref` semantics, idempotency
- **CI smoke step** — packs the CLI, installs as a global tool, `dotnet new blazor` → `shellui init` → `shellui add chart pie-chart dashboard-02 data-table` → `dotnet build`. Asserts the auto-injected pieces (`@rendermode`, theme bootstrap, `shellui.js` script, `charts.css` link, NuGet refs, `DataTableModels.cs` presence, full theme in `input.css`) all appear in the produced files. The previous CI did neither tests-on-PR nor end-to-end scaffolding

**Test total:** 53 unit tests (was 0). All pass in Release. Drift canary verified by stashing a fix, observing the precise line-level diff in the test output, and restoring.

## 📦 Installation

```bash
# CLI (prerelease channel — the suffix is intentional)
dotnet tool install -g ShellUI.CLI --version 0.3.0-rc.1

# or upgrade from any alpha
dotnet tool update -g ShellUI.CLI --version 0.3.0-rc.1

shellui init
shellui add button card dialog
```

```bash
# NuGet packages (prerelease channel)
dotnet add package ShellUI.Core --version 0.3.0-rc.1 --prerelease
dotnet add package ShellUI.Components --version 0.3.0-rc.1 --prerelease
```

If `shellui init` runs cleanly and `dotnet build` succeeds on a fresh `dotnet new blazor`, you have everything. There are no manual `<script>` / `<link>` / `@rendermode` patches to make.

## ⏭ What's next

- **`v0.3.0`** stable — same code as rc.1 with the suffix dropped, after a soak window. No further alphas planned for the 0.3 line
- **`v0.4.0-alpha.1`** — .NET 10 upgrade. Begins after 0.3.0 ships. TFM bump, package updates, CI matrix
- **`v0.4.0` and beyond** — generic `DataPicker<TItem, TKey>` (Fix 11 from integration notes), Preview app rewrite, ApexCharts chrome restyle (default legend / axis text Tailwind-styled to match shadcn polish)

See [docs/COMPONENT_ROADMAP.md](COMPONENT_ROADMAP.md) for the longer view.

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.3.0-alpha.3...v0.3.0-rc.1

---

# ShellUI v0.3.0-alpha.3 🚧

> Third alpha of v0.3.0 — dashboard sidebar blocks, JS install fixes, and NuGet Core packaging. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## What's in this release

v0.3.0-alpha.3 adds shadcn-style dashboard layout blocks, completes the sidebar component install chain (including JavaScript interop), and fixes NuGet so `ShellUI.Components` can resolve `ShellUI.Core`.

### ✨ New

- **Dashboard layout blocks** — `dotnet shellui add dashboard-01` (header scrolls, shadcn sidebar-01) and `dashboard-02` (sticky header, shadcn sidebar-02)
- **`shellui-js` template** — installs `wwwroot/shellui.js` at init and as a dependency of `copy-button` / `file-upload`
- **Blocks demo** — `/blocks` page with live previews, install commands, and `CopyButton` per block
- **`DashboardDemoSidebar`** — isolated demo sidebar content (separate from main app sidebar)

### 🐛 Fixes

- **NuGet: `ShellUI.Core` now published** — `ShellUI.Core` is packable; release workflow pushes Core + Components so `dotnet add package ShellUI.Components` restores successfully (was NU1102 when only Core 0.1.0 existed on NuGet)
- **Dependency auto-install** — variant/service templates (`alert-variants`, `badge-variants`, `avatar-variants`, `sonner-variants`, `sonner-service`, `toggle-variants`) marked `IsAvailable = false` so they install only with their parent
- **Breadcrumb** — single `dotnet shellui add breadcrumb`; `breadcrumb-item` auto-installed
- **Blazor error UI** — hidden until an error occurs (`.show` class)
- **Init** — installs `shellui.js` and reminds you to add `<script src="shellui.js"></script>` in App.razor or index.html

### 🔧 Sidebar / JS

| Asset | Installed when | CLI usage |
|-------|----------------|-----------|
| `shellui-sidebar.js` | `sidebar-provider` → `sidebar-js` | Dynamic import in `SidebarProvider` |
| `shellui.js` | `init`, `copy-button`, `file-upload` | Global `ShellUI.*`; requires script tag in HTML |

NuGet consumers: both JS files ship as static web assets under `_content/ShellUI.Components/`.

### 📦 Installation

```bash
# CLI
dotnet tool install -g ShellUI.CLI --version 0.3.0-alpha.3
# or
dotnet tool update -g ShellUI.CLI --version 0.3.0-alpha.3

shellui init
shellui add dashboard-01   # or dashboard-02
```

```bash
# NuGet (requires ShellUI.Core on NuGet at same version)
dotnet add package ShellUI.Core --version 0.3.0-alpha.3 --prerelease
dotnet add package ShellUI.Components --version 0.3.0-alpha.3 --prerelease
```

After `shellui init`, add before the Blazor script:

```html
<script src="shellui.js"></script>
```

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.3.0-alpha.2...v0.3.0-alpha.3

---

# ShellUI v0.3.0-alpha.2 🚧 (Historical)

> Second alpha of v0.3.0 — fixes CLI registry for Drawer/Sheet compositional subcomponents. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## What's in this release

v0.3.0-alpha.2 fixes the CLI template registry so that `Drawer` and `Sheet` correctly ship with their compositional subcomponent pattern introduced in alpha.1.

### 🐛 Fixes (from alpha.1)

- **Drawer/Sheet templates upgraded to v0.3.0** — `Open`/`OpenChanged` replaces `IsOpen`/`IsOpenChanged`, `DrawerSide`/`SheetSide` enums replace string-based side props, explicit `Compositional` parameter enables subcomponent mode
- **Missing subcomponent templates added** — `DrawerTrigger`, `DrawerContent`, `SheetTrigger`, `SheetContent` were missing from the registry entirely
- **Missing variant templates added** — `DrawerVariants` (with `DrawerSide` enum) and `SheetVariants` (with `SheetSide` enum) now installable
- **Dependency direction fixed** — `shellui add drawer` now auto-installs all 4 files (Drawer, DrawerVariants, DrawerTrigger, DrawerContent); same for `shellui add sheet`

### ✨ What changed

| Command | alpha.1 | alpha.2 |
|---|---|---|
| `shellui add drawer` | Installed old v0.2.x Drawer only | Installs Drawer + DrawerVariants + DrawerTrigger + DrawerContent |
| `shellui add sheet` | Installed old v0.2.x Sheet only | Installs Sheet + SheetVariants + SheetTrigger + SheetContent |

### 🔧 Improvements
- **Version bump** — 0.3.0-alpha.1 → 0.3.0-alpha.2
- **Subcomponent dependency pattern** — Follows parent-owns-children convention (same as Dialog, Collapsible, etc.)

## 📦 Installation

```bash
# Install CLI (alpha)
dotnet tool install -g ShellUI.CLI --version 0.3.0-alpha.2

# Or upgrade from alpha.1
dotnet tool update -g ShellUI.CLI --version 0.3.0-alpha.2
```

Initialize and add components:
```bash
shellui init
shellui add drawer       # installs Drawer + DrawerVariants + DrawerTrigger + DrawerContent
shellui add sheet        # installs Sheet + SheetVariants + SheetTrigger + SheetContent
```

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.3.0-alpha.1...v0.3.0-alpha.2

---

# ShellUI v0.3.0-alpha.1 🚧 (Historical)

> First alpha of v0.3.0 — test thoroughly before stable. Report issues via [GitHub Issues](https://github.com/shellui-dev/shellui/issues).

## What's in this release

v0.3.0-alpha.1 includes everything from v0.2.0 (charts, Tailwind 4.x) plus new components and improvements.

### ✨ New Components

**Docs essentials:**
- **Callout** / **CalloutVariants** — Info, warning, tip, danger admonition boxes
- **CopyButton** — One-click copy to clipboard
- **LinkCard** — Card-style links for related pages
- **PrevNextNav** — Previous/Next page navigation for docs

**Feedback & Data:**
- **Sonner** / **SonnerService** / **SonnerVariants** — Modern toast notifications (shadcn-style)
- **Stepper** / **StepperList** / **StepperStep** / **StepperContent** — Step-by-step wizard with value-based API
- **ChartVariants** — Variant styling support for charts


### 🔧 Improvements
- **Version bump** — 0.2.0 → 0.3.0-alpha.1 across all packages
- **CI/CD** — NuGet caching, explicit solution paths, concurrency, pre-release tag support
- **Documentation** — Tailwind setup guide now generic for Blazor, versioning strategy updated for alpha workflow
- **Release workflow** — Tag pattern updated to match `v0.3.0-alpha.1`-style prereleases

### ⚠️ Known issues
- **Stepper** — Active-state highlighting may not always reflect current step; documented, shipping as-is

---

# ShellUI v0.2.0 📊 (Historical)

> Feature release - Charts & Data Visualization

## ✨ New Features

### Charts & Data Visualization (7 new components)
Built on **ApexCharts.Blazor** with full ShellUI theme integration:

- **Chart** - Base chart component with theme-aware styling
- **BarChart** - Vertical bar charts
- **LineChart** - Smooth line charts
- **AreaChart** - Filled area charts
- **PieChart** - Pie charts with custom tooltips
- **MultiSeriesChart** - Multiple data series on one chart
- **ChartSeries** - Flexible series composition

### Chart Themes
Three built-in color themes via `ChartTheme` enum:
- **Default** - Professional blue-based palette (blue, red, green, yellow, purple)
- **Colorful** - Vibrant multi-color palette with 7 colors
- **Monochrome** - Slate grays for minimal aesthetic

### Theme-Aware Chart Containers
Charts automatically use your theme's CSS variables:
- `var(--radius)` for border radius
- `var(--shadow)` for box shadow
- `var(--border)`, `var(--card)`, `var(--card-foreground)` for colors

### Custom Tooltips
Fully custom HTML tooltips replacing ApexCharts defaults:
- Compact, shadcn-inspired design
- Proper marker/text alignment via flexbox
- Multi-series support (shows all values at a data point)
- Light/dark mode support via CSS variables
- Separate pie chart tooltip using `seriesIndex`

## 🐛 Bug Fixes

- **Fixed version mismatch** - Component versions now correctly read from assembly metadata instead of hardcoded fallback
- **Fixed Tailwind version in config** - `shellui.json` now correctly shows Tailwind v4.1.18 for all install methods
- **Fixed CS1998 warnings** - Removed unnecessary `async` from synchronous methods in `ComponentInstaller`

## 🔧 Improvements

- **Tailwind CSS updated to v4.1.18**
- **Component count: 100** installable (dependencies like *-variants auto-installed)
- **X-axis labels** - Charts use `XAxisType.Category` for proper string label display
- **Version system** - Fallback version now reads `AssemblyInformationalVersion` baked at build time

## 📦 Installation

```bash
# Install CLI globally
dotnet tool install -g ShellUI.CLI

# Initialize your project
shellui init

# Add chart components
shellui add chart bar-chart line-chart pie-chart area-chart multi-series-chart
```

Or via NuGet:
```bash
dotnet add package ShellUI.Components
```

**Note:** Charts require the `Blazor-ApexCharts` NuGet package:
```bash
dotnet add package Blazor-ApexCharts
```

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

---

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.1.1...v0.2.0
