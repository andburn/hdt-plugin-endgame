using System.Windows.Controls;
using HDT.Plugins.EndGame.ViewModels;

namespace HDT.Plugins.EndGame.Views
{
	public partial class NoteView : UserControl
	{
		public NoteView()
		{
			InitializeComponent();
			ViewModelHelper.FocusTextBox(NoteTextBox);
		}

		private void NoteTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			ViewModelHelper.FocusTextBox(NoteTextBox);
		}
	}
}