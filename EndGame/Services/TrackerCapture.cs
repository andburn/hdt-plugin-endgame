using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Properties;
using HDT.Plugins.EndGame.Utilities;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility;
using Hearthstone_Deck_Tracker.Utility.Logging;
using CoreAPI = Hearthstone_Deck_Tracker.API.Core;

namespace HDT.Plugins.EndGame.Services
{
	public class TrackerCapture : IImageCaptureService
	{
		public async Task<ObservableCollection<Screenshot>> CaptureSequence(int delay, string dir, int num, int delayBetween)
		{
			Log.Info("Capture Screen @ " + delay + "/" + delayBetween);

			ObservableCollection<Screenshot> screenshots = new ObservableCollection<Screenshot>();

			// initial delay, after end of game is triggered
			await Task.Delay(delay);

			// take num screenshots
			for (int i = 0; i < num; i++)
			{
				Bitmap img = await CaptureScreenShot();
				if (img != null)
				{
					var thumb = img.ResizeImage();
					screenshots.Add(new Screenshot(img, thumb.ToMediaImage(), i + 1));
					Log.Debug($"Saving image #{i}");
					thumb.Save(@"E:\Dump\screen_" + i + ".bmp");
				}
				await Task.Delay(delayBetween);
			}

			return screenshots;
		}

		private static async Task<Bitmap> CaptureScreenShot()
		{
			var rect = Helper.GetHearthstoneRect(true);
			return await ScreenCapture.CaptureHearthstoneAsync(new Point(0, 0), rect.Width, rect.Height, altScreenCapture: true);
		}

		public async Task SaveImage(Screenshot screenshot)
		{
			if (screenshot != null)
			{
				var dir = Settings.Default.OutputDir;
				if (!Directory.Exists(dir))
				{
					Log.Info($"Output dir does not exist ({dir}), using desktop");
					dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				}
				var filename = DateTime.Now.ToString("dd.MM.yyyy_HH.mm");

				var stats = CoreAPI.Game?.CurrentGameStats;
				if (stats != null)
				{
					// save with game details
					var pattern = Settings.Default.FileNamePattern;
					NamingPattern np = null;
					if (!NamingPattern.TryParse(pattern, out np))
						Log.Info("Invalid file name pattern, using default");
					filename = np.Apply(stats.PlayerHero, stats.OpponentHero, stats.PlayerName, stats.OpponentName);
				}
				await SaveAsPng(screenshot.Full, Path.Combine(dir, filename));
			}
			else
			{
				throw new ArgumentNullException("Screenshot was null");
			}
		}

		private static async Task SaveAsPng(Bitmap bmp, string file)
		{
			Log.Info($"Saving screenshot to '{file}'");
			await Task.Run(() => bmp.Save(file + ".png", ImageFormat.Png));
		}
	}
}