using System.Windows.Controls;
using System.Windows.Input;
using HDT.Plugins.EndGame.Utilities;

namespace HDT.Plugins.EndGame.Views
{
	public partial class NoteView : UserControl
	{
		public NoteView()
		{
			InitializeComponent();
			FocusNoteBox();
		}

		private void FocusNoteBox()
		{
			NoteTextBox.Focus();
			if (!string.IsNullOrEmpty(NoteTextBox.Text) && NoteTextBox.CaretIndex <= 0)
			{
				NoteTextBox.CaretIndex = NoteTextBox.Text.Length;
			}
		}

		private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && EndGame.Settings.Get(Strings.CloseNoteWithEnter).Bool)
			{
				e.Handled = true;
				// TODO close parent window
				//Close();
			}
		}

		private void NoteTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			FocusNoteBox();
		}
	}
}