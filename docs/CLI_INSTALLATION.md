# ShellUI CLI Installation Guide

This guide covers different ways to install and manage the ShellUI CLI tool.

## Installation Methods

### Option 1: Global Installation (Recommended for Most Users)

Global installation makes the `shellui` command available system-wide.

```bash
# Install globally
dotnet tool install -g ShellUI.CLI

# Use without dotnet prefix
shellui init
shellui add button input card
shellui list
```

**Best for:**
- Individual developers
- Quick prototyping
- Personal projects
- Trying out ShellUI

### Option 2: Local Tool Installation (Recommended for Teams)

Local installation locks the CLI version per-project and shares it via source control.

```bash
# Step 1: Create tool manifest (one-time, in project root)
dotnet new tool-manifest

# Step 2: Install as local tool
dotnet tool install ShellUI.CLI

# Step 3: Use with dotnet prefix (required for local tools)
dotnet shellui init
dotnet shellui add button input card
dotnet shellui list
```

This creates a `.config/dotnet-tools.json` file:

```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "shellui.cli": {
      "version": "0.1.0",
      "commands": ["shellui"]
    }
  }
}
```

**Best for:**
- Team projects (everyone uses the same version)
- CI/CD pipelines (reproducible builds)
- Version-controlled environments
- Enterprise projects

### Comparison

| Aspect | Global (`-g`) | Local (manifest) |
|--------|--------------|------------------|
| **Command** | `shellui init` | `dotnet shellui init` |
| **Install location** | User profile | Project directory |
| **Sharing** | Each dev installs | Auto-restored via manifest |
| **Version lock** | Latest (unless pinned) | Locked in manifest |
| **CI/CD** | Requires explicit install | `dotnet tool restore` |
| **Best for** | Individual devs | Teams |

---

## Version Management

### Check Current Version

```bash
# Global tool
shellui --version

# Or list all global tools
dotnet tool list -g | findstr ShellUI

# Local tool
dotnet tool list | findstr ShellUI
```

### Update to Latest Version

```bash
# Update global tool
dotnet tool update -g ShellUI.CLI

# Update local tool
dotnet tool update ShellUI.CLI
```

### Install Specific Version

```bash
# Global - specific version
dotnet tool install -g ShellUI.CLI --version 0.1.0

# Local - specific version
dotnet tool install ShellUI.CLI --version 0.1.0

# Update to specific version
dotnet tool update -g ShellUI.CLI --version 0.2.1
```

### Downgrade to Previous Version

```bash
# Uninstall current, install specific version
dotnet tool uninstall -g ShellUI.CLI
dotnet tool install -g ShellUI.CLI --version 0.1.0
```

---

## Uninstallation

```bash
# Uninstall global tool
dotnet tool uninstall -g ShellUI.CLI

# Uninstall local tool
dotnet tool uninstall ShellUI.CLI
```

---

## CI/CD Setup

### GitHub Actions (Global Tool)

```yaml
- name: Install ShellUI CLI
  run: dotnet tool install -g ShellUI.CLI

- name: Initialize ShellUI
  run: shellui init --yes

- name: Add components
  run: shellui add button input card
```

### GitHub Actions (Local Tool - Recommended)

```yaml
- name: Restore tools
  run: dotnet tool restore

- name: Initialize ShellUI
  run: dotnet shellui init --yes

- name: Add components
  run: dotnet shellui add button input card
```

### Azure DevOps

```yaml
- script: dotnet tool install -g ShellUI.CLI
  displayName: 'Install ShellUI CLI'

- script: shellui init --yes
  displayName: 'Initialize ShellUI'
```

---

## Troubleshooting

### Command Not Found (Global Installation)

If `shellui` command is not recognized after global installation:

**Windows:**
```powershell
# Add to PATH (PowerShell)
$env:PATH += ";$env:USERPROFILE\.dotnet\tools"

# Or restart your terminal
```

**macOS/Linux:**
```bash
# Add to PATH
export PATH="$PATH:$HOME/.dotnet/tools"

# Add to .bashrc or .zshrc for persistence
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc
```

### Tool Restore Fails (Local Installation)

```bash
# Clear NuGet cache and retry
dotnet nuget locals all --clear
dotnet tool restore
```

### Version Conflicts

```bash
# Check what's installed
dotnet tool list -g

# Uninstall and reinstall
dotnet tool uninstall -g ShellUI.CLI
dotnet tool install -g ShellUI.CLI
```

---

## Best Practices

### For Individual Developers
1. Use global installation for convenience
2. Update regularly: `dotnet tool update -g ShellUI.CLI`
3. Check for updates before starting new projects

### For Teams
1. Use local tool installation with manifest
2. Commit `.config/dotnet-tools.json` to source control
3. Add `dotnet tool restore` to your README setup instructions
4. Pin to specific versions for stability

### For CI/CD
1. Prefer local tools for reproducibility
2. Always use `--yes` flag for non-interactive mode
3. Cache the .dotnet/tools directory if using global installation

---

## Quick Reference

| Task | Command |
|------|---------|
| Install (global) | `dotnet tool install -g ShellUI.CLI` |
| Install (local) | `dotnet tool install ShellUI.CLI` |
| Update (global) | `dotnet tool update -g ShellUI.CLI` |
| Update (local) | `dotnet tool update ShellUI.CLI` |
| Uninstall (global) | `dotnet tool uninstall -g ShellUI.CLI` |
| Check version | `shellui --version` |
| List tools | `dotnet tool list -g` |
| Restore local tools | `dotnet tool restore` |

---

## Related Documentation

- [README.md](../README.md) - Main documentation
- [VERSIONING_STRATEGY.md](../VERSIONING_STRATEGY.md) - Version management
- [ShellUI Website](https://shellui.dev) - Official documentation
