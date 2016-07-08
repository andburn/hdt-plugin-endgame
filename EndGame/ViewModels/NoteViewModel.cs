using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;
using HDT.Plugins.EndGame.Utilities;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class NoteViewModel : ViewModelBase
	{
		private ITrackerRepository _repository;
		private IImageCaptureService _cap;
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

		public ObservableCollection<Screenshot> Screenshots { get; set; }

		public RelayCommand<string> NoteTextChangeCommand { get; private set; }
		public RelayCommand UpdateCommand { get; private set; }
		public RelayCommand WindowClosingCommand { get; private set; }
		public RelayCommand<MatchResult> DeckSelectedCommand { get; private set; }

		public NoteViewModel()
		{
			Cards = new ObservableCollection<Card>();
			Decks = new ObservableCollection<MatchResult>();
			HasScreenshots = false;

			if (IsInDesignMode)
			{
				_repository = new DesignerRepository();
				Screenshots = DesignerData.GenerateScreenshots();
				HasScreenshots = true;
			}
			else
			{
				_repository = new TrackerRepository();
			}
			_cap = new TrackerCapture();
			_log = new TrackerLogger();

			Update();

			NoteTextChangeCommand = new RelayCommand<string>(x => _repository.UpdateGameNote(x));
			UpdateCommand = new RelayCommand(() => Update());
			WindowClosingCommand = new RelayCommand(() => Closing());
			DeckSelectedCommand = new RelayCommand<MatchResult>(x => DeckSelected(x));
		}

		public NoteViewModel(ObservableCollection<Screenshot> screenshots)
			: this()
		{
			Screenshots = screenshots;
			HasScreenshots = Screenshots != null && Screenshots.Any();
		}

		private void Update()
		{
			var deck = _repository.GetOpponentDeck();

			Cards.Clear();
			deck.Cards.ForEach(c => Cards.Add(c));
			PlayerClass = deck.Klass.ToString();

			Decks.Clear();
			var alldecks = _repository.GetAllArchetypeDecks();
			var results = ViewModelHelper.MatchArchetypes(deck, alldecks);
			results.ForEach(r => Decks.Add(r));

			Note = _repository.GetGameNote()?.ToString();

			SelectedDeck = Decks.FirstOrDefault();
			if (SelectedDeck != null)
				DeckSelected(SelectedDeck);
		}

		private void Closing()
		{
			var screenshot = Screenshots?.FirstOrDefault(s => s.IsSelected);
			if (screenshot != null)
			{
				_log.Debug($"Attempting to save screenshot #{screenshot.Index}");
				try
				{
					_cap.SaveImage(screenshot);
				}
				catch (Exception e)
				{
					_log.Error(e.Message);
				}
			}
			else
			{
				_log.Debug($"No screenshot selected (len={Screenshots?.Count})");
			}
			// should update with binding, but just in case
			_repository.UpdateGameNote(Note);
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