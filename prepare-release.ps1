param(
    [Parameter(Mandatory=$true)]
    [string]$Version,

    [string]$Suffix = "",
    [switch]$DryRun
)

Write-Host "🚀 Preparing ShellUI Release v$Version" -ForegroundColor Cyan
if ($Suffix) {
    Write-Host "📦 Pre-release suffix: $Suffix" -ForegroundColor Yellow
}

# Check if working directory is clean
$status = git status --porcelain
if ($status) {
    Write-Host "❌ Working directory is not clean. Please commit or stash changes." -ForegroundColor Red
    Write-Host "Changes:" -ForegroundColor Red
    Write-Host $status -ForegroundColor Red
    exit 1
}

# Update version in Directory.Build.props
if (-not $DryRun) {
    Write-Host "📝 Updating version to $Version..." -ForegroundColor Yellow
    $propsFile = "Directory.Build.props"
    $content = Get-Content $propsFile -Raw
    $content = $content -replace '<ShellUIVersion>.*?</ShellUIVersion>', "<ShellUIVersion>$Version</ShellUIVersion>"
    if ($Suffix) {
        $content = $content -replace '<ShellUIVersionSuffix>.*?</ShellUIVersionSuffix>', "<ShellUIVersionSuffix>$Suffix</ShellUIVersionSuffix>"
    } else {
        $content = $content -replace '<ShellUIVersionSuffix>.*?</ShellUIVersionSuffix>', "<ShellUIVersionSuffix></ShellUIVersionSuffix>"
    }
    Set-Content $propsFile $content
    Write-Host "✅ Updated Directory.Build.props" -ForegroundColor Green
}

# Clean and build all projects
Write-Host "🔨 Building all projects..." -ForegroundColor Yellow
dotnet clean
if ($LASTEXITCODE -ne 0) { exit 1 }

dotnet build --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed" -ForegroundColor Red
    exit 1
}
Write-Host "✅ All projects built successfully" -ForegroundColor Green

# Check created packages
$packages = Get-ChildItem -Path "src/*/bin/Release/*.nupkg" -Recurse | Where-Object { $_.Name -like "*$Version*" }
if ($packages) {
    Write-Host "`n📦 Created packages:" -ForegroundColor Cyan
    $packages | ForEach-Object {
        Write-Host "  • $($_.Name)" -ForegroundColor White
    }
} else {
    Write-Host "⚠️ No packages found with version $Version" -ForegroundColor Yellow
}

# Run tests
Write-Host "`n🧪 Running tests..." -ForegroundColor Yellow
dotnet test --configuration Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Tests failed" -ForegroundColor Red
    exit 1
}
Write-Host "✅ All tests passed" -ForegroundColor Green

if ($DryRun) {
    Write-Host "`n🔍 DRY RUN COMPLETE" -ForegroundColor Cyan
    Write-Host "Run without -DryRun to apply changes" -ForegroundColor Yellow
    exit 0
}

# Create git tag
Write-Host "`n🏷️ Creating git tag v$Version..." -ForegroundColor Yellow
$tagName = "v$Version"
if ($Suffix) {
    $tagName = "v$Version-$Suffix"
}

git tag $tagName
git push origin $tagName
Write-Host "✅ Created and pushed tag $tagName" -ForegroundColor Green

# Instructions for next steps
Write-Host "`n🎉 Release v$Version prepared!" -ForegroundColor Green
Write-Host "`nNext steps:" -ForegroundColor Cyan
Write-Host "1. Create GitHub release at: https://github.com/shellui-dev/shellui/releases/new" -ForegroundColor White
Write-Host "2. Upload the NuGet packages (.nupkg files)" -ForegroundColor White
Write-Host "3. Add release notes" -ForegroundColor White
Write-Host "4. Update documentation if needed" -ForegroundColor White
Write-Host "5. Announce on social media/discord" -ForegroundColor White

Write-Host "`n📋 Release Checklist:" -ForegroundColor Cyan
Write-Host "✅ Version updated in Directory.Build.props" -ForegroundColor Green
Write-Host "✅ All projects build successfully" -ForegroundColor Green
Write-Host "✅ All tests pass" -ForegroundColor Green
Write-Host "✅ Git tag created and pushed" -ForegroundColor Green
Write-Host "☐ GitHub release created" -ForegroundColor Yellow
Write-Host "☐ NuGet packages uploaded" -ForegroundColor Yellow
Write-Host "☐ Documentation updated" -ForegroundColor Yellow