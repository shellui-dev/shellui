# Releasing ShellUI

How `ShellUI.CLI` and `ShellUI.Components` get to nuget.org. Pushing a `v*` tag runs `.github/workflows/release.yml`: it checks the version, rebuilds the precompiled CSS bundle, builds, tests, packs, publishes both packages and creates the GitHub release from `docs/RELEASE_NOTES.md`. A tag with a `-` (`v0.4.0-alpha.3`) is published as a prerelease.

## One-time setup: NuGet Trusted Publishing

The workflow doesn't store a NuGet API key. On each run it exchanges a GitHub OIDC token for a key that's valid for one hour ([NuGet docs](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing)). This needs three things, set up once:

1. **GitHub environment.** Repository → Settings → Environments → New environment, named exactly `release`. Nothing to configure inside (add required reviewers if you want a manual approval before each publish).
2. **`NUGET_USER` secret.** Repository → Settings → Secrets and variables → Actions → New repository secret: name `NUGET_USER`, value your **nuget.org profile name** (the name in `nuget.org/profiles/<name>`, not your email).
3. **Trusted Publishing policy on nuget.org.** Sign in → your username → **Trusted Publishing** → **Add**, choose the account that owns the `ShellUI.*` packages, and fill in:
   - Repository Owner: `shellui-dev`
   - Repository: `shellui`
   - Workflow File: `release.yml` (the file name only, no `.github/workflows/`)
   - Environment: `release`

Then run one release. When it has published, finish the switch:

4. **Delete the old `NUGET_API_KEY` secret** from the repository, and **revoke that key** on nuget.org (your username → API Keys).

The `release/v0.3` branch has its own copy of `release.yml`, and a tag runs the workflow from the tagged commit. Bring the same change to `release/v0.3` before deleting the old key, or the next 0.3.x tag can't publish.

If a run fails at **NuGet login**, check in this order: the `NUGET_USER` secret is missing or wrong; the policy's Workflow File has a path in it; the policy's Environment doesn't match `environment: release`.

## Cutting a release

The version rules and the `prepare-release.ps1` dry run are in [VERSIONING_STRATEGY.md](../VERSIONING_STRATEGY.md#release-workflow). In short:

### 1. Version

`Directory.Build.props` sets the version for every project:

```xml
<ShellUIVersion>0.4.0</ShellUIVersion>
<ShellUIVersionSuffix>alpha.3</ShellUIVersionSuffix>  <!-- empty for a stable release -->
```

Also update the version mentioned in `README.md`, `VERSIONING_STRATEGY.md`, `docs/CLI_INSTALLATION.md`, `docs/CLI_SYNTAX.md`, `docs/FAQ.md`, `docs/QUICKSTART.md`, and the fallback in `src/ShellUI.Templates/VersionHelper.cs` and `src/ShellUI.Core/Models/ComponentMetadata.cs`. `git grep` the previous version to find them all.

### 2. Release notes

Add a `# ShellUI v0.4.0-alpha.3` section at the top of `docs/RELEASE_NOTES.md`: what's new, fixes, breaking changes, and how to upgrade. Each section describes only its own release. Check that the workflow will find it:

```bash
bash scripts/extract-release-notes.sh 0.4.0-alpha.3
```

### 3. Check

- If component CSS changed, regenerate the safelist (`dotnet run --project tools/ShellUI.SafelistGenerator -- …`, full command in VERSIONING_STRATEGY.md).
- `pwsh ./prepare-release.ps1 -Version 0.4.0 -Suffix alpha.3 -DryRun` builds, tests and checks the notes section.
- Pack both projects to a local folder, install the CLI from it into a tool path, and run `shellui init` and `shellui add` in fresh apps on .NET 8, 9 and 10 (all direct targets together, app named `my-app`).
- Use the packed `ShellUI.Components` in a fresh app and click through the components the release changed, in light and dark mode.
- Delete the local test packages from `~/.nuget/packages` afterwards, so later restores fetch the published ones.

### 4. Merge and tag

Open a `chore(release): v0.4.0-alpha.3` pull request, merge it, then tag the merge commit:

```bash
git switch main && git pull --ff-only
git tag v0.4.0-alpha.3
git push origin v0.4.0-alpha.3
```

Watch the **Release** workflow in the Actions tab. If it fails before **Publish to NuGet**, fix it, delete the tag (`git push --delete origin v0.4.0-alpha.3`, `git tag -d v0.4.0-alpha.3`) and tag again. Once a version is on nuget.org it can't be reused, only unlisted.

## 0.3.x patches

0.3 lives on `release/v0.3`, which is never merged into `main`. For a patch: branch `fix/v0.3.N` off `release/v0.3`, cherry-pick the fixes from `main`, bump the version and add a `# ShellUI v0.3.N` section to that branch's release notes, open the pull request **into `release/v0.3`**, and tag the merge commit there. The same workflow publishes it as a stable release.
