using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.Common.Util;
using HDT.Plugins.EndGame.Utilities;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private static readonly ViewModelBase SettingsVM = new SettingsViewModel();
		private static readonly NoteViewModelBase NoteVM = new NoteViewModel();
		private static readonly NoteViewModelBase BasicNoteVM = new BasicNoteViewModel();
		private static readonly NoteViewModelBase EmptyNoteVM = new EmptyNoteViewModel();

		private string _contentTitle;

		public string ContentTitle
		{
			get { return _contentTitle; }
			set { Set(() => ContentTitle, ref _contentTitle, value); }
		}

		private ViewModelBase _contentViewModel;

		public ViewModelBase ContentViewModel
		{
			get { return _contentViewModel; }
			set { Set(() => ContentViewModel, ref _contentViewModel, value); }
		}

		public RelayCommand<string> NavigateCommand { get; private set; }

		public MainViewModel()
		{
			// set default content to screenshot view
			ContentViewModel = SettingsVM;
			NavigateCommand = new RelayCommand<string>(x => OnNavigation(x));
		}

		public void OnNavigation(string location)
		{
			var loc = location.ToLower();
			if (loc == Strings.NavSettings)
			{
				ContentViewModel = SettingsVM;
			} 
			else if (loc == Strings.NavNote)
			{
				LoadNote();
			}
			else
			{
				EndGame.Logger.Error($"Unknown Main navigation '{location}'");
				return;
			}

			if (loc.Length > 2)
				ContentTitle = loc.Substring(0, 1).ToUpper() + loc.Substring(1);
		}

		private void LoadNote()
		{
			NoteViewModelBase viewModel = EmptyNoteVM;
			var mode = EndGame.Data.GetGameMode();
			if (EndGame.Settings.Get(Strings.DeveloperMode).Bool)
			{
				viewModel = BasicNoteVM;
			}
			else if (IsDeckAvailable())
			{
				if (ViewModelHelper.IsModeEnabledForArchetypes(mode))
				{
					viewModel = NoteVM;
				}				
				else if (EndGame.Settings.Get(Strings.ShowRegularNoteBox).Bool)
				{
					viewModel = BasicNoteVM;
				}
			}
			viewModel.Update();
			ContentViewModel = viewModel;
		}

		public static bool IsDeckAvailable()
		{
			var deck = EndGame.Data.GetOpponentDeck();
			return deck.Cards.Count >= 1 && deck.Class != PlayerClass.ALL;
		}
	}
}