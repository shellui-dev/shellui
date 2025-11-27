# ShellUI Quick Start Guide

**Note:** ShellUI is currently in development. This guide shows what the experience will be like when v1.0 is released.

## What is ShellUI?

ShellUI is a CLI-first Blazor component library inspired by shadcn/ui. Instead of installing a NuGet package, you use a CLI tool to copy components directly into your project, giving you full control and customization capabilities.

## Prerequisites

- .NET 8.0 SDK or higher
- Node.js 18+ (for Tailwind CSS)
- A Blazor project (Server, WASM, or SSR)

## Installation

### Step 1: Install the CLI globally

```bash
dotnet tool install -g ShellUI.CLI
```

### Step 2: Initialize ShellUI in your project

Navigate to your Blazor project directory:

```bash
cd YourBlazorProject
dotnet shellui init
```

This will:
- Detect your project type (Server/WASM/SSR)
- Create `Components/UI/` folder structure
- Set up Tailwind CSS v4
- Create `shellui.json` configuration
- Update necessary files

You'll see output like:
```
✓ Detected project type: Blazor Server
✓ Created Components/UI/ directory
✓ Initialized Tailwind CSS v4
✓ Created shellui.json configuration
✓ Updated _Imports.razor

ShellUI is ready! Add your first component:
  dotnet shellui add button
```

## Adding Components

### Add a single component

```bash
dotnet shellui add button
```

This copies the Button component to `Components/UI/Button.razor` in your project.

### Add multiple components

```bash
# Space-separated
dotnet shellui add button card alert dialog

# Comma-separated
dotnet shellui add button,card,alert,dialog

# Mix both (why not!)
dotnet shellui add button,card alert dialog

# Add many at once
dotnet shellui add button,input,label,card,alert,badge,skeleton,separator
```

### Add with dependencies

ShellUI automatically resolves and installs dependencies:

```bash
dotnet shellui add dialog
# Also installs: button (dependency)
```

## Using Components

After adding components, use them in your Razor pages:

```razor
@page "/example"

<div class="container mx-auto p-4">
    <Card>
        <CardHeader>
            <CardTitle>Welcome to ShellUI</CardTitle>
            <CardDescription>
                Build beautiful Blazor apps with ease
            </CardDescription>
        </CardHeader>
        <CardContent>
            <p class="mb-4">ShellUI provides production-ready components that you own.</p>
            <Input Placeholder="Enter your email" Type="email" />
        </CardContent>
        <CardFooter>
            <Button OnClick="HandleSubscribe">Subscribe</Button>
        </CardFooter>
    </Card>
</div>

@code {
    private void HandleSubscribe()
    {
        // Your logic here
    }
}
```

## Customizing Components

Since components are copied to your project, you can customize them freely:

1. Open `Components/UI/Button.razor`
2. Modify the Tailwind classes
3. Add new variants
4. Change behavior
5. It's YOUR code!

Example customization:

```razor
@* Components/UI/Button.razor *@

@code {
    // Add a new "neon" variant
    private string BuildCssClass()
    {
        var classes = new List<string> { /* ... base classes ... */ };
        
        classes.Add(Variant switch
        {
            "default" => "bg-primary text-primary-foreground",
            "neon" => "bg-gradient-to-r from-pink-500 to-purple-500 text-white shadow-neon",
            // ... other variants
            _ => "bg-primary text-primary-foreground"
        });
        
        return string.Join(" ", classes);
    }
}
```

Then use it:

```razor
<Button Variant="neon">Neon Button</Button>
```

## Managing Components

### List all available components

```bash
dotnet shellui list
```

Output:
```
Component      Status       Version    Description
───────────────────────────────────────────────────────────────
button         installed    1.0.0      Interactive button
card           installed    1.0.0      Content container
alert          available    1.0.0      Alert messages
dialog         available    1.0.0      Modal dialogs
input          available    1.0.0      Text input field
...
```

### List only installed components

```bash
dotnet shellui list --installed
```

### Update components

```bash
# Update a specific component
dotnet shellui update button

# Update all components
dotnet shellui update --all
```

Note: If you've customized a component, you'll be warned before updating.

### Show differences

```bash
dotnet shellui diff button
```

Shows what's different between your version and the latest version.

### Remove components

```bash
dotnet shellui remove button
```

Removes the component file. You'll be warned if other components depend on it.

## Theming

ShellUI uses Tailwind CSS v4 with CSS variables for theming.

### Customize colors

Edit `wwwroot/styles/input.css`:

```css
@layer base {
  :root {
    --primary: 262 83% 58%;        /* Change primary color */
    --primary-foreground: 0 0% 100%;
    /* ... other colors ... */
  }

  .dark {
    --primary: 263 70% 50%;        /* Dark mode primary */
    --primary-foreground: 0 0% 100%;
    /* ... other colors ... */
  }
}
```

### Dark mode

ShellUI includes automatic dark mode support. Toggle via:

```razor
@inject IThemeService Theme

<Button OnClick="() => Theme.ToggleThemeAsync()">
    Toggle Theme
</Button>
```

Or manually set:

```razor
await Theme.SetThemeAsync("dark");
await Theme.SetThemeAsync("light");
await Theme.SetThemeAsync("system"); // Follow system preference
```

## Project Types

### Blazor Server

```bash
dotnet new blazor -o MyApp
cd MyApp
dotnet shellui init
dotnet shellui add button card
dotnet run
```

### Blazor WebAssembly

```bash
dotnet new blazorwasm -o MyApp
cd MyApp
dotnet shellui init
dotnet shellui add button card
dotnet run
```

