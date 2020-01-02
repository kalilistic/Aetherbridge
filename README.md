<h1 align="center">
  <br><a href="https://github.com/kalilistic/aetherbridge"><img src="img/bannerIcon.png" alt="Aetherbridge"></a>
  <br>Aetherbridge<br>
</h1>
<h4 align="center">FFXIV ACT Plugin Library for easier development.</h4>

<p align="center">
  <a href="https://github.com/kalilistic/aetherbridge/releases/latest"><img src="https://img.shields.io/github/v/release/kalilistic/aetherbridge"></a>
  <a href="https://ci.appveyor.com/project/kalilistic/aetherbridge/branch/master"><img src="https://img.shields.io/appveyor/ci/kalilistic/aetherbridge"></a>
  <a href="https://ci.appveyor.com/project/kalilistic/aetherbridge/branch/master/tests"><img src="https://img.shields.io/appveyor/tests/kalilistic/aetherbridge"></a>
  <a href="https://codecov.io/gh/kalilistic/aetherbridge/branch/master"><img src="https://img.shields.io/codecov/c/gh/kalilistic/aetherbridge"></a>
  <a href="https://github.com/kalilistic/aetherbridge/blob/master/LICENSE"><img src="https://img.shields.io/github/license/kalilistic/aetherbridge?color=lightgrey"></a>
  <a href="https://discord.gg/ftn4k7x"><img src="https://img.shields.io/badge/chat-on%20discord-7289da.svg"></a>
</p>

## Background

.NET Library for FFXIV ACT Plugin development to more easily get data from ACT and other sources. When I was developing my loot plugin, I found I spent a lot of time on integrating with ACT, the parsing plugin, and parsing log lines. I thought it'd be an interesting exercise to abstract this work into a separate project for reuse across my plugins.

## Key Features

* Provide all-in-one library for FFXIV data for ACT plugins.
* Provide wrapper for the following:
  * ACT Globals and other ACT functions
  * Ravahn's FFXIV Parsing plugin
  * CrescentCove (Game Data mostly from SaintCoinach)


## How To Install

You can install from <a href="https://www.nuget.org/packages/aetherbridge/">nuget</a>, <a href="https://github.com/kalilistic/aetherbridge/packages">github packages</a>, or <a href="https://github.com/kalilistic/aetherbridge/releases/latest">github releases</a>.

## How To Use

```csharp
public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
{
    // setup aetherbridge
    var aetherBridge = Aetherbridge.GetInstance();

    // get data from aetherbridge
    var player = aetherBridge.GetCurrentPlayer();

    // capture enriched events
    aetherBridge.EnableLogLineParser();
    _aetherbridge.LogLineCaptured += ParseEvent;
}

private void ParseEvent(object sender, LogLineEvent logLineEvent)
{
    // get data from log line
    if (logLineEvent?.XIVEvent == null) return;
    var actorName = logLineEvent.XIVEvent.Actor.Name;
    var itemName = logLineEvent.XIVEvent.Item.Name;
}

```

## How To Update for Patch

### 1. Update impacted dependencies.

* Ravahn's FFXIV Parsing Plugin
* Crescent Cove

### 2. Push changes to GitHub.

```shell
git checkout -b patch-5.1
git add .
git commit -m "update for patch 5.1"
git push --set-upstream origin patch-5.1
```

### 3. Create PR to merge into master.

* <a href="https://help.github.com/en/desktop/contributing-to-projects/creating-a-pull-request">Create a Pull Request</a>

## Software Used

* <a href="https://github.com/EQAditu/AdvancedCombatTracker">Advanced Combat Tracker</a>
* <a href="https://github.com/ravahn/FFXIV_ACT_Plugin">FFXIV_ACT_Plugin</a>
* <a href="https://github.com/ufx/SaintCoinach">SaintCoinach</a>
* <a href="https://github.com/kalilistic/CrescentCove">CrescentCove</a>
* <a href="https://xivapi.com/docs/Icons">XIVAPI (for repo icon)</a>



## How To Contribute

Feel free to open an issue or submit a PR.