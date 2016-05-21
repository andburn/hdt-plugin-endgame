using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using HDT.Plugins.EndGame.Archetype;
using HDT.Plugins.EndGame.Enums;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker;
using HDTCard = Hearthstone_Deck_Tracker.Hearthstone.Card;

namespace HDT.Plugins.EndGame.Controls
{
	public class ArchetypeDeckViewModel
	{
		private ArchetypeDeck _deck;
		private CultureInfo _culture;
		private ObservableCollection<HDTCard> _cards;
		private List<HDTCard> _viableCards;

		public ArchetypeDeckViewModel(ArchetypeDeck deck)
		{
			_deck = deck;

			_cards = new ObservableCollection<HDTCard>(_deck.Cards.Select(x => new HDTCard(HearthDb.Cards.Collectible[x.Id])));

			_culture = new CultureInfo(Config.Instance.SelectedLanguage.Insert(2, "-"));

			_viableCards = new List<HDTCard>();
			UpdateViableList();
		}

		private void UpdateViableList()
		{
			_viableCards = HearthDb.Cards.Collectible.Values
				.Where(c => (int)c.Class == (int)Klass || c.Class == CardClass.NEUTRAL)
				.Select(c => new HDTCard(c)).ToList();
		}

		public List<HDTCard> ViableCardSearch(string text)
		{
			// language dependent case-insensitivity
			// http://stackoverflow.com/questions/444798/case-insensitive-containsstring/15464440#15464440)
			var predictions = _viableCards.Where(x =>
				_culture.CompareInfo.IndexOf(x.LocalizedName, text, CompareOptions.IgnoreCase) >= 0).ToList();
			return predictions;
		}

		public string Name
		{
			get { return _deck.Name; }
			set { _deck.Name = value; }
		}

		public PlayerClass Klass
		{
			get { return _deck.Klass; }
			set { _deck.Klass = value; UpdateViableList(); }
		}

		public GameFormat Format
		{
			get { return _deck.Format; }
			set { _deck.Format = value; }
		}

		public ObservableCollection<HDTCard> Cards
		{
			get
			{
				return _cards;
			}
		}

		public List<HDTCard> ViableCards
		{
			get
			{
				return _viableCards;
			}
		}

		public void AddCard(HDTCard card)
		{
			if (!_cards.Contains(card))
			{
				_cards.Add(card);
				_deck.Cards.Add(new SingleCard(card.Id));
			}
		}

		public void RemoveCard(HDTCard card)
		{
			_cards.Remove(card);
			_deck.Cards.Remove(new SingleCard(card.Id));
		}
	}
}