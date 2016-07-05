using System.Drawing;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace HDT.Plugins.EndGame.Models
{
	public class Screenshot : ObservableObject
	{
		private bool isSelected;

		public Bitmap Full { get; private set; }
		public BitmapImage Thumbnail { get; private set; }

		private bool _isSelected;

		public bool IsSelected
		{
			get { return _isSelected; }
			set { Set(() => IsSelected, ref _isSelected, value); }
		}

		public Screenshot(Bitmap image, BitmapImage thumb)
		{
			Full = image;
			Thumbnail = thumb;
			IsSelected = false;
		}
	}
}