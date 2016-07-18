## End Game
[![Build status](https://ci.appveyor.com/api/projects/status/oi4kpe3xiod62qw3?svg=true)](https://ci.appveyor.com/project/andburn/hdt-plugin-endgame)
[![Coverage Status](https://coveralls.io/repos/github/andburn/hdt-plugin-endgame/badge.svg?branch=master)](https://coveralls.io/github/andburn/hdt-plugin-endgame?branch=master)
[![GitHub release](https://img.shields.io/github/release/andburn/hdt-plugin-endgame.svg?maxAge=604800)](https://github.com/andburn/hdt-plugin-endgame/releases/latest)
[![Github Releases](https://img.shields.io/github/downloads/andburn/hdt-plugin-endgame/latest/total.svg?maxAge=604800)](https://github.com/andburn/hdt-plugin-endgame/releases/latest)

A [Hearthstone Deck Tracker](https://hsdecktracker.net/) plugin that adds some extra functionality to the built-in end of game note window.

---

## Install
- Download the [latest release](https://github.com/andburn/hdt-plugin-endgame/releases/latest) (*not the source code*)
- Unzip the contents into you Hearthstone Deck Tracker's plugins folder
- Enable the plugin in HDT's settings
- *Optional*: Use the plugin menu to import meta decks (see [archetypes](#archetypes))

## Features
The plugin has two main features, saving [screenshots](#screenshots) of the victory/defeat screen and writing your opponents deck [archetype](#archetypes) to Hearthstone Deck Tracker's (HDT) game note field. To do this it replaces HDT's built in end of game note dialog box.

![screens](http://i.imgur.com/WAlfayX.png)

### Archetypes
The archetypes module is designed to help players who like to keep track of their opponents deck types in HDT's game note field. It works by comparing your opponents deck against decks in HDT that have the tag *Archetype*.

![menu](http://i.imgur.com/KWokxXP.png)

To quickly add some archetype decks you can import the latest [TempoStorm](https://tempostorm.com/hearthstone/meta-snapshot/) snapshot, by using the `Import Meta Decks` option from the plugin menu. These decks are archived by default, as it is recommended to not use them as regular decks in HDT. You can also add your own archetype decks to HDT as normal and just tag them with *Archetype*. It is also recommended to archive these decks and not use them as regular decks.

![decks](http://i.imgur.com/Cp35Wcv.png)

The archetype settings allow some customization of the module.

- The archetype function can be disabled using the toggle button at the top.
- *Auto archive decks*, imported meta decks will be archived (recommended)
- *Remove class from deck name*, when importing decks change `Control Warrior` to just `Control`
- *Delete previously imported decks*, when enabled, using `Import Meta Decks` will delete any previous meta decks the plugin has imported to avoid duplication (recommended).

### Screenshots
When the screenshot module is enabled it will take a series of screenshots at the end of a game allowing you to select one to save.

There is a time delay between HDT detecting the end of a game and the actual display of the Victory/Defeat screen. This delay can vary depending on the game and game mode. The settings may need to be tweaked to better suit your system.

![screen_settings](http://i.imgur.com/8r7u6DF.png)

- The screenshot function can be disabled using the toggle button at the top.
- *Delay* specifies the number of seconds to wait from the detected end of game to the actual display on screen.
- *Image Number* is the number of images to take in sequence. (If this is set to 1 the end game dialog is not shown, but the screenshot is still saved).
- *Spacing* is the time to wait in between each image snapshot, in milliseconds (1000 ms = 1 second).
- *Set Output Directory* allows you to set the default save directory for screen shots (defaults to the Desktop).
- *Filename Pattern* customization of screenshot filename (see below).
- *Game Modes* selection of game modes that you want to capture end game screenshots.

#### File Naming Pattern
The file name pattern accepts the following special strings:
- **{PlayerName}** Your Battle.net username.
- **{PlayerClass}** The class you are currently playing.
- **{OpponentName}** The opponents username.
- **{OpponentClass}** The opponents class.
- **{Date:<date_format>}** The date/time when the screenshot was taken, uses the standard date formatting patterns.

**Example**: the default pattern is of the following format:
```
{PlayerName} ({PlayerClass}) VS {OpponentName} ({OpponentClass}) {Date:dd.MM.yyyy_HH.mm}
```
This would make the file name be something like:
```
Player1 (Mage) VS Player2 (Warlock) 12.08.2015_22:00.png
```

---

## License
The plugin is licensed under the terms of the MIT license. The full license text is available in the LICENSE file.

## Attribution
This plugin uses [IcoMoon](https://icomoon.io/) free font icons licensed under [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/)
