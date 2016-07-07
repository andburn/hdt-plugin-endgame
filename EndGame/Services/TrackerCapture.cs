using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Utilities;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility;
using Hearthstone_Deck_Tracker.Utility.Logging;

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
	}
}