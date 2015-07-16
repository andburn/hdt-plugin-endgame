using System;
using HDT.Plugins.EndGame.Properties;
using HDT.Plugins.EndGame.Screenshot;

namespace HDT.Plugins.EndGame
{
	public static class EndGame
	{
		private static DateTime lastCall;

		public async static void ScreenShot()
		{
			// multiple calls may occur, only process one in a time span
			if(lastCall != null)
			{
				var diff = DateTime.Now - lastCall;
				lastCall = DateTime.Now;
				if(diff.TotalSeconds < 20)
					return;
			}
			lastCall = DateTime.Now;

			// load settigns
			string outputDir = Settings.Default.OutputDir;
			// stored as seconds, used in millis
			int delay = Settings.Default.Delay * 1000;
			int numImages = Settings.Default.NumberOfImages;
			int delayBetween = Settings.Default.DelayBetweenShots;

			// select method
			if (Settings.Default.UseAdvancedShot)
			{
				await Capture.Advanced(delay, outputDir, numImages, delayBetween);
			}
			else
			{
				await Capture.Simple(delay);				
			}
		}

	}
}
