## End Game
A plugin to automatically save a screenshot at the end of a game.

**NOTE: This plugin is in a beta testing phase. It may not work as expected and stats for a particular game may be lost.**

---

There is a time delay between Hearthstone Deck Tracker detecting the end of a game and the actual display of the Victory/Defeat screen. This delay can vary depending on the game and game mode. In order to take the screenshot the plugin needs to have a delay configured in the settings menu (see below).

The plugin has two modes:

1. **Simple**: automatically presses the *Print Screen* key to trigger the built-in Hearthstone screenshot mechanism. The accuracy is limited as it is entirely dependent on the delay setting.
- **Advanced**: uses the same delay as Simple mode but takes a series of screenshots and allows you to select the best one. This gives a better chance of getting the shot you want.

### Settings
- You may need to tweak these settings so the plugin works the way you want it to.

 ![Plugin Settings](http://i.imgur.com/XqjhSnA.png)

- The first setting *Delay* specifies the number of seconds to wait from the detected end of game to the actual display on screen.
- To use Advanced mode you check the *Advanced Mode* box.
- This enables additional options
 - *Image Number* is the number of images to take in sequence.
 - *Spacing* is the time to wait in between each image snapshot, in milliseconds (1000 ms = 1 second).
 - *Filename Prefix* set the text to go in front of all saved images.
 - *Set Output Directory* allows you to set the default save directory for screen shots (defaults to the Desktop).
- To use simple mode you uncheck *Advanced Mode* and set the delay amount in seconds.

### Advanced Dialog
- In Advanced mode, the standard end of game note dialog is replaced with a new one that includes thumbnails of captured screenshots.

 ![Advanced Dialog](http://i.imgur.com/Wb00EVh.png)

- You can add a note as normal, and now can click on an image that you want saved.
- You need to press *Set* or use the *Save with ENTER* option, to make sure the screenshot and game data are saved.
