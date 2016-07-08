using System.Windows;
using System.Windows.Input;
using HDT.Plugins.EndGame.Properties;
using MahApps.Metro.Controls;

namespace HDT.Plugins.EndGame.Views
{
	public partial class NoteView : MetroWindow
	{
		public NoteView()
		{
			InitializeComponent();
			NoteTextBox.Focus();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Width += ScreenshotList.ActualWidth;
			MinWidth += ScreenshotList.ActualWidth;
		}

		private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && Settings.Default.CloseNoteWithEnter)
			{
				e.Handled = true;
				Close();
			}
		}
	}
}