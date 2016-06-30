using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class NoteViewModel : ViewModelBase
	{
		private ITrackerRepository _repository;

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

		public RelayCommand<string> NoteTextChangeCommand { get; private set; }

		public NoteViewModel()
		{
			_repository = new TrackerRepository();

			var deck = _repository.GetOpponentDeck();
			Cards = new ObservableCollection<Card>(deck.Cards);
			PlayerClass = deck.Klass.ToString();
			Log.Info(PlayerClass);
			var alldecks = _repository.GetAllArchetypeDecks();
			Decks = new ObservableCollection<MatchResult>(ViewModelHelper.MatchArchetypes(deck, alldecks));

			SelectedDeck = Decks.FirstOrDefault();

			Note = _repository.GetGameNote()?.ToString();

			NoteTextChangeCommand = new RelayCommand<string>(x => _repository.UpdateGameNote(x));
		}
	}
}