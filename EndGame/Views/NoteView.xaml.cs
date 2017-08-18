using HDT.Plugins.EndGame.ViewModels;
using System.Windows.Controls;

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