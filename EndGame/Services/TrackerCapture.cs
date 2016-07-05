using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using HDT.Plugins.EndGame.Models;
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
					var thumb = ResizeImage(img);
					screenshots.Add(new Screenshot(img, ToMediaImage(thumb)));
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

		// Based on: http://stackoverflow.com/a/2001692
		// "c# Image resizing to different size while preserving aspect ratio"
		// Resize and crop to 4:3
		private static Bitmap ResizeImage(Bitmap original)
		{
			double ratio = 4.0 / 3.0;
			int height = 100;
			int width = Convert.ToInt32(height * ratio);

			int cropWidth = Convert.ToInt32(original.Height * ratio);
			int posX = Convert.ToInt32((original.Width - cropWidth) / 2);

			Image thumbnail = new Bitmap(width, height);
			Graphics graphic = Graphics.FromImage(thumbnail);
			graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphic.SmoothingMode = SmoothingMode.HighQuality;
			graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphic.CompositingQuality = CompositingQuality.HighQuality;

			graphic.DrawImage(original,
				new Rectangle(0, 0, width, height),
				new Rectangle(posX, 0, cropWidth, original.Height),
				GraphicsUnit.Pixel);

			graphic.Dispose();

			return new Bitmap(thumbnail);
		}

		// Based on: http://stackoverflow.com/a/3427387
		// "using XAML to bind to a System.Drawing.Image into a System.Windows.Image control"
		// Convert a bitmap to a Bitmap Image to be used as XAML Image source
		private static BitmapImage ToMediaImage(Bitmap bmp)
		{
			if (bmp == null)
				return null;

			var image = (Image)bmp;

			var bitmap = new BitmapImage();
			bitmap.BeginInit();
			MemoryStream memoryStream = new MemoryStream();
			// Save to a memory stream
			image.Save(memoryStream, ImageFormat.Bmp);
			// Rewind the stream
			memoryStream.Seek(0, SeekOrigin.Begin);
			bitmap.StreamSource = memoryStream;
			bitmap.EndInit();

			return bitmap;
		}
	}
}