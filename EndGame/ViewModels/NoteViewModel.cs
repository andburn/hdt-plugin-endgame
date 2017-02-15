using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;
using HDT.Plugins.EndGame.Utilities;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class NoteViewModel : ViewModelBase
	{
		private IDataRepository _repository;
		private ILoggingService _log;

		public ObservableCollection<Card> Cards { get; set; }
		public ObservableCollection<MatchResult> Decks { get; set; }

		private string _note;

		public string Note
		{
			get { return _note; }
			set { Set(() => Note, ref _note, value); }
		}

		private string _playerClass;

		public string PlayerClass
		{
			get { return _playerClass; }
			set { Set(() => PlayerClass, ref _playerClass, value); }
		}

		private MatchResult _selectedDeck;

		public MatchResult SelectedDeck
		{
			get { return _selectedDeck; }
			set { Set(() => SelectedDeck, ref _selectedDeck, value); }
		}

		private bool _hasScreenshots;

		public bool HasScreenshots
		{
			get { return _hasScreenshots; }
			set { Set(() => HasScreenshots, ref _hasScreenshots, value); }
		}

		public RelayCommand WindowClosingCommand { get; private set; }

		public NoteViewModel()
			: this(EndGame.Data, EndGame.Logger)
		{
		}

		public NoteViewModel(IDataRepository track, ILoggingService logger)
		{
			Cards = new ObservableCollection<Card>();
			Decks = new ObservableCollection<MatchResult>();
			HasScreenshots = false;

			if (IsInDesignMode)
			{
				_repository = new DesignerRepository();
				HasScreenshots = true;
			}
			else
			{
				_repository = track;
			}
			_log = logger;

			PropertyChanged += NoteViewModel_PropertyChanged;

			WindowClosingCommand = new RelayCommand(() => Closing());

			Update();
		}

		private void Update()
		{
			Note = _repository.GetGameNote()?.ToString();

			var deck = _repository.GetOpponentDeck();

			Cards.Clear();
			deck.Cards.ForEach(c => Cards.Add(c));
			PlayerClass = deck.Class.ToString();

			Decks.Clear();
			var alldecks = _repository
				.GetAllDecksWithTag(Strings.ArchetypeTag)
				.Select(d => new ArchetypeDeck(d))
				.ToList();
			var results = ViewModelHelper.MatchArchetypes(deck, alldecks);
			results.ForEach(r => Decks.Add(r));
			results.ForEach(r => _log.Info($"{r.Deck.Name} [{r.Similarity}, {r.Containment}]"));

			var firstDeck = Decks.FirstOrDefault();
			if (firstDeck != null && firstDeck.Similarity > MatchResult.THRESHOLD)
				DeckSelected(firstDeck);
		}

		private void NoteViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Note")
				_repository.UpdateGameNote(Note);
			else if (e.PropertyName == "SelectedDeck")
				DeckSelected(SelectedDeck);
		}

		private void Closing()
		{
			// TODO remove if no need for it
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
			_log.Debug($"Adding {text} to note");
			const string regex = "\\[(?<tag>(.*?))\\]";
			var match = Regex.Match(Note, regex);
			if (match.Success)
			{
				var tag = match.Groups["tag"].Value;
				Note = Note.Replace(match.Value, $"[{text}]");
			}
			else
			{
				Note = $"[{text}] {Note}";
			}
		}
	}
}