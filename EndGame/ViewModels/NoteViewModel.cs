using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class NoteViewModel : ViewModelBase
	{
		private ITrackerRepository _repository;

		public List<Card> Cards { get; set; }
		public List<string> Decks { get; set; }

		private string _note;

		public string Note
		{
			get { return _note; }
			set { Set(() => Note, ref _note, value); }
		}

		public RelayCommand<string> NoteTextChangeCommand { get; private set; }

		public NoteViewModel()
		{
			_repository = new TrackerRepository();

			var deck = _repository.GetOpponentDeck();
			Cards = new List<Card>(deck.Cards);
			var alldecks = _repository.GetAllArchetypeDecks();
			Decks = alldecks
				.OrderByDescending(x => x.Similarity(deck))
				.Select(d => $"{d.Name} {d.Klass} {d.Similarity(deck)}")
				.ToList();
			Note = _repository.GetGameNote()?.ToString();

			NoteTextChangeCommand = new RelayCommand<string>(x => _repository.UpdateGameNote(x));
		}
	}
}