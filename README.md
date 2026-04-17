# CupHeads

Steam P2P multiplayer for Cuphead, plus a desktop installer that handles the mod setup for you.

## What this repo contains

- `CupheadOnline/` - the BepInEx + Harmony mod
- `CupheadInstaller/` - the Electron installer app
- `build.ps1` - root build script that builds the mod and packages the Electron installer

## Features

- Steam P2P multiplayer transport
- Steam lobby and invite flow
- Automatic Cuphead detection through Steam
- Automatic BepInEx installation when needed
- One-click mod installation
- Portable desktop installer

## Requirements

- Windows 10 or later
- Cuphead installed through Steam
- Internet connection for BepInEx download during install

## Quick Start

1. Download `Cupheads.exe` from [Releases](https://github.com/Germanized/CupHeads/releases).
2. Run the installer.
3. Let it detect your Cuphead folder, or browse to it manually.
4. Click Install.
5. Launch Cuphead through Steam.

If you test outside the Steam launcher, you may need a `steam_appid.txt` next to `Cuphead.exe`.

## How installation works

The Electron installer:

- detects your Cuphead install
- installs BepInEx 5 if it is missing
- copies `CupheadOnline.dll` into `Cuphead\\BepInEx\\plugins\\CupheadOnline\\`

The mod then patches the game's menu and gameplay flow through Harmony and uses Steamworks P2P for multiplayer.

## Building from source

### Build the mod only

```powershell
dotnet build .\CupheadOnline\CupheadOnline.csproj -c Release
```

### Build the full release package

```powershell
.\build.ps1 -Release
```

That produces:

- `dist\CupheadOnline.dll`
- `dist\Cupheads.exe`

### Build the installer manually

```powershell
cd .\CupheadInstaller
npm install
npm run dist
```

The packaged installer is written to `CupheadInstaller\dist\Cupheads.exe`.

## Tech stack

- BepInEx 5
- Harmony 2
- Steamworks P2P
- Electron + Node.js

## Credits

- Germanized and Sh0kr for the mod
- BepInEx for the mod framework
- Harmony for patching
- Electron for the installer shell
