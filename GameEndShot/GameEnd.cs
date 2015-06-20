using GameEndShot.Properties;
using Hearthstone_Deck_Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDT.Plugins.GameEndShot
{
	public static class GameEnd
	{
		public static void ScreenShot()
		{
			// load config values
			string outputDir = Settings.Default.OutputDir;
			int delay = Settings.Default.Delay;

			// select method
			if (Settings.Default.UseAdvancedShot)
			{
				Advanced(delay, outputDir);
			}
			else
			{
				Simple(delay);				
			}
		}

		private static void Simple(int delay)
		{
			Logger.WriteLine("Simple Called with " + delay + " delay","Gameshot");
			System.Threading.Thread.Sleep(delay);
			Logger.WriteLine("Delay Over", "Gameshot");
			var wi = new WindowsInput.InputSimulator();
			wi.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SNAPSHOT);
		}

		private static void Advanced(int delay, string dir)
		{
			
		}

	}
}