### Blazor SSR (Server-Side Rendering)

```bash
dotnet new blazor -o MyApp --interactivity None
cd MyApp
dotnet shellui init
dotnet shellui add button card
dotnet run
```

## Common Patterns

### Form with validation

```razor
@page "/register"

<Card class="max-w-md mx-auto">
    <CardHeader>
        <CardTitle>Create Account</CardTitle>
    </CardHeader>
    <CardContent>
        <EditForm Model="@model" OnValidSubmit="HandleRegister">
            <DataAnnotationsValidator />
            
            <div class="space-y-4">
                <div>
                    <Label For="email">Email</Label>
                    <Input @bind-Value="model.Email" Type="email" Id="email" />
                    <ValidationMessage For="() => model.Email" />
                </div>
                
                <div>
                    <Label For="password">Password</Label>
                    <Input @bind-Value="model.Password" Type="password" Id="password" />
                    <ValidationMessage For="() => model.Password" />
                </div>
                
                <Button Type="submit" IsLoading="@isSubmitting">
                    Register
                </Button>
            </div>
        </EditForm>
    </CardContent>
</Card>

@code {
    private RegisterModel model = new();
    private bool isSubmitting;
    
    private async Task HandleRegister()
    {
        isSubmitting = true;
        try
        {
            // Your registration logic
            await Task.Delay(1000); // Simulate API call
        }
        finally
        {
            isSubmitting = false;
        }
    }
    
    public class RegisterModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";
        
        [Required, MinLength(8)]
        public string Password { get; set; } = "";
    }
}
```

### Dialog/Modal

```razor
<Button OnClick="() => showDialog = true">Open Dialog</Button>

<Dialog Open="@showDialog" OnOpenChange="(open) => showDialog = open">
    <DialogContent>
        <DialogHeader>
            <DialogTitle>Are you sure?</DialogTitle>
            <DialogDescription>
                This action cannot be undone.
            </DialogDescription>
        </DialogHeader>
        <DialogFooter>
            <Button Variant="outline" OnClick="() => showDialog = false">
                Cancel
            </Button>
            <Button Variant="destructive" OnClick="HandleConfirm">
                Confirm
            </Button>
        </DialogFooter>
    </DialogContent>
</Dialog>

@code {
    private bool showDialog;
    
    private void HandleConfirm()
    {
        // Your logic
        showDialog = false;
    }
}
```

### Data table with actions

```razor
<Card>
    <CardHeader>
        <CardTitle>Users</CardTitle>
    </CardHeader>
    <CardContent>
        <Table>
            <TableHeader>
                <TableRow>
                    <TableHead>Name</TableHead>
                    <TableHead>Email</TableHead>
                    <TableHead>Role</TableHead>
                    <TableHead>Actions</TableHead>
                </TableRow>
            </TableHeader>
            <TableBody>
                @foreach (var user in users)
                {
                    <TableRow>
                        <TableCell>@user.Name</TableCell>
                        <TableCell>@user.Email</TableCell>
                        <TableCell>
                            <Badge>@user.Role</Badge>
                        </TableCell>
                        <TableCell>
                            <DropdownMenu>
                                <DropdownMenuTrigger>
                                    <Button Variant="ghost" Size="sm">•••</Button>
                                </DropdownMenuTrigger>
                                <DropdownMenuContent>
                                    <DropdownMenuItem OnClick="() => EditUser(user)">
                                        Edit
                                    </DropdownMenuItem>
                                    <DropdownMenuItem OnClick="() => DeleteUser(user)">
                                        Delete
                                    </DropdownMenuItem>
                                </DropdownMenuContent>
                            </DropdownMenu>
                        </TableCell>
                    </TableRow>
                }
            </TableBody>
        </Table>
    </CardContent>
</Card>
```

## Tips & Best Practices

### 1. Start with essential components
```bash
dotnet shellui add button input label card alert
```

### 2. Use composition
Build complex UIs by composing simple components together.

### 3. Customize freely
Don't be afraid to modify components - they're yours!

### 4. Keep Tailwind running in watch mode
During development:
```bash
npm run css:watch
```

### 5. Use variants consistently
Stick to the default variants (default, outline, ghost, etc.) for consistency.

### 6. Follow accessibility guidelines
Components are accessible by default - keep it that way when customizing.

### 7. Test across browsers
Especially if you've customized components significantly.

### 8. Update regularly
```bash
dotnet shellui update --all
```

## Troubleshooting

### Tailwind CSS not updating

```bash
# Rebuild Tailwind CSS
npm run css:build

# Or in watch mode
npm run css:watch
```

### Component not found

```bash
# Make sure you're in the project directory
cd YourBlazorProject

# List available components
dotnet shellui list
```

### Import errors

Make sure `_Imports.razor` includes:
```razor
@using YourProject.Components.UI
```

### Dark mode not working

1. Check `wwwroot/styles/input.css` has dark mode variables
2. Ensure theme toggle component is implemented
3. Verify `<html>` or `<body>` tag gets `dark` class applied

## Getting Help

- Documentation: https://shellui.dev/docs (Coming soon)
- GitHub Issues: https://github.com/yourorg/shellui/issues
- Discussions: https://github.com/yourorg/shellui/discussions

## Next Steps

- Explore all available components: `dotnet shellui list`
- Check out examples: https://shellui.dev/examples (Coming soon)
- Join the community: Discord link (Coming soon)
- Star the repo: https://github.com/yourorg/shellui

---

**Remember:** ShellUI is currently in development. This guide represents the planned developer experience for v1.0. Star the repo to follow progress!

