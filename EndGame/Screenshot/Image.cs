using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace HDT.Plugins.EndGame.Screenshot
{
	public class Image : INotifyPropertyChanged
	{
		private Boolean isSelected;

		public Bitmap Full { get; private set; }
		public BitmapImage Thumbnail { get; private set; }
		public Boolean IsSelected {
			get 
			{
				return isSelected;
			}
			set
			{
				if (isSelected != value)
				{
					isSelected = value;
					NotifyPropertyChanged("IsSelected");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Image(Bitmap image, BitmapImage thumb)
		{
			Full = image;
			Thumbnail = thumb;
			IsSelected = false;
		}

		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
	}
}
