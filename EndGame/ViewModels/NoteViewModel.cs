using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class NoteViewModel : ViewModelBase
	{
		private ITrackerRepository _repository;

		public ObservableCollection<Card> Cards { get; set; }
		public ObservableCollection<ArchetypeDeck> Decks { get; set; }

		public NoteViewModel()
		{
			_repository = new TrackerRepository();
			var deck = _repository.GetOpponentDeck().Result;
			Cards = new ObservableCollection<Card>(deck.Cards);
			Decks = new ObservableCollection<ArchetypeDeck>(
				_repository.GetAllArchetypeDecks().Result);
		}
	}
}