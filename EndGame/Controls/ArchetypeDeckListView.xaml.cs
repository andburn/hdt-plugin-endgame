using System.Windows;
using System.Windows.Controls;
using HDT.Plugins.EndGame.Archetype;

namespace HDT.Plugins.EndGame.Controls
{
	public partial class ArchetypeDeckListView : UserControl
	{
		private ArchetypeManager _manager;
		private ArchetypeDeckListViewModel _viewModel;

		public ArchetypeDeck SelectedDeck { get; set; }

		public ArchetypeDeckListView()
		{
			InitializeComponent();
			_manager = ArchetypeManager.Instance;
			_manager.LoadDecks();

			//DeckClassFilterSelection.ItemsSource = Enum.GetValues(typeof(PlayerClass));
			//DeckClassFilterSelection.SelectedItem = PlayerClass.ANY;
			//DeckFormatFilterSelection.ItemsSource = Enum.GetValues(typeof(GameFormat));
			//DeckFormatFilterSelection.SelectedItem = GameFormat.ANY;

			_viewModel = new ArchetypeDeckListViewModel(_manager.Decks);
			DataContext = _viewModel;

			//DeckList.ItemsSource = _manager.Decks;
			//var deck = _manager.Decks.FirstOrDefault();
			//if (deck != null)
			//{
			//	DeckList.SelectedItem = SelectedDeck = deck;
			//	RaiseDeckSelectEvent();
			//}
		}

		public static readonly RoutedEvent DeckSelectEvent = EventManager.RegisterRoutedEvent(
			"DeckSelect", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ArchetypeDeckListView));

		public event RoutedEventHandler DeckSelect
		{
			add { AddHandler(DeckSelectEvent, value); }
			remove { RemoveHandler(DeckSelectEvent, value); }
		}

		private void RaiseDeckSelectEvent()
		{
			RoutedEventArgs newEventArgs = new RoutedEventArgs(DeckSelectEvent);
			RaiseEvent(newEventArgs);
		}

		private void DeckList_SelectionChanged(object sender, RoutedEventArgs e)
		{
			ListBox box = sender as ListBox;
			if (box != null)
				SelectedDeck = _viewModel.GetDeck(box.SelectedItem);

			RaiseDeckSelectEvent();
		}

		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			var newDeck = new ArchetypeDeck();
			newDeck.Name = "New Deck";
			_viewModel.AddDeck(newDeck);
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
		}
	}
}