# ShellUI Versioning Strategy

## Overview

ShellUI follows a **unified versioning approach** where all components, CLI, and packages share the same version number. This ensures consistency and simplifies dependency management while providing users with multiple ways to access specific versions.

## Version Number: v0.3.0-alpha.1 ✅

**Current Release:** v0.3.0-alpha.1
- ✅ 100 production-ready components
- ✅ CLI tool + NuGet packages
- ✅ Full Blazor WebAssembly + Server support
- ✅ Tailwind CSS v4.1.17 integration

## Unified Versioning System

### How It Works

**Single Source of Truth:**
```xml
<!-- Directory.Build.props (root) -->
<ShellUIVersion>0.3.0</ShellUIVersion>
<ShellUIVersionSuffix>alpha.1</ShellUIVersionSuffix>  <!-- Empty for stable -->
```

**Automatically Applied To:**
- ✅ All NuGet packages (`ShellUI.CLI`, `ShellUI.Components`, `ShellUI.Core`)
- ✅ All ~100 component templates
- ✅ Build configurations
- ✅ Component metadata

### Version Update Process

```bash
# 1. Update version in Directory.Build.props
<ShellUIVersion>0.3.0</ShellUIVersion>

# 2. Clean and rebuild
dotnet clean && dotnet build --configuration Release
```

## User Access to Versions

### 1. CLI Always Installs Latest ✅
```bash
# Always installs latest compatible version
shellui add button input card
```

**Why?** CLI-first approach prioritizes latest features and bug fixes.

### 2. NuGet Packages (Versioned) ✅
```bash
# Install specific version via NuGet (current: 0.3.0-alpha.1)
dotnet add package ShellUI.Components --version 0.3.0-alpha.1
dotnet add package ShellUI.CLI --version 0.3.0-alpha.1
```

### 3. Git Tags & Releases ✅
```bash
# Clone specific version (current: v0.3.0-alpha.1)
git clone --branch v0.3.0-alpha.1 https://github.com/shellui-dev/shellui.git

# Or download release archive
# https://github.com/shellui-dev/shellui/releases/tag/v0.3.0-alpha.1
```

### 4. Versioned Documentation ✅
```
docs/
├── v0.2.x/          # v0.2 stable docs
├── v0.3.0-alpha/    # Current alpha (v0.3.0-alpha.1)
└── latest/          # Always points to latest (main)
```

## Component Version Storage

### shellui.json Structure
```json
{
  "installedComponents": [
    {
      "name": "button",
      "version": "0.3.0-alpha.1",
      "installedAt": "2026-01-11T...",
      "isCustomized": false
    }
  ]
}
```

**✅ Dynamic, Not Hardcoded:** Versions are computed from centralized source, not hardcoded in components.

## Release Process

### For v0.3.0-alpha.1 (Current Alpha):
```bash
# 1. Version already set in Directory.Build.props:
#    <ShellUIVersion>0.3.0</ShellUIVersion>
#    <ShellUIVersionSuffix>alpha.1</ShellUIVersionSuffix>

# 2. Build all packages
dotnet build --configuration Release

# 3. Create git tag
git tag v0.3.0-alpha.1
git push origin v0.3.0-alpha.1

# 4. GitHub release runs via CI (publishes NuGet, creates release)
```

### For v0.3.0 Stable (Future):
```bash
# 1. Update Directory.Build.props: set ShellUIVersionSuffix to empty
<ShellUIVersion>0.3.0</ShellUIVersion>
<ShellUIVersionSuffix></ShellUIVersionSuffix>

# 2. Build, test, tag, release
git tag v0.3.0
git push origin v0.3.0

# 3. Update docs/v0.3.x/
```

## Version Compatibility

### Semantic Versioning
- **MAJOR.MINOR.PATCH** (e.g., 0.3.0)
- **Breaking Changes:** MAJOR version bump
- **New Features:** MINOR version bump
- **Bug Fixes:** PATCH version bump

### Component Compatibility
- Components within same MINOR version are fully compatible
- CLI can upgrade components within same MAJOR version
- Breaking changes require MAJOR version bump

## Migration Strategy

### For Existing Users:
1. **CLI users:** Automatically get latest via `shellui add`
2. **NuGet users:** Can pin to specific versions
3. **Custom components:** Can be manually updated

### Documentation Access:
- **Latest:** `shellui.dev/docs` (main)
- **v0.2.x:** `shellui.dev/docs/v0.2.x` (stable)
- **v0.3.0-alpha:** `shellui.dev/docs` (current alpha on main)

## Implementation Details

### ✅ No Hardcoded Versions
- Component templates use dynamic versioning
- CLI computes versions at runtime
- shellui.json stores computed versions (not hardcoded)

### ✅ Centralized Control
- Single file controls all versioning
- Automated propagation to all components
- Easy version management for maintainers

### ✅ User Flexibility
- CLI for latest features
- NuGet for version pinning
- Git for source access
- Docs for version-specific guidance

## Summary

**ShellUI v0.3.0-alpha.1** (current) and **v0.3.0** (future stable) with:
- ✅ Unified versioning system
- ✅ No hardcoded component versions
- ✅ Multiple user access methods
- ✅ Professional release management
- ✅ Future-proof architecture

**Ready to ship! 🚀**