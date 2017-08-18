using GalaSoft.MvvmLight.Command;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class BasicNoteViewModel : NoteViewModelBase
	{
		private static object _cardLock = new object();

		public ObservableCollection<Card> Cards { get; set; }

		private IDataRepository _repository;
		private ILoggingService _logger;

		private string _note;

		public string Note
		{
			get
			{
				EndGame.Logger.Debug("BasicNoteVM: Get Note");
				if (_note == null)
					_note = _repository.GetGameNote();
				return _note;
			}
			set
			{
				EndGame.Logger.Debug("BasicNoteVM: Set Note");
				Set(() => Note, ref _note, value);
				_repository.UpdateGameNote(Note);
			}
		}

		private string _playerClass;

		public string PlayerClass
		{
			get { return _playerClass; }
			set { Set(() => PlayerClass, ref _playerClass, value); }
		}

		private bool _isNoteFocused;

		public bool IsNoteFocused
		{
			get { return _isNoteFocused; }
			set { Set(() => IsNoteFocused, ref _isNoteFocused, value); }
		}

		public bool OpponentDeckIsValid
		{
			get
			{
				return Cards != null && Cards.Count > 0;
			}
		}

		public RelayCommand AddOpponentDeck { get; private set; }

		public BasicNoteViewModel()
			: this(EndGame.Data, EndGame.Logger)
		{
		}

		public BasicNoteViewModel(IDataRepository track, ILoggingService logger)
		{
			Cards = new ObservableCollection<Card>();
			BindingOperations.EnableCollectionSynchronization(Cards, _cardLock);
			_repository = track;
			_logger = logger;
			AddOpponentDeck = new RelayCommand(() => ViewModelHelper.EditNewArchetypeDeck(Cards));
		}

		public override async Task Update()
		{
			Note = _repository.GetGameNote();
			var deck = _repository.GetOpponentDeck();
			Cards.Clear();
			await Task.Run(() => deck.Cards.ForEach(c => Cards.Add(c)));
			PlayerClass = deck.Class.ToString();
			IsNoteFocused = true;
		}
	}
}