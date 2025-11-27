# ShellUI Templates

Component templates for ShellUI CLI tool. This package contains the template definitions used by the ShellUI CLI when installing components.

## Overview

This package is an internal dependency of `ShellUI.CLI`. It provides the component templates that are copied to your project when you run `dotnet shellui add <component>`.

## Installation

This package is automatically installed as a dependency when you install `ShellUI.CLI`:

```bash
dotnet tool install -g ShellUI.CLI
```

You typically don't need to install this package directly.

## Usage

When you use the ShellUI CLI to add components, this package provides the templates:

```bash
# The CLI uses templates from this package
dotnet shellui add button
dotnet shellui add card dialog
```

## What's Included

This package contains:
- **Component Templates** - Razor component templates for all ShellUI components
- **Metadata** - Component metadata including dependencies, categories, and descriptions
- **Registry** - Component registry for CLI discovery

## For Developers

If you're developing ShellUI or creating custom components:

```bash
dotnet add package ShellUI.Templates
```

Then access templates programmatically:

```csharp
using ShellUI.Templates;

var buttonTemplate = ButtonTemplate.Content;
var metadata = ButtonTemplate.Metadata;
```

## Related Packages

- **ShellUI.CLI** - Command-line tool that uses these templates
- **ShellUI.Components** - Pre-built component library (alternative to CLI)
- **ShellUI.Core** - Core models and utilities

## Documentation

- [ShellUI Documentation](https://shellui.dev)
- [CLI Usage Guide](https://shellui.dev/docs/cli)
- [Component Development](https://shellui.dev/docs/components)

## License

MIT License - see [LICENSE](https://github.com/shelltechlabs/shellui/blob/main/LICENSE) for details.

