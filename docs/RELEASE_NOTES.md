# ShellUI v0.1.1 🔧

> Hotfix release - Package publishing fix

## 🐛 Bug Fixes

### Fixed Package Publishing
- **Prevented `ShellUI.Core` from being published separately** - Set `IsPackable=false` on `ShellUI.Core` project
- **Updated release workflow** - Now only publishes the 2 intended packages:
  - ✅ `ShellUI.CLI` - CLI tool for component management
  - ✅ `ShellUI.Components` - Component library package
- **Updated documentation** - README now correctly reflects that only 2 packages are published

### What Changed
- `ShellUI.Core` is an internal dependency of `ShellUI.Components` and should not be published as a standalone package
- Future releases will only publish the 2 intended packages

## 📦 Installation

No changes to installation process:

```bash
# Install CLI globally
dotnet tool install -g ShellUI.CLI

# Initialize your project
shellui init

# Add components
shellui add button badge alert card
```

Or via NuGet:
```bash
dotnet add package ShellUI.Components
```

## 🔗 Links

- **Documentation**: https://shellui.dev
- **GitHub**: https://github.com/shellui-dev/shellui
- **NuGet**: https://www.nuget.org/packages/ShellUI.Components

---

**Full Changelog**: https://github.com/shellui-dev/shellui/compare/v0.1.0...v0.1.1
