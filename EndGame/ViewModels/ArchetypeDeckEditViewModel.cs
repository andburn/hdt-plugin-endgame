using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.EndGame.Enums;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility.Logging;
using HDTCard = Hearthstone_Deck_Tracker.Hearthstone.Card;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class ArchetypeDeckEditViewModel : ViewModelBase
	{
		private List<HDTCard> _allCards;
		private CultureInfo _culture;
		private IArchetypeDecksRepository _repository;

		public ObservableCollection<HDTCard> Cards { get; private set; }
		public IEnumerable KlassList { get; private set; }
		public IEnumerable FormatList { get; private set; }

		private ArchetypeDeck _deck;

		public ArchetypeDeck Deck
		{
			get { return _deck; }
			set { Set(() => Deck, ref _deck, value); }
		}

		private List<HDTCard> _autoComplete;

		public List<HDTCard> AutoComplete
		{
			get { return _autoComplete; }
			set { Set(() => AutoComplete, ref _autoComplete, value); }
		}

		private Visibility _autoCompleteVisibile;

		public Visibility AutoCompleteVisibile
		{
			get { return _autoCompleteVisibile; }
			set { Set(() => AutoCompleteVisibile, ref _autoCompleteVisibile, value); }
		}

		private string _searchText;

		public string SearchText
		{
			get { return _searchText; }
			set { Set(() => SearchText, ref _searchText, value); }
		}

		public RelayCommand<HDTCard> DeleteCardCommand { get; private set; }
		public RelayCommand<HDTCard> AddCardCommand { get; private set; }
		public RelayCommand<string> SearchTextChangeCommand { get; private set; }

		public ArchetypeDeckEditViewModel()
		{
			_allCards = HearthDb.Cards.Collectible.Values.Select(x => new HDTCard(x)).ToList();
			_culture = GetCurrentCulture();
			_repository = RepositoryFactory.Create<IArchetypeDecksRepository>();

			var decks = _repository.GetAllDecks().Result;
			Deck = decks.FirstOrDefault();
			var cards = _deck.Cards.Select(x => new HDTCard(HearthDb.Cards.Collectible[x.Id]));
			Cards = new ObservableCollection<HDTCard>(cards);

			SearchText = null;
			AutoCompleteVisibile = Visibility.Collapsed;

			KlassList = Enum.GetValues(typeof(PlayerClass));
			FormatList = Enum.GetValues(typeof(GameFormat));

			DeleteCardCommand = new RelayCommand<HDTCard>(x => Cards.Remove(x));
			AddCardCommand = new RelayCommand<HDTCard>(x => AddCard(x));
			SearchTextChangeCommand = new RelayCommand<string>(x => UpdateAutoComplete(x));
		}

		private void AddCard(HDTCard card)
		{
			if (card != null)
				Cards.Add(card);
		}

		private void UpdateAutoComplete(string text)
		{
			AutoComplete = _allCards
				.Where(c =>
					(StringUpperEquals(c.GetPlayerClass, _deck.Klass) || StringUpperEquals(c.GetPlayerClass, "NEUTRAL"))
					&& CulturalCompare(c.LocalizedName, text, _culture) >= 0)
				.OrderBy(x => CulturalCompare(x.LocalizedName, text, _culture) == 0 ? 0 : 1)
				.ThenBy(x => x.LocalizedName)
				.ToList();

			if (AutoComplete.Count() <= 0)
				AutoCompleteVisibile = Visibility.Collapsed;
			AutoCompleteVisibile = Visibility.Visible;
		}

		internal void Update(ArchetypeDeck deck)
		{
			Deck = deck;
			var cards = _deck.Cards.Select(x => new HDTCard(HearthDb.Cards.Collectible[x.Id]));
			Cards.Clear();
			foreach (var c in cards)
			{
				Cards.Add(c);
			}

			UpdateAutoComplete("");
			SearchText = null;
			AutoCompleteVisibile = Visibility.Collapsed;
		}

		private CultureInfo GetCurrentCulture()
		{
			CultureInfo ci = CultureInfo.InvariantCulture;
			try
			{
				ci = new CultureInfo(Config.Instance.SelectedLanguage.Insert(2, "-"));
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
			return ci;
		}

		private int CulturalCompare(string a, string b, CultureInfo culture)
		{
			// language dependent case-insensitivity
			// http://stackoverflow.com/questions/444798/case-insensitive-containsstring/15464440#15464440
			return culture.CompareInfo.IndexOf(a, b, CompareOptions.IgnoreCase);
		}

		private bool StringUpperEquals(object a, object b)
		{
			return a.ToString().ToUpperInvariant().Equals(b.ToString().ToUpperInvariant());
		}
	}
}