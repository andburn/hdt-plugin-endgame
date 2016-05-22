using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;
using HDTCard = Hearthstone_Deck_Tracker.Hearthstone.Card;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class ArchetypeDeckViewModel : ViewModelBase
	{
		private IArchetypeDecksRepository _repository = new ArchetypeDecksFileRepository();

		private ArchetypeDeck _deck;

		public ArchetypeDeck Deck
		{
			get { return _deck; }
			set { Set(() => Deck, ref _deck, value); }
		}

		public ObservableCollection<HDTCard> Cards { get; private set; }

		public ArchetypeDeckViewModel()
		{
			var decks = _repository.GetAllDecks().Result;
			Deck = decks.FirstOrDefault();
			var cards = _deck.Cards.Select(x => new HDTCard(HearthDb.Cards.Collectible[x.Id]));
			Cards = new ObservableCollection<HDTCard>(cards);
		}

		internal void Update(ArchetypeDeck deck)
		{
			Deck = deck;
		}
	}
}