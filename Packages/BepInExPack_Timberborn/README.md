![BepInEx logo](https://avatars2.githubusercontent.com/u/39589027?s=256)

# BepInExPack for Timberborn

This is [BepInEx 5.4.19](https://github.com/BepInEx/BepInEx) pack for Timberborn.

BepInEx is a general purpose framework for Unity modding.
BepInEx includes tools and libraries to

* load custom code (hereafter *plugins*) into the game on launch;
* patch in-game methods, classes and even entire assemblies without touching original game files;
* configure plugins and log game to desired outputs like console or file;
* manage plugin dependencies.

BepInEx is currently [one of the most popular modding tools for Unity on GitHub](https://github.com/topics/modding?o=desc&s=stars).

## This pack's contents

This pack is preconfigured and usable for Timberborn modding.  
In particular, this pack comes with preconfigured `BepInEx.cfg` that enables the BepInEx console and more extensive logging.

## Installation (game, automated)

This is the recommended way to install BepInEx on the game.

1. Download and install [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager) or [r2modman](https://Timberborn.thunderstore.io/package/ebkr/r2modman/)
2. Click **Install with Mod Manager** button on top of the page
3. Run the game via the mod manager

## Installation (manual)

If you are installing this manually, do the following

1. Extract the archive into a folder. **Do not extract into the game folder.**
2. Move the contents of `BepInExPack_Timberborn` folder into the game folder (where the game executable is located).
3. Run the game. If everything runs correctly, you will see BepInEx console pop up on your desktop.
4. Follow the game running instructions below:

### Configuration

No need to configure. Simply run the game. If everything is correct, you will see a console pop up.

## Useful links

* [BepInEx: writing basic plugin walkthrough](https://docs.bepinex.dev/articles/dev_guide/plugin_tutorial/index.html)
* [BepInEx: useful plugins for modding](https://docs.bepinex.dev/articles/dev_guide/dev_tools.html)
* [BepInEx: patching game methods at runtime](https://docs.bepinex.dev/articles/dev_guide/runtime_patching.html)

## Issues, questions, etc.

At this moment, you can use the following channels to ask for help

* [Timberborn Discord](https://discord.gg/timberborn)
* [BepInEx Discord](https://discord.gg/MpFEDAg) -- **Only technical support for THIS PACKAGE. No support for plugins.**

## Changelog

### 5.4.19

* Updated BepInEx to 5.4.19
* Removed TBMoreMapDirectories as it is not functional at the moment

### 5.4.16

* Updated BepInEx to 5.4.16
* Added TBMoreMapDirectories for in-built Thunderstore map support for r2mm/TMM

#### 5.4.15

* Initial release
