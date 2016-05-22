using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class ArchetypeDeckListViewModel : ViewModelBase
	{
		private IArchetypeDecksRepository _repository = new ArchetypeDecksFileRepository();

		public ObservableCollection<ArchetypeDeck> Decks { get; set; }
		public RelayCommand NewDeckCommand { get; private set; }
		public RelayCommand<ArchetypeDeck> DeleteDeckCommand { get; private set; }
		public RelayCommand<ArchetypeDeck> DeckSelectedCommand { get; private set; }

		public event Action<Guid> DeckSelectedEvent = delegate { };

		public ArchetypeDeckListViewModel()
		{
			if (IsInDesignMode)
				return;
			var data = _repository.GetAllDecks().Result;
			Decks = new ObservableCollection<ArchetypeDeck>(data);
			NewDeckCommand = new RelayCommand(() => Decks.Add(new ArchetypeDeck() { Name = "New Deck" }));
			DeleteDeckCommand = new RelayCommand<ArchetypeDeck>(x => Decks.Remove(x));
			DeckSelectedCommand = new RelayCommand<ArchetypeDeck>(x => DeckSelectedEvent(x.Id));
		}
	}
}