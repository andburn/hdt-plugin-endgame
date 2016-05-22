using System;
using GalaSoft.MvvmLight;
using HDT.Plugins.EndGame.Services;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private IArchetypeDecksRepository _repository;

		private ArchetypeDeckListViewModel _deckList;

		public ArchetypeDeckListViewModel DeckList
		{
			get { return _deckList; }
			set { Set(() => DeckList, ref _deckList, value); }
		}

		private ArchetypeDeckEditViewModel _deckView;

		public ArchetypeDeckEditViewModel DeckView
		{
			get { return _deckView; }
			set { _deckView = value; }
		}

		public MainViewModel()
		{
			_repository = new ArchetypeDecksFileRepository(); // TODO singleton/injection
			_deckList = new ArchetypeDeckListViewModel();
			_deckList.DeckSelectedEvent += _deckListViewModel_DeckSelectedEvent;
			_deckView = new ArchetypeDeckEditViewModel();
		}

		// TODO async void not good?
		private async void _deckListViewModel_DeckSelectedEvent(Guid id)
		{
			Log.Info("deck select event triggered on parent viewmodel");
			var deck = await _repository.GetDeck(id);
			Log.Info("deck is null = " + (deck == null));
			if (deck != null)
				_deckView.Update(deck);
		}
	}
}