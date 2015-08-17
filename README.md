## End Game
A Hearthstone Deck Tracker plugin to automatically save a screenshot at the end of a game (i.e. the Victory/Defeat screen).

![EndGame](http://i.imgur.com/gXUHxTK.png)

**NOTE**: *This nature of this plugin means it will never be able to reliably capture the screenshot 100% of time. As it is still under testing, it may not work as expected and stats for a particular game may be lost.*

---

There is a time delay between Hearthstone Deck Tracker detecting the end of a game and the actual display of the Victory/Defeat screen. This delay can vary depending on the game and game mode. In order to take the screenshot the plugin needs to have a delay configured in the settings menu (see below).

The plugin has two modes:

1. **Simple**: automatically presses the *Print Screen* key to trigger the built-in Hearthstone screenshot mechanism. The accuracy is limited as it is entirely dependent on the delay setting.
- **Advanced**: uses the same delay as Simple mode but takes a series of screenshots and allows you to select the best one. This gives a better chance of getting the shot you want.

### Settings
- You may need to tweak these settings so the plugin works the way you want it to.

 ![Plugin Settings](http://i.imgur.com/X7H4NtO.png)

- The first setting *Delay* specifies the number of seconds to wait from the detected end of game to the actual display on screen.
- To use Advanced mode you check the *Advanced Mode* box.
- This enables additional options
 - *Image Number* is the number of images to take in sequence. (If this is set to 1 the end game dialog is not shown, but the screenshot is still saved).
 - *Spacing* is the time to wait in between each image snapshot, in milliseconds (1000 ms = 1 second).
 - *Filename Prefix* set the text to go in front of all saved images.
 - *Set Output Directory* allows you to set the default save directory for screen shots (defaults to the Desktop).
 - Selection of game modes that you want to capture end game screenshots.
 - Customization of screenshot filename
- To use simple mode you uncheck *Advanced Mode* and set the delay amount in seconds.

### Advanced Dialog
- In Advanced mode, the standard end of game note dialog is replaced with a new one that includes thumbnails of captured screenshots (unless you have set *Image Number* to 1 in the options).

 ![Advanced Dialog](http://i.imgur.com/Wb00EVh.png)

- You can add a note as normal, and now can click on an image that you want saved.
- You need to press *Set* or use the *Save with ENTER* option, to make sure the screenshot and game data are saved.

### File Naming Pattern
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
