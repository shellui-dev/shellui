# ShellUI Components

Blazor components styled with Tailwind CSS, in the spirit of shadcn/ui, as a Razor class library.

Prefer to own and edit the component source? Use the [ShellUI CLI](https://www.nuget.org/packages/ShellUI.CLI) instead; it copies the components into your project.

## Install

Requires a .NET 10 project:

```bash
dotnet add package ShellUI.Components
```

On .NET 8 or 9, use the [ShellUI CLI](https://www.nuget.org/packages/ShellUI.CLI) instead. The source it installs builds on .NET 8, 9 and 10.

## Set up

1. Import the namespace in `Components/_Imports.razor`:

   ```razor
   @using ShellUI.Components
   ```

2. Make the app interactive. Components with state or events (dialogs, dropdowns, tabs) need an interactive render mode, for example in `Components/App.razor`:

   ```razor
   <HeadOutlet @rendermode="InteractiveServer" />
   ...
   <Routes @rendermode="InteractiveServer" />
   ```

3. Add the styles, in one of two ways:

   - **Precompiled stylesheet.** No Tailwind build needed. Add it to the `<head>` in `App.razor`:

     ```html
     <link href="_content/ShellUI.Components/shellui-all.css" rel="stylesheet" />
     ```

   - **Your own Tailwind build.** On build, the package writes the class names it uses to `wwwroot/shellui-classes.txt`. Add it as a source in your input stylesheet, and copy the theme variables from the package's `shellui-theme.css` into the same file:

     ```css
     @import "tailwindcss";
     @source "./shellui-classes.txt";
     ```

     ShellUI uses Tailwind CSS `4.3.2`.

## Use

```razor
<Button Variant="ButtonVariant.Outline">Cancel</Button>
<Button>Save</Button>

<Card>
    <CardHeader>
        <CardTitle>ShellUI</CardTitle>
    </CardHeader>
    <CardContent>
        <p>Card content</p>
    </CardContent>
</Card>
```

## Themes

Themes come from [tweakcn](https://tweakcn.com). With the CLI, write a theme as a stylesheet and load it after `shellui-all.css`:

```bash
shellui theme apply https://tweakcn.com/themes/THEME_ID --emit-override wwwroot/theme.css
```

```html
<link href="theme.css" rel="stylesheet" />
```

## Documentation

- [README](https://github.com/shellui-dev/shellui/blob/v0.3.3/README.md)
- [Tailwind setup](https://github.com/shellui-dev/shellui/blob/v0.3.3/docs/tailwind-setup.md)
- [Release notes](https://github.com/shellui-dev/shellui/blob/v0.3.3/docs/RELEASE_NOTES.md)

## License

[MIT](https://github.com/shellui-dev/shellui/blob/v0.3.3/LICENSE.txt)
