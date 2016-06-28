using HDT.Plugins.EndGame.ViewModels;
using MahApps.Metro.Controls;

namespace HDT.Plugins.EndGame.Views
{
	public partial class NoteView : MetroWindow
	{
		public NoteView()
		{
			InitializeComponent();
			DataContext = new NoteViewModel();
		}
	}
}