# Changelog

## v1.0.2 - 2026-04-17

- Fixed the multiplayer submenu back behavior so `BACK`, `Escape`, and controller `B` cleanly return to the main menu instead of appearing to refresh the submenu.
- Added a permanent multiplayer hint that explicitly says to press `Escape` or controller `B` to go back.

## v1.0.1 - 2026-04-17

- Fixed Steam startup guards so Steam P2P polling only runs after Steamworks is initialized and stops spamming errors when the game is not launched through Steam.
- Fixed the Slot Select reflection crash by resolving the live `SlotSelectScreen` instance before reading non-static fields.
- Reworked the multiplayer and credits menus so they render reliably, respect back/cancel input, and no longer soft-lock or hide the screen.
- Fixed the credits screen formatting by switching it to explicit line placement instead of fragile multiline layout behavior in Unity 2017 UI.
- Fixed the multiplayer submenu layout so the actions stack correctly instead of overlapping on top of each other.
- Added clearer Steam status, richer connection HUD feedback, retry/invite/diagnostics actions, and safer host/join state transitions.
- Removed the obsolete legacy installer artifact and standardized the repo on the Electron web installer workflow.
- Added installer utility actions for opening the Cuphead folder, launching Steam, and verifying the install.
