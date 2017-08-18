using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class DevViewModel : BasicNoteViewModel
	{
		private static object _deckLock = new object();

		private IDataRepository _repository;
		private ILoggingService _log;

		public ObservableCollection<MatchResult> Decks { get; set; }

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

		public DevViewModel()
			: this(EndGame.Data, EndGame.Logger)
		{
		}

		public DevViewModel(IDataRepository track, ILoggingService logger)
			: base(track, logger)
		{
			Cards = new ObservableCollection<Card>();
			Decks = new ObservableCollection<MatchResult>();
			BindingOperations.EnableCollectionSynchronization(Decks, _deckLock);
			_repository = track;
			_log = logger;
			PropertyChanged += NoteViewModel_PropertyChanged;
		}

		public override async Task Update()
		{
			IsNoteFocused = true;
			IsLoadingDecks = true;
			Note = _repository.GetGameNote()?.ToString();

			Deck deck = _repository.GetOpponentDeckLive();;			

			Decks.Clear();
			if (Cards.Count <= 0)
			{
				EndGame.Logger.Debug("DevVM: Opponent deck is empty, skipping");
				IsLoadingDecks = false;
				return;
			}
			await Task.Run(() =>
			{
				var alldecks = _repository
					.GetAllDecksWithTag(Strings.ArchetypeTag)
					.Select(d => new ArchetypeDeck(d))
					.ToList();
				var format = EndGame.Data.GetGameFormat();
				var results = ViewModelHelper.MatchArchetypes(format, deck, alldecks);
				results.ForEach(r => Decks.Add(r));
				results.ForEach(r => _log.Debug($"Archetype: ({r.Similarity}, {r.Containment}) " +
					$"{r.Deck.DisplayName} ({r.Deck.Class})"));				
				IsLoadingDecks = false;
			});

			var firstDeck = Decks.FirstOrDefault();
			if (firstDeck != null && firstDeck.Similarity > MatchResult.THRESHOLD)
				DeckSelected(firstDeck);

			Cards.Clear();
			deck.Cards.ForEach(c => Cards.Add(c));
			PlayerClass = deck.Class.ToString();
		}

		private void DeckSelected(MatchResult item)
		{
			SelectedDeck = item ?? SelectedDeck;
			if (SelectedDeck == null)
				return;
			AddDeckToNote(SelectedDeck.Deck.Name);
		}

		private void AddDeckToNote(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return;

			Note = Note ?? string.Empty;
			const string regex = "\\[(?<tag>(.*?))\\]";
			var match = Regex.Match(Note, regex);
			if (match.Success)
			{
				var tag = match.Groups["tag"].Value;
				_log.Debug($"NoteVM: Replacing '{tag}' with {text}'");
				Note = Note.Replace(match.Value, $"[{text}]");
			}
			else
			{
				_log.Debug($"NoteVM: Adding '{text}' to note");
				Note = $"[{text}] {Note}";
			}
		}

		private void NoteViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "SelectedDeck")
			{
				DeckSelected(SelectedDeck);
				_log.Debug($"NoteVM: DeckSelected ({SelectedDeck.Deck.DisplayName})");
			}
		}
	}
}