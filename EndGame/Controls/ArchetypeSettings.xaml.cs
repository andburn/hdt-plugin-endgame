using System.Windows.Controls;

namespace HDT.Plugins.EndGame.Controls
{
	public partial class ArchetypeSettings : UserControl
	{
		public ArchetypeSettings()
		{
			InitializeComponent();

			if (ArchetypeDeckList.SelectedDeck != null)
			{
				ArchetypeDeck.DataContext = new ArchetypeDeckViewModel(ArchetypeDeckList.SelectedDeck);
			}
		}

		private void ArchetypeDeckList_DeckSelect(object sender, System.Windows.RoutedEventArgs e)
		{
			var d = sender as ArchetypeDeckListView;
			if (d != null)
			{
				ArchetypeDeck.DataContext = new ArchetypeDeckViewModel(d.SelectedDeck);
			}
		}
	}
}