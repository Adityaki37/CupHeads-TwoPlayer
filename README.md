# CupheadOnline

A complete online multiplayer mod for Cuphead with a user-friendly installer.

## About

**CupheadOnline** adds online co-op multiplayer to Cuphead, allowing two players to fight bosses together over the internet. The mod is production-ready with automatic BepInEx installation and one-click setup.

## Features

- Online co-op multiplayer (Steam P2P networking)
- Automatic Cuphead detection via Steam
- Automatic BepInEx installation (if not present)
- One-click mod installation
- Real-time progress tracking
- Portable executable (no admin rights needed)
- Safe and fully removable

## Requirements

- Windows 10 or later
- Cuphead installed via Steam
- Internet connection (for BepInEx download if needed)

## Quick Start

1. Download `CupheadOnline-Installer.exe` from [Releases](https://github.com/Germanized/CupHeads/releases)
2. Run the installer (no admin rights required)
3. Select your Cuphead installation folder
4. Click "Install" — it handles everything automatically
5. Launch Cuphead via Steam and play together!

## How It Works

The installer:
- **Detects** your Cuphead installation automatically
- **Downloads & installs** BepInEx 5.4.23.2 (mod loader) if needed
- **Copies** the CupheadOnline mod to `Cuphead/BepInEx/plugins/CupheadOnline/`
- **Activates** the mod via Harmony patches and LiteNetLib networking

The mod enables the game's built-in 2P co-op system (`PlayerManager.Multiplayer = true`) and handles player synchronization, input replication, and networked boss logic.

## Troubleshooting

**Cuphead not detected?**
- Ensure Cuphead is installed via Steam in the default location
- Manually browse to your Steam Cuphead folder if auto-detection fails
- Check that you have write permissions to the Cuphead folder

**Installation issues?**
- The installer requires write access to the Cuphead directory
- Close Cuphead and the Steam launcher before installing
- Try running the installer as Administrator if needed

## Building from Source

### Build the mod:
```bash
dotnet build CupheadOnline/CupheadOnline.csproj -c Release
```

### Build the installer:
```bash
cd CupheadInstaller
npm install
npm run dist
```

Output: `CupheadInstaller/dist/CupheadOnline-Installer.exe`

## Technical Stack

- **Mod Framework:** BepInEx 5.4.23.2 (Harmony 2 patches)
- **Networking:** LiteNetLib 1.1.0 (UDP with sequencing)
- **Mod Language:** C# with decompiled Assembly-CSharp.vb references
- **Installer:** Electron + Node.js (portable executable)

## GitHub Release Package

- **Source:** Full `CupheadOnline/` and `CupheadInstaller/` directories
- **Release Asset:** `CupheadOnline-Installer.exe` (portable, all-in-one)
- **Auto-Setup:** The installer detects and installs the correct BepInEx version for your system

## License

See [LICENSE](LICENSE) file for details.

## Credits

- CupheadOnline mod by Germanized
- BepInEx team for the mod loader
- Harmony team for method patching
- LiteNetLib team for networking
- Electron team for the installer framework
