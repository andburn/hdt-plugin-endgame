using HDT.Plugins.EndGame.Properties;
using HDT.Plugins.EndGame.Screenshot;

namespace HDT.Plugins.EndGame
{
	public static class EndGame
	{
		// TODO: refactor to make async where necessary
		// TODO: investigate problem with no deck selected
		// TODO: add update github
		// TODO: adv screenshot off in window

		public async static void ScreenShot()
		{
			// load settigns
			string outputDir = Settings.Default.OutputDir;
			int delay = Settings.Default.Delay;
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
