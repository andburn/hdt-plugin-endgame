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