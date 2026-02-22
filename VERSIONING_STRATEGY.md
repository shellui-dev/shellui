# ShellUI Versioning Strategy

## Overview

ShellUI follows a **unified versioning approach** where all components, CLI, and packages share the same version number. This ensures consistency and simplifies dependency management while providing users with multiple ways to access specific versions.

## Version Number: v0.1.0 ✅

**Current Release:** v0.2.1
- ✅ 80 production-ready components
- ✅ CLI tool + NuGet packages
- ✅ Full Blazor WebAssembly + Server support
- ✅ Tailwind CSS v4.1.18 integration

## Unified Versioning System

### How It Works

**Single Source of Truth:**
```xml
<!-- Directory.Build.props (root) -->
<ShellUIVersion>0.1.0</ShellUIVersion>
```

**Automatically Applied To:**
- ✅ All NuGet packages (`ShellUI.CLI`, `ShellUI.Components`, `ShellUI.Core`)
- ✅ All 73 component templates
- ✅ Build configurations
- ✅ Component metadata

### Version Update Process

```bash
# 1. Update version in Directory.Build.props
<ShellUIVersion>0.2.1</ShellUIVersion>

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
# Install specific version via NuGet
dotnet add package ShellUI.Components --version 0.1.0
dotnet add package ShellUI.CLI --version 0.1.0
```

### 3. Git Tags & Releases ✅
```bash
# Clone specific version
git clone --branch v0.1.0 https://github.com/shellui-dev/shellui.git

# Or download release archive
# https://github.com/shellui-dev/shellui/releases/tag/v0.1.0
```

### 4. Versioned Documentation ✅
```
docs/
├── v0.1.x/          # Current docs
├── v0.2.x/          # Future docs
└── latest/          # Always points to latest
```

## Component Version Storage

### shellui.json Structure
```json
{
  "installedComponents": [
    {
      "name": "button",
      "version": "0.1.0",
      "installedAt": "2026-01-11T...",
      "isCustomized": false
    }
  ]
}
```

**✅ Dynamic, Not Hardcoded:** Versions are computed from centralized source, not hardcoded in components.

## Release Process

### For v0.1.0 Release:
```bash
# 1. Update version (already done)
# Directory.Build.props has <ShellUIVersion>0.1.0</ShellUIVersion>

# 2. Build all packages
dotnet build --configuration Release

# 3. Create git tag
git tag v0.1.0
git push origin v0.1.0

# 4. Create GitHub release
# - Upload NuGet packages
# - Generate release notes
# - Update docs/v0.1.x/
```

### Future Releases:
```bash
# For v0.2.1
# 1. Update Directory.Build.props
<ShellUIVersion>0.2.1</ShellUIVersion>

# 2. Build, test, tag, release
# 3. Update docs/v0.2.x/
```

## Version Compatibility

### Semantic Versioning
- **MAJOR.MINOR.PATCH** (e.g., 0.1.0)
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
- **Latest:** `shellui.dev/docs`
- **Specific version:** `shellui.dev/docs/v0.1.x`

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

**ShellUI v0.1.0** is ready for release with:
- ✅ Unified versioning system
- ✅ No hardcoded component versions
- ✅ Multiple user access methods
- ✅ Professional release management
- ✅ Future-proof architecture

**Ready to ship! 🚀**