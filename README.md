# Valheim UI Mod - Display Day & Time in HUD

Simple UI mod to display the in-game day & time in the HUD above the minimap.
The panel game object with the day & time in it, is added as a child to the hudroot of the game, so it should only be visible when the HUD is visible.

## Requirements

[BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)

## Installation guide

1. Download and install [BepInEx](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/).
2. Download this mod from [Nexus Mods](https://www.nexusmods.com/valheim/mods/861) or from the the latest [release](https://github.com/OryxenMods/Oryxen.Valheim.UI.DisplayDayTime/releases), you can skip the next step if installed with Vortex.
3. Move the file **Oryxen.Valheim.UI.DisplayDayTime.dll** into BepInEx\plugins (you can find this directory under your Valheim directory).
4. Run the game!
5. Optional: you can edit the configuration file under BepInEx\config\oryxen.valheim.ui.displaydaytime.cfg to change the settings to your liking.

## Build guide

1. Clone this repository.
2. Optional: make some changes 😉.
3. Build the release.
4. Copy the file **Oryxen.Valheim.UI.DisplayDayTime.dll** from this directory in the project directory: Oryxen.Valheim.UI.DisplayDayTime\bin\Release
5. Paste the file **Oryxen.Valheim.UI.DisplayDayTime.dll** into directory BepInEx\plugins from your Valheim directory.
6. Now you can run the game and the mod should be active if there are no issues.
7. Optional: you can edit the configuration file under BepInEx\config\oryxen.valheim.ui.displaydaytime.cfg to change the settings to your liking.

## Configuration

The configuration file will be generated when the game runs with this mod installed.
You can find the configuration file for this mod under the Valheim directory: BepInEx\config\oryxen.valheim.ui.displaydaytime.cfg

Configuration settings you can change:

1. Display under minimap: set to "false" as default value (if set to false, the mod displays above the minimap).
2. Display time: set to "true" as default.
3. Display day: set to "true" as default.
4. 24-hour clock: set to "true" as default (if set to false, it will display the time in 12-hour notation).
5. Display background: set to "true" as default.
6. Font name: set "AveriaSansLibre-Bold" as default.
7. Font size: set to 16 as default.
8. Font color: set to RGBA(1, 1, 1, 0.791) as default.
9. Background color: set to RGBA(0, 0, 0, 0.3921569) as default.
10. Text outline color: set to black as default.
11. Text outline enabled: to to "true" as default.
12. Margin between minimap and this panel: set to 0 as default.
13. Padding left and right between text and border: set to 10 as default.
14. Reverse text position: set to "false" as default, if set to "true", time will display to the left and day will display to the right.
15. Panel width: set to 200 as default.
16. Panel height: set to 30 as default.

## Links

[Nexus Mods](https://www.nexusmods.com/valheim/mods/861)

[BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
