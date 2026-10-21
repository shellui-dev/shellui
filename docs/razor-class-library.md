# Using ShellUI from a Razor class library

Put ShellUI components in a Razor class library when several apps share one UI: for example a Blazor Web App and a .NET MAUI Blazor Hybrid app with the same pages. The library owns the components, the theme and the generated CSS; each app references the library and links its assets.

```text
MyApp.UI/        Razor class library: ShellUI components, pages, wwwroot/input.css and app.css
MyApp.Web/       Blazor Web App      → references MyApp.UI
MyApp.Mobile/    MAUI Blazor Hybrid  → references MyApp.UI
```

## 1. Initialise ShellUI in the library

Run the CLI in the library's folder, the one with its `.csproj`:

```bash
cd MyApp.UI
shellui init --yes --tailwind standalone
shellui add button card dialog
```

`init` recognises a Razor class library (`Microsoft.NET.Sdk.Razor` with no `Program.cs`) and prints the steps below for each app. Components go into `MyApp.UI/Components/UI`, and the library's build compiles `wwwroot/input.css` into `wwwroot/app.css`.

The apps reference the library as usual:

```xml
<ProjectReference Include="..\MyApp.UI\MyApp.UI.csproj" />
```

The library's `wwwroot` is served to each app under `_content/<LibraryName>/`, where the name is the library's `PackageId` or assembly name (`MyApp.UI` here), unless it sets `StaticWebAssetBasePath`. The examples below use `_content/MyApp.UI`.

## 2. Link the CSS and JavaScript in each app

Link the library's files from each app's host page, and load `shellui.js` **before** the Blazor script. Don't copy `app.css` into the app's own `wwwroot`: the app would keep a stale copy when the library's components change.

**Blazor Web App** (`MyApp.Web/Components/App.razor`):

```razor
<head>
    ...
    <link rel="stylesheet" href="_content/MyApp.UI/app.css" />
    <HeadOutlet @rendermode="InteractiveServer" />
</head>
<body>
    <Routes @rendermode="InteractiveServer" />
    <script src="_content/MyApp.UI/shellui.js"></script>
    <script src="_framework/blazor.web.js"></script>
</body>
```

**MAUI Blazor Hybrid** (`MyApp.Mobile/wwwroot/index.html`):

```html
<head>
    ...
    <link rel="stylesheet" href="_content/MyApp.UI/app.css" />
</head>
<body>
    <div id="app">Loading...</div>
    <script src="_content/MyApp.UI/shellui.js"></script>
    <script src="_framework/blazor.webview.js" autostart="false"></script>
</body>
```

## 3. Add the theme script to each app

`ThemeToggle` stores the choice in `localStorage` and sets the `dark` class on `<html>`, but only after the first render. In a normal app, `init` puts this script in the host page's `<head>` so the right theme applies before the first paint. A library has no host page, so add it to each app yourself:

```html
<script>
    // ShellUI theme bootstrap — runs before Blazor mounts to avoid a light-flash on dark pages.
    if (!localStorage.getItem('theme')) { localStorage.setItem('theme', 'light'); }
    if (localStorage.getItem('theme') === 'dark') {
        document.documentElement.classList.add('dark');
    }
</script>
```

Without it, a user who picked dark mode sees a flash of the light theme on every load, and the toggle starts out of step.

## 4. Classes used in an app's own markup

Tailwind generates CSS only for classes it finds in the files it scans, and the library's build scans the library. Pages and components that live in the library are covered. If an app has its own markup (a page or layout only one app has), point Tailwind at it from the library's `wwwroot/input.css`. Paths are relative to `input.css`:

```css
@import "tailwindcss";
@source "../../MyApp.Web/Components";
@source "../../MyApp.Mobile/Components";
```

Rebuild the library after adding a `@source` line.

## 5. Apps that keep state in browser storage

Apps that read `ProtectedLocalStorage`, `ProtectedSessionStorage` or `localStorage` during startup (for example to restore a signed-in session) can't do it while prerendering, because the page renders on the server before the browser is connected. Render those apps without prerendering:

```razor
<Routes @rendermode="new InteractiveServerRenderMode(prerender: false)" />
```

MAUI Blazor Hybrid apps don't prerender, so this applies only to the web app.

## Checklist

- [ ] `shellui init` ran in the library, and components are in `MyApp.UI/Components/UI`
- [ ] Each app references the library and links `_content/<LibraryName>/app.css` and `shellui.js` (before the Blazor script)
- [ ] Each app's host page has the theme script in `<head>`
- [ ] `@source` lines for markup that lives in an app rather than the library
- [ ] Web apps that restore state from browser storage use `prerender: false`
- [ ] `dotnet build` the library after changing components or `input.css`, then run each app and toggle dark mode
