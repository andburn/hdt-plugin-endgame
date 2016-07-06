using System.Drawing;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace HDT.Plugins.EndGame.Models
{
	public class Screenshot : ObservableObject
	{
		public Bitmap Full { get; private set; }
		public BitmapImage Thumbnail { get; private set; }

		private bool _isSelected;

		public bool IsSelected
		{
			get { return _isSelected; }
			set { Set(() => IsSelected, ref _isSelected, value); }
		}

		private int _index;

		public int Index
		{
			get { return _index; }
			set { Set(() => Index, ref _index, value); }
		}

		public Screenshot(Bitmap image, BitmapImage thumb, int index)
		{
			Index = index;
			Full = image;
			Thumbnail = thumb;
			IsSelected = false;
		}
	}
}