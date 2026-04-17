<#
.SYNOPSIS
    CupheadOnline -- full build + package script.

.DESCRIPTION
    1. Locates the Cuphead install (Steam registry or CUPHEAD_PATH env-var)
    2. Builds the mod DLL  (CupheadOnline.csproj  -> net462)
    3. Builds the installer (CupheadOnlineInstaller.csproj -> net48)
    4. Copies mod DLLs next to the installer project so they get embedded
    5. Rebuilds the installer with the embedded DLLs
    6. Produces the final artefacts in  ./dist/

.USAGE
    # Auto-detect Cuphead:
    .\build.ps1

    # Explicit path:
    $env:CUPHEAD_PATH = "D:\Games\Cuphead"
    .\build.ps1

    # Skip deploy to BepInEx (just produce the installer):
    .\build.ps1 -NoDeploy
#>
param(
    [string] $CupheadPath = $env:CUPHEAD_PATH,
    [switch] $NoDeploy,
    [switch] $Release
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$Configuration = if ($Release) { "Release" } else { "Debug" }
$Root          = $PSScriptRoot
$DistDir       = Join-Path $Root "dist"
$ModProject    = Join-Path $Root "CupheadOnline\CupheadOnline.csproj"
$InstallerProj = Join-Path $Root "CupheadOnlineInstaller\CupheadOnlineInstaller.csproj"

# -----------------------------------------------------------------------------
function Write-Step([string]$msg) {
    Write-Host ""
    Write-Host "  >>> $msg" -ForegroundColor Cyan
}

function Fail([string]$msg) {
    Write-Host ""
    Write-Host "  FATAL: $msg" -ForegroundColor Red
    exit 1
}

# -----------------------------------------------------------------------------
#  Banner
# -----------------------------------------------------------------------------
Write-Host ""
Write-Host "  +======================================+" -ForegroundColor Yellow
Write-Host "  |   CupheadOnline Build Script  v1.0  |" -ForegroundColor Yellow
Write-Host "  +======================================+" -ForegroundColor Yellow
Write-Host ""

# -----------------------------------------------------------------------------
#  1. Locate Cuphead
# -----------------------------------------------------------------------------
Write-Step "Locating Cuphead"

if (-not $CupheadPath) {
    # Try Steam registry (64-bit and 32-bit keys)
    $steamKey = "HKLM:\SOFTWARE\WOW6432Node\Valve\Steam"
    if (-not (Test-Path $steamKey)) {
        $steamKey = "HKLM:\SOFTWARE\Valve\Steam"
    }
    if (Test-Path $steamKey) {
        $steamPath = (Get-ItemProperty $steamKey).InstallPath
        $candidate = Join-Path $steamPath "steamapps\common\Cuphead"
        if (Test-Path "$candidate\Cuphead.exe") { $CupheadPath = $candidate }
    }
}

# Common fallback paths if registry failed
if (-not $CupheadPath) {
    $fallbacks = @(
        "C:\Program Files (x86)\Steam\steamapps\common\Cuphead",
        "C:\Program Files\Steam\steamapps\common\Cuphead",
        "D:\SteamLibrary\steamapps\common\Cuphead",
        "D:\Steam\steamapps\common\Cuphead",
        "E:\SteamLibrary\steamapps\common\Cuphead"
    )
    foreach ($fb in $fallbacks) {
        if (Test-Path "$fb\Cuphead.exe") { $CupheadPath = $fb; break }
    }
}

if (-not $CupheadPath -or -not (Test-Path "$CupheadPath\Cuphead.exe")) {
    Write-Host "  Could not auto-detect Cuphead." -ForegroundColor Yellow
    $CupheadPath = Read-Host "  Enter full path to Cuphead folder"
    if (-not (Test-Path "$CupheadPath\Cuphead.exe")) {
        Fail "Cuphead.exe not found at: $CupheadPath"
    }
}

Write-Host "  Found: $CupheadPath" -ForegroundColor Green
$env:CUPHEAD_PATH = $CupheadPath

# -----------------------------------------------------------------------------
#  2. Restore NuGet packages
# -----------------------------------------------------------------------------
Write-Step "Restoring NuGet packages"
dotnet restore $ModProject --nologo
if ($LASTEXITCODE -ne 0) { Fail "NuGet restore failed." }

# -----------------------------------------------------------------------------
#  3. Build the mod DLL
# -----------------------------------------------------------------------------
Write-Step "Building mod DLL ($Configuration)"
dotnet build $ModProject -c $Configuration --nologo --no-restore
if ($LASTEXITCODE -ne 0) { Fail "Mod build failed." }

# Locate output DLLs
$ModOutput = Join-Path $Root "CupheadOnline\bin\$Configuration\net462"
$ModDll    = Join-Path $ModOutput "CupheadOnline.dll"
$LnlDll    = Join-Path $ModOutput "LiteNetLib.dll"

if (-not (Test-Path $ModDll)) { Fail "CupheadOnline.dll not found after build." }
Write-Host "  DLL: $ModDll" -ForegroundColor Green

# -----------------------------------------------------------------------------
#  4. Prepare dist/ folder
# -----------------------------------------------------------------------------
Write-Step "Preparing dist/"
New-Item -ItemType Directory -Force $DistDir | Out-Null
Copy-Item $ModDll $DistDir -Force
if (Test-Path $LnlDll) { Copy-Item $LnlDll $DistDir -Force }

# -----------------------------------------------------------------------------
#  5. Stage DLLs next to installer so they get embedded as resources
# -----------------------------------------------------------------------------
Write-Step "Staging DLLs for installer embedding"
$InstallerDir = Join-Path $Root "CupheadOnlineInstaller"
Copy-Item $ModDll $InstallerDir -Force
if (Test-Path $LnlDll) { Copy-Item $LnlDll $InstallerDir -Force }

# -----------------------------------------------------------------------------
#  6. Build the installer (DLLs are now embedded resources)
# -----------------------------------------------------------------------------
Write-Step "Building installer ($Configuration)"
dotnet build $InstallerProj -c $Configuration --nologo
if ($LASTEXITCODE -ne 0) { Fail "Installer build failed." }

$InstallerOutput = Join-Path $Root "CupheadOnlineInstaller\bin\$Configuration\net48"
$InstallerExe    = Join-Path $InstallerOutput "CupheadOnlineInstaller.exe"
if (Test-Path $InstallerExe) {
    Copy-Item $InstallerExe $DistDir -Force
    Write-Host "  Installer: $DistDir\CupheadOnlineInstaller.exe" -ForegroundColor Green
}

# -----------------------------------------------------------------------------
#  7. Optional: deploy directly to BepInEx plugin folder
# -----------------------------------------------------------------------------
if (-not $NoDeploy) {
    Write-Step "Deploying to BepInEx plugin folder"
    $PluginDir = Join-Path $CupheadPath "BepInEx\plugins\CupheadOnline"
    New-Item -ItemType Directory -Force $PluginDir | Out-Null
    Copy-Item $ModDll $PluginDir -Force
    if (Test-Path $LnlDll) { Copy-Item $LnlDll $PluginDir -Force }
    Write-Host "  Deployed to: $PluginDir" -ForegroundColor Green
}

# -----------------------------------------------------------------------------
#  Summary
# -----------------------------------------------------------------------------
Write-Host ""
Write-Host "  +==================================================+" -ForegroundColor Green
Write-Host "  |   Build complete!                                |" -ForegroundColor Green
Write-Host "  |                                                  |" -ForegroundColor Green
Write-Host "  |   dist\CupheadOnline.dll        <- mod          |" -ForegroundColor Green
Write-Host "  |   dist\LiteNetLib.dll            <- networking  |" -ForegroundColor Green
Write-Host "  |   dist\CupheadOnlineInstaller.exe <- installer  |" -ForegroundColor Green
Write-Host "  +==================================================+" -ForegroundColor Green
Write-Host ""
Write-Host "  To install on a fresh machine, run:" -ForegroundColor White
Write-Host "    dist\CupheadOnlineInstaller.exe" -ForegroundColor Cyan
Write-Host ""
