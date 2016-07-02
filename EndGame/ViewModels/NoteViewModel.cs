﻿using System.Collections.ObjectModel;
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
		public RelayCommand UpdateCommand { get; private set; }

		public NoteViewModel()
		{
			_repository = new TrackerRepository();

			Cards = new ObservableCollection<Card>();
			Decks = new ObservableCollection<MatchResult>();

			Update();

			NoteTextChangeCommand = new RelayCommand<string>(x => _repository.UpdateGameNote(x));
			UpdateCommand = new RelayCommand(() => Update());
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

			SelectedDeck = Decks.FirstOrDefault();

			Note = _repository.GetGameNote()?.ToString();
		}
	}
}