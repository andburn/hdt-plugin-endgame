using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Utilities
{
	public class DesignerData
	{
		public static ObservableCollection<Screenshot> GenerateScreenshots()
		{
			System.Windows.Resources.StreamResourceInfo sri = System.Windows.Application.GetResourceStream(
					new Uri("pack://application:,,,/EndGame;component/Resources/thumb_sample.bmp"));

			BitmapImage bmp = new BitmapImage();
			bmp.BeginInit();
			bmp.StreamSource = sri.Stream;
			bmp.EndInit();

			return new ObservableCollection<Screenshot>() {
					new Screenshot(null, bmp, 1),
					new Screenshot(null, bmp, 2),
					new Screenshot(null, bmp, 3),
					new Screenshot(null, bmp, 4)
				};
		}
	}
}