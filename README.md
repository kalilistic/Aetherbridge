<h1 align="center">
  <br><a href="https://github.com/kalilistic/aetherbridge"><img src="img/bannerIcon.png" alt="Aetherbridge"></a>
  <br>Aetherbridge<br>
</h1>
<h4 align="center">Library for easier ACT FFXIV Plugin development</h4>

<p align="center">
  <a href="https://github.com/kalilistic/aetherbridge/blob/master/LICENSE"><img src="https://img.shields.io/github/license/kalilistic/aetherbridge?color=lightgrey"></a>
  <a href="https://discord.gg/ftn4k7x"><img src="https://img.shields.io/badge/chat-on%20discord-7289da.svg"></a>
</p>

## Background

This is an evolving effort to extract common code from my ACT FFXIV Plugins.

## Key Features

* Wrappers for easier access to:
  * ACT Globals and other ACT functions
  * Ravahn's FFXIV Parsing plugin
  * Ngld's Overlay Plugin
  * SaintCoinach Data
  * Universalis API
* Config Manager for storing user settings.
* Updater using GitHub Releases and SemVer.
* Basic Logger with no deps.
* Custom WinForm Controls.

## How To Install
1. Clone repo to local workspace.
2. Update cake.build configuration section.
3. Run ./build.ps1 as admin in powershell.

## How To Update SaintCoinach Data
1. Update config in ./scripts/game-data-config.bat.
2. Open your Aetherbridge/scripts dir in command line.
3. Run update-game-data.bat

## Considerations

* By default, a mock dll is generated for easier unit testing.
* Not all SaintCoinach data is available - only what I've needed to-date.

## Software Used

* <a href="https://github.com/EQAditu/AdvancedCombatTracker">Advanced Combat Tracker</a>
* <a href="https://github.com/ravahn/FFXIV_ACT_Plugin">FFXIV_ACT_Plugin</a>
* <a href="https://github.com/ngld/OverlayPlugin">ngld's OverlayPlugin</a>
* <a href="https://github.com/ufx/SaintCoinach">SaintCoinach</a>

