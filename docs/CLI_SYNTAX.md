# ShellUI CLI Syntax

This reference covers CLI `0.4.0-alpha.3` (prerelease): it runs on .NET 8 or later (`0.4.0-alpha.3` itself needs .NET 10), sets up Blazor projects on .NET 8, 9 and 10, uses Tailwind CSS `4.3.2`, and has 98 direct component targets. Some options, such as `--dashboard`, are not in the stable 0.3.4; see the [v0.3.4 reference](https://github.com/shellui-dev/shellui/blob/v0.3.4/docs/CLI_SYNTAX.md) for that version.

## Command prefix

A global tool is invoked as `shellui`. After creating a .NET tool manifest, a local tool is invoked as `dotnet shellui`.

The examples below use a global tool. Replace `shellui` with `dotnet shellui` when using a local tool manifest.

```bash
shellui --help
shellui --version
```

## Command overview

```text
shellui init [--force] [--style <style>] [--tailwind standalone|npm] [--yes] [--dashboard 01|02|none] [--replace-layout]
shellui add <name...>|all [--force] [--replace-layout] [--except <name...>]
shellui list [--installed|--available]
shellui remove <name...>
shellui update [name...] [--all] [--force]
shellui doctor
shellui migrate [name...] [--dry-run] [--yes]
shellui theme init <url-or-id> [--force] [--style <style>] [--tailwind standalone|npm] [--yes] [--dashboard 01|02|none] [--replace-layout]
shellui theme apply <url-or-id> [--emit-override <path>]
shellui theme update
```

`sidebar-js` is retained as a hidden legacy component for existing generated providers. New sidebar and provider templates depend on `shellui-js` instead.

## `init`

Initializes ShellUI in the current Blazor project.

```bash
shellui init
shellui init --force
shellui init --style new-york
shellui init --tailwind npm --yes
shellui init --yes --dashboard 02
```

Options:

- `--force` reinitializes a project that already has `shellui.json`.
- `--style <style>` selects `default`, `new-york`, or `minimal`.
- `--tailwind standalone|npm` selects the Tailwind setup method and skips the prompt. With `--yes` and no `--tailwind`, `init` uses standalone.
- `--yes` runs without prompts and uses the selected defaults. Without an explicit method, the default is `standalone`.
- `--dashboard 01|02|none` sets up a dashboard layout: `02` has a sticky header, `01` a scrolling one. Without the option, `init` asks; with `--yes` alone, no dashboard is added.
- `--replace-layout` makes the dashboard the default layout even when the app already uses a custom layout. See [Dashboard layouts](#dashboard-layouts).

Initialization creates or updates:

- `Components/UI/`
- `wwwroot/input.css` and `wwwroot/app.css`
- `tailwind.config.js` at the project root
- `Build/ShellUI.targets`
- `shellui.json`
- `Components/_Imports.razor` when it already exists
- the host file when applicable

`Components/Layout/` is created when a layout target is installed.

`init` removes the template's local Bootstrap copy (`wwwroot/lib/bootstrap` or, on .NET 8, `wwwroot/bootstrap`) and its `<link>` in `App.razor`; Bootstrap loaded from a CDN is left alone. The sample pages (`Home`, `Counter`, `Weather`, `Error`, `NotFound`, `Auth`) are restyled with Tailwind classes when they are unchanged from `dotnet new blazor`. Pages you have edited are kept, and `init` lists any that still use Bootstrap classes.

In apps created with `--auth Individual`, `init` renders the `/Account` pages statically while the rest of the app stays interactive, because those pages set sign-in cookies and need a real HTTP request. It leaves their Bootstrap markup alone and points to the [auth layouts](#auth-layouts) instead.

With `--dashboard`, `init` then runs `shellui add dashboard-02` (or `dashboard-01`), including the layout wiring described under [Dashboard layouts](#dashboard-layouts).

Standalone mode downloads the Tailwind executable once per machine into `~/.shellui/bin` (set `SHELLUI_CACHE_DIR` to move it), and every project builds with that copy; a copy in a project's `.shellui/bin/` takes precedence. If the download fails, `init` still completes and the build retries it. npm mode installs `tailwindcss@^4.3.2` and `@tailwindcss/cli@^4.3.2` and requires Node.js and npm. The current CLI invokes npm through `cmd`; use standalone mode on non-Windows systems or run npm manually.

## `add`

Copies one or more component targets and their source dependencies into the project.

```bash
shellui add button
shellui add button card dialog
shellui add button,card,dialog
shellui add button,card dialog
shellui add button --force
shellui add all
shellui add all --except chart,qr-code
```

`add` accepts space-separated names, comma-separated names, and a mixture of both. Use the exact target names printed by `shellui list`.

`--force` overwrites an existing component file. Dependencies are installed automatically; there is no separate dependency option.

`shellui add all` installs the whole component library: every target `shellui list` shows except the layout blocks (`dashboard-01`, `dashboard-02`, `auth-01`, `auth-02`, `auth-03`), which rewire the app and are added by name. Components already installed are skipped. `--except` leaves components out (space or comma-separated), along with anything that needs them: `--except chart` also leaves out `bar-chart`, `line-chart` and the other charts, so `Blazor-ApexCharts` isn't added.

Some components need a NuGet package (`chart` needs `Blazor-ApexCharts`, `data-table` needs `System.Linq.Dynamic.Core`, `qr-code` needs `QRCoder`). `add` references it if the project doesn't, and raises an older version to the one ShellUI needs. A version that is already the same or newer is left as it is (including one set in `Directory.Packages.props`), as are floating versions such as `7.*`, ranges, and references whose version ShellUI can't find.

### Dashboard layouts

`shellui add dashboard-01` and `shellui add dashboard-02`, and `init --dashboard`, also wire the layout into the app:

- `Routes.razor` gets `DefaultLayout="typeof(Layout.DashboardLayout02)"`. Switching between the two dashboards is automatic.
- If the app uses a custom layout, or a `MainLayout` that was modified, the CLI asks before switching. Without a terminal it leaves the layout alone and prints a hint; pass `--replace-layout` to switch anyway.
- The stock `MainLayout` and `NavMenu` files (and their `.razor.css`) are deleted only when they are byte-identical to `dotnet new blazor` output for .NET 8, 9 or 10 and nothing else references them. Modified files are kept with a warning.
- Pages that pin `@layout MainLayout`, such as the .NET 10 `NotFound` page, move to the dashboard layout.
- The Blazor error bar (`blazor-error-ui`) moves from `MainLayout` into `App.razor`.
- `AppSidebar` links are built from the app's `@page` routes. Parameterized routes, `/Account/*`, `/Error` and `/not-found` are skipped. Links you have edited are left alone.
- In apps that use ASP.NET Core Identity, the sidebar footer shows Log in and Register, or the signed-in user (linking to the account page) and Log out. This adds the hidden `sidebar-account` component and replaces the placeholder user in the footer; a footer you have edited is left alone.
- `ReconnectModal` and pages are not changed otherwise.

Running the command again is safe. It prints a summary of what changed.

### Auth layouts

For apps that use ASP.NET Core Identity (`dotnet new blazor --auth Individual`), three blocks style the sign-in, sign-up and account recovery pages:

| Block | Design |
|---|---|
| `auth-01` | Centered card on a muted background |
| `auth-02` | Split screen: the form beside a brand panel |
| `auth-03` | Minimal centered form |

```bash
shellui add auth-01
```

Adding one:

- Sets `@layout` in `Components/Account/Pages/_Imports.razor`, so those pages render without the dashboard sidebar. The account management pages under `Account/Manage` keep the app's layout. Adding another auth block switches to it.
- Restyles the Identity pages that are unchanged from `dotnet new blazor` for .NET 8, 9 or 10 with ShellUI's button, input and alert styles. Pages you have edited are kept and listed.

The layouts show a placeholder logo and "My App"; edit the installed `AuthLayout0x.razor` to brand them.

## `list`

Lists the direct targets and their installation status.

```bash
shellui list
shellui list --installed
shellui list --available
```

There are 98 direct targets. Registry entries used only as dependencies are not counted as direct targets. Choose either `--installed` or `--available` to filter the output.

## `remove`

Removes the named component files and their entries from `shellui.json`.

```bash
shellui remove button
shellui remove button card dialog
```

Pass names separated by spaces. `remove` has no force flag or other options, does not prompt, and does not perform reverse-dependency cleanup. If another component still references a removed file, update that usage or remove the dependent component yourself. Layout blocks such as `dashboard-02` and `auth-01` are removed from `Components/Layout`, and `remove` lists any files that still use the layout (usually `Routes.razor`) so you can point them at another one.

## `update`

Replaces installed component source with the current template source.

```bash
shellui update button
shellui update button card
shellui update --all
shellui update
shellui update card --force
```

Names are separated by spaces. With no names, or with `--all`, every installed component is updated.

- **Your changes are kept.** `shellui.json` records a hash of each file as the CLI wrote it. If a file has changed since, `update` skips it and says so; `--force` overwrites it. Files installed by a CLI older than 0.4.0-alpha.2 have no hash, so for those `update` keeps the blocks apps are meant to edit (`app-sidebar`, `sidebar-account`, the `dashboard-*` and `auth-*` layouts) when they differ from the template, and rewrites everything else.
- **New dependencies are installed.** When a component gained dependencies since you installed it (for example `tabs` now needs `tab-models`), `update` adds them, with their NuGet packages and `_Imports.razor` usings, the same way `add` does.

`update` has no diff or merge. Commit before updating so you can review the changes. When a template's API changed, run `shellui migrate` afterwards to update your pages.

## `doctor`

Checks the project's ShellUI setup and prints a fix for each problem it finds:

```bash
shellui doctor
```

- `shellui.json` is valid and `ComponentsPath` exists.
- The Tailwind CLI is available (the shared copy in `~/.shellui/bin`, a project copy, or the npm packages), `Build/ShellUI.targets` exists, and `wwwroot/app.css` is newer than `input.css` and the components.
- The host page (`Components/App.razor` or `wwwroot/index.html`) links `app.css`, loads `shellui.js` and has the theme script.
- An interactive render mode is set in `App.razor`. When only some pages set one, components on the other pages don't respond to clicks.
- Every installed component's file exists and its dependencies are installed.
- `_Imports.razor` has the ShellUI `@using` lines.
- The project doesn't use the `ShellUI.Components` package and CLI-installed components together.

It exits with code 1 when it finds a problem, so it can run in CI. Warnings, such as a stale `app.css`, don't change the exit code. In a Razor class library it skips the host page checks, which apply to each app that uses the library.

## `migrate`

Rewrites your own pages for template API changes listed in the release notes, after `shellui update` has replaced the components:

```bash
shellui migrate --dry-run
shellui migrate
shellui migrate date-picker pagination
shellui migrate --yes
```

It reads the project's `.razor` and `.cs` files (not the installed components under `ComponentsPath`, or `bin`, `obj` and `node_modules`), shows each changed line, and asks before writing. `--dry-run` only shows the changes; `--yes` writes them without asking, and is required when the terminal isn't interactive. With no names it runs every migration.

| Name | Change |
|---|---|
| `input` | `Input.InputVariant.Error` → `InputVariant.Error` (0.4.0-alpha.3) |
| `date-picker` | `SelectedDate`, `SelectedDateChanged` and `@bind-SelectedDate` → `Value`, `ValueChanged` and `@bind-Value`, on `<DatePicker>` only (0.4.0-alpha.3) |
| `time-picker` | the same for `<TimePicker>`'s `SelectedTime` (0.4.0-alpha.3) |
| `pagination` | `OnPageChange` → `CurrentPageChanged`, except on a tag that already uses `@bind-CurrentPage` (0.4.0-alpha.3) |
| `calendar` | Reports handlers of `Calendar.SelectedDateChanged` that take a `DateTime`; they need a `DateTime?` since 0.4.0-alpha.1. Nothing is rewritten. |

Commit first, then build the app afterwards.

## `theme`

The theme commands fetch a public tweakcn theme by URL or theme ID and write its CSS variables into the project.

### `theme init`

Initializes a project and applies a theme in one step.

```bash
shellui theme init <url-or-id>
shellui theme init <url-or-id> --force --style default --tailwind standalone --yes
```

`--style` accepts `default`, `new-york`, or `minimal`. The command uses the same initialization options as `init`.

### `theme apply`

Applies a theme to `wwwroot/input.css` by default.

```bash
shellui theme apply <url-or-id>
shellui theme apply <url-or-id> --emit-override wwwroot/theme.css
```

With `--emit-override`, the command writes a separate CSS file instead of modifying the input stylesheet. Link that file after the ShellUI stylesheet.

### `theme update`

Re-fetches the source recorded in `shellui.theme.lock`.

```bash
shellui theme update
```

Run it from the project root after a theme has been applied.

## Tailwind files

| Path | Purpose |
|---|---|
| `wwwroot/input.css` | Tailwind v4 input and theme variables |
| `wwwroot/app.css` | Generated Tailwind output |
| `tailwind.config.js` | Project content globs and dark-mode setting |
| `~/.shellui/bin/tailwindcss-<version>-<platform>` | Standalone CLI, shared by every project on the machine |
| `.shellui/.gitignore` | Keeps `.shellui/` out of git |
| `.shellui/bin/tailwindcss(.exe)` | Optional per-project CLI, used instead of the shared one |
| `Build/ShellUI.targets` | MSBuild integration |
| `shellui.json` | ShellUI project configuration |
| `shellui.theme.lock` | Theme source and integrity record |

The npm v4 command is:

```bash
npm install -D tailwindcss@^4.3.2 @tailwindcss/cli@^4.3.2
npx @tailwindcss/cli -i ./wwwroot/input.css -o ./wwwroot/app.css --watch
```

The generated `wwwroot/input.css` uses the Tailwind v4 import:

```css
@import "tailwindcss";
```

## Configuration fields

`ShellUIConfig` is serialized with these property names:

| Property | Meaning |
|---|---|
| `Schema` | Schema identifier |
| `Style` | `default`, `new-york`, or `minimal` |
| `ComponentsPath` | Usually `Components/UI` |
| `LayoutPath` | Usually `Components/Layout` |
| `Tailwind` | Tailwind settings |
| `InstalledComponents` | Installed component records |
| `ProjectType` | Detected Blazor project type |

Representative fields look like this:

```json
{
  "Style": "default",
  "ComponentsPath": "Components/UI",
  "LayoutPath": "Components/Layout",
  "Tailwind": {
    "Enabled": true,
    "Version": "4.3.2",
    "Method": "standalone",
    "ConfigPath": "tailwind.config.js",
    "CssPath": "wwwroot/app.css"
  },
  "InstalledComponents": [
    {
      "Name": "button",
      "Version": "0.4.0-alpha.3",
      "InstalledAt": "2026-01-01T00:00:00Z",
      "IsCustomized": false
    }
  ],
  "ProjectType": 1
}
```

`ProjectType` is serialized as the enum value; `1` is `BlazorServer`. `IsCustomized` is metadata, not a merge mechanism. `update` still overwrites the file.

## Common workflow

```bash
shellui init --yes --tailwind standalone
shellui add button,input,card
dotnet build
```

For a theme-aware setup:

```bash
shellui theme init <url-or-id> --yes --tailwind standalone
shellui add button,card
```

## Troubleshooting

- Run `shellui doctor`; it checks the setup and says how to fix what it finds.
- If the command is not found, use `shellui` for a global tool or `dotnet shellui` for a local manifest.
- Run `shellui list` to verify a target name.
- Confirm `wwwroot/input.css` exists and contains `@import "tailwindcss";`.
- Run `dotnet build` to invoke the generated Tailwind build target.
- In standalone mode, check `~/.shellui/bin`; in npm mode, run `npm install`.

## Related documentation

- [CLI installation](CLI_INSTALLATION.md)
- [Quick start](QUICKSTART.md)
- [FAQ](FAQ.md)
- [GitHub repository](https://github.com/shellui-dev/shellui)
- [CLI package](https://www.nuget.org/packages/ShellUI.CLI)
- [Component package](https://www.nuget.org/packages/ShellUI.Components)
