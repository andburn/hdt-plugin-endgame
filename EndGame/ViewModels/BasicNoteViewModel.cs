using System.Collections.ObjectModel;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using HDT.Plugins.EndGame.Utilities;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class BasicNoteViewModel : NoteViewModelBase
	{
		public ObservableCollection<Card> Cards { get; set; }

		private IDataRepository _repository;
		private ILoggingService _logger;

		private string _note;

		public string Note
		{
			get { return _note; }
			set
			{
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

		public BasicNoteViewModel()
			: this(EndGame.Data, EndGame.Logger)
		{
		}

		public BasicNoteViewModel(IDataRepository track, ILoggingService logger)
		{
			Cards = new ObservableCollection<Card>();

			if (IsInDesignMode)
			{
				_repository = new DesignerRepository();
			}
			else
			{
				_repository = track;
			}
			_logger = logger;

			Update();
		}

		public override void Update()
		{
			Note = _repository.GetGameNote()?.ToString();
			var deck = _repository.GetOpponentDeck(false);
			Cards.Clear();
			deck.Cards.ForEach(c => Cards.Add(c));
			PlayerClass = deck.Class.ToString();
		}
	}
}