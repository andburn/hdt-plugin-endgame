using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using HDT.Plugins.EndGame.Properties;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Stats;
using Hearthstone_Deck_Tracker.Utility;
using Hearthstone_Deck_Tracker.Utility.Logging;
using DrawImage = System.Drawing.Image;
using ScreenImage = HDT.Plugins.EndGame.Screenshot.Image;

namespace HDT.Plugins.EndGame.Screenshot
{
	public class Capture
	{
		// Take a screenshot using the print screen button
		public static async Task Simple(int delay)
		{
			Log.Info("Capture (Simple) @ " + delay, "EndGame");
			await Task.Delay(delay);
			var sim = new WindowsInput.InputSimulator();
			sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SNAPSHOT);
		}

		// Take a series of screenshots and display them so one can be selected to be saved
		public static async Task Advanced(int delay, string dir, int num, int delayBetween)
		{
			Log.Info("Capture (Advanced) @ " + delay + "/" + delayBetween, "EndGame");

			List<ScreenImage> screenshots = new List<ScreenImage>();

			await Task.Delay(delay);
			// disable overlay, before captures
			ForceHideOverlay(true);
			// take num screenshots
			for (int i = 0; i < num; i++)
			{
				Bitmap img = CaptureScreenShot();
				if (img != null)
				{
					Bitmap thb = ResizeImage(img);
					screenshots.Add(new ScreenImage(img, ToMediaImage(thb)));
					await Task.Delay(delayBetween);
				}
				else
				{
					Log.Info("Capture failed, reverting to Simple mode.", "EndGame");
					await Simple(delay);
				}
			}
			// enable overlay
			ForceHideOverlay(false);

			// copy the reference, TODO: clone?
			var cstats = Hearthstone_Deck_Tracker.API.Core.Game.CurrentGameStats;

			// Bit of a hack to allow selecting of game mode
			await GameModeDectection(30);
			await RankedDectection(20);

			if (IsSetToCapture())
			{
				if (screenshots.Count == 1)
				{
					// only one image, no need for dialog
					// TODO: would mean no dialog if it was expected, however
					SaveImage(cstats, screenshots[0]);
				}
				else
				{
					new NoteDialog(cstats, screenshots);
				}
			}
		}

		public static void SaveImage(GameStats game, Image screenshot, String note = null)
		{
			if (game != null)
			{
				if (!String.IsNullOrEmpty(note))
					game.Note = note;
				DeckStatsList.Save();

				//if(Config.Instance.StatsInWindow)
				//{
				//	((DeckStatsControl)Hearthstone_Deck_Tracker.Core.Windows.StatsWindow.FindName("StatsControl")).Refresh();
				//}
				//else
				//{
				//	((DeckStatsControl)Hearthstone_Deck_Tracker.API.Core.MainWindow.FindName("DeckStatsFlyout")).Refresh();
				//}

				if (screenshot != null)
				{
					try
					{
						var dir = Settings.Default.OutputDir;
						if (!Directory.Exists(dir))
						{
							dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
						}
						var pattern = Settings.Default.FileNamePattern;
						NamingPattern np = null;
						if (!NamingPattern.TryParse(pattern, out np))
							Log.Info("Invalid file name pattern, using default", "EndGame");
						var filename = np.Apply(game);
						SaveAsPng(screenshot.Full, Path.Combine(dir, filename));
					}
					catch (Exception e)
					{
						Log.Info("Error saving image: " + e.Message, "EndGame");
					}
				}
				else
				{
					Log.Info("Screenshot is null", "EndGame");
				}
			}
			else
			{
				Log.Info("Game is null", "EndGame");
			}
		}

		private static void SaveAsPng(Bitmap bmp, string filename)
		{
			bmp.Save(Path.Combine(Settings.Default.OutputDir, filename) + ".png", ImageFormat.Png);
		}

		private static void ForceHideOverlay(bool force = true)
		{
			Hearthstone_Deck_Tracker.Core.Overlay.ForceHidden = force;
			Hearthstone_Deck_Tracker.Core.Overlay.UpdatePosition();
		}

		// check if the currrent game mode is to be captured
		private static bool IsSetToCapture()
		{
			return Hearthstone_Deck_Tracker.API.Core.Game.CurrentGameMode == GameMode.None && Settings.Default.RecordOther
				|| Hearthstone_Deck_Tracker.API.Core.Game.CurrentGameMode == GameMode.Practice && Settings.Default.RecordPractice
				|| Hearthstone_Deck_Tracker.API.Core.Game.CurrentGameMode == GameMode.Arena && Settings.Default.RecordArena
				|| Hearthstone_Deck_Tracker.API.Core.Game.CurrentGameMode == GameMode.Brawl && Settings.Default.RecordBrawl
				|| Hearthstone_Deck_Tracker.API.Core.Game.CurrentGameMode == GameMode.Ranked && Settings.Default.RecordRanked
				|| Hearthstone_Deck_Tracker.API.Core.Game.CurrentGameMode == GameMode.Friendly && Settings.Default.RecordFriendly
				|| Hearthstone_Deck_Tracker.API.Core.Game.CurrentGameMode == GameMode.Casual && Settings.Default.RecordCasual;
		}

		private static Bitmap CaptureScreenShot()
		{
			var rect = Helper.GetHearthstoneRect(true);
			var bmp = ScreenCapture.CaptureHearthstone(new Point(0, 0), rect.Width, rect.Height);
			return bmp;
		}

		// Hacky method to wait on game mode
		private static async Task GameModeDectection(int timeoutInSeconds)
		{
			int seconds = 0;
			while (Hearthstone_Deck_Tracker.API.Core.Game.CurrentGameMode == GameMode.None && seconds < timeoutInSeconds)
			{
				await Task.Delay(1000);
				seconds++;
			}
		}

		// Hacky method to wait on ranked mode
		private static async Task RankedDectection(int timeoutInSeconds)
		{
			int seconds = 0;
			while (Hearthstone_Deck_Tracker.API.Core.Game.CurrentGameMode == GameMode.Casual && seconds < timeoutInSeconds)
			{
				await Task.Delay(1000);
				seconds++;
			}
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

			DrawImage thumbnail = new Bitmap(width, height);
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

			var image = (DrawImage)bmp;

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