using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace HDT.Plugins.EndGame.Utilities
{
	public static class BitmapExtensions
	{
		// Based on: http://stackoverflow.com/a/2001692
		// "c# Image resizing to different size while preserving aspect ratio"
		// Resize and crop to 4:3
		public static Bitmap ResizeImage(this Bitmap original)
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
		public static BitmapImage ToMediaImage(this Bitmap bmp)
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