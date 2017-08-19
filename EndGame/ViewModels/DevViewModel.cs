using GalaSoft.MvvmLight.Command;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Utils;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using HDTCard = Hearthstone_Deck_Tracker.Hearthstone.Card;
using HDTDb = Hearthstone_Deck_Tracker.Hearthstone.Database;

// TODO dev mode resize window
// TODO clear spare cards on start
// TODO when not watching select should show deck list
// TODO replace stars with numbers
// TODO add tooltips
// TODO sort by count then mana
// TODO re compare archs on all updates?
// TODO stop should reset

namespace HDT.Plugins.EndGame.ViewModels
{
	public class DevViewModel : BasicNoteViewModel
	{
		private static object _deckLock = new object();

		private IDataRepository _repository;
		private ILoggingService _log;

		public ObservableCollection<MatchResult> Decks { get; set; }

		public ObservableCollection<Card> SpareCards { get; set; }

		private MatchResult _selectedDeck;

		public MatchResult SelectedDeck
		{
			get { return _selectedDeck; }
			set { Set(() => SelectedDeck, ref _selectedDeck, value); }
		}

		private bool _isLoadingDecks;

		public bool IsLoadingDecks
		{
			get { return _isLoadingDecks; }
			set { Set(() => IsLoadingDecks, ref _isLoadingDecks, value); }
		}

		private List<ArchetypeDeck> _archetypes;

		private List<ArchetypeDeck> ArchetypeDecks
		{
			get
			{
				if (_archetypes == null || _archetypes.Count <= 0)
					_archetypes = _repository
						.GetAllDecksWithTag(Strings.ArchetypeTag)
						.Select(d => new ArchetypeDeck(d))
						.ToList();
				return _archetypes;
			}
			set
			{
				_archetypes = value;
			}
		}

		private bool _isWatching;

		public bool IsWatching
		{
			get { return _isWatching; }
			set { Set(() => IsWatching, ref _isWatching, value); }
		}

		private string _watchButtonText;

		public string WatchButtonText
		{
			get { return _watchButtonText; }
			set { Set(() => WatchButtonText, ref _watchButtonText, value); }
		}

		public RelayCommand WatchButtonClick { get; private set; }

		public DevViewModel()
			: this(EndGame.Data, EndGame.Logger)
		{
		}

		public DevViewModel(IDataRepository track, ILoggingService logger)
			: base(track, logger)
		{
			Cards = new ObservableCollection<Card>();
			SpareCards = new ObservableCollection<Card>();
			Decks = new ObservableCollection<MatchResult>();
			BindingOperations.EnableCollectionSynchronization(Decks, _deckLock);
			_repository = track;
			_log = logger;
			WatchButtonClick = new RelayCommand(() => ToggleWatching());
			IsWatching = false;
			WatchButtonText = "Watch";
			PropertyChanged += DevViewModel_PropertyChanged;
			// Set events
			Hearthstone_Deck_Tracker.API.GameEvents.OnOpponentPlay.Add(x => Played(x));
			Hearthstone_Deck_Tracker.API.GameEvents.OnTurnStart.Add(x => TurnStart(x));
			Hearthstone_Deck_Tracker.API.GameEvents.OnGameStart.Add(() => GameStart());
		}

		public override async Task Update()
		{
			IsNoteFocused = false;
			Deck deck = _repository.GetOpponentDeckLive();
			PlayerClass = deck.Class.ToString();

			if (deck.Cards.Count <= 0)
			{
				EndGame.Logger.Debug("DevVM: Opponent deck is empty, skipping");
				IsLoadingDecks = false;
				return;
			}
			// if watching don't rerun archtype comparison
			if (!IsWatching)
			{
				Decks.Clear();
				IsLoadingDecks = true;
				await Task.Run(() =>
				{
					var results = ViewModelHelper.MatchArchetypes(
							EndGame.Data.GetGameFormat(), deck, ArchetypeDecks);
					results.ForEach(r => Decks.Add(r));
					results.ForEach(r => _log.Debug($"Archetype: ({r.Similarity}, {r.Containment}) " +
						$"{r.Deck.DisplayName} ({r.Deck.Class})"));
					IsLoadingDecks = false;
				});
			}

			var archDeck = SelectedDeck ?? Decks.FirstOrDefault();

			if (IsWatching && archDeck != null)
			{
				PlayerClass = archDeck.Deck.Name.ToUpper();
				Common.Common.Log.Debug($"DevVM: watching & deck selected '{archDeck.Deck.Name}'");
				var lookup = deck.Cards.ToDictionary(x => x.Id);

				Cards.Clear();
				SpareCards.Clear();
				var found = new List<string>();
				foreach (var card in archDeck.Deck.Cards)
				{
					var c = HDTDb.GetCardFromId(card.Id);
					if (lookup.ContainsKey(card.Id))
					{
						// TODO handle found case of having 1 copy in archetype and 2 played
						found.Add(card.Id);
						c.Count = card.Count - lookup[card.Id].Count;
						Common.Common.Log.Debug($"DevVM: {card.Id} count was {card.Count} now {c.Count}");
					}
					Cards.Add(new Card(c.Id, c.Name, c.Count, c.Background));
				}
				found.ForEach(k => lookup.Remove(k));
				lookup.Values.ToList().ForEach(v => SpareCards.Add(v));
			}
			else
			{
				// just show opponent deck straight
				Common.Common.Log.Debug($"DevVM: not watching, showing played cards");
				Cards.Clear();
				deck.Cards.ForEach(c => Cards.Add(c));
			}
		}

		private void ToggleWatching()
		{
			IsWatching = !IsWatching;
			Common.Common.Log.Debug($"DevVM: toggled {IsWatching}");
			WatchButtonText = IsWatching ? "Stop" : "Watch";
			Common.Common.Log.Debug($"DevVM: text {WatchButtonText}");
		}

		private void Played(HDTCard c)
		{
			Common.Common.Log.Debug($"DevVM: Played {c.Name}");
			EventTrigger();
		}

		private void TurnStart(Hearthstone_Deck_Tracker.Enums.ActivePlayer p)
		{
			Common.Common.Log.Debug($"DevVM: TurnStart {p}");
			EventTrigger();
		}

		private void GameStart()
		{
			Common.Common.Log.Debug($"DevVM: GameStart");
			_archetypes.Clear();
			if (IsWatching)
				ToggleWatching();
		}

		private void EventTrigger()
		{
			Common.Common.Log.Debug($"DevVM: Event Triggered");
			Update().Forget();
		}

		private void DeckSelected(MatchResult item)
		{
			SelectedDeck = item ?? SelectedDeck;
			if (SelectedDeck != null)
				Update().Forget();
		}

		private void DevViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "SelectedDeck")
			{
				DeckSelected(SelectedDeck);
				_log.Debug($"DevVM: DeckSelected ({SelectedDeck.Deck.DisplayName})");
			}
		}
	}
}