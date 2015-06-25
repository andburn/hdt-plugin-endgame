using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using HDT.Plugins.EndGame.Properties;
using Hearthstone_Deck_Tracker.Stats;
using System.Drawing.Drawing2D;

namespace HDT.Plugins.EndGame
{
	public static class EndGame
	{
		public static void ScreenShot()
		{
			// load settigns
			string outputDir = Settings.Default.OutputDir;
			int delay = Settings.Default.Delay * 1000;
			int numImages = Settings.Default.NumberOfImages;

			// select method
			if (Settings.Default.UseAdvancedShot)
			{
				Advanced(delay, outputDir, numImages);
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

		public static void Advanced(int delay, string dir, int num)
		{
			List<Screenshot> screenshots = new List<Screenshot>();
				
			Logger.WriteLine("Advanced Called with " + delay + " delay", "Gameshot");
			System.Threading.Thread.Sleep(delay);
			Logger.WriteLine("Delay Over", "Gameshot");
			for (int i = 0; i < num; i++)
			{
				Logger.WriteLine("Capture", "Gameshot");
				Bitmap img = CaptureScreenShot();
				Bitmap thb = ResizeImage(img);
				screenshots.Add(new Screenshot(img, ToMediaImage(thb)));
				Logger.WriteLine("Sleeping", "Gameshot");
				// TODO: config this, longer
				System.Threading.Thread.Sleep(500); 
			}

			new NoteDialog(Game.CurrentGameStats, screenshots);
		}

		private static Bitmap CaptureScreenShot()
		{
			// TODO: getting overlay in capture
			var rect = Helper.GetHearthstoneRect(false);
			var bmp = Helper.CaptureHearthstone(new Point(rect.X, rect.Y), rect.Width, rect.Height);
			return bmp;
		}
		
		// Based on: http://stackoverflow.com/a/2001692
		// "c# Image resizing to different size while preserving aspect ratio"
		// Resize and crop to 4:3
		private static Bitmap ResizeImage(Bitmap original)
		{
			double ratio = 4.0 / 3.0;
			int height = 100;
			int width = Convert.ToInt32(height * ratio);
			
			// crop image
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
			if (bmp == null) { return null; }

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
