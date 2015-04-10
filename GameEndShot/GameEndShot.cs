using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker;

namespace AndBurn.HDT.Plugins.GameEndShot
{
    public class GameEndShotPlugin : IPlugin
    {
        public string Name
        {
            get { return "GameEndShot"; }
        }

        public string Description
        {
            get { return "Saves an image of the game end screen."; }
        }

        public string ButtonText
        {
            get { return "Nothing"; }
        }

        public string Author
        {
            get { return "andburn"; }
        }

        public Version Version
        {
            get { return new Version(0, 0, 1); }
        }

        public void OnLoad()
        {
            GameEvents.OnGameEnd.Add(TakeScreenshot);
        }

        public void OnUnload()
        {
            // Nothing for now
        }

        public void OnUpdate()
        {
            // Nothing for now
        }

        public void OnButtonPress()
        {
            // Nothing for now
        }

        private void TakeScreenshot()
        {
            System.Threading.Thread.Sleep(5000);
            var wi = new WindowsInput.InputSimulator();
            wi.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SNAPSHOT);
        }

    }
}

