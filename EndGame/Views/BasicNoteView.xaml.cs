using System.Windows.Input;
using HDT.Plugins.EndGame.Properties;
using MahApps.Metro.Controls;

namespace HDT.Plugins.EndGame.Views
{
	public partial class BasicNoteView : MetroWindow
	{
		public BasicNoteView()
		{
			InitializeComponent();
			NoteTextBox.Focus();
		}

		private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && EndGame.Settings.Get("ScreenShot", "CloseNoteWithEnter").Bool)
			{
				e.Handled = true;
				Close();
			}
		}
	}
}