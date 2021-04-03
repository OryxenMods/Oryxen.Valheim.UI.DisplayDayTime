# Valheim UI Mod - Display Day & Time in HUD

Simple UI mod to display the day & time in the HUD above the minimap.
The panel game object with the day & time in it, is added as a child to the hudroot of the game, so it should only be visible when the HUD is visible.

## Requirements

[BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)

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

## Installation guide

1. Download and install [BepInEx](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/).
2. Download this mod from [Nexus Mods](https://www.nexusmods.com/valheim/mods/861) or from the the latest [release](https://github.com/OryxenMods/Oryxen.Valheim.UI.DisplayDayTime/releases), you can skip the next step if installed with Vortex.
3. Move the file **Oryxen.Valheim.UI.DisplayDayTime.dll** into BepInEx\plugins (you can find this directory under your Valheim directory).
4. Run the game!
5. Optional: you can edit the configuration file under BepInEx\config\oryxen.valheim.ui.displaydaytime.cfg to change the settings to your liking.

## Links

[Nexus Mods](https://www.nexusmods.com/valheim/mods/861)

[BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
