using System.Linq;
using HDT.Plugins.EndGame.Archetype;
using HDT.Plugins.EndGame.Controls;

namespace HDT.Plugins.EndGame.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private ArchetypeManager _manager;
		//private PlayedDeck _opponentDeck;

		public MainWindow()
		{
			InitializeComponent();

			_manager = ArchetypeManager.Instance;
			_manager.LoadDecks();

			DeckView.DataContext = new ArchetypeDeckViewModel(_manager.Decks.First());

			//// TODO watch for changes from "Toast"
			//_opponentDeck = new PlayedDeck(game.OpponentHero, game.Format ?? Hearthstone_Deck_Tracker.Enums.Format.All, game.Turns, game.OpponentCards);
			//// TODO also add format here, deal with "All"
			//ArchetypeSelection.ItemsSource = _manager.Decks.Select(d => d.Klass == _opponentDeck.Klass);
			//ArchetypeSelection.SelectedIndex = 0;
			//// TODO some indication of no match
			//var bestMatch = _manager.Find(_opponentDeck).FirstOrDefault();
			//if (ArchetypeSelection.Items.Contains(bestMatch))
			//	ArchetypeSelection.SelectedItem = bestMatch;
		}

		//internal void SetOpponentDeck(List<TrackedCard> cards)
		//{
		//	var deck = new Hearthstone_Deck_Tracker.Hearthstone.Deck();
		//	foreach (var c in cards)
		//	{
		//		var existing = deck.Cards.FirstOrDefault(x => x.Id == c.Id);
		//		if (existing != null)
		//		{
		//			existing.Count++;
		//			continue;
		//		}
		//		var card = Database.GetCardFromId(c.Id);
		//		card.Count = c.Count;
		//		deck.Cards.Add(card);
		//		if (string.IsNullOrEmpty(deck.Class) && !string.IsNullOrEmpty(card.PlayerClass))
		//			deck.Class = card.PlayerClass;
		//	}
		//	//SetDeck(deck, showImportButton);
		//	//_deck = deck;
		//	PlayedDeck.Items.Clear();
		//	foreach (var card in deck.Cards.ToSortedCardList())
		//		PlayedDeck.Items.Add(card);
		//	Helper.SortCardCollection(PlayedDeck.Items, false);
		//}

		//internal void SetArchetypeDeck(List<ArchetypeDeck> decks)
		//{
		//	if (decks.Count > 0)
		//	{
		//		var cards = decks.First().Cards;
		//		var deck = new Hearthstone_Deck_Tracker.Hearthstone.Deck();
		//		foreach (var c in cards)
		//		{
		//			var existing = deck.Cards.FirstOrDefault(x => x.Id == c.Id);
		//			if (existing != null)
		//			{
		//				existing.Count++;
		//				continue;
		//			}
		//			var card = Database.GetCardFromId(c.Id);
		//			card.Count = c.Count;
		//			deck.Cards.Add(card);
		//			if (string.IsNullOrEmpty(deck.Class) && !string.IsNullOrEmpty(card.PlayerClass))
		//				deck.Class = card.PlayerClass;
		//		}
		//		//SetDeck(deck, showImportButton);
		//		//_deck = deck;
		//		ArchetypeDeck.Items.Clear();
		//		foreach (var card in deck.Cards.ToSortedCardList())
		//			ArchetypeDeck.Items.Add(card);
		//		Helper.SortCardCollection(ArchetypeDeck.Items, false);
		//	}
		//}
	}
}